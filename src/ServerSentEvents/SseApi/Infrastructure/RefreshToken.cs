using System.Text.Json.Serialization;

namespace SseApi.Infrastructure;

public class RefreshToken
{
    [JsonPropertyName("username")]
    public string UserName { get; set; }    // can be used for usage tracking
    // can optionally include other metadata, such as user agent, ip address, device name, and so on

    [JsonPropertyName("tokenString")]
    public string TokenString { get; set; }

    [JsonPropertyName("expireAt")]
    public DateTime ExpireAt { get; set; }
        
    [JsonPropertyName("accessLevel")] 
    public string[] AccessLevel { get; set; } = { "ServiceSheets", "ServiceSheets.Repairs", "ServiceSheets.Repairs.Insert", "ServiceSheets.Repairs.Edit" };
}