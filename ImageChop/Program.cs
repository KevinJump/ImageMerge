using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing.Imaging;

namespace ImageChop
{
    class Program
    {
        static void Main(string[] args)
        {
            string imageFolder = "";
            if (args.Count() == 1)
            {
                imageFolder = args[0];
            }
            else
            {
                Console.WriteLine("ImageChop <path>\n");
                Console.WriteLine("Chop a load of images down to 1280x1028");
            }

            int count = 0;


            var savefolder = string.Format("{0}crop\\", imageFolder);
            if (!Directory.Exists(savefolder))
                Directory.CreateDirectory(savefolder);

            foreach (string file in Directory.GetFiles(imageFolder, "*.png"))
            {
                Console.Write(".");
                count++;


                Bitmap image = new Bitmap(file);

                var saveFile = string.Format("{0}{1}_crop.png", savefolder, Path.GetFileNameWithoutExtension(file));
                //Console.WriteLine(saveFile);
                try {
                    //var cropImage = new Bitmap(1280,1024);
                    //Graphics g = Graphics.FromImage(cropImage);
                    //g.DrawImage(image, new Rectangle(0, 0, 1280, 1024));
                    // Console.WriteLine(saveFile);
                    var cropImage = image.Clone(new Rectangle(0, 0, 1280, 1024), PixelFormat.DontCare);

                    if (File.Exists(saveFile))
                        File.Delete(saveFile);

                    cropImage.Save(saveFile, ImageFormat.Png);
                    cropImage.Dispose();
                }
                catch( Exception e)
                {
                    Console.WriteLine("Error doing that : " + e.ToString());
                }
                image.Dispose();
            }

            Console.WriteLine("Done, saved {0}", count);
        }
    }
}
