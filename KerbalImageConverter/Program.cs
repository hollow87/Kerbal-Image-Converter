/*
 * Copyright (c) 2011, M. Combs
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, 
 * this list of conditions and the following disclaimer.
 * 
 * Redistributions in binary form must reproduce the above copyright notice, 
 * this list of conditions and the following disclaimer in the documentation and/or 
 * other materials provided with the distribution.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
 * THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, 
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
 * OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY 
 * OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * 
*/

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
