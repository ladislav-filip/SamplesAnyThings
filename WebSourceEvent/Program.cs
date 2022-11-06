using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using WebSourceEvent.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
const string secret = "fullcom-asdhlas-adcfhswj45646-sdagfvae";

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(options =>
{
    
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    
}).AddJwtBearer(options =>
{
    
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(secret)),
        ValidateLifetime = true,
        ValidateAudience = false,
        ValidateIssuer = false
    };
});

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

app.MapGet("/token", () => HttpContextExpensions.CreateToken(secret));

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