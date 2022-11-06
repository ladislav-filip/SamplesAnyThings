using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Sse.Infrastructure;

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
        var claims = new[]
        {
            new Claim(ClaimTypes.Sid, "p-1"),
            new Claim(ClaimTypes.Email, "user@mail.com"),
            new Claim(ClaimTypes.NameIdentifier, "USER-ID-1")
        };
        var result = _jwtAuthManager.GenerateTokens("pilif", claims, DateTime.Now);
        return Ok(result);
    }
}