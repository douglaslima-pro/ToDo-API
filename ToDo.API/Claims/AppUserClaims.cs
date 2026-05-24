using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ToDo.API.Claims
{
    public static class AppUserClaims
    {
        public static AppUser? ExtractClaims(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null || !claimsPrincipal.Identity!.IsAuthenticated)
            {
                return null;
            }

            if (!int.TryParse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier), out int id))
            {
                return null;
            }

            return new AppUser
            {
                Id = id,
                FirstName = claimsPrincipal.FindFirstValue(ClaimTypes.Name),
                Email = claimsPrincipal.FindFirstValue(ClaimTypes.Email),
            };
        }
    }
}
