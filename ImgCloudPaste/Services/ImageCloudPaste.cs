using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ImgCloudPaste.Controls;
using ImgCloudPaste.Models;
using StringIdLibrary;
using System.Collections.Generic;
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

        public async Task<Result> AddAsync(Image image)
        {
            var info = GetFileInfo(image);
            var name = StringId.New(10, StringIdRanges.Upper | StringIdRanges.Numeric) + info.extension;
            return await UpdateAsync(name, image);
        }

        public async Task<Result> UpdateAsync(string name, Image image)
        {
            var info = GetFileInfo(image);
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

            return GetResult(client);
        }

        public IEnumerable<BlobItem> GetBlobs()
        {
            var client = new BlobContainerClient(_settings.ConnectionString, _settings.ContainerName);
            var pages = client.GetBlobs().AsPages();

            List<BlobItem> results = new List<BlobItem>();
            foreach (var page in pages) results.AddRange(page.Values);
            return results;
        }

        private (string extension, string contentType, ImageFormat format) GetFileInfo(Image image) =>
            (image.RawFormat.Equals(ImageFormat.Png)) ? (".png", "image/png", image.RawFormat) :
            (image.RawFormat.Equals(ImageFormat.Jpeg)) ? (".jpg", "image/jpg", image.RawFormat) :
            (image.RawFormat.Equals(ImageFormat.Gif)) ? (".gif", "image/gif", image.RawFormat) :
            (image.RawFormat.Equals(ImageFormat.Bmp)) ? (".bmp", "image/bitmap", image.RawFormat) :
            // SnagIt has some kind of custom image format that isn't recognized by the built in ImageFormat values,
            // so I just fall back to png when the type isn't recognized
            (".png", "image/png", ImageFormat.Png);

        public async Task<(Image image, Result result)> GetBlobImageAsync(BlobItem blob)
        {
            var client = new BlobClient(_settings.ConnectionString, _settings.ContainerName, blob.Name);
                       
            using (var stream = await client.OpenReadAsync())
            {
                return (Image.FromStream(stream), GetResult(client));
            }
        }

        private Result GetResult(BlobClient blobClient) => new Result()
        {
            RawUrl = blobClient.Uri.AbsoluteUri,
            MarkdownUrl = $"![img]({blobClient.Uri.AbsoluteUri})"
        };

        public class Result
        {
            public string RawUrl { get; set; }
            public string MarkdownUrl { get; set; }
        }
    }
}
