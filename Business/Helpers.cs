using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ImageResizer;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Business
{
    public static class Helpers
    {
        public static void SetColors(string connectionString, string imagePath, out string textColor,
            out string bgColor, out string textColorSecond)
        {
            textColor = string.Empty;
            bgColor = string.Empty;
            textColorSecond = string.Empty;

            //Get colors that will be used in list display
            var topFiveColor = GetThreeColors(connectionString, imagePath);
            if (topFiveColor != null)
            {
                textColor = topFiveColor[1].R + "," + topFiveColor[1].G + "," + topFiveColor[1].B;
                bgColor = topFiveColor[0].R + "," + topFiveColor[0].G + "," + topFiveColor[0].B;
                textColorSecond = topFiveColor[2].R + "," + topFiveColor[2].G + "," + topFiveColor[2].B;
                if (int.Parse(topFiveColor[0].R.ToString()) < 50)
                {
                    textColor = textColorSecond;
                    textColorSecond = bgColor;
                }
            }
        }

        private static IEnumerable<Color> GetPixels(Bitmap bitmap)
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                for (var y = 0; y < bitmap.Height; y++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    yield return pixel;
                }
            }
        }

        private static List<Color> GetThreeColors(string connectionString, string fileName)
        {
            var bitmapImage = DownloadImageFromBlob(connectionString, fileName);
            var image = ConvertBitmapImageToBitmap(bitmapImage);
            var list = new List<Color>();
            using (image)
            {
                var colorsWithCount =
                    GetPixels(image)
                        .Where(c => c.R < 240)
                        .GroupBy(color => color)
                        .Select(grp =>
                            new
                            {
                                Color = grp.Key,
                                Count = grp.Count()
                            })
                        .OrderByDescending(x => x.Count);

                var firstColor = colorsWithCount.FirstOrDefault();
                if (firstColor != null) list.Add(firstColor.Color);

                var colorsWithBrightness =
                    GetPixels(image)
                        .Where(c => c.R < 240)
                        .GroupBy(color => color)
                        .Select(grp =>
                            new
                            {
                                Color = grp.Key,
                                Brightness = grp.Key.GetBrightness()
                            })
                        .OrderByDescending(x => x.Brightness);


                var avgValue = Convert.ToInt32(colorsWithCount.Average(c => c.Color.R));
                var secounColor = colorsWithCount.FirstOrDefault(c => c.Color.R == avgValue);
                if (secounColor != null)
                    list.Add(secounColor.Color);
                var thridColor = colorsWithBrightness.FirstOrDefault();
                if (thridColor != null) list.Add(thridColor.Color);

                return list;
            }
        }

        public static string StripTagsCharArray(string source)
        {
            try
            {
                var array = new char[source.Length];
                var arrayIndex = 0;
                var inside = false;

                foreach (var @let in source)
                {
                    if (@let == '<')
                    {
                        inside = true;
                        continue;
                    }
                    if (@let == '>')
                    {
                        inside = false;
                        continue;
                    }
                    if (!inside)
                    {
                        array[arrayIndex] = @let;
                        arrayIndex++;
                    }
                }
                return new string(array, 0, arrayIndex);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string AddSpacesToSentence(string text, bool preserveAcronyms)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            var newText = new StringBuilder(text.Length*2);
            newText.Append(text[0]);
            for (var i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }

        public static string HandleSpecialChars(string tempCat)
        {
            tempCat = tempCat.Replace("&hellip;", "...");
            tempCat = tempCat.Replace("&#8221;", "\"");
            tempCat = tempCat.Replace("&amp;", "&");
            tempCat = tempCat.Replace("&#228;", "ä");
            tempCat = tempCat.Replace("&#246;", "ö");
            tempCat = tempCat.Replace("&#229;", "å");
            tempCat = tempCat.Replace("&#149;", "*");
            return tempCat;
        }

        private static CloudBlobContainer ConnectToBlob(string connectionString)
        {
            var containerName = "bookcovers";
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            return container;
        }

        private static void SetPermissionToBlob(CloudBlobContainer container)
        {
            var permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };
            container.SetPermissions(permissions);
        }

        private static async Task<string> AddToBlob(CloudBlobContainer container, Stream fileStream, string fileName)
        {
            // Verify that the user selected a file
            if (fileStream == null) return string.Empty;
            using (var ms = new MemoryStream())
            {
                var i = new ImageJob(fileStream,
                    ms, new Instructions("width=200;height=200;format=jpg;mode=max"));
                i.Build();
                var blob = container.GetBlockBlobReference(fileName);
                blob.Properties.ContentType = "image/jpg";
                ms.Seek(0, SeekOrigin.Begin);
                await blob.UploadFromStreamAsync(ms);
                return blob.Uri.AbsoluteUri;
            }
        }

        public static async Task<string> UploadImageToBlob(string connectionString, Stream fileStream, string fileName)
        {
            var container = ConnectToBlob(connectionString);
            SetPermissionToBlob(container);
            return await AddToBlob(container, fileStream, fileName);
        }

        private static BitmapImage GetBlobImage(CloudBlobContainer container, string fileName)
        {
            var blob = container.GetBlockBlobReference(fileName);
            using (var memoryStream = new MemoryStream())
            {
                blob.DownloadToStream(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = memoryStream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return bitmap;
            }
        }

        public static BitmapImage DownloadImageFromBlob(string connectionString, string fileName)
        {
            var container = ConnectToBlob(connectionString);
            SetPermissionToBlob(container);
            return GetBlobImage(container, fileName);
        }

        private static Bitmap ConvertBitmapImageToBitmap(BitmapSource bitmapImage)
        {
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                var bitmap = new Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
    }
}