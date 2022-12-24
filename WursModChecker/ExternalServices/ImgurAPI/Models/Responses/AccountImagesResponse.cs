using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WursModChecker.ExternalServices.ImgurAPI.Models.Responses
{
    public class AccountImagesResponse
    {
        [JsonPropertyName("data")]
        public IList<ImageInformationResponse> ImagesInformation { get; set; }
    }
}
