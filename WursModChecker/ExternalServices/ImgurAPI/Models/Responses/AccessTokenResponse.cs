using System.Text.Json.Serialization;

namespace WursModChecker.ExternalServices.ImgurAPI.Models.Responses
{
    public class AccessTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;

        [JsonPropertyName("scope")]
        public string Scope { get; set; } = string.Empty;

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonPropertyName("account_id")]
        public int AccountId { get; set; }

        [JsonPropertyName("account_username")]
        public string AccountUsername { get; set; } = string.Empty;
    }
}
