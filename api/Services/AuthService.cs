using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Mappers;
using api.Models;
using Isopoh.Cryptography.Argon2;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class AuthService(IConfiguration configuration, ContactService contactService)
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ContactService _contactService = contactService;

        public async Task<(string Token, ContactDTO User)> Login(LoginDTO loginDTO)
        {
            var user = await _contactService.GetContact(loginDTO.Email) ?? throw new KeyNotFoundException();

            if (!Argon2.Verify(user.Password, loginDTO.Password))
            {
                throw new UnauthorizedAccessException();
            }

            return (GenerateToken(user), user.ToContactDto());
        }

        public async Task<(string Token, ContactDTO User)> Register(RegisterDTO registerDTO)
        {
            var user = await _contactService.CreateUser(registerDTO);
            return (GenerateToken(user), user.ToContactDto());
        }

        private string GenerateToken(Contact user)
        {
            var bearer = _configuration.GetSection("Authentication:Bearer");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(bearer["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = bearer["Issuer"],
                Audience = bearer["Audience"],
                Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                ]),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}