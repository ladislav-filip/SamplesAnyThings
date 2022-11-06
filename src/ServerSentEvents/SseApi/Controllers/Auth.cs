using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SseApi.Infrastructure;

namespace SseApi.Controllers;

[ApiController]
[Route("[controller]")]
public class Auth : ControllerBase
{
    private readonly JwtAuthManager _jwtAuthManager;

    public Auth(JwtAuthManager jwtAuthManager)
    {
        _jwtAuthManager = jwtAuthManager;
    }
    
    [HttpGet, Route("token")]
    public async Task<IActionResult> GetToken()
    {
        await Task.CompletedTask;
        var claims = new[] { new Claim(JwtRegisteredClaimNames.NameId, "p-1") };
        var result = _jwtAuthManager.GenerateTokens("pilif", claims, DateTime.Now);
        return Ok(result);
    }
}