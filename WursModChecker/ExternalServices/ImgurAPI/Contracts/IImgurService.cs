using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WursModChecker.ExternalServices.ImgurAPI.Models.Requests;
using WursModChecker.ExternalServices.ImgurAPI.Models.Responses;

namespace WursModChecker.ExternalServices.ImgurAPI.Contracts
{
    public interface IImgurService
    {
        Task<IDictionary<ImageInformationResponse, Stream>> GetImagesAsStream(int lastCountImages);

        Task<bool> UploadMedia(UploadMediaRequest uploadMediaRequest);
    }
}
