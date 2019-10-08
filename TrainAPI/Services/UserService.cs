using System.Collections.Generic;
using TrainAPI.Models;
using TrainAPI.Helpers;
using System.Linq;
using System;

namespace TrainAPI.Services
{
    public interface IUserService
    {
        User Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(int id);
        User Authenticate(string login, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
    public class UserService : IUserService
    {
        private UserContext userdb;

        public UserService(UserContext userdb)
        {
            this.userdb = userdb;
        }

        public User Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (userdb.Users.Any(x => x.Login == user.Login))
                throw new AppException("Login \"" + user.Login + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            userdb.Users.Add(user);
            userdb.SaveChanges();

            return user;
        }

        public void Update(User userToUpdate, string password = null)
        {
            var user = userdb.Users.Find(userToUpdate.UserId);

            if (user == null)
                throw new AppException("User not found");

            if (userToUpdate.Login != user.Login)
            {
                if (userdb.Users.Any(x => x.Login == userToUpdate.Login))
                    throw new AppException("Login " + userToUpdate.Login + " is already taken");
            }

            user.Login = userToUpdate.Login;

            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            userdb.Users.Update(user);
            userdb.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = userdb.Users.Find(id);
            if (user != null)
            {
                userdb.Users.Remove(user);
                userdb.SaveChanges();
            }
        }

        public User Authenticate(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return null;

            var user = userdb.Users.SingleOrDefault(x => x.Login == login);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return userdb.Users;
        }

        public User GetById(int id)
        {
            return userdb.Users.Find(id);
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmacsha512 = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmacsha512.Key;
                passwordHash = hmacsha512.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");

            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");

            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmacsha512 = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmacsha512.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }
    }
}
