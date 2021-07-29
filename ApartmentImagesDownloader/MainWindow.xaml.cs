using ApartmentImagesDownloader.ImageUrlsExtractor;
using ApartmentImagesDownloader.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ApartmentImagesDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GetImagesButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isUrlValid(UrlBox.Text))
            {
                ShowErrorBox("Provided url is invalid. Only HTTP and HTTPS urls are supported.");
                return;
            }
            ImageUrlsExtractorProvider imagesExtractorProvider = new ImageUrlsExtractorProvider();
            IImageUrlsExtractor urlsExtractor = imagesExtractorProvider.GetByUrl(UrlBox.Text);
            if (urlsExtractor == null)
            {
                ShowErrorBox("Extracting images from this url is not supported.");
                return;
            }
            List<string> imageUrls = urlsExtractor.GetImageUrls(UrlBox.Text);
            if (imageUrls.Count > 0)
            {
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string localImagesPath = dialog.SelectedPath + "\\Apartment_images_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
                    Directory.CreateDirectory(localImagesPath);
                    ImagesDownloader downloader = new ImagesDownloader();
                    for (int i = 0; i < imageUrls.Count; i++)
                        downloader.Download(imageUrls[i], localImagesPath, $"{i}.jpg");
                }
            }
            MessageBox.Show(imageUrls.Count + " images downloaded");
        }

        bool isUrlValid(string url)
        {
            bool canCreateUri = Uri.TryCreate(url, UriKind.Absolute, out Uri uri);
            if (canCreateUri && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                return true;
            return false;
        }

        void ShowErrorBox(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
