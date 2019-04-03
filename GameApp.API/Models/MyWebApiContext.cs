using System;
using Microsoft.EntityFrameworkCore;
namespace GameApp.API.Models
{
    public class MyWebApiContext : DbContext
    {
        public MyWebApiContext(DbContextOptions<MyWebApiContext> options) : base (options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
    }
}
