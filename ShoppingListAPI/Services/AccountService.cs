using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShoppingListAPI.Entities;
using ShoppingListAPI.Exceptions;
using ShoppingListAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ShoppingListAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        string GenerateJwt(LoginDto dto);
    }
    public class AccountService : IAccountService
    {
        private readonly ShoppingListDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(ShoppingListDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }
        public void RegisterUser(RegisterUserDto dto)
        {
            var user = new User
            {
                Login = dto.Login,
                RoleId = dto.RoleId,
                UserData = new UserData
                {
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    PhoneNumber = dto.PhoneNumber,
                }
            };

            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.PasswordHash = hashedPassword;

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }
        public string GenerateJwt(LoginDto dto)
        {
            var user = _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.UserData)
                .FirstOrDefault(u => u.Login == dto.Login);

            if (user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("Email", user.UserData.Email),
                new Claim(ClaimTypes.Name, $"{user.UserData.FirstName} {user.UserData.LastName}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, 
                _authenticationSettings.JwtIssuer, 
                claims, 
                expires: expires, 
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
