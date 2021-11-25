using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityExampleToken.Common
{
    public static class TokenClaims
    {
        public static async Task<List<Claim>> GetValidClaims(IdentityUser user, UserManager<IdentityUser> _userManager)
        {
            
            TimeSpan t = DateTime.UtcNow - DateTime.Now;
            Int64 secondsSinceEpoch = (Int64)t.TotalSeconds;
            IdentityOptions _options = new IdentityOptions();
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, await Task.FromResult(user.Id.ToString())),
            new Claim(JwtRegisteredClaimNames.Iat, secondsSinceEpoch.ToString(), ClaimValueTypes.Integer64),
            new Claim(_options.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
            new Claim(_options.ClaimsIdentity.UserNameClaimType, user.UserName)
        };
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            return claims;
        }
    }
}
