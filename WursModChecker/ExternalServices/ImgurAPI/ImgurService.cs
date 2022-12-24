using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WursModChecker.ExternalServices.ImgurAPI.Contracts;
using WursModChecker.ExternalServices.ImgurAPI.Models.Requests;
using WursModChecker.ExternalServices.ImgurAPI.Models.Responses;

namespace WursModChecker.ExternalServices.ImgurAPI
{
    public class ImgurService : IImgurService
    {
        private readonly HttpClient _httpClient;

        public ImgurService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IDictionary<ImageInformationResponse, Stream>> GetImagesAsStream(int lastCountImages)
        {
            var apiResponse = await _httpClient.GetAsync("/3/account/me/images");

            Dictionary<ImageInformationResponse, Stream> accountImageData = new();

            if (apiResponse.IsSuccessStatusCode)
            {
                AccountImagesResponse accountImages = await apiResponse.Content.ReadFromJsonAsync<AccountImagesResponse>();

                if (accountImages != null)
                    if (accountImages.ImagesInformation.Any())
                        using (var httpClient = new HttpClient())
                            for (int i = 0; i < accountImages.ImagesInformation.Take(lastCountImages).Count(); i++)
                            {
                                var downloadedImage = await httpClient.GetAsync(accountImages.ImagesInformation[i].Link);
                                accountImageData.Add(accountImages.ImagesInformation[i], await downloadedImage.Content.ReadAsStreamAsync());
                            }
            }

            return accountImageData;
        }

        public async Task<bool> UploadMedia(UploadMediaRequest uploadMediaRequest)
        {
            return (await _httpClient.PostAsJsonAsync("/3/image", uploadMediaRequest)).IsSuccessStatusCode;
        }
    }
}
