using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Sse.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
const string secret = "fullcom-asdhlas-adcfhswj45646-sdagfvae";

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddCustomSwagger(builder.Configuration);
builder.Services.AddSingleton<JwtAuthManager>();
builder.Services.AddJwtInitializer(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapGet("/token", () =>
{
    var authManager = app.Services.GetRequiredService<JwtAuthManager>();
    var claims = new[]
    {
        new Claim(ClaimTypes.Sid, "p-1"),
        new Claim(ClaimTypes.Email, "user@mail.com"),
        new Claim(ClaimTypes.NameIdentifier, "USER-ID-1")
    };
    var result = authManager.GenerateTokens("pilif", claims, DateTime.Now);
    return result.AccessToken;
});

app.MapGet("/sse", [Authorize] async (ctx) =>
{
    Debug.WriteLine(ctx.User.Identity.Name);
    var id = 0;
    await ctx.SourceEventInitAsync();
    var rnd = new Random();

    var events = new[] { "message", "info" };
    
    while (true)
    {
        var evn = events[rnd.Next(2)];
        Thread.Sleep((int)rnd.NextInt64(5000));
        var tm = DateTime.Now.ToLongTimeString();
        await ctx.SourceEventSendDataAsync(id++, evn, tm + ": " + HttpContextExpensions.GenerateName(10));
    }
});

app.Run();