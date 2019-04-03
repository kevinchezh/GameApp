using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameApp.API.Data
{
    public class GameRepository : IGameRepository
    {
        readonly MyWebApiContext context;
        // need to modify database in this repo
        public GameRepository(MyWebApiContext context)
        {
            this.context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            // this would save entity to memory, not database yet, so this op so far
            // is still an sync opertion
            this.context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            this.context.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
            // include method, would include the referenced value into user as well.
            var User = await this.context.Users.Include(p => p.Games).FirstOrDefaultAsync(u => u.Id == id);
            return User;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await this.context.Users.Include(a => a.Games).ToListAsync();
            return users;
        }

        public async Task<bool> SaveAll()
        {
            // if have more than 0 changes then we successfully made some changes, return true
            // other wise return false;
            return await this.context.SaveChangesAsync() > 0;
        }
    }
}
