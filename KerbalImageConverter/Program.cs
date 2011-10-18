using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace KerbalImageConverter
{
	class Program
	{
		static void Main(string[] args)
		{
		//	if(args.Length != 1)
			//	Console.WriteLine("Error 1");

			string path = Path.Combine(Environment.CurrentDirectory, "Parts");

			if (!Directory.Exists(path))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Please ensure this program is placed in the same directory as \"ksp.exe\"");

				Console.ResetColor();
				Console.WriteLine("Press any key to exit.");
				Console.ReadKey(true);

				return;
			}

			string currentPartImage = string.Empty;

			try
			{
				foreach (var part in Directory.EnumerateDirectories(path))
				{
					string textures = Path.Combine(part, "textures");

					if (!Directory.Exists(textures))
					{
						Console.ForegroundColor = ConsoleColor.Yellow;
						Console.WriteLine("Could not find a textures folder in: {0}", part);
						continue;
					}

					foreach (var image in Directory.EnumerateFiles(textures, "*.png"))
					{
						currentPartImage = image;
						bool result = Convert(image);

						if (result)
						{
							Console.ResetColor();
							Console.ForegroundColor = ConsoleColor.Green;
							Console.WriteLine("Image \"{0}\" Converted Successfully :)",
								Path.GetFileNameWithoutExtension(image));
						}
						else
						{
							Console.ForegroundColor = ConsoleColor.Yellow;
							Console.WriteLine("Image \"{0}\" did not need to be converted...",
								Path.GetFileNameWithoutExtension(image));
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(ex.ToString());
				Console.WriteLine();
				Console.WriteLine("Fatal error when trying to convert: {0}", currentPartImage);
				Console.ResetColor();
				Console.WriteLine();
				Console.WriteLine("Press any key to exit.");
				Console.ReadKey(true);
				return;
			}
		

			
			Console.ResetColor();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine();
			Console.WriteLine("Done :)");
			Console.WriteLine();

			Console.ResetColor();
			Console.WriteLine("Press any key to exit.");
			Console.ReadKey(true);
		}

		static bool Convert(string filename)
		{
			Bitmap bmp = Image.FromFile(filename) as Bitmap;

			if (bmp.PixelFormat != PixelFormat.Format24bppRgb)
			{
				bmp.Dispose();
				return false;
			}

			Bitmap output = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb);

			for (int y = 0; y < bmp.Height; y++)
			{
				for (int x = 0; x < bmp.Width; x++)
				{
					
					Color c = bmp.GetPixel(x, y);
					output.SetPixel(x, y, c);
				}
			}

			bmp.Dispose();

			output.Save(filename, ImageFormat.Png);

			output.Dispose();


			return true;
		}
	}
}
