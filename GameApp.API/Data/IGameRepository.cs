using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameApp.API.Models;

namespace GameApp.API.Data
{
    public interface IGameRepository
    {
        // generic method that takes generic class
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        Task<Game> GetGame(int id);

        Task<Game> GetMainGame(int userId);
    }
}
