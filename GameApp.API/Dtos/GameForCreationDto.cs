using System;
using Microsoft.AspNetCore.Http;

namespace GameApp.API.Dtos
{
    public class GameForCreationDto
    {
        public string Url { get; set; }
        // IFormFile: represent a file send along with HTTP request
        public IFormFile File { get; set; }
        public string Descriptioin { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }

        public GameForCreationDto()
        {
            DateAdded = DateTime.Now;
        }
    }
}