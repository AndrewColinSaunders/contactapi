using contactsapi.Models;

namespace contactsapi.Services
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDto registerDto);

        Task<string> Login(LoginDto loginDto);
    }
}