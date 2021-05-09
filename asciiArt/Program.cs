using System;
using System.Text;
using ImageMagick;
using CommandLine;

namespace asciiArt
{
    class Program
    {
        public static char[] asciiMapping = "`^\",:;Il!i~+_-?][}{1)(|\\/tfjrxnuvczXYUJCLQ0OZmwqpdbkhao*#MW&8%B@$".ToCharArray();

        public static void Main(string[] args)
        {
            Options options = new Options();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o => { options = o; })
                .WithNotParsed(e =>
                {
                    foreach (Error er in e)
                    {
                        Console.WriteLine(er.ToString());
                    }
                    Console.ReadLine();
                    Environment.Exit(0);
                });

            string imagePath = options.ImagePath;
            string color = options.Color;
            try
            {
                Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color);
            }
            catch(Exception e)
            {
                Console.WriteLine("Please input the correct color. Type --help for help");
                Console.ReadLine();
                Environment.Exit(0);
            }
            

            var info = new MagickImageInfo(imagePath);

            using (var image = new MagickImage(imagePath))
            {
                var pixels = image.GetPixels();
                var bytes = pixels.ToByteArray(PixelMapping.RGB);
                byte[,] pixelArray = new byte[info.Width * info.Height, image.ChannelCount];
                for(int i = 0; i < bytes.Length; i++)
                {
                    pixelArray[i / 3, i % 3] = bytes[i];
                }

                byte[] brightness = new byte[info.Width * info.Height];
                for(int i = 0; i < brightness.Length; i++)
                {
                    int sum = 0;
                    for(int c = 0; c < image.ChannelCount; c++)
                    {
                        sum += pixelArray[i, c];
                    }
                    if(options.Invert)
                    {
                        brightness[i] = (byte)(255 - (sum / 3));
                    }
                    else
                    {
                        brightness[i] = (byte)(sum / 3);
                    }
                }

                char[] ascii = new char[info.Width * info.Height];
                int asciiMapLen = asciiMapping.Length;
                for(int i = 0; i < brightness.Length; i++)
                {
                    float mapInteval = 256.0f / asciiMapLen;
                    int index = (int)(brightness[i] / mapInteval);
                    ascii[i] = asciiMapping[index];
                }
                int r = 0;
                
                StringBuilder builder = new StringBuilder();
                for(int i = 0; i < ascii.Length; i++) 
                {
                    builder.Append(ascii[i]);
                    builder.Append(ascii[i]);
                    r++;
                    if (r == info.Width)
                    {
                        r = 0;
                        Console.WriteLine(builder.ToString());
                        builder.Clear();
                    }
                }
            }
            Console.ResetColor();
            Console.ReadLine();
        }
    }
}
