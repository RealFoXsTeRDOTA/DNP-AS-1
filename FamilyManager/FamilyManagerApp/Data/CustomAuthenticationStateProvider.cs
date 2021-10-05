using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using FileData;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Models;

namespace FamilyManagerApp.Data {
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider {

       private readonly IJSRuntime jsRuntime;
        private readonly IUserService userService;
        private User cachedUser;

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime, IUserService userService) {
            this.jsRuntime = jsRuntime;
            this.userService = userService;
        }
        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
            var identity = new ClaimsIdentity();
            if (cachedUser == null) {
                string userAsJson = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "currentUser");
                if (!string.IsNullOrEmpty(userAsJson)) {
                    User tmp = JsonSerializer.Deserialize<User>(userAsJson);
                    ValidateLogin(tmp.Username, tmp.Password);
                }
            }
            else 
                    identity = SetupClaimsForUser(cachedUser);
            ClaimsPrincipal cachedClaimsPrincipal = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(cachedClaimsPrincipal));
        }

        public void ValidateLogin(string username, string password) {
            Console.WriteLine("Validating login");
            if (string.IsNullOrEmpty(username)) throw new Exception("Enter username");
            if (string.IsNullOrEmpty(password)) throw new Exception("Enter password");

            ClaimsIdentity identity = new ClaimsIdentity();
            try {
                User user = userService.ValidateUser(username, password);
                identity = SetupClaimsForUser(user);
                string serializedData = JsonSerializer.Serialize(user);
                jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", serializedData);
                cachedUser = user;
            }
            catch (Exception e) {
                throw e;
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
        }

        public void ValidateRegister(string username, string password, string confirmationPassword) {
            if (string.IsNullOrEmpty(username)|| username.Trim().Equals("")) throw new Exception("Enter username");
            if (string.IsNullOrEmpty(password) || password.Trim().Equals("")) throw new Exception("Enter password");
            if (string.IsNullOrEmpty(confirmationPassword)) throw new Exception("Enter confirmationPassword");
            if (!password.Equals(confirmationPassword))
                throw new Exception("Password and confirmation password do not match");
            List<User> users = userService.GetUserList();
            foreach (var user in users) {
                if (user.Username.Equals(username.Trim()))
                    throw new Exception("Username is already used");
            }
            userService.AddUser(username.TrimStart().TrimEnd(), password.TrimStart().TrimEnd());
        }

        public void Logout() {
            cachedUser = null;
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "currentUser", "");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

       

        private ClaimsIdentity SetupClaimsForUser(User user) {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Username));

            ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth_type");
            return identity;
        }
    }
}