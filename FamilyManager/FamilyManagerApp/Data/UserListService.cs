using System;
using System.Collections.Generic;
using Models;
using System.Linq;
using FileData;

namespace FamilyManagerApp.Data {
    public class UserListService : IUserService {

        private List<User> users;

        public UserListService() {
            users = new[] {
                new User {
                    
                    Password = "123456",
                    Username = "Rory"
                },
                new User {
                    Password = "654321",
                    Username = "Bory"
                }
            }.ToList();
        }

        public User ValidateUser(string userName, string password) {
            User first = users.FirstOrDefault(user => user.Username.Equals(userName));
            if (first == null)
                throw new Exception("User not found");
            if (!first.Password.Equals(password))
                throw new Exception("Incorrect password");
            return first;
        }

        public List<User> GetUserList() {
            return users;
        }
    }
}