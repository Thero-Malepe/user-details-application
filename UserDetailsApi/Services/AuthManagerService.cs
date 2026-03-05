using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserDetailsApi.Data;
using UserDetailsApi.DTOs;
using UserDetailsApi.Interfaces;
using UserDetailsApi.Models;

namespace UserDetailsApi.Services
{
    public class AuthManagerService(UserDetailsDbContext context, IConfiguration _configuration) : IAuthManagerService
    {
        public async Task<User?> Register (UserDto request)
        {
            if (await context.Users.AnyAsync(x => x.Email.ToLower().Trim() == request.Email.ToLower().Trim()))
            {
                return null;
            }

            var user = new User();
            var hashed = new PasswordHasher<User>().HashPassword(user, request.Password);

            user.Email = request.Email;
            user.PasswordHash = hashed;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<TokenResponseDto?> Login(LoginDto request)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Trim() == request.Email.ToLower().Trim());
            if (user is null)
            {
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash.Trim(), request.Password.Trim())
                            == PasswordVerificationResult.Failed)
            {
                return null;
            }

            TokenResponseDto tokens = await CreateTokenResponse(user);

            return tokens;
        }

        private async Task<TokenResponseDto> CreateTokenResponse(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = GenerateToken(user),
                RefreshToken = await SaveRefreshToken(user)
            };
        }

        public async Task<TokenResponseDto?> RefreshToken(RefreshTokenDto request)
        {
            var user = await ValidateRefreshToken(request.UserId, request.RefreshToken);
            if (user is null)
                return null;

            return await CreateTokenResponse(user);
        }

        private async Task<User?> ValidateRefreshToken(Guid userId, string refreshToken)
        {
            var user = await context.Users.FindAsync(userId);
            if(user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow) 
            {
                return null;
            }
            
            return user;
        }

        private async Task<string> SaveRefreshToken(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddHours(6);
            await context.SaveChangesAsync();
            return refreshToken;
        }

        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.FirstName),
                new(ClaimTypes.Surname, user.LastName)
            };

            //Sign-in key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Token"]!));

            //Alternate syntax
            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:JwtSettings")!));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var randNum = RandomNumberGenerator.Create();
            randNum.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        
    }
}
