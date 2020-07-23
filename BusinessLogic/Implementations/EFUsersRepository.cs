using BusinessLogic.Interfaces;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;

namespace BusinessLogic.Implementations
{
    public class EFUsersRepository : IUsersRepository
    {
        private EFDbContext context;
        public EFUsersRepository(EFDbContext context)
        {
            this.context = context;
        }

        public void CreateUser(string userName, string password, string email, string firstname, string lastName, string middleName)
        {
            User user = new User
            {
                UserName = userName,
                Password = password,
                Email = email,
                CreatedDate = DateTime.UtcNow,
                FirstName = firstname,
                LastName = lastName,
                MiddleName = middleName
            };
            SaveUser(user);
        }

        public MembershipUser GetMembershipUserByName(string userName)
        {
            User user = null;
            try
            {
                user = context.Users.FirstOrDefault(x => x.UserName == userName);
            }
            catch (Exception)
            {
            }
            if (user != null)
            {
                return new MembershipUser(
                    "CustomMembershipProvider",
                    user.UserName,
                    user.Id,
                    user.Email,
                    "",
                    null,
                    true,
                    false,
                    user.CreatedDate,
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now
                    );
            }
            return null;
        }

        public User GetUserById(int id)
        {
            return context.Users.FirstOrDefault(x => x.Id == id);
        }

        public User GetUserByName(string userName)
        {
            return context.Users.FirstOrDefault(x => x.UserName == userName);
        }

        public string GetUserNameByEmail(string email)
        {
            User user = context.Users.FirstOrDefault(x => x.Email == email);
            if (user != null)
            {
                return user.UserName;
            }
            return "";
        }

        public IEnumerable<User> GetUsers()
        {
            return context.Users;
        }

        public void SaveUser(User user)
        {
            if (user.Id == 0)
                context.Users.Add(user);
            else
                context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();
        }

        public bool ValidateUser(string userName, string password)
        {
            User user = context.Users.FirstOrDefault(x => x.UserName == userName);
            if (user != null && user.Password == password)
            {
                return true;
            }
            return false;
        }
    }
}
