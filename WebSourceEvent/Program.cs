using WebSourceEvent.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

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

app.UseAuthorization();

app.MapRazorPages();

app.MapGet("/sse", async (ctx) =>
{
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