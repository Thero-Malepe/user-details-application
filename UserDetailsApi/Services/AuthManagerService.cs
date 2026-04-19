using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserDetailsApi.Data;
using UserDetailsApi.DTOs.AuthDtos;
using UserDetailsApi.Interfaces;
using UserDetailsApi.Models;

namespace UserDetailsApi.Services
{
    public class AuthManagerService(UserDetailsDbContext context, IConfiguration configuration, ILogger<AuthManagerService> logger) : IAuthManagerService
    {
        public async Task<User?> Register (UserDto request)
        {
            logger.LogInformation("Registering user with email: {email}", request.Email);

            if (await context.Users.AnyAsync(x => x.Email.ToLower().Trim() == request.Email.ToLower().Trim()))
            {
                logger.LogError("User with email: {email} already exists", request.Email);
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

            logger.LogInformation("User email: {email} registered successfully", request.Email);
            return user;
        }

        public async Task<TokenResponseDto?> Login(LoginDto request)
        {
            logger.LogInformation("Logging in user");

            var user = await context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Trim() == request.Email.ToLower().Trim());
            if (user is null)
            {
                logger.LogError("User with email: {email} not found", request.Email);
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash.Trim(), request.Password.Trim())
                            == PasswordVerificationResult.Failed)
            {
                logger.LogError("Incorrect Password");
                return null;
            }

            TokenResponseDto tokens = await CreateTokenResponse(user);

            logger.LogInformation("User with email: {email} logged in Successfully", request.Email);
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
            logger.LogInformation("Refreshing user token");

            var user = await ValidateRefreshToken(request.RefreshToken);
            if (user is null)
            {
                logger.LogError("Token invalid");
                return null;
            }

            logger.LogInformation("Token refreshed successfully");
            return await CreateTokenResponse(user);
        }

        private async Task<User?> ValidateRefreshToken(string refreshToken)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken.Trim());
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
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
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
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Token"]!));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var randNum = RandomNumberGenerator.Create();
            randNum.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
        
        public async Task<bool> PasswordResetEmail(string email)
        {
            logger.LogInformation("Resetting password for user with email: {email}", email);
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Trim() == email.ToLower().Trim());

            if (user is null)
            {
                logger.LogError("User with email: {email} not found", email);
                return false;
            }

            var token = GenerateRefreshToken();
            var reset = new PasswordResetToken
            {
                UserId = user.Id,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                Used = false
            };

            context.PasswordResetTokens.Add(reset);
            await context.SaveChangesAsync();

            var safeToken = WebUtility.UrlEncode(token);
            var resetLink = $"http://localhost:4200/reset-password?email={email}&token={safeToken}";

            var message = new MailMessage
            {
                From = new MailAddress(configuration["SMTP:EmailFrom"]!),
                Subject = "RESET PASSWORD",
                Body = $"<p>Hello {user.FirstName} {user.LastName},</p><br/><p>Click <a href='{resetLink}'> here <a/> to reset your password <br/><br/> Link is valid for 15 minutes. <br/><br/> Kind regards,<br/> Admin </p>",
                IsBodyHtml = true
            };

            message.To.Add(email);

            var smtp = new SmtpClient("localhost", 25)   // FakeSMTP or smtp4dev
            {
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            await smtp.SendMailAsync(message);
            return true;
        }

        public async Task<bool> ResetPassword(ResetDto details)
        {
            var resetToken = await ValidateResetToken(details.Token);
            if (resetToken is null)
            {
                return false;
            }

            var user = await context.Users.FindAsync(resetToken.UserId);

            var hashed = new PasswordHasher<User>().HashPassword(user, details.Password);
            user.PasswordHash = hashed;
            resetToken.Used = true;
            await context.SaveChangesAsync();

            logger.LogInformation("User email: {email} password changed successfully", details.Email);
            return true;
        }

        public async Task<PasswordResetToken?> ValidateResetToken(string token)
        {
            logger.LogInformation("Validating token for user");
            var reset = await context.PasswordResetTokens.FirstOrDefaultAsync(x => x.Token.Trim() == token.Trim() && !x.Used);

            if (reset is null || reset.ExpiresAt < DateTime.UtcNow)
            {
                logger.LogError("Token Invalid");
                return null;
            }

            return reset;
        }

        public async Task<bool?> DeleteUser(string userId)
        {
            if (Guid.TryParse(userId, out Guid id))
            {
                var userAffected = await context.Users.Where(x => x.Id == id).ExecuteDeleteAsync();
                var tasksAffected = await context.Tasks.Where(x => x.UserId == userId).ExecuteDeleteAsync();
                if (userAffected > 0)
                {
                    logger.LogInformation("User with ID: {id} deleted", id);
                    logger.LogInformation("Tasks for user with ID: {id} deleted", id);
                    return true;
                }

                logger.LogInformation("User with ID: {id} not found", id);
                return false;
            }

            return null;
        }
    }
}
