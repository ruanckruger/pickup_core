using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace pickupsv2.Helpers
{
    public static class WriteHelper
    {
        private static IHostingEnvironment environment;
        private static IConfiguration config;
        public static IServiceProvider SetupWriteHelperService(this IServiceProvider serviceProvider, IHostingEnvironment _environment, IConfiguration _configuration)
        {
            environment = _environment;
            config = _configuration;
            return serviceProvider;
        }
        public enum ImageFormat
        {
            bmp,
            jpeg,
            gif,
            tiff,
            png,
            unknown
        }

        public static ImageFormat GetImageFormat(byte[] bytes)
        {
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };              // PNG
            var tiff = new byte[] { 73, 73, 42 };                  // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };                 // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 };          // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 };         // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return ImageFormat.bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return ImageFormat.gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return ImageFormat.png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return ImageFormat.tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return ImageFormat.tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return ImageFormat.jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return ImageFormat.jpeg;

            return ImageFormat.unknown;
        }
        public static async Task<string> UploadImage(IFormFile image, string imagePath, string fileName)
        {
            string extension = string.Empty;
            try
            {
                var filePath = string.Empty;
                using (var ms = new MemoryStream())
                {
                   image.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    extension = WriteHelper.GetImageFormat(fileBytes).ToString();
                    fileName += "." + extension;
                    filePath = Path.Combine(environment.WebRootPath, "img", imagePath);                    
                    Directory.CreateDirectory(filePath);
                    var fullPath = Path.Combine(filePath, fileName);
                    if (File.Exists(fullPath))
                    {

                    }
                    else
                    {
                        FileStream fs = File.Create(fullPath);
                        fs.Close();
                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }
                    }
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return extension;
        }
    }
}


