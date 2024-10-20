using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Shared.Models;

namespace Shared.Helpers
{
    public static class JwtHelper
    {
        private static AppSettings AppSetting = new AppSettings();//in future, add to program.cs

        public static string GenerateJwtToken(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = Encoding.UTF8.GetBytes(AppSetting.ContactConfig.SecretKey);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = AppSetting.ContactConfig.Issuer,
                Audience = AppSetting.ContactConfig.Audience,
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}