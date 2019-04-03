using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GameApp.API.Data;
using GameApp.API.Dtos;
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
    }
}
