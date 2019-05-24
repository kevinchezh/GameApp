using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GameApp.API.Data;
using GameApp.API.Dtos;
using GameApp.API.Helpers;
using GameApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GameApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/games")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public GamesController(IGameRepository repo, IMapper mapper, 
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _repo = repo;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;
            // create a cloudinary account
            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
                );
            // use that account to config cloudinary service
            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetGame")]
        public async Task<IActionResult> GetGame(int id)
        {
            var gameFromRepo = await _repo.GetGame(id);
            var game = _mapper.Map<GameForReturnDto>(gameFromRepo);
            return Ok(game);
        }
        
        // where we upload the photo to cloudinary
        // pretty cool
        [HttpPost]
        public async Task<IActionResult> AddGameForUser(int userId, [FromForm]GameForCreationDto
            gameForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            // get the current user
            User userFromRepo = await _repo.GetUser(userId);

            var file = gameForCreationDto.File;

            var uploadResult = new ImageUploadResult();
            // config the upload params and get the upload result from cloudinary
            if (file.Length > 0)
            {
                // the logic below is the cloudinary api logic, see 
                // https://cloudinary.com/documentation/dotnet_integration#installation
                
                // using block would clean up the variable inside when this block
                // is over
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).
                            Crop("fill").Gravity("face")
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            // create the gameForCreationDto object
            gameForCreationDto.Url = uploadResult.Uri.ToString();
            gameForCreationDto.PublicId = uploadResult.PublicId;
            
            var game = _mapper.Map<Game>(gameForCreationDto);

            // Any method, determine whether any element in games sequence satisfy 
            // the condition inside, for this case is whether there is an Game whose
            // IsMain property is true
            // if no such element Any method would return false, so that we know
            // there is not main game yet
            if (!userFromRepo.Games.Any(u => u.IsMain))
            {
                game.IsMain = true;
            }
            // add that game into current user
            userFromRepo.Games.Add(game);
            // save changes
            if (await _repo.SaveAll())
            {
                // usually the post request should return a 201 code, and that
                // code will contain the url that you can use to get the newly 
                // created object(in this case is the newly created game), and
                // also contain the newly created object that will be returned
                var gameToReturn = _mapper.Map<GameForReturnDto>(game);
                return CreatedAtRoute("GetGame", new {id = game.Id},
                    gameToReturn);
            }

            return BadRequest("Could not add the game");
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            // get the current user
            User user = await _repo.GetUser(userId);
            // no such game
            if (!user.Games.Any(g => g.Id == id))
            {
                return Unauthorized();
            }
            // get the game
            var game = await _repo.GetGame(id);
            if (game.IsMain)
            {
                return BadRequest("This is already the main game");
            }
            // get original main game
            var currentMainGame = await _repo.GetMainGame(userId);
            // change current main to false
            currentMainGame.IsMain = false;
            // set new one to true
            game.IsMain = true;

            if (await _repo.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Could not set game to main");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            // get the current user
            User user = await _repo.GetUser(userId);
            // no such game
            if (!user.Games.Any(g => g.Id == id))
            {
                return Unauthorized();
            }
            // get the game
            var game = await _repo.GetGame(id);
            if (game.IsMain)
            {
                return BadRequest("You cannot delete the main game");
            }

            if (game.PublicID != null)
            {
                var deleteParams = new DeletionParams(game.PublicID);
                // first delete the game in cloudinary
                var res = _cloudinary.Destroy(deleteParams);
                if (res.Result == "ok")
                {
                    // the deletion on cloudinary is ok
                    // then delete the game in our database
                    _repo.Delete(game);
                }
            }

            else
            {
                // if the game itself does not have a public id which means
                // it is only stored on database not on cloudinary
                _repo.Delete(game);
            }
            

            if (await _repo.SaveAll())
            {
                return Ok();
            }
            // delete fails
            return BadRequest("Fail to delete the game");
            
        }
        
    }
}