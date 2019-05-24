using System;

namespace GameApp.API.Dtos
{
    public class GameForReturnDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public string publicID { get; set; }
        public GameForReturnDto()
        {
        }
    }
}