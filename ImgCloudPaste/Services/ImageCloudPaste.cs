using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using StringIdLibrary;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace ImgCloudPaste.Services
{
    public class Settings
    {
        /// <summary>
        /// azure storage connection string
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// upload container
        /// </summary>
        public string ContainerName { get; set; }
    }

    public class ImageCloudPaste
    {
        private readonly Settings _settings;

        public ImageCloudPaste(Settings settings)
        {
            _settings = settings;
        }

        public async Task<string> UploadAndGetUrlAsync(Image image)
        {
            var info = GetFileInfo(image);
            var name = StringId.New(10, StringIdRanges.Upper | StringIdRanges.Numeric) + info.extension;            
            var client = new BlobClient(_settings.ConnectionString, _settings.ContainerName, name);
            
            using (var ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                await client.UploadAsync(ms, new BlobHttpHeaders()
                {
                    ContentType = info.contentType
                });
            }

            return client.Uri.AbsoluteUri;
        }

        private (string extension, string contentType) GetFileInfo(Image image) =>
            (image.RawFormat.Equals(ImageFormat.Png)) ? (".png", "image/png") :
            (image.RawFormat.Equals(ImageFormat.Jpeg)) ? (".jpg", "image/jpg") :
            (image.RawFormat.Equals(ImageFormat.Gif)) ? (".gif", "image/gif") :
            (image.RawFormat.Equals(ImageFormat.Bmp)) ? (".bmp", "image/bitmap") :
            throw new InvalidOperationException($"Unsupported image format: {image.RawFormat}");
    }
}
