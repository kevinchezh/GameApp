using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GameApp.API.Data;
using GameApp.API.Dtos;
using GameApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        readonly IGameRepository repo;
        readonly IMapper mapper;

        public UsersController(IGameRepository repo, IMapper mapper)
        {
            this.mapper = mapper;
            this.repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await this.repo.GetUsers();
            var usersToReturn = mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await this.repo.GetUser(id);
            var userToReturn = mapper.Map<UserForDetailDto>(user);
            return Ok(userToReturn);
        }

        // update user
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            /*
             * The User object in this case is being obtained from the Controller base class, 
             * which has access to the HttpContext  which contains various aspects of the Http Request 
             * (including the User claims).  When the user has to authenticate to get to the action in 
             * the controller, the User claims are stored in the HttpContext and we can fish it out 
             * from there.           
             */
             /*
                User here is not the User data model we set up. It actually means the API user. If we 
                are right now in different controller like ValueController, User is still usesable, it 
                just means the API user who is trying to make request. And that user have many functions 
                and properties along with his request, so we can use that               
             */

            // here we check if the id passed in is the same with the person who is making the request
            // we dont want a user to modify other user's information
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            User userFromRepo = await repo.GetUser(id);
            Console.WriteLine("in here");
            mapper.Map(userForUpdateDto, userFromRepo);
            var res = await repo.SaveAll();
            if (res)
            {
                return NoContent();
            }
            else
            {
                throw new Exception($"Updating user {id} failed on save");
            }

        }

    }
}
