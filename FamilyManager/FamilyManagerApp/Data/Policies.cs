using Microsoft.AspNetCore.Authorization;

namespace FamilyManagerApp.Data {
    public class Policies {
        public const string IsAdmin = "IsAdmin";
        public const string IsUser = "IsUser";

        // public static AuthorizationPolicy isAdminPolicy() {
        //     return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole("Admin").Build();
        // }

        public static AuthorizationPolicy isUserPolicy() {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole("User").Build();
        }
    }
}