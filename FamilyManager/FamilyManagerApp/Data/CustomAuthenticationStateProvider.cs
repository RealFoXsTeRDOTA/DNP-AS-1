using System;
using System.Collections.Generic;
using System.Linq;
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
            } else {
                identity = SetupClaimsForUser(cachedUser);
            }
            
            ClaimsPrincipal cachedClaimsPrincipal = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(cachedClaimsPrincipal));
        }

        public void ValidateLogin(string username, string password) {
            if (string.IsNullOrWhiteSpace(username)) throw new Exception("Please specify a username");
            if (string.IsNullOrWhiteSpace(password)) throw new Exception("Please specify a password");

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
            if (string.IsNullOrWhiteSpace(username)) throw new Exception("Please specify a username");
            if (string.IsNullOrWhiteSpace(password)) throw new Exception("Please specify a password");
            if (string.IsNullOrWhiteSpace(confirmationPassword)) throw new Exception("Please confirm your password");
            
            if (!password.Equals(confirmationPassword))
                throw new Exception("Passwords do not match");
            
            List<User> users = userService.GetUserList();
            if (users.FirstOrDefault(u => u.Username.Equals(username)) != null) {
                throw new Exception("Username is already used");
            }
            
            userService.AddUser(new User() {
                Username = username,
                Password = password,
                Role = Role.User
            });
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
            claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));
            ClaimsIdentity identity = new ClaimsIdentity(claims, "apiauth_type");
            return identity;
        }
    }
}