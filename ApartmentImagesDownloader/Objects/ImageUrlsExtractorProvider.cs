using ApartmentImagesDownloader.ImageUrlsExtractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentImagesDownloader.Objects
{
    public class ImageUrlsExtractorProvider
    {
        public IImageUrlsExtractor GetByUrl(string url)
        {
            var uri = new Uri(url);
            if (string.Equals(uri.Host, "www.aruodas.lt", StringComparison.InvariantCultureIgnoreCase))
                return new AruodasImageUrlsExtractor();
            return null;
        }
    }
}
