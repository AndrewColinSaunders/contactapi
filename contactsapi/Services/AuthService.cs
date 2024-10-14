//namespace contactsapi.Services
//{
//    using contactsapi.Models;
//    using contactsapi.Data;
//    using Microsoft.IdentityModel.Tokens;
//    using System.IdentityModel.Tokens.Jwt;
//    using System.Security.Claims;
//    using System.Text;
//    using Microsoft.EntityFrameworkCore;
//    using Microsoft.Extensions.Logging;

//    public class AuthService : IAuthService
//    {
//        private readonly IConfiguration _configuration;
//        private readonly ContactsContext _context;
//        private readonly ILogger<AuthService> _logger;

//        public AuthService(IConfiguration configuration, ContactsContext context, ILogger<AuthService> logger)
//        {
//            _configuration = configuration;
//            _context = context;
//            _logger = logger;
//        }

//        public async Task<string> Register(RegisterDto registerDto)
//        {
//            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
//                throw new Exception("User already exists");

//            var user = new User
//            {
//                Username = registerDto.Username,
//                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
//            };

//            _context.Users.Add(user);
//            await _context.SaveChangesAsync();

//            return "User registered successfully";
//        }

//        public async Task<string> Login(LoginDto loginDto)
//        {
//            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == loginDto.Username);
//            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
//                throw new Exception("Invalid credentials");

//            _logger.LogInformation("Generating token for userId: {UserId}", user.Id);

//            var token = GenerateJwtToken(user);
//            return token;
//        }

//        private string GenerateJwtToken(User user)
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);

//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
//            };

//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(claims),
//                Expires = DateTime.UtcNow.AddHours(1),
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
//                Issuer = _configuration["JwtSettings:Issuer"],
//                Audience = _configuration["JwtSettings:Audience"]
//            };

//            var token = tokenHandler.CreateToken(tokenDescriptor);
//            var tokenString = tokenHandler.WriteToken(token);

//            _logger.LogInformation("Generated token: {Token} for userId: {UserId}", tokenString, user.Id);

//            return tokenString;
//        }
//    }
//}

namespace contactsapi.Services
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using contactsapi.Data;
    using contactsapi.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ContactsContext _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IConfiguration configuration, ContactsContext context, ILogger<AuthService> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        public async Task<string> Register(RegisterDto registerDto)
        {
            _logger.LogInformation("Starting registration for user: {Username}", registerDto.Username);

            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
            {
                _logger.LogWarning("Registration failed - User already exists: {Username}", registerDto.Username);
                throw new Exception("User already exists");
            }

            var user = new User
            {
                Username = registerDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User registered successfully: {Username}, UserID: {UserId}", user.Username, user.Id);
            return "User registered successfully";
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            _logger.LogInformation("Login attempt for user: {Username}", loginDto.Username);

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == loginDto.Username);
            if (user == null)
            {
                _logger.LogWarning("Login failed - User not found: {Username}", loginDto.Username);
                throw new Exception("Invalid credentials");
            }

            bool passwordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            if (!passwordValid)
            {
                _logger.LogWarning("Login failed - Invalid password for user: {Username}", loginDto.Username);
                throw new Exception("Invalid credentials");
            }

            _logger.LogInformation("Login successful - Generating token for userId: {UserId}", user.Id);
            var token = GenerateJwtToken(user);

            _logger.LogInformation("Token generated successfully for userId: {UserId}", user.Id);
            return token;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            _logger.LogInformation("Claims set for token generation: {Claims}", claims.Select(c => $"{c.Type}: {c.Value}"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"]
            };

            _logger.LogInformation("Token descriptor created with expiration: {Expiration} and issuer: {Issuer}", tokenDescriptor.Expires, tokenDescriptor.Issuer);

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            _logger.LogInformation("Generated token: {Token} for userId: {UserId}", tokenString, user.Id);

            return tokenString;
        }
    }
}