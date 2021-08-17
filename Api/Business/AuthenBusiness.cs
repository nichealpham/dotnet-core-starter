using AppGlobal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppGlobal.Entities;
using AppGlobal.Commons;
using AppGlobal.Models;

namespace Api.Business
{
    public class AuthenBusiness
    {
        private AccessTokenService _TokenService { get; set; }
        public AuthenBusiness(AccessTokenService accessTokenService)
        {
            _TokenService = accessTokenService;
        }

        public LoginResult Login(LoginEntity data)
        {
            var user = new AuthEntity()
            {
                FullName = "Full Name",
                UserID = "abc-123",
                Email = data.Email,
            };

            if (user == null)
            {
                throw new ApiError((int)ErrorCodes.CredentialsInvalid);
            }

            var token = _TokenService.GenerateJwtToken(user);

            return new LoginResult()
            {
                Token = token,
                User = user,
            };
        }

        public AuthEntity GetAuthData(string token)
        {
            return _TokenService.ParseJwtToken(token);
        }
    }
}
