using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WursModChecker.ExternalServices.ImgurAPI.Contracts;
using WursModChecker.ExternalServices.ImgurAPI.Models.Requests;
using WursModChecker.Utilities.Styles;

namespace WursModChecker.Windows
{
    public partial class UploadImageWindow : MetroWindow
    {
        private readonly IImgurService _imgurService;
        private UploadMediaRequest _uploadMediaRequest = new();

        public UploadImageWindow(IImgurService imgurService)
        {
            _imgurService = imgurService;
            ThemeSelector.SetDefaultTheme(this);
            InitializeComponent();
            FlipView1st.BannerText = "";
        }

        private async void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new()
            {
                Filter = "Image Files (JPG,PNG,GIF) | *.JPG;*.PNG;*.GIF"
            };

            if (fileDialog.ShowDialog() == true)
            {
                var fileName = fileDialog.FileName;
                var fileBytes = await File.ReadAllBytesAsync(fileName);
                SetMediaPreview(new MemoryStream(fileBytes));
                BuildUploadMediaRequest(fileName, fileBytes);
            }
        }

        private void TextChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            _uploadMediaRequest.Description = descriptionTxtBox.Text;
            FlipView1st.BannerText = descriptionTxtBox.Text;
        }

        private async void BtnUploadImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var controller = await this.ShowProgressAsync("Porfavor espera...", "Estamos subiendo tu imagen");
                controller.SetProgress(0.2);
                if (await _imgurService.UploadMedia(_uploadMediaRequest))
                {
                    controller.SetProgress(0.8);
                    await RefreshMainWindowImgurImages();
                    controller.SetProgress(1);
                    await controller.CloseAsync();
                    await this.ShowMessageAsync("¡Subido con exito!", "Tu imagen fue subida con exito, se empezara a mostrar en el carrousel de la" +
        "pantalla principal");
                    Close();
                }
                else
                {
                    controller.SetProgress(0.8);
                    controller.SetProgress(1);
                    await controller.CloseAsync();
                    await this.ShowMessageAsync("!Error¡", "Hubo un error subiendo tu imagen, prueba más tarde.");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Problema subiendo imagen a Imgur, errorMessage: {ex.Message}");
            }


        }

        private void SetMediaPreview(MemoryStream stream)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.StreamSource = stream;
            bitmap.EndInit();
            var flipViewItems = FlipView1st.Items;
            var currentNestedGrid = (Grid)flipViewItems[0];
            var imageControl = currentNestedGrid.Children.OfType<Image>().FirstOrDefault()!;
            imageControl.Source = bitmap;
            imageControl.Stretch = Stretch.UniformToFill;
            imageControl.StretchDirection = StretchDirection.Both;
        }
        private async Task RefreshMainWindowImgurImages()
        {
            App.ImagesAsStream = await _imgurService.GetImagesAsStream(9);
            App.AppHost!.Services.GetRequiredService<MainWindow>().LoadImgurUserImages();
        }

        private void BuildUploadMediaRequest(string fileName, byte[] fileBytes)
        {
            var isVideo = Path.GetExtension(fileName).ToUpperInvariant().Equals(".MP4");
            _uploadMediaRequest.AlbumId = "UwZtPS2";
            _uploadMediaRequest.DisableAudio = false;


            if (isVideo)
            {
                _uploadMediaRequest.Video = Convert.ToBase64String(fileBytes);
            }
            else
            {
                _uploadMediaRequest.Image = Convert.ToBase64String(fileBytes);
            }
        }
    }
}
