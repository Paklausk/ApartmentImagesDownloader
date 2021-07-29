using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentImagesDownloader.Objects
{
    public class ImagesDownloader
    {
        public bool Download(string imageUrl, string downloadPath, string imageFileName)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(imageUrl, Path.Combine(downloadPath, imageFileName));
            }
            return true;
        }
    }
}
