using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentImagesDownloader.ImageUrlsExtractor
{
    public interface IImageUrlsExtractor
    {
        List<string> GetImageUrls(string url);
    }
}
