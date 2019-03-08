using System;
using System.Threading.Tasks;
using GameApp.API.Models;

namespace GameApp.API.Data
{
    // interface for user
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}
