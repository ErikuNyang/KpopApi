using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KpopApi.Dtos;
using KpopApi;
using Microsoft.AspNetCore.Identity;
using KpopApi.Models;
using System.IdentityModel.Tokens.Jwt;

namespace WorldCitiesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<KpopUser> _userManager;
    private readonly JwtHandler _jwtHandler;

    public AccountController(UserManager<KpopUser> userManager, JwtHandler jwtHandler)
    {
        _userManager = userManager;
        _jwtHandler = jwtHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        KpopUser? user = await _userManager.FindByNameAsync(loginRequest.UserName);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
        {
            return Unauthorized(new LoginResponse
            {
                Success = false,
                Message = "Invalid Username or Password."
            });
        }

        JwtSecurityToken secToken = await _jwtHandler.GetTokenAsync(user);
        string? jwt = new JwtSecurityTokenHandler().WriteToken(secToken);
        return Ok(new LoginResponse
        {
            Success = true,
            Message = "Login successful",
            Token = jwt
        });
    }
}