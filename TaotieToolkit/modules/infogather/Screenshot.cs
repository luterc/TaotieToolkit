using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using TaotieToolkit.config;

namespace TaotieToolkit
{
    internal class Screenshot : ICommand, ICommandMarker
    {
        public string Name => "Screenshot";
        public string Description => "获取屏幕截图";
        public void Execute(string[] args)
        {
            string filename = Guid.NewGuid().ToString() + ".jpg";
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            bmpScreenshot.Save(filename, ImageFormat.Png);

            // Check
            if (File.Exists(filename))
            {
                Console.WriteLine("[*] Desktop screenshot created:"+ filename);
            }
            else
            {
                Console.WriteLine("\t[!] Desktop screenshot not created");
            }
        }


    }
}
