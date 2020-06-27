using lcapis.Helpers;
using lcapis.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing.Printing;
using lcapis.Utils;

namespace lcapis.Services
{
    public interface IUserService
    {
        LCUser Authenticate(string email, string password);
        LCUser GetByUserID(long id);
        LCUser Create(LCUser userParam, string password);
        void Update(LCUser userParam, string password = null);
        void Delete(long id);
    }
    public class LCUserService : IUserService
    {
        private LCProdDbContext _context;
        public LCUserService(LCProdDbContext context)
        {
            _context = context;
        }

        public LCUser Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) return null;

            LCUser user = _context.LCUsers.SingleOrDefault(u => u.NormalizedEmail == email.ToUpper());

            if (user == null) return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public LCUser Create(LCUser userParam, string password)
        {
            userParam.NormalizedEmail = userParam.Email.ToUpper();

            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password required");

            if (_context.LCUsers.Any(u => u.NormalizedEmail == userParam.NormalizedEmail))
                throw new AppException("Email is already taken");

            if (!Utils.InputVerifiers.IsValidEmail(userParam.Email))
                throw new AppException("Email is not valid");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            userParam.PasswordHash = passwordHash;
            userParam.PasswordSalt = passwordSalt;

            userParam.UserID = Utils.SnowflakeGeneratorSingleton.Instance.CreateId();

            //tries to get unique discriminator 1000 times. if not, say it's not available :D
            int i;
            for(i = 0; i < 1000; i++)
            {
                userParam.Discrim = Utils.DiscrimGenerator.GenerateDiscrim();
                if (_context.LCUsers.Any(u => u.Discrim != userParam.Discrim))
                    break;
            }
            if (i == 999)
                throw new AppException("Too many users with that username");

            userParam.IsBot = false;

            _context.LCUsers.Add(userParam);
            _context.SaveChanges();

            return userParam;
        }

        public void Delete(long id)
        {
            LCUser user = _context.LCUsers.Find(id);
            if (user != null)
            {
                _context.LCUsers.Remove(user);
                _context.SaveChanges();
            }
        }

        public LCUser GetByUserID(long id)
        {
            return _context.LCUsers.Find(id);
        }

        public void Update(LCUser userParam, string password = null)
        {
            LCUser user = _context.LCUsers.Find(userParam.UserID);

            if (user == null)
                throw new AppException("User not found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            {
                // throw error if the new username is already taken
                if (_context.LCUsers.Any(u => u.Discrim == userParam.Discrim))
                    throw new AppException("Username already exists with that discriminator");

                user.Username = userParam.Username;
            }

            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.Email))
            {
                if (_context.LCUsers.Any(u => u.NormalizedEmail == userParam.Email.ToUpper()))
                    throw new AppException("An account already exists with that email");

                user.NormalizedEmail = userParam.Email.ToUpper();
            }

            if (userParam.Discrim != user.Discrim)
            {
                if (_context.LCUsers.Any(u => u.Discrim == userParam.Discrim))
                    throw new AppException("That discriminator is already taken");

                user.Discrim = userParam.Discrim;
            }

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.LCUsers.Update(user);
            _context.SaveChanges();
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
