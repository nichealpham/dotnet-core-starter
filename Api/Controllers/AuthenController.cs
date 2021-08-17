using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Business;
using AppGlobal.Entities;
using AppGlobal.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenController : Controller
    {
        private readonly AuthenBusiness _AuthenService;

        public AuthenController(AuthenBusiness AuthenService)
        {
            _AuthenService = AuthenService;
        }

        [HttpPost("Login")]
        public LoginResult Login([FromBody]LoginEntity data)
        {
            return _AuthenService.Login(data);
        }

        [HttpGet("AuthData")]
        [Authorize]
        public AuthEntity GetAuthData()
        {
            var token = (string)HttpContext.Items["Token"];
            return _AuthenService.GetAuthData(token);
        }
    }
}
