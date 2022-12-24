using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WursModChecker.Utilities.Styles;
using WursModChecker.Windows;

namespace WursModChecker
{
    public partial class MainWindow : MetroWindow
    {
        private static readonly System.Windows.Threading.DispatcherTimer timer = new();

        public MainWindow()
        {
            InitializeComponent();
            LoadImgurUserImages();
            SetAutomatedImageScrollingTimer();
            ThemeSelector.SetDefaultTheme(this);
            WindowTransitionsEnabled = true;
        }

        private void OpenModUpdaterWindow(object sender, RoutedEventArgs e)
        {
            App.AppHost!.Services.GetRequiredService<ModUpdaterWindow>().Show();
        }

        private void OpenImageUploadWindow(object sender, RoutedEventArgs e)
        {
            App.AppHost!.Services.GetRequiredService<UploadImageWindow>().Show();
        }

        public void LoadImgurUserImages()
        {
            if (App.ImagesAsStream.Any())
            {
                try
                {
                    var ImageDataArray = App.ImagesAsStream.ToArray();
                    var flipViewItems = FlipView1st.Items;
                    for (int i = 0; i < ImageDataArray.Length; i++)
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = ImageDataArray[i].Value;
                        bitmap.EndInit();
                        var currentNestedGrid = (Grid)flipViewItems[i];
                        var imageControl = currentNestedGrid.Children.OfType<Image>().FirstOrDefault()!;
                        imageControl.Source = bitmap;
                        imageControl.Stretch = Stretch.UniformToFill;
                        imageControl.StretchDirection = StretchDirection.Both;
                    }
                    FlipView1st.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Problema convirtiendo imageStreams a bitmap images, errorMessage: {ex.Message}");
                }

            }
        }

        private void ImageFlipView_Scroll(object sender)
        {
            var flipView = ((FlipView)sender);
            flipView!.SelectedIndex = flipView!.SelectedIndex == 8 ? flipView!.SelectedIndex = 0 : flipView!.SelectedIndex + 1;
        }

        private void SetAutomatedImageScrollingTimer()
        {
            timer.Interval = TimeSpan.FromSeconds(8);
            timer.Tick += (s, e) => { ImageFlipView_Scroll(FlipView1st); };
            timer.Start();
        }

        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (App.ImagesAsStream.Any())
            {
                var flipview = ((FlipView)sender);
                flipview.BannerText = App.ImagesAsStream.ElementAtOrDefault(flipview.SelectedIndex).Key.Description;
            }
        }
    }
}
