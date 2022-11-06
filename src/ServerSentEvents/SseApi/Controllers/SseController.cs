using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sseApi.Infrastructure;

namespace SseApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SseController : ControllerBase
{
    [HttpGet, Route("anonymous")]
    public async Task<IActionResult> GetAnonymous()
    {
        var id = 0;
        await HttpContext.SourceEventInitAsync();
        var rnd = new Random();

        var events = new[] { "message", "info" };
    
        while (true)
        {
            var evn = events[rnd.Next(2)];
            Thread.Sleep((int)rnd.NextInt64(5000));
            var tm = DateTime.Now.ToLongTimeString();
            await HttpContext.SourceEventSendDataAsync(id++, evn, tm + ": " + HttpContextExpensions.GenerateName(10));
        }
    }
    
    [Authorize]
    [HttpGet, Route("auth")]
    public async Task<IActionResult> GetAuth()
    {
        var email = User.Claims.First(p => p.Type == ClaimTypes.Email).Value;
        
        var id = 0;
        await HttpContext.SourceEventInitAsync();
        var rnd = new Random();

        var events = new[] { "message", "info" };
    
        while (true)
        {
            var evn = events[rnd.Next(2)];
            Thread.Sleep((int)rnd.NextInt64(3000));
            var tm = DateTime.Now.ToLongTimeString();
            var text = tm + ": " + HttpContextExpensions.GenerateName(15) + $" ( Authenticated = {User.Identity.IsAuthenticated}, Email = {email})";
            await HttpContext.SourceEventSendDataAsync(id++, evn, text);
        }
    }
}