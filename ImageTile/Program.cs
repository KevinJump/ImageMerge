using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;
using System.Drawing.Imaging;


namespace ImageTile
{
    class Program
    {

        /// <summary>
        ///  tile a load of screenshots into one huge image...
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string imagefolder = "";
            if (args.Count() == 1)
            {
                imagefolder = args[0];
            }
            else
            {
                Console.WriteLine("ImageTile <path>\n");
                Console.WriteLine("go through a folder of images, and make a large tiled image\nworks best if all the images are the samesize");
            }
            int acrossCount = 23;

            int width = 0;
            int height = 0;
            int count = 0;

            foreach (string file in Directory.GetFiles(imagefolder, "*_desktop.jpg"))
            {
                Console.Write(".");

                count++;

                System.Drawing.Bitmap image = new Bitmap(file);

                if (image.Width > width)
                    width = image.Width;

                if (image.Height > height)
                    height = image.Height;
            }

            int imageWidth = width * acrossCount;

                
            int rows = (int)Math.Ceiling( (double)(count / acrossCount));
            int depth = height * (rows +1);

            Console.WriteLine("Image Will be {0} x {1}", imageWidth, depth);

            Bitmap final = new Bitmap(imageWidth, depth);
            Graphics g = Graphics.FromImage(final);
            g.Clear(Color.White);

            Console.WriteLine("Processing {0}", count);
            int processed = 0; 

            foreach (string file in Directory.GetFiles(imagefolder, "*_desktop.jpg"))
            {
                Console.Write(".");
                System.Drawing.Bitmap image = new Bitmap(file);

                int x = (processed % acrossCount) * width ;
                int y = (int)Math.Ceiling( (double)(processed / acrossCount)) * height;

                Console.WriteLine("{0} on Col:{1} Row:{2} At {3}, {4}", processed, (processed % acrossCount),(int)Math.Ceiling( (double)(processed / acrossCount)),  x, y);

                g.DrawImage(image, new Point(x,y));
                
                processed++;
            }

            Console.WriteLine(" Done.");

            string saveFile = String.Format("{0}\\output\\tiles_desktop_{1}.png", imagefolder, DateTime.Now.ToString("hhss"));

            if ( !Directory.Exists( Path.GetDirectoryName(saveFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(saveFile));

            final.Save(saveFile, ImageFormat.Png);
        }
    }
}
