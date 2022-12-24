using FluentFTP;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WursModChecker.ExternalServices.Contracts;
using WursModChecker.Models;
using WursModChecker.Resources;
using WursModChecker.Resources.Enums;
using WursModChecker.Utilities.MessageProviders;

namespace WursModChecker.ExternalServices
{
    public class WursFtpClient : IWursFtpClient
    {
        private TransactionStatus _transactionStatus;
        private readonly IFtpClient _ftpClient;
        private readonly IMessageProvider _msgProvider;

        public WursFtpClient(IFtpClient ftpClient, IMessageProvider messageProvider)
        {
            _transactionStatus = App.AppHost!.Services.GetRequiredService<TransactionStatus>();
            ftpClient.Host = FtpConnectionData.HOST;
            ftpClient.Credentials = new NetworkCredential(FtpConnectionData.USERNAME, FtpConnectionData.PASSWORD);
            _ftpClient = ftpClient;
            _msgProvider = messageProvider;
        }
        public async Task<IList<FtpResult>> DownloadFromServer(string localTargetPath, ResourceType type, FtpDownloadOptions ftpOptions)
        {
            _msgProvider.InformMessage("Connecting");
            await _ftpClient.ConnectAsync();
            _msgProvider.InformMessage("SuccessCon");
            var progress = CreateFtpProgressModel(type);
            _msgProvider.InformMessage("CheckinSource", type);
            var ftpResultList = await _ftpClient.DownloadDirectoryAsync(localTargetPath,
                SetSourcePath(type), ftpOptions.FolderSyncMode, ftpOptions.LocalExists,
                ftpOptions.Verify, null, progress);
            await _ftpClient.DisconnectAsync();

            return ftpResultList;
        }

        public async Task<byte[]> DownloadFileFromServerAsByteArray(ResourceType type)
        {
            await _ftpClient.ConnectAsync();
            return await _ftpClient.DownloadBytesAsync(SetSourcePath(type));
        }


        private Progress<FtpProgress> CreateFtpProgressModel(ResourceType type)
        {
            return new(ftpProgress =>
            {
                if (ftpProgress.Progress < 0)
                {
                    _transactionStatus.IsProgressBarIndeterminate = true;
                }
                else
                {
                    _transactionStatus.IsProgressBarIndeterminate = false;
                    _transactionStatus.ProgressBarPercentage = ftpProgress.Progress;
                }
                string downloadingStr = "Descargando: ";
                var dynamicPathStr = type == ResourceType.Mods ?
                downloadingStr + ftpProgress.RemotePath.Replace("/mods/", "") :
                downloadingStr + ftpProgress.RemotePath.Replace("/INSTRUCCIONES_PARA_UNIRSE/", "");
                _transactionStatus.StatusMessage = dynamicPathStr;
            });
        }

        private static string SetSourcePath(ResourceType resourceType)
        {
            return resourceType switch
            {
                ResourceType.Mods => $@"/{FtpConnectionData.MOD_FOLDER_NAME}",
                ResourceType.Instructions => $@"/{FtpConnectionData.INSTRUCTIONS_FOLDER_NAME}",
                ResourceType.News => $@"/{FtpConnectionData.NEWS_FILE_NAME}",
                _ => $@"/{FtpConnectionData.MOD_FOLDER_NAME}",
            };
        }
    }
}
