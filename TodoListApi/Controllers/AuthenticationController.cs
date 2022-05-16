using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TodoListApi.Data;
using TodoListApi.Models;

namespace TodoListApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
       private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        public AuthenticationController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] SignInModel signInModel)
        {
            Microsoft.AspNetCore.Identity.SignInResult signInResult =  await _signInManager.PasswordSignInAsync(signInModel.UserName, signInModel.Password,true,false);

            if(signInResult.Succeeded)
            {
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"])
                    );
                SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                ApplicationUser user = await _userManager.FindByNameAsync(signInModel.UserName);

                if (user == null) { return NotFound("User Not Found"); }

                JwtSecurityToken token = GenerateToken(credentials, user);
                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            return Ok(signInResult.Succeeded);
            
        }

        private JwtSecurityToken GenerateToken(SigningCredentials credentials, ApplicationUser user)
        {
            List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Role,nameof(ApplicationUserRoles.User)),
                    new Claim(ClaimTypes.NameIdentifier,user.Id),
                    new Claim(ClaimTypes.Email,user.Email),
                };

            var token = new JwtSecurityToken(
                _configuration["Jwt:ValidIssuer"],
                _configuration["Jwt:ValidAudience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
                );
            return token;
        }
    }
}
