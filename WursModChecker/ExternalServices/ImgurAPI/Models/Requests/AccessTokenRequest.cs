using System.Text.Json.Serialization;

namespace WursModChecker.ExternalServices.ImgurAPI.Models.Requests
{
    public class AccessTokenRequest
    {
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = "YOUR_DEFAULT_REFRESH_TOKEN";

        [JsonPropertyName("client_id")]
        public string ClientId { get; set; } = "YOUR_DEFAULT_CLIENT_ID";

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; } = "YOUR_DEFAULT_CLIENT_SECRET";

        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; } = "refresh_token";
    }
}
