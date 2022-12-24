using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WursModChecker.ExternalServices.ImgurAPI.Models.Requests;
using WursModChecker.ExternalServices.ImgurAPI.Models.Responses;
using WursModChecker.Resources.ConnectionData;

namespace WursModChecker.Helpers
{
    public static class ImgurAPIConfigurationBuilder
    {
        public static async Task<AccessTokenResponse> GetHttpClientData()
        {
            using var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ImgurAPIData.ACCESS_TOKEN);

            var asyncResponse = await httpClient
                .PostAsJsonAsync("https://api.imgur.com/oauth2/token", new AccessTokenRequest());

            return await asyncResponse.Content.ReadFromJsonAsync<AccessTokenResponse>() ?? new AccessTokenResponse();
        }
    }
}
