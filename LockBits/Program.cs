using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace LockBits
{
    class Program
    {
        static void Main()
        {
            const int rp = (int)(0.298912 * 1024);
            const int gp = (int)(0.586611 * 1024);
            const int bp = (int)(0.114478 * 1024);

            var bmp = (Bitmap)Image.FromFile("test.bmp");
            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);
            var pixelSize = bmp.Width * bmp.Height * 4;
            var dest = new byte[pixelSize];
            Marshal.Copy(bmpData.Scan0, dest, 0, dest.Length);

            for (var i = 0; i < pixelSize; i += 4)
            {
                var g = (byte)((bp * dest[i] + gp * dest[i + 1] + rp * dest[i + 2]) >> 10);
                dest[i] = g;
                dest[i + 1] = g;
                dest[i + 2] = g;
                dest[i + 3] = 255;
            }

            Marshal.Copy(dest, 0, bmpData.Scan0, dest.Length);
            bmp.UnlockBits(bmpData);
            bmp.Save("result.bmp");
            bmp.Dispose();
        }
    }
}
