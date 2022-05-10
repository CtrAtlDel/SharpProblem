using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lsn2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Lsn2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : Controller
{
    [HttpPost, Route("login")]
    public IActionResult Login(LoginDTO loginDTO)
    {
        try
        {
            if (string.IsNullOrEmpty(loginDTO.UserName) ||
                string.IsNullOrEmpty(loginDTO.Password))
                return BadRequest("Username and/or Password not specified");
            if (loginDTO.UserName.Equals("joydip") &&
                loginDTO.Password.Equals("joydip123"))
            {
                var secretKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes("thisisasecretkey@123"));
                var signinCredentials = new SigningCredentials
                    (secretKey, SecurityAlgorithms.HmacSha256);
                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: "ABCXYZ",
                    audience: "http://localhost:51398",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: signinCredentials
                );
                Ok(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
            }
        }
        catch
        {
            return BadRequest
                ("An error occurred in generating the token");
        }

        return Unauthorized();
    }
}