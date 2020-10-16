using System;
using System.Collections.Generic;
using System.Net.Mime;
using ClassLibrary;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace LearningNetCore
{
	class Program
	{
		public static string resultPath = @"../Images/Result/";
		public static string srcPath = @"../Images/SrcImages/";
		public static string tempPath = @"../Images/temp/";
		public static string midIconPath = @"../Images/Icon/MiddleIcon.png";
		public static string botIconPath = @"../Images/Icon/BottomIcon.png";
		public static int botIconWidth = 114;
		public static int botIconHeight = 52;

		static void Main(string[] args)
		{
			//var paths = Directory.GetFiles(@"C:\Users\Dino\Desktop\TestChangeImages");
			//foreach (var filePath in paths)
			//{
			//	var img = new Bitmap(filePath);
			//	var newImg = ChangeBackgroundColor(img);
			//	newImg = ApplyContrast(40.0, newImg);
			//	var fileName = Path.GetFileName(filePath);
			//	newImg.Save($@"C:\Users\Dino\Desktop\New folder/{fileName}");
			//}
			

			Console.OutputEncoding = Encoding.UTF8;
			Console.WriteLine("------------------MENU------------------");
			Console.WriteLine("Nhập 1 để chèn icon chính giữa hình. ");
			Console.WriteLine("Nhập 2 để chèn icon phía dưới hình. ");
			Console.WriteLine("Nhập 3 để chèn cả 2 icon. ");
			Console.WriteLine("Nhập 4 để đổi màu nền. ");
			Console.WriteLine("Nhập 5 để đổi màu nền và chèn icon chính giữa hình. ");
			Console.WriteLine("Nhập 6 để đổi màu nền và chèn icon phía dưới hình. ");
			Console.WriteLine("Nhập 7 để đổi màu nền và chèn cả 2 icon.");
			Console.Write("Em chọn: ");

			var option = Console.ReadLine();
			var opacity = 0.2f;
			var filePaths = Directory.GetFiles(srcPath);

			RemoveOldFiles(resultPath);
			RemoveOldFiles(tempPath);

			//if (option != "1")
			//{
			//	CreateTempFiles(filePaths);
			//	filePaths = Directory.GetFiles(tempPath);
			//}
			//else
			//{
			//	filePaths = Directory.GetFiles(srcPath);
			//}

			foreach (var filePath in filePaths)
			{
				try
				{
					var fileName = Path.GetFileName(filePath);
					var srcImage = new Bitmap(filePath);
					var midIcon = new Bitmap(midIconPath);
					var botIcon = new Bitmap(botIconPath);
					var graphics = Graphics.FromImage((Image)srcImage);
					graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
					switch (option)
					{
						case "1":
							//Nhập 1 để chèn icon chính giữa hình.
							DrawMidIcon(srcImage, opacity, graphics, midIcon);
							break;
						case "2":
							//Nhập 2 để chèn icon phía dưới hình.
							srcImage = DrawBotIcon(srcImage, botIcon);
							break;
						case "3":
							//Nhập 3 để chèn cả 2 icon.
							//srcImage = AddSpaceBottomImage(srcImage);
							DrawMidIcon(srcImage, opacity, graphics, midIcon);
							srcImage = DrawBotIcon(srcImage, botIcon);
							break;
						case "4":
							//Nhập 4 để đổi màu nền.
							srcImage = ChangeBackgroundColor(srcImage);
							break;
						case "5":
							//Nhập 5 để đổi màu nền và chèn icon chính giữa hình.
							srcImage = ChangeBackgroundColor(srcImage);
							DrawMidIcon(srcImage, opacity, graphics, midIcon);
							break;
						case "6":
							//Nhập 6 để đổi màu nền và chèn icon phía dưới hình.
							srcImage = ChangeBackgroundColor(srcImage);
							srcImage = DrawBotIcon(srcImage, botIcon);
							break;
						case "7":
							//Nhập 7 để đổi màu nền và chèn cả 2 icon.
							srcImage = ChangeBackgroundColor(srcImage);
							DrawMidIcon(srcImage, opacity, graphics, midIcon);
							srcImage = DrawBotIcon(srcImage, botIcon);
							break;

					}

					//if (option == "1")
					//{
					//	DrawMidIcon(srcImage, opacity, graphics, midIcon);
					//}
					//else if (option == "2")
					//{
					//	DrawBotIcon(srcImage, graphics, botIcon);
					//}
					//else
					//{
					//	DrawMidIcon(srcImage, opacity, graphics, midIcon);
					//	DrawBotIcon(srcImage, graphics, botIcon);
					//}

					graphics.Dispose();
					srcImage.Save($@"{resultPath}{fileName}");
					Console.WriteLine($"{filePath} - Done");
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}
			Console.WriteLine("All files are completed");
			Console.ReadKey();
		}

		private static Bitmap DrawBotIcon(Bitmap srcImage, Bitmap botIcon)
		{
			srcImage = AddSpaceBottomImage(srcImage);
			var graphics = Graphics.FromImage(srcImage);
			var startIconHeight = srcImage.Height - botIconHeight;
			var startIconWidth = srcImage.Width - botIconWidth - 10;
			graphics.DrawImage(botIcon, new Rectangle(startIconWidth, startIconHeight, botIconWidth, botIconHeight), 0, 0,
				botIcon.Width, botIcon.Height, GraphicsUnit.Pixel);
			return srcImage;
		}

		private static void DrawMidIcon(Bitmap srcImage, float opacity, Graphics graphics, Bitmap midIcon)
		{
			var size = 500;
			var x = (srcImage.Width - size) / 2;
			var y = (srcImage.Height - size) / 2;
			var matrix = new ColorMatrix();
			matrix.Matrix33 = opacity;
			var attributes = new ImageAttributes();
			attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
			graphics.DrawImage(midIcon, new Rectangle(x, y, size, size), 0, 0, midIcon.Width, midIcon.Height,
				GraphicsUnit.Pixel,
				attributes);
		}

		private static void CreateTempFiles(string[] filePaths)
		{
			foreach (var filePath in filePaths)
			{
				var fileName = Path.GetFileName(filePath);
				var srcImage = new Bitmap(filePath);
				var startIconHeight = srcImage.Height - botIconHeight;
				var backgroundColor = "";
				var bgColor = new Color();
				Point? point = null;
				for (int h = srcImage.Height - 13; h >= startIconHeight; h--)
				{
					for (int w = 5; w < srcImage.Width; w++)
					{
						var pixel = srcImage.GetPixel(w, h);
						if (backgroundColor == "")
						{
							backgroundColor = pixel.Name;
							bgColor = Color.FromArgb(pixel.ToArgb());
						}
						else
						{
							if (backgroundColor != pixel.Name)
							{
								point = new Point(w, h);
								break;
							}
						}
					}

					if (point != null)
						break;
				}

				if (point != null)
				{
					var heightImage = srcImage.Height + 60 - (srcImage.Height - point.Value.Y);
					using (Bitmap bmp = new Bitmap(srcImage.Width, heightImage))
					{
						var cropArea = new Rectangle(0, 0, srcImage.Width, srcImage.Height - 3);
						var cropImage = CropImage(srcImage, cropArea);
						var g = Graphics.FromImage(bmp);
						g.Clear(bgColor);
						g.DrawImageUnscaled(cropImage, 0, 0);
						bmp.Save($@"{tempPath}{fileName}");
					}


				}
				else
				{
					File.Copy(filePath, $@"{tempPath}{fileName}");
				}
			}
		}

		private static Bitmap AddSpaceBottomImage(Bitmap srcImage)
		{
				var startIconHeight = srcImage.Height - botIconHeight;
				var backgroundColor = "";
				var bgColor = new Color();
				Point? point = null;
				for (int h = srcImage.Height - 13; h >= startIconHeight; h--)
				{
					for (int w = 5; w < srcImage.Width; w++)
					{
						var pixel = srcImage.GetPixel(w, h);
						if (backgroundColor == "")
						{
							backgroundColor = pixel.Name;
							bgColor = Color.FromArgb(pixel.ToArgb());
						}
						else
						{
							if (backgroundColor != pixel.Name)
							{
								point = new Point(w, h);
								break;
							}
						}
					}

					if (point != null)
						break;
				}

				if (point != null)
				{
					var heightImage = srcImage.Height + 60 - (srcImage.Height - point.Value.Y);
					Bitmap bmp = new Bitmap(srcImage.Width, heightImage);
					//using (Bitmap bmp = new Bitmap(srcImage.Width, heightImage))
					//{
						var cropArea = new Rectangle(0, 0, srcImage.Width, srcImage.Height - 3);
						var cropImage = CropImage(srcImage, cropArea);
						var g = Graphics.FromImage(bmp);
						g.Clear(bgColor);
						g.DrawImageUnscaled(cropImage, 0, 0);
						return bmp;
					//}


				}
				else
				{
					return srcImage;
				}
		}

		private static void RemoveOldFiles(string path)
		{
			var dicNeedDeleteFile = new DirectoryInfo(path);
			foreach (var file in dicNeedDeleteFile.EnumerateFiles())
			{
				file.Delete();
			}
		}

		public static Image CropImage(Image img, Rectangle cropArea)
		{
			Bitmap bmpImage = new Bitmap(img);
			return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
		}

		public static ColorModel GetBackgroundColor(Bitmap img)
		{
			var colorModels = new List<ColorModel>();
			var dictionary = new Dictionary<int, int>();
			for (int w = 0; w < img.Width; w++)
			{
				for (int h = 0; h < img.Height; h++)
				{
					var pixel = img.GetPixel(w, h);
					colorModels.Add(new ColorModel()
					{
						R = pixel.R,
						G = pixel.G,
						B=pixel.B
					});
				}
			}

			var groupBy = colorModels.GroupBy(x=>new ColorModel{R = x.R, G=x.G, B = x.B}).OrderByDescending(g=>g.Count()).Last();
			return groupBy.Key;
		}

		public static Bitmap ChangeBackgroundColor(Bitmap img, bool ignoreBlue = true)
		{
			for (int w = 0; w < img.Width; w++)
			{
				for (int h = 0; h < img.Height; h++)
				{
					var newColor = img.GetPixel(w, h);
					var r = 255 - newColor.R;
					var g = 255 - newColor.G;
					var b = ignoreBlue ? newColor.B : 255 - newColor.B;
					img.SetPixel(w, h, Color.FromArgb(r, g, b));
				}
			}
			img = ApplyContrast(40.0, img);

			return img;
		}

		public class ColorModel
		{
			public int R { get; set; }
			public int G { get; set; }
			public int B { get; set; }
		}

		private static Bitmap ApplyContrast(double contrast, Bitmap img)
		{
			//byte contrast_lookup[256];
			byte[] contrast_lookup = new byte[256];
			double c = (100.0 + contrast) / 100.0;

			c *= c;

			for (int i = 0; i < 256; i++)
			{
				var newValue = (double)i;
				newValue /= 255.0;
				newValue -= 0.5;
				newValue *= c;
				newValue += 0.5;
				newValue *= 255;

				if (newValue < 0)
					newValue = 0;
				if (newValue > 255)
					newValue = 255;
				contrast_lookup[i] = (byte)newValue;
			}

			for (int w = 0; w < img.Width; w++)
			{
				for (int h = 0; h < img.Height; h++)
				{
					var pixelColor = img.GetPixel(w, h);
					var a = contrast_lookup[pixelColor.A];
					var r = contrast_lookup[pixelColor.R];
					var g = contrast_lookup[pixelColor.G];
					var b = contrast_lookup[pixelColor.B];
					img.SetPixel(w, h, Color.FromArgb(a, r, g, b));
				}
			}
			return img;
		}
	}
}
