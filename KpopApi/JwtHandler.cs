using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using KpopApi.Models;

namespace KpopApi
{
    public class JwtHandler
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<KpopUser> _userManager;
        public JwtHandler(IConfiguration configuration, UserManager<KpopUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<JwtSecurityToken> GetTokenAsync(KpopUser user) =>
        new(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: await GetClaimsAsync(user),
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpirationTimeInMinutes"])),
            signingCredentials: GetSigningCredentials());

        private SigningCredentials GetSigningCredentials()
        {
            byte[] key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecurityKey"]!);
            SymmetricSecurityKey secret = new(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaimsAsync(KpopUser user)
        {
            List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, user.UserName!)
        };
            claims.AddRange(from role in await _userManager.GetRolesAsync(user) select new Claim(ClaimTypes.Role, role));
            return claims;
        }
    }
}
