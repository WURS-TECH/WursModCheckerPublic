using FluentFTP;
using System.Collections.Generic;
using System.Threading.Tasks;
using WursModChecker.Models;
using WursModChecker.Resources.Enums;

namespace WursModChecker.ExternalServices.Contracts
{
    public interface IWursFtpClient
    {
        Task<IList<FtpResult>> DownloadFromServer(string localModsFolder, ResourceType type, FtpDownloadOptions ftpOptions);
        Task<byte[]> DownloadFileFromServerAsByteArray(ResourceType type);
    }
}
