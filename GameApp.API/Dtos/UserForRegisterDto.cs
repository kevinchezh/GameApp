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
        [Required]
        public string Gender { get; set; }
        [Required]
        public string KnownAs { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        public UserForRegisterDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}
