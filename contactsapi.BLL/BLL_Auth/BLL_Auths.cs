using contactsapi.DAL.ContextConfigurations;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;
using Shared.Models;
using Shared.ViewModels;

namespace contactsapi.BLL.BLL_Auth
{
    public class BLL_Auths
    {
        public async Task<string> Login(LoginDto loginDto, ContactsContext context)
        {
            User? user = await context.Users.SingleOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null)
            {
                throw new Exception("User does not exist.");
            }

            bool passwordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

            if (!passwordValid)
            {
                throw new Exception("Invalid credentials.");
            }

            string token = JwtHelper.GenerateJwtToken(user);

            return token;
        }
    }
}