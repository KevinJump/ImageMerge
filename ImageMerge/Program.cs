using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageMerge
{
    class Program
    {
        static void Main(string[] args)
        {
            string imagefolder = "";
            if (args.Count() == 1)
            {
                imagefolder = args[0];
            }
            else
            {
                Console.WriteLine("ImageMerge <path>\n");
                Console.WriteLine("go through a folder of images, and make a merged overlay image of all the images");
            }
            

            int width = 0;
            int height = 0;
            int count = 0 ; 

            foreach(string file in Directory.GetFiles(imagefolder, "*.png" )) 
            {
                Console.Write(".");

                count++;

                System.Drawing.Bitmap image = new Bitmap(file);

                if (image.Width > width)
                    width = image.Width;

                if (image.Height > height)
                    height = image.Height;

                image.Dispose();
            }


            if ( count == 0 )
            {
                Console.WriteLine("Nothing found");
                return;

            }
            Console.WriteLine("Max Image: {0} x {1}", width, height);

            Bitmap final = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(final);
            g.Clear(Color.White);

            Console.WriteLine("Processing {0}", count);
            float opacitiy = (float)1 / count;
            Console.WriteLine("Processing at {0} opactiy", opacitiy);

            foreach (string file in Directory.GetFiles(imagefolder, "*.png"))
            {
                Console.Write(".");
                System.Drawing.Bitmap image = new Bitmap(file);

                image.MakeTransparent(Color.White);
                
                Bitmap newImage = new Bitmap(SetImageOpacity((Image)image, opacitiy));
                g.DrawImage(newImage, new Point(0, 0));
                newImage.Dispose();
                image.Dispose();
            }

            Console.WriteLine(" Done.");

            string saveFile = String.Format("{0}\\output\\desktop_all_{1}.png", imagefolder, DateTime.Now.ToString("hhss"));
            
            if (!Directory.Exists(Path.GetDirectoryName(saveFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(saveFile));

            final.Save(saveFile, ImageFormat.Png);
        }

        /// <param name="opacity">percentage of opacity</param>  
        /// <returns></returns>  
        public static Image SetImageOpacity(Image image, float opacity)
        {
            try
            {
                //create a Bitmap the size of the image provided  
                Bitmap bmp = new Bitmap(image.Width, image.Height);

                //create a graphics object from the image  
                using (Graphics gfx = Graphics.FromImage(bmp))
                {

                    //create a color matrix object  
                    ColorMatrix matrix = new ColorMatrix();

                    //set the opacity  
                    matrix.Matrix33 = opacity;

                    //create image attributes  
                    ImageAttributes attributes = new ImageAttributes();

                    //set the color(opacity) of the image  
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    //now draw the image  
                    gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
                return bmp;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        } 
    }
}
