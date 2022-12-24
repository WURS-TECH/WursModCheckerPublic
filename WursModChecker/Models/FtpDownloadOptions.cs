using FluentFTP;

namespace WursModChecker.Models
{
    public class FtpDownloadOptions
    {
        public FtpFolderSyncMode FolderSyncMode { get; set; } = FtpFolderSyncMode.Mirror;

        public FtpLocalExists LocalExists { get; set; } = FtpLocalExists.Skip;

        public FtpVerify Verify { get; set; } = FtpVerify.Retry;
    }
}
