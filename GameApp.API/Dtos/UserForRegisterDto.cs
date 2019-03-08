using System;
using System.ComponentModel.DataAnnotations;

namespace GameApp.API.Dtos
{
    public class UserForRegisterDto
    {
        // this data field is required
        // theses are annotations which is used to validate input data just like spring
        [Required]
        public string Username { get; set; }
        [Required]
        // validate length
        [StringLength(8, MinimumLength = 4, ErrorMessage = "you must specify password between 4 and 8 chars")]
        public string Password { get; set; }
    }
}
