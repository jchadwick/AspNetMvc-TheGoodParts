using System;
using System.IO;

namespace Common.Util
{
    public class ImageRepository
    {
        public string BaseUrl { get; set; }
        public string ImagesFolder { get; set; }

        public ImageRepository(string imagesFolder, string baseUrl)
        {
            ImagesFolder = imagesFolder;
            BaseUrl = baseUrl;
        }

        public void SaveImage(string imageFilename, Stream stream, out string imageUrl, out string thumbnailUrl, int thumbWidth = 100, int thumbHeight = 100)
        {
            var thumbnailFilename =
                string.Format("{0}_thumb.{1}",
                    Path.GetFileNameWithoutExtension(imageFilename),
                    Path.GetExtension(imageFilename)
                );


            var image = System.Drawing.Image.FromStream(stream);

            image.Save(Path.Combine(ImagesFolder, imageFilename));

            image.GetThumbnailImage(thumbWidth, thumbHeight, null, IntPtr.Zero)
                .Save(Path.Combine(ImagesFolder, thumbnailFilename));


            imageUrl = string.Format("{0}/{1}", BaseUrl, imageFilename).Replace("//", "/");
            thumbnailUrl = string.Format("{0}/{1}", BaseUrl, thumbnailFilename).Replace("//", "/");
        }
    }
}