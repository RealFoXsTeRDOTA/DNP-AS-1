using System.ComponentModel.DataAnnotations;

namespace Models {
    
    public class User {
        public string Username { get; set; }
        public string Password { get; set; }

        public User() {
            
        }

        public User(string username, string password) {
            this.Username = username;
            this.Password = password;
        }

    }
}