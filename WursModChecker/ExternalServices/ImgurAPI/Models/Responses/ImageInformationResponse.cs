using System;
using System.Text.Json.Serialization;

namespace WursModChecker.ExternalServices.ImgurAPI.Models.Responses
{
    public class ImageInformationResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("datetime")]
        public int DateTime { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("animated")]
        public bool Animated { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("favorite")]
        public bool Favorite { get; set; }

        [JsonPropertyName("account_url")]
        public string AccountUrl { get; set; } = string.Empty;

        [JsonPropertyName("account_id")]
        public int AccountId { get; set; }

        [JsonPropertyName("is_ad")]
        public bool IsAd { get; set; }

        [JsonPropertyName("has_sound")]
        public bool HasSound { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; } = string.Empty;
    }
}
