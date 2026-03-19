using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace NexuSys.Helper
{
    public class UserContext : IUserContext
    {
        public int UserId { get; }
        public string UserName { get; }
        public bool IsAuthenticated { get; }

        public UserContext(AuthenticationStateProvider auth)
        {
            var user = auth.GetAuthenticationStateAsync()
                           .GetAwaiter().GetResult().User;

            IsAuthenticated = user.Identity?.IsAuthenticated ?? false;

            if (!IsAuthenticated) return;

            UserId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            UserName = user.Identity!.Name!;
        }
    }
}
