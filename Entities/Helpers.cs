using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
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

		public static List<Color> GetThreeColors(string fileName)
		{//TODO: Add to config
			string workingFolder = AppDomain.CurrentDomain.BaseDirectory;
			string clientFolder = workingFolder + "BookCovers\\";
			fileName = clientFolder + fileName;
			if (!fileName.Contains(".")) { return null; };
			Bitmap bitmap = new Bitmap(fileName);
			List<Color> list = new List<Color>();

			using (bitmap)
			{
				var colorsWithCount =
					GetPixels(bitmap)
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

				//Change order of colors if background is bright
				if (colorsWithCount.FirstOrDefault().Color.R > 200)
				{
					list.Add(colorsWithCount.LastOrDefault().Color);
				}

				if (colorsWithCount.LastOrDefault().Color.R < colorsWithCount.FirstOrDefault().Color.R + 50)
				{
					var maxValue = colorsWithCount.Max(c => c.Color.R) - 100;
					list.Add(colorsWithCount.Where(c => c.Color.R > maxValue).FirstOrDefault().Color);
				}
				else
				{
					list.Add(colorsWithCount.LastOrDefault().Color);
				}

				var avgValue = colorsWithCount.Average(c => c.Color.R);

				if (colorsWithCount.FirstOrDefault().Color.R <= 200)
				{
					list.Add(colorsWithCount.LastOrDefault().Color);
				}

				list.Add(colorsWithCount.LastOrDefault().Color);

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
	}
}
