using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApartmentImagesDownloader.ImageUrlsExtractor
{
    class AruodasImageUrlsExtractor : IImageUrlsExtractor
    {
        public List<string> GetImageUrls(string url)
        {
            List<string> imageUrls = new List<string>();

            string requiredUrl = GenerateRequiredUrl(url);
            var web = new HtmlWeb();
            var htmlDocument = web.Load(requiredUrl);
            var html = htmlDocument.DocumentNode;
            var imageNodes = html.SelectNodes("//div[@class='obj-photos']/*");
            Regex regex = new Regex("thumb\\d+");
            foreach (var imageNode in imageNodes)
                if (regex.IsMatch(imageNode.GetAttributeValue("data-id", "")) && imageNode.GetAttributeValue("data-type", "") != "last_thumb")
                {
                    string href = "";
                    if (imageNode.Name.Equals("a"))
                        href = imageNode.Attributes["href"].Value;
                    else if (imageNode.Name.Equals("span"))
                    {
                        HtmlNode tag = imageNode.ChildNodes.Where(node => node.HasClass("obj-thumb")).FirstOrDefault();
                        if (tag != null)
                            href = tag.GetAttributeValue("data-url", "");
                    }
                    if (href.Contains("aruodas-img"))
                        imageUrls.Add(href);
                }
            return imageUrls;
        }

        private void WaitPageLoad(WebBrowser webBrControl, ManualResetEvent sleeper)
        {
            WebBrowserReadyState loadStatus;
            DateTime waitUntil = DateTime.Now.AddSeconds(30);
            while (waitUntil > DateTime.Now)
            {
                loadStatus = webBrControl.ReadyState;
                Application.DoEvents();
                if ((loadStatus == WebBrowserReadyState.Uninitialized) || (loadStatus == WebBrowserReadyState.Loading) || (loadStatus == WebBrowserReadyState.Interactive))
                {
                    break;
                }
                sleeper.WaitOne(1);
            }

            while (waitUntil > DateTime.Now)
            {
                loadStatus = webBrControl.ReadyState;
                Application.DoEvents();
                if (loadStatus == WebBrowserReadyState.Complete && webBrControl.IsBusy != true)
                {
                    break;
                }
                sleeper.WaitOne(1);
            }
        }
        private void WaitJsExecute(int waitSeconds, ManualResetEvent sleeper)
        {
            DateTime waitUntil = DateTime.Now.AddSeconds(waitSeconds);
            while (waitUntil > DateTime.Now)
            {
                Application.DoEvents();
            }
        }
        private string GenerateRequiredUrl(string url)
        {
            string apartmentId = new Regex("\\d+-\\d+/?$").Match(url).Value;
            Uri uri = new Uri(url);
            return uri.Scheme + "://" + uri.Host + '/' + apartmentId;
        }
    }
}
