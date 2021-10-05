﻿using System;
using System.Collections.Generic;
using System.IO;
using Models;
using System.Linq;
using System.Text.Json;
using FileData;

namespace FamilyManagerApp.Data {
    public class UserListService : IUserService {
        private string UsersFile = "Data/users.json";
        public List<User> users { get; set; }

        public UserListService() {
            if (!File.Exists(UsersFile)) {
                seed();
                WriteUsersToFile();
            }
            else {
                string content = File.ReadAllText(UsersFile);
                users = JsonSerializer.Deserialize<List<User>>(content);
            }
        }

        private void seed() {
            users = new List<User>();
            users.Add(new User("Adriana", "1234"));
        }
        
        private void WriteUsersToFile() {
            string usersAsJson = JsonSerializer.Serialize(users);
            File.WriteAllText(UsersFile, usersAsJson);
        }
        public User ValidateUser(string userName, string password) {
            User first = users.FirstOrDefault(user => user.Username.Equals(userName));
            if (first == null)
                throw new Exception("User not found");
            if (!first.Password.Equals(password))
                throw new Exception("Incorrect password");
            return first;
        }

        public void AddUser(string username, string password) {
            users.Add(new User(username, password));
            WriteUsersToFile();
        }

        public List<User> GetUserList() {
            return users;
        }
    }
}