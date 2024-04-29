using Microsoft.IdentityModel.Tokens;
using OurBeautyReferralNetwork.Controllers;
using OurBeautyReferralNetwork.Models;
using OurBeautyReferralNetwork.Repositories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OurBeautyReferralNetwork.Utilities
{
    public class JWTUtilities
    {
        private readonly IConfiguration _configuration;
        private readonly UserRoleRepo _userRoleRepo;

        public JWTUtilities(IConfiguration configuration,
                            UserRoleRepo userRoleRepo)
        {
            _configuration = configuration;
            _userRoleRepo = userRoleRepo;
        }

        public static string GenerateRandomKey(int keyLength)
        {
            byte[] keyBytes = new byte[keyLength / 8];
            RandomNumberGenerator.Fill(keyBytes);
            return Convert.ToBase64String(keyBytes);
        }

        public async Task<string> GenerateJwtToken(string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };

            var roles = await _userRoleRepo.GetUserRolesAsync(email);
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}