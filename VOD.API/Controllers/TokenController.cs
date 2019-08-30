using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VOD.API.Services;
using VOD.Common.DTOModels;

namespace VOD.API.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        [HttpPost]
        public async Task<ActionResult<TokenDTO>> GenerateTokenAsync(LoginUserDTO loginUserDto)
        {
            try
            {
                var jwt = await _tokenService.GenerateTokenAsync(loginUserDto);
                if (jwt.Token == null) return Unauthorized();
                return jwt;
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<TokenDTO>> GetTokenAsync(string userId, LoginUserDTO loginUserDto)
        {
            try
            {
                var jwt = await _tokenService.GetTokenAsync(loginUserDto, userId);
                if (jwt.Token == null) return Unauthorized();
                return jwt;
            }
            catch
            {
                return Unauthorized();
            }
        }
    }
}