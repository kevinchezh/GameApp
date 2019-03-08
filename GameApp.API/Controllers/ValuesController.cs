using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameApp.API.Controllers
{
    // to apply authentication we need to config authentication first in startup.cs
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly MyWebApiContext _context;

        public ValuesController(MyWebApiContext context)
        {
            this._context = context;
        }
        // GET api/values
        [HttpGet]
        // IActionResult allow return HTTP responses 
        /* 
            Async: why async?
                Sync ops would block other request while dealing with one request, so it is bad behavior 
                when app is in scale.

            How to change to async, add async key word in signiture, and async ops return a Task with 
            generic types. (Like promise in js)
            Also add await keyword and change method to async version
        */
        public async Task<IActionResult> GetValues()
        {
            var values = await _context.Users.ToListAsync();
            return Ok(values);
        }

        // GET api/values/5

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            // return first match or default value if no match
            var value = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
