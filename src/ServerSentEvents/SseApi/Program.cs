using Sse.Infrastructure;

const string CustomPolicyKey = "CustomPolicy";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCustomSwagger(builder.Configuration);

builder.Services.AddSingleton<JwtAuthManager>();
builder.Services.AddJwtInitializer(builder.Configuration);

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: CustomPolicyKey, policy =>
    {
        policy.WithOrigins("https://localhost:7208")
            .AllowAnyHeader()
            .AllowCredentials()
            ;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors(CustomPolicyKey);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();