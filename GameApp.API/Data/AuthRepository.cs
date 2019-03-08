using System;
using System.Threading.Tasks;
using GameApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameApp.API.Data
{
    /*
        Password encrypt logic: 
                   System.Security.Cryptography.HMACSHA512 will give back a object contains infos about
                   a password.
                   Each password is encrypted with password itself and a randomly generated key(salt),
                   then hmacsha512 will use that salt and password together to generate a computed hash.
                   For the same input password, because the salt is random, therefore the result hash is 
                   not the same which makes it hard to decrypt.

        How to validate?
            We save the salt of that user when it is registed. And when we do the login, we reterive the 
            user first and then find the salt value. And take the salt as an argument of System.Security.Cryptography.HMACSHA512
            method to inform HMAC to use this specific salt rather than random generate another.
            So by doing this way, if the salt match and password match, the result hash would be always the 
            same.           
    */
    public class AuthRepository : IAuthRepository
    {
        private readonly MyWebApiContext _context;

        public AuthRepository(MyWebApiContext context)
        {
            this._context = context;
        }

        public async Task<User> Login(string username, string password)
        {
            // find the user first using username
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                // no user match
                return null;
            }
            if (!VarifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                // password not match
                return null;
            }
            return user;
        }

        private bool VarifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            // here we must take passwordSalt as an argument to make sure hmac would use this salt 
            // rather than random generate another.
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                // with the same salt and same password, the hash should be the same
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0; i < computedHash.Length; i++)
                {
                    // not match return false
                    if (computedHash[i] != passwordHash[i]) return false;
                }

            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            // out keyword would pass the reference of that variable rather than the value copy
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await this._context.AddAsync(user);
            await this._context.SaveChangesAsync();
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // using () {} everything happens within {} will be disposed after it's done
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x => x.Username == username)) {
                return true;
            }
            return false;
        }
    }
}
