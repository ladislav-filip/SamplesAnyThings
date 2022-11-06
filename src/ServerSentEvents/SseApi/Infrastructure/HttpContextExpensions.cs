namespace sseApi.Infrastructure;

public static class HttpContextExpensions
{
    public static async Task SourceEventInitAsync(this HttpContext ctx)
    {
        ctx.Response.Headers.Add("Cache-Control", "no-cache");
        ctx.Response.Headers.Add("Content-Type", "text/event-stream");
        await ctx.Response.Body.FlushAsync();
    }
    
    public static async Task SourceEventSendDataAsync(this HttpContext ctx, int id, string eventName, string data)
    {
        await ctx.Response.WriteAsync("id: " + id + "\n");
        await ctx.Response.WriteAsync("event: " + eventName + "\n");
        foreach(var line in data.Split('\n'))
            await ctx.Response.WriteAsync("data: " + line + "\n");
        
        await ctx.Response.WriteAsync("\n");
        await ctx.Response.Body.FlushAsync();
    }
    
    public static string GenerateName(int len)
    { 
        var r = new Random();
        string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
        string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
        string Name;
        Name = "";
        Name += consonants[r.Next(consonants.Length)].ToUpper();
        Name += vowels[r.Next(vowels.Length)];
        int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
        while (b < len)
        {
            Name += consonants[r.Next(consonants.Length)];
            b++;
            Name += vowels[r.Next(vowels.Length)];
            b++;
        }

        return Name;
    }
}