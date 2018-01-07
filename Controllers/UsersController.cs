using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using DotNetAPI2;
using DotNetAPI2.Data;
using DotNetAPI2.Models;

namespace DotNetAPI2.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        // *********** REQUIRED FOR DATABASE CALLS *********
        private readonly UserDbContext _context;
        public UsersController(UserDbContext context) { _context = context; }
        // *********** REQUIRED FOR DATABASE CALLS *********

        // GET api/Users
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _context.Users.ToList();
        }

        // GET api/Users/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            return _context.Users.First(t => t.Id == id);
        }

        // POST api/Users
        [HttpPost]
        public void Post([FromForm] User user)
        {
            // will fail if Password not set in post-request!
            var passStruct = GeneratePassword(user.Password);
            user.Salt = passStruct.Salt;
            user.Password = passStruct.Hash;

            _context.Users.Add(user);
            _context.SaveChanges(); // EF requires you to commit your changes by default
        }

        private string GenerateHash(string password, string salt)
        {
            return GenerateHash(password, Convert.FromBase64String(salt));
        }

        // PUT api/Users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromForm] User user)
        {
            user.Id = id; // Ensure an id is attached
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        // DELETE api/Users/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
                // Check if element exists
            if ( _context.Users.Where(t => t.Id == id).Count() > 0 ) {
                _context.Users.Remove(_context.Users.First(t => t.Id == id));
                _context.SaveChanges();
            }
        }





        struct PasswordStruct { public string Salt; public string Hash; }
        private PasswordStruct GeneratePassword(string password)
        {
            byte[] saltGen = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltGen);
            }
            string salt = Convert.ToBase64String(saltGen);
            string hashedPassword = GenerateHash(password, saltGen);

            PasswordStruct pwdStruct;
            pwdStruct.Salt = salt;
            pwdStruct.Hash = hashedPassword;
            return pwdStruct;
        }

        // Returns True if match, False if no match
        private bool VerifyPassword(string password, string hash, string salt)
        {
            string hashedPassword = GenerateHash(password, salt);
            return (hashedPassword == hash) ? true : false;
        }

        private string GenerateHash(string password, byte[] salt)
        {
            return Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );
        }
    }
}
