using FluentFTP;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WursModChecker.Core;
using WursModChecker.Core.Contracts;
using WursModChecker.ExternalServices;
using WursModChecker.ExternalServices.Contracts;
using WursModChecker.ExternalServices.ImgurAPI;
using WursModChecker.ExternalServices.ImgurAPI.Contracts;
using WursModChecker.ExternalServices.ImgurAPI.Models.Responses;
using WursModChecker.Helpers;
using WursModChecker.Models;
using WursModChecker.Resources.Enums;
using WursModChecker.Utilities;
using WursModChecker.Utilities.MessageProviders;
using WursModChecker.Windows;

namespace WursModChecker
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public static List<string> ServerNews { get; private set; } = new List<string>();
        public static bool ServerNewsOk { get; set; } = false;
        public static IDictionary<ImageInformationResponse, Stream> ImagesAsStream { get; set; }
            = new Dictionary<ImageInformationResponse, Stream>();

        public App()
        {

        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
#if DEBUG
                ConfigureDependencies("957ce6457118d7baac7f27afcf7e1d0dea7a0ed2");
#else
                ConfigureDependencies((await GetImgurAccessToken()).AccessToken);         
#endif

                await AppHost!.StartAsync();
                /*Se comenta el proceso de Server News hasta que se decida si eliminarlo por completo
                /ya que quizás se pueda reutilizar toda la lógica de procesamiento para una función futura.
                await LoadServerNews();
                */
#if DEBUG
#else
                await LoadImgurImages();
#endif
                base.OnStartup(e);
                var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
                startupForm.Show();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Problema en startup, errorMessage : {ex.Message}");
            }

        }
        private static async Task<AccessTokenResponse> GetImgurAccessToken()
        {
            var accessToken = await ImgurAPIConfigurationBuilder.GetHttpClientData();
            if (string.IsNullOrEmpty(accessToken.AccessToken))
                Console.WriteLine("Hubo un problema recuperando la información de acceso a la Imgur API");

            return accessToken;
        }
        private static void ConfigureDependencies(string imgurPersonalToken)
        {
            AppHost = Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<MainWindow>();
                services.AddTransient<ModUpdaterWindow>();
                services.AddTransient<UploadImageWindow>();
                services.AddTransient<ILocalFileProcessor, LocalFileProcessor>();
                services.AddTransient<IWursFtpClient, WursFtpClient>();
                services.AddTransient<IFtpClient, FtpClient>();
                services.AddSingleton<TransactionStatus>();
                services.AddTransient<IMessageProvider, SmartInformer>();
                services.AddHttpClient<IImgurService, ImgurService>(client =>
                {
                    client.BaseAddress = new Uri("https://api.imgur.com");
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", imgurPersonalToken);
                });
            }).Build();
        }
        private static async Task LoadServerNews()
        {
            var ftpClient = AppHost!.Services.GetRequiredService<IWursFtpClient>();

            try
            {
                ServerNews = DataHelper.SplitStringToListBySeparator(
                    DataHelper.ConvertByteArrayToString(
                        await ftpClient.DownloadFileFromServerAsByteArray(ResourceType.News)), '.').ToList();
                ServerNewsOk = true;
            }
            catch (System.Exception)
            {
                ServerNewsOk = false;
            }
        }

        private static async Task<IDictionary<ImageInformationResponse, Stream>> LoadImgurImages()
        {
            var imgurClient = AppHost!.Services.GetRequiredService<IImgurService>();

            return ImagesAsStream = await imgurClient.GetImagesAsStream(9);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}
