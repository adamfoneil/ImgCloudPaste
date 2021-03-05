using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ImgCloudPaste.Models;
using StringIdLibrary;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace ImgCloudPaste.Services
{
    public class ImageCloudPaste
    {
        private readonly Settings _settings;

        public ImageCloudPaste(Settings settings)
        {
            _settings = settings;
        }

        public string RawUrl { get; private set; }
        public string MarkdownUrl { get; private set; }

        public async Task UploadAndGetUrlsAsync(Image image)
        {
            var info = GetFileInfo(image);
            var name = StringId.New(10, StringIdRanges.Upper | StringIdRanges.Numeric) + info.extension;            
            var client = new BlobClient(_settings.ConnectionString, _settings.ContainerName, name);
            
            using (var ms = new MemoryStream())
            {
                image.Save(ms, info.format);
                ms.Position = 0;
                await client.UploadAsync(ms, new BlobHttpHeaders()
                {
                    ContentType = info.contentType
                });
            }

            RawUrl = client.Uri.AbsoluteUri;
            MarkdownUrl = $"![img]({RawUrl})";
        }

        private (string extension, string contentType, ImageFormat format) GetFileInfo(Image image) =>
            (image.RawFormat.Equals(ImageFormat.Png)) ? (".png", "image/png", image.RawFormat) :
            (image.RawFormat.Equals(ImageFormat.Jpeg)) ? (".jpg", "image/jpg", image.RawFormat) :
            (image.RawFormat.Equals(ImageFormat.Gif)) ? (".gif", "image/gif", image.RawFormat) :
            (image.RawFormat.Equals(ImageFormat.Bmp)) ? (".bmp", "image/bitmap", image.RawFormat) :
            // SnagIt has some kind of custom image format that isn't recognized by the built in ImageFormat values,
            // so I just fall back to png when the type isn't recognized
            (".png", "image/png", ImageFormat.Png);
    }
}
