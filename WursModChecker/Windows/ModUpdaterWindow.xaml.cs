using FluentFTP;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WursModChecker.Core.Contracts;
using WursModChecker.ExternalServices.Contracts;
using WursModChecker.Models;
using WursModChecker.Resources.Enums;
using WursModChecker.Utilities.MessageProviders;
using WursModChecker.Utilities.Styles;

namespace WursModChecker.Windows
{
    public partial class ModUpdaterWindow : MetroWindow
    {
        private string _localModsFolderPath = string.Empty;
        private IDictionary<string, byte[]> _backup = new Dictionary<string, byte[]>();
        private readonly ILocalFileProcessor _fileProcessor;
        private readonly IWursFtpClient _ftpClient;
        private IMessageProvider _messageProvider;
        private static readonly System.Windows.Threading.DispatcherTimer timer = new();
        private int msgCounter;
        private static readonly List<string> messages = App.ServerNews;
        private static bool isNewsFirstTick = true;

        public ModUpdaterWindow(
            ILocalFileProcessor localFileProcessor,
            IWursFtpClient ftpClient,
            TransactionStatus transactionStatus,
            IMessageProvider messageProvider
            )
        {
            _fileProcessor = localFileProcessor;
            _ftpClient = ftpClient;
            DataContext = transactionStatus;
            _messageProvider = messageProvider;
            GetDefaultLocalModsFolder();
            if (App.ServerNewsOk)
            {
                InitializeNewsEvent();
            }
            InitializeComponent();
            ThemeSelector.SetDefaultTheme(this);
            _messageProvider.InformMessage("IOnlyWorkWithDefaultPath");
            WindowTransitionsEnabled = true;
        }

        private async void CheckForModUpdates(object sender, RoutedEventArgs e)
        {
            var modsType = ResourceType.Mods;
            try
            {
                _messageProvider.InformMessage("CreatingBackup", modsType);

                _backup = await _fileProcessor.CreateLocalModsInMemoryBackup(_localModsFolderPath);

                _messageProvider.InformMessage(_backup.Any() ?
                    "SuccessBackup" :
                    "BackupEmpty", modsType);

                var remoteModList = await _ftpClient.DownloadFromServer(_localModsFolderPath, ResourceType.Mods,
                    new FtpDownloadOptions { });

                _messageProvider.InformMessage(remoteModList.All(file => file.IsSkipped) ?
                    "HasLastVersion" :
                    "SucccessUpdated", modsType);
            }
            catch (Exception)
            {
                await RestoreFromBackup(_backup);
                _messageProvider.InformMessage("ProcessError", modsType);
            }
        }

        private async void DownloadHowToConnectInstructions(object sender, RoutedEventArgs e)
        {
            try
            {
                var instructions = await _ftpClient.DownloadFromServer(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                     ResourceType.Instructions, new FtpDownloadOptions { FolderSyncMode = FtpFolderSyncMode.Update });
                _messageProvider.InformMessage(instructions.All(file => file.IsSkipped) ?
                    "AlreadyHaveInstructions" : "SuccessDownload", ResourceType.Instructions);
            }
            catch (Exception)
            {
                _messageProvider.InformMessage("ProcessError", ResourceType.Instructions);
            }
        }

        private void GetDefaultLocalModsFolder()
        {
            _localModsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\.minecraft\mods";
        }


        private async Task RestoreFromBackup(IDictionary<string, byte[]> backup)
        {
            try
            {
                await _fileProcessor.RestoreFromBackup(backup, _localModsFolderPath);
            }
            catch (Exception)
            {
                _messageProvider.InformMessage("RestoreFromBackupError", ResourceType.Mods);
            }
        }

        private void TextBlock_Load(object sender, RoutedEventArgs e)
        {
            if (isNewsFirstTick == true)
            {
                timer.Interval = TimeSpan.FromSeconds(4);
            }
            if (msgCounter > messages.Count - 1)
            {
                msgCounter = 0;
                return;
            }

            var block = sender as TextBlock;
            block!.Text = messages[msgCounter];
            msgCounter++;
            isNewsFirstTick = false;
        }

        private void InitializeNewsEvent()
        {
            if (App.ServerNewsOk)
            {
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += (s, e) => { TextBlock_Load(MyFadingText, new RoutedEventArgs(LoadedEvent)); };
                timer.Start();
                msgCounter = 0;
            }
            else
            {
                MyFadingText.Text = "Hubo un error cargando las últimas noticias del server.";
            }
        }
    }
}
