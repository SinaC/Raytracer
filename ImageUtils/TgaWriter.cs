using System.IO;

namespace ImageUtils
{
    //http://nokola.com/blog/post/2010/01/21/Quick-and-Dirty-Output-of-WriteableBitmap-as-TGA-Image.aspx
    public static class TgaWriter
    {
        public static void Write(byte[] rgbBitmap, int width, int height, Stream output)
        {
            byte[] pixelsArr = new byte[(width * height) * 4];

            int width4 = width * 4;
            int width8 = width * 8;
            int offsetDest = (height - 1) * width4;
            int offsetSource = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    pixelsArr[offsetDest] = rgbBitmap[offsetSource + 2]; // b
                    pixelsArr[offsetDest + 1] = rgbBitmap[offsetSource + 1]; // g
                    pixelsArr[offsetDest + 2] = rgbBitmap[offsetSource + 0]; // r
                    pixelsArr[offsetDest + 3] = 0xFF; // a

                    offsetDest += 4;
                    offsetSource += 3;
                }
                offsetDest -= width8;
            }

            byte[] header = new byte[]
                {
                    0, // ID length
                    0, // no color map
                    2, // uncompressed, true color
                    0, 0, 0, 0,
                    0,
                    0, 0, 0, 0, // x and y origin
                    (byte) (width & 0x00FF),
                    (byte) ((width & 0xFF00) >> 8),
                    (byte) (height & 0x00FF),
                    (byte) ((height & 0xFF00) >> 8),
                    32, // 32 bit bitmap
                    0
                };

            using (BinaryWriter writer = new BinaryWriter(output))
            {
                writer.Write(header);
                writer.Write(pixelsArr);
            }
        }

        //public static void Write(Color[,] bitmap, int width, int height, Stream output)
        //{
        //    byte[] pixelsArr = new byte[(width * height) * 4];

        //    int width4 = width * 4;
        //    int width8 = width * 8;
        //    int offsetDest = (height - 1) * width4;
        //    for (int y = 0; y < height; y++)
        //    {
        //        for (int x = 0; x < width; x++)
        //        {
        //            Color color = bitmap[x, y];
        //            pixelsArr[offsetDest] = ConvertColorComponent(color.B); // b
        //            pixelsArr[offsetDest + 1] = ConvertColorComponent(color.G); // g
        //            pixelsArr[offsetDest + 2] = ConvertColorComponent(color.R); // r
        //            pixelsArr[offsetDest + 3] = 0xFF; // a

        //            offsetDest += 4;
        //        }
        //        offsetDest -= width8;
        //    }

        //    byte[] header = new byte[]
        //        {
        //            0, // ID length
        //            0, // no color map
        //            2, // uncompressed, true color
        //            0, 0, 0, 0,
        //            0,
        //            0, 0, 0, 0, // x and y origin
        //            (byte) (width & 0x00FF),
        //            (byte) ((width & 0xFF00) >> 8),
        //            (byte) (height & 0x00FF),
        //            (byte) ((height & 0xFF00) >> 8),
        //            32, // 32 bit bitmap
        //            0
        //        };

        //    using (BinaryWriter writer = new BinaryWriter(output))
        //    {
        //        writer.Write(header);
        //        writer.Write(pixelsArr);
        //    }
        //}

        //private static byte ConvertColorComponent(double c)
        //{
        //    return (byte)(((int)Math.Max(0, Math.Min(255, c * 255))) & 255);
        //}
    }
}
