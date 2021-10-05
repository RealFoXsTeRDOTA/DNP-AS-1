using System.Collections.Generic;
using Models;

namespace FileData {
    public interface IUserService {
        List<User> GetUserList();
        User ValidateUser(string username, string password);

        void AddUser(string username, string password);
    }
}