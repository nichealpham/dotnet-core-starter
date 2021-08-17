using Microsoft.Extensions.Configuration;
using AppGlobal.Extensions;
using System;
using AppGlobal.Models;
using System.Collections.Generic;
using System.Linq;
using AppGlobal.Entities;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace AppGlobal.Services
{
    public class AccessTokenService
    {
        private readonly string _EncriptionKey;

        public AccessTokenService(IConfiguration Configuration)
        {
            _EncriptionKey = Configuration["AccessTokenEncriptionKey"];
        }

        public string GenerateJwtToken(AuthEntity auth)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_EncriptionKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { 
                    new Claim("FullName", auth.FullName),
                    new Claim("UserID", auth.UserID),
                    new Claim("Email", auth.Email),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public AuthEntity ParseJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_EncriptionKey);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var authUser = new AuthEntity()
            {
                FullName = jwtToken.Claims.First(x => x.Type == "FullName").Value,
                UserID = jwtToken.Claims.First(x => x.Type == "UserID").Value,
                Email = jwtToken.Claims.First(x => x.Type == "Email").Value,
            };

            return authUser;
        }
    }
}
