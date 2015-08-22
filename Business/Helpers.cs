using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Business
{
	//Many of this methods are not used since started with Azure
	//when all clients work - remove unnessary code 
	public static class Helpers
	{
		private static void DeleteImage(string fileName)
		{
			try
			{
				if (File.Exists(fileName))
				{
					File.Delete(fileName);
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private static bool BitmapIsRightSize(Bitmap bitImage)
		{
			return bitImage.Height == 200 || bitImage.Width == 200;
		}

		public static string SetRightSizeImageAndSave(string fileName, string isbn)
		{
			int stop = fileName.LastIndexOf("\\");
			string newFileName = fileName.Substring(0, stop) + "\\" + isbn + fileName.Substring(fileName.Length - 4, 4);

			if (fileName == newFileName)
			{
				fileName = RenameOriginalFile(fileName);
			}

			using (var image = new Bitmap(fileName))
			{
				if (!BitmapIsRightSize(image))
				{
					using (var imageResided = ResizeImage(image, 200, 200))
					{
						imageResided.Save(newFileName);
					}
				}
				else
				{
					image.Save(newFileName);
				}
			}
			//Delete original image
			DeleteImage(fileName);
			return newFileName;
		}

		private static string RenameOriginalFile(string fileName)
		{
			int stop = fileName.LastIndexOf("\\");
			string newFileName = fileName.Substring(0, stop) + "\\temp" + fileName.Substring(fileName.Length - 4, 4);
			System.IO.File.Move(fileName, newFileName);
			return newFileName;
		}

		private static Bitmap ResizeImage(Image image, int width, int height)
		{
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}

		private static IEnumerable<Color> GetPixels(Bitmap bitmap)
		{
			for (int x = 0; x < bitmap.Width; x++)
			{
				for (int y = 0; y < bitmap.Height; y++)
				{
					Color pixel = bitmap.GetPixel(x, y);
					yield return pixel;
				}
			}
		}

		public static List<Color> GetThreeColors(string connectionString, string path, string fileName)
		{
			var bitmapImage=DownloadImageFromBlob(connectionString, path, fileName);
			var image = ConvertBitmapImageToBitmap(bitmapImage);
			List<Color> list = new List<Color>();
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

				list.Add(colorsWithCount.FirstOrDefault().Color);


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


				//list.Add(colorsWithBrightness.LastOrDefault().Color);
				var avgValue = Convert.ToInt32(colorsWithCount.Average(c => c.Color.R));
				list.Add(colorsWithCount.Where(c => c.Color.R == avgValue).FirstOrDefault().Color);
				list.Add(colorsWithBrightness.FirstOrDefault().Color);



				return list;
			}
		}

		public static string StripTagsCharArray(string source)
		{
			try
			{
				char[] array = new char[source.Length];
				int arrayIndex = 0;
				bool inside = false;

				for (int i = 0; i < source.Length; i++)
				{
					char let = source[i];
					if (let == '<')
					{
						inside = true;
						continue;
					}
					if (let == '>')
					{
						inside = false;
						continue;
					}
					if (!inside)
					{
						array[arrayIndex] = let;
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
			StringBuilder newText = new StringBuilder(text.Length * 2);
			newText.Append(text[0]);
			for (int i = 1; i < text.Length; i++)
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
			string containerName = "bookcovers";
			var storageAccount =  CloudStorageAccount.Parse(connectionString);
			var blobClient = storageAccount.CreateCloudBlobClient();
			var container = blobClient.GetContainerReference(containerName);
			return container;
		}

		private static void SetPermissionToBlob(CloudBlobContainer container)
		{
			var permissions = new BlobContainerPermissions()
			{
				PublicAccess = BlobContainerPublicAccessType.Blob
			};
			container.SetPermissions(permissions);
		}
		private static async Task<string> AddToBlob(CloudBlobContainer container, Stream fileStream, string fileName)
		{		  // Verify that the user selected a file
			if (fileStream != null)
			{
				using (var ms = new MemoryStream())
				{
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(fileStream,
							ms, new ImageResizer.ResizeSettings("width=200;height=200;format=jpg;mode=max"));
					i.Build();
					CloudBlockBlob blob = container.GetBlockBlobReference(fileName);
					blob.Properties.ContentType = "image/jpg";
					ms.Seek(0, SeekOrigin.Begin);
					await blob.UploadFromStreamAsync(ms);
					return blob.Uri.AbsoluteUri;
				}
			}
			return string.Empty;
		}
		public static async Task<string> UploadImageToBlob(string connectionString, Stream fileStream, string fileName)
		{
			var container = ConnectToBlob(connectionString);
			SetPermissionToBlob(container);
			return await AddToBlob(container, fileStream, fileName);
		}
		private static BitmapImage GetBlobImage(CloudBlobContainer container, string path, string fileName)
		{

			//string path = ConfigurationManager.ConnectionStrings["storagePath"].ConnectionString;
			CloudBlockBlob blob = container.GetBlockBlobReference(fileName);
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
		public static BitmapImage DownloadImageFromBlob(string connectionString, string path, string fileName)
		{
			var container = ConnectToBlob(connectionString);
			SetPermissionToBlob(container);
			return GetBlobImage(container, path, fileName);
		}
		private static Bitmap ConvertBitmapImageToBitmap(BitmapImage bitmapImage)
		{
			using (MemoryStream outStream = new MemoryStream())
			{
				BitmapEncoder enc = new BmpBitmapEncoder();
				enc.Frames.Add(BitmapFrame.Create(bitmapImage));
				enc.Save(outStream);
				System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

				return new Bitmap(bitmap);
			}
		}


	}}
