using IdentityExampleToken.Common;
using IdentityExampleToken.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;


namespace IdentityExampleToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenEndPoint : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public TokenEndPoint(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Login([FromForm] IFormCollection model)
        {
            string userNameEmail = model["userNameEmail"];
            string password = model["password"];
            var result = await _signInManager.PasswordSignInAsync(userNameEmail, password, true, false);
            if (result.Succeeded)
            {
                string key = _configuration["TokenSettings:JWTTokenSecretKey"];
                var issuer = _configuration["TokenSettings:Issuer"];
                var audience = _configuration["TokenSettings:Issuer"];
                var user = await _userManager.FindByNameAsync(userNameEmail);
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = await TokenClaims.GetValidClaims(user, _userManager);
                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.Now.AddSeconds(int.Parse(_configuration["TokenSettings:expires_in"])),
                    signingCredentials: credentials
                    );

                var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
                Token obj = new Token();
                obj.access_token = jwt_token;
                obj.expires_in = int.Parse(_configuration["TokenSettings:expires_in"]);
                obj.token_type = _configuration["TokenSettings:token_type"];
                obj.scope = _configuration["TokenSettings:scope"];
                return Ok(obj);
            }
            else
            {
                return Unauthorized();
            }
        }
        [HttpGet]
        [Route("GetToken")]
        [Consumes("application/json")]
        //[Authorize(Roles = "admin,members")]
        [Authorize]
        public IActionResult GetToken()
        {
            IdentityUser user = _userManager.FindByNameAsync(this.User.Identity.Name).Result;
            return Ok();
        }

    }
}
