using System;
using GulImage.Properties;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GulImage
{
    public static class Tools
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

        public static string PrepareAsmDbStrings(byte[] bytes, int bytesInRow, int rowsInChapter)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int index1 = 0;
            for (int index2 = 0; index2 < bytes.Length / bytesInRow / rowsInChapter + 1 && index1 < bytes.Length; ++index2)
            {
                for (int index3 = 0; index3 < rowsInChapter && index1 < bytes.Length; ++index3)
                {
                    stringBuilder.Append("\t.db ");
                    for (int index4 = 0; index4 < bytesInRow; ++index4)
                    {
                        index1 = index3 * bytesInRow + index2 * bytesInRow * rowsInChapter + index4;
                        if (index1 < bytes.Length)
                        {
                            byte num = bytes[index1];
                            stringBuilder.Append(num);
                            stringBuilder.Append(",");
                        }
                        else
                            break;
                    }
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                    stringBuilder.Append(Environment.NewLine);
                }
                stringBuilder.Append(Environment.NewLine);
            }
            return stringBuilder.ToString();
        }

        public static List<byte[]> ParseTiles(string[] fileContent)
        {
            List<byte[]> result = new List<byte[]>();
            for (int index1 = 0; index1 < fileContent.Length; ++index1)
            {
                var bytes = new byte[16];
                for (var index2 = 0; index2 < 8; ++index2)
                {
                    string str1 = fileContent[index1]
                        .Split(new[] { "\t.db " }, StringSplitOptions.None)[1]
                        .Replace("b", string.Empty);
                    string[] strArray = str1.Split(',');
                    for (int index3 = 0; index3 < 2; ++index3)
                    {
                        var str2 = strArray[index3].Trim();
                        int num = 0;
                        for (int index4 = 0; index4 < 8; ++index4)
                            num |= (str2[index4] != '1' ? 0 : 1) << index4;
                        bytes[index2 * 2 + index3] = (byte)num;
                    }
                    ++index1;
                }
                result.Add(bytes);
            }

            return result;
        }

        public static void DrawTile(Graphics graph, int palette, byte[] bytes, int x = 0, int y = 0, int scale = 2)
        {
            for (int ty = 0; ty < 8; ++ty)
            {
                var num0 = bytes[ty * 2];
                var num1 = bytes[ty * 2 + 1];
                for (var tx = 0; tx < 8; ++tx)
                {
                    int colorIndex = compute_color_index(palette, ((num0 >> tx) & 1) * 2 + ((num1 >> tx) & 1));
                    var color = Color.FromArgb(r[colorIndex], g[colorIndex], b[colorIndex]);
                    graph.FillRectangle(new SolidBrush(color), x + tx * scale, y + ty * scale, scale, scale);
                }
            }
        }

        public static Bitmap PrepareTileBitmap(int palette, byte[] bytes, int scale = 2)
        {
            var bitmap = new Bitmap(8 * scale, 8 * scale);
            using (var graph = Graphics.FromImage(bitmap))
            {
                DrawTile(graph, palette, bytes, 0, 0, scale);
            }

            return bitmap;
        }
    }
}
