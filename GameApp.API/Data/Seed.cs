using System;
using System.Collections.Generic;
using System.Data.Linq;
using GameApp.API.Models;
using Newtonsoft.Json;

namespace GameApp.API.Data
{
    public class Seed
    {
        private readonly MyWebApiContext context;
        // seed some data into our User schema, using MyWebApiContext to insert data into Users
        // remember to saveChanges after everything, it behave just like commit()
        public Seed(MyWebApiContext context)
        {
            this.context = context;
        }

        public void SeedUsers()
        { 
        // read all the text in json file
        var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            // deserialize json to .net object
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            foreach(var user in users)
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash("password", out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Username = user.Username.ToLower();
                Console.WriteLine(user.Gender);
                Console.WriteLine("Add one user");
                this.context.Users.Add(user);
            }
            this.context.SaveChanges();
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
    }
}
