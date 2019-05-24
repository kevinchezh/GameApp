using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameApp.API.Data;
using GameApp.API.Dtos;
using GameApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GameApp.API.Controllers
{
    /*
    what does the ApiController do. One thing is to reterive data from request body and 
    set that information to Dtos object and pass that as an argument.
    Because post request contain information in the body, and without ApiController, 
    UserForRegisterDto have no access to the information in the body(we can add [FromBody] front to make
    it work). So ApiController would automatically do that for us
    Without ApiController annotation and no [FromBody], the dto would just be null on every field

    Second, ApiController would apply data validate in dto object for us. Without it, we should add more
    code to apply the validation
    */
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repo, IConfiguration config,
            IMapper mapper)
        {
            this._repo = repo;
            this._config = config;
            _mapper = mapper;
        }
        //route api/auth/register
        [HttpPost("register")]
        // without [ApiController] then we need to [FromBody]UserForRegisterDto dto
        // to tell dto to retrieve the data in post request's body
            public async Task<IActionResult> Register(UserForRegisterDto dto)
        {
            //DTO data transfer object
            //The incoming parameter of a http request is more likely to be an object rather than
            // two seperate string username and password


            string username = dto.Username;
            string password = dto.Password;
            username = username.ToLower();
            if(await _repo.UserExists(username))
            {
                return BadRequest("username already exists");
            }

            var UserToCreate = new User
            {
                Username = username
            };
            var createdUser = await _repo.Register(UserToCreate, password);
            return StatusCode(201);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto dto)
        {
            var userFromRepo = await _repo.Login(dto.Username.ToLower(), dto.Password);
            if(userFromRepo == null)
            {
                return Unauthorized();
            }
            // build token
            // 
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            // return token as well as some user information(except the salt and 
            // password)
            var user = _mapper.Map<UserForListDto>(userFromRepo);
            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });
        }
    }
}
