using System.IO;
using System.Text.Json.Serialization;

namespace WursModChecker.ExternalServices.ImgurAPI.Models.Requests
{
    public class UploadMediaRequest
    {
        [JsonPropertyName("image")]
        public string Image { get; set; } = string.Empty;

        [JsonPropertyName("video")]
        public string Video { get; set; } = string.Empty;

        [JsonPropertyName("album")]
        public string AlbumId { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("disable_audio")]
        public bool DisableAudio { get; set; }
    }
}
