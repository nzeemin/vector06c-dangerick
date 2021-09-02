using GulImage.Properties;
using System.Collections.Generic;
using System.Drawing;

namespace GulImage
{
    public class Tools
    {
        private static byte BLACK = 0;
        private static byte BLUE = 1;
        private static byte GREEN = 2;
        private static byte RED = 4;
        private static byte[] b = new byte[8] { 0, 192, 0, 192, 0, 192, 0, byte.MaxValue };
        private static byte[] g = new byte[8] { 0, 0, 192, 192, 0, 0, 192, byte.MaxValue };
        private static byte[] r = new byte[8] { 0, 0, 0, 0, 192, 192, 192, byte.MaxValue };
        private static List<byte> palettes;

        public static byte compute_color_index(int port, int color)
        {
            byte black = Tools.BLACK;
            if ((port & 64) != 0)
                black ^= Tools.BLUE;
            if ((port & 32) != 0)
                black ^= Tools.GREEN;
            if ((port & 16) != 0)
                black ^= Tools.RED;
            switch (color)
            {
                case 0:
                    if ((port & 8) == 0)
                        black ^= Tools.RED;
                    if ((port & 4) == 0)
                    {
                        black ^= Tools.BLUE;
                        break;
                    }
                    break;
                case 1:
                    black ^= Tools.BLUE;
                    if ((port & 1) == 0)
                    {
                        black ^= Tools.RED;
                        break;
                    }
                    break;
                case 2:
                    black ^= Tools.GREEN;
                    break;
                case 3:
                    black ^= Tools.RED;
                    if ((port & 2) == 0)
                    {
                        black ^= Tools.GREEN;
                        break;
                    }
                    break;
            }
            return black;
        }

        public static List<byte> GetDiffPalettes()
        {
            if (Tools.palettes == null)
            {
                Tools.palettes = new List<byte>();
                foreach (char ch in Resources.pal)
                    Tools.palettes.Add((byte)ch);
            }
            return Tools.palettes;
        }

        public static Color GetColorFromNumber(int number)
        {
            return Color.FromArgb((int)Tools.r[number], (int)Tools.g[number], (int)Tools.b[number]);
        }
    }
}
