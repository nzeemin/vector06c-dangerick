using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using GulImage.Properties;

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

        public static string PrepareAsmDbStrings(byte[] bytes, int bytesInRow, int rowsInChapter)
        {
            var sb = new StringBuilder();
            int index1 = 0;
            for (var index2 = 0; index2 < bytes.Length / bytesInRow / rowsInChapter + 1 && index1 < bytes.Length; ++index2)
            {
                for (var index3 = 0; index3 < rowsInChapter && index1 < bytes.Length; ++index3)
                {
                    sb.Append("\t.db ");
                    for (int index4 = 0; index4 < bytesInRow; ++index4)
                    {
                        index1 = index3 * bytesInRow + index2 * bytesInRow * rowsInChapter + index4;
                        if (index1 >= bytes.Length)
                            break;

                        byte num = bytes[index1];
                        sb.Append(num);
                        sb.Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(Environment.NewLine);
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public static List<byte[]> ParseTiles(string[] fileContent)
        {
            var result = new List<byte[]>();
            for (var index1 = 0; index1 < fileContent.Length; ++index1)
            {
                var bytes = new byte[16];
                for (var index2 = 0; index2 < 8; ++index2)
                {
                    var str1 = fileContent[index1]
                        .Split(new[] { "\t.db " }, StringSplitOptions.None)[1]
                        .Replace("b", string.Empty);
                    string[] strArray = str1.Split(',');
                    for (int index3 = 0; index3 < 2; ++index3)
                    {
                        var str2 = strArray[index3].Trim();
                        int num = 0;
                        for (var index4 = 0; index4 < 8; ++index4)
                            num |= (str2[index4] != '1' ? 0 : 1) << index4;
                        bytes[index2 * 2 + index3] = (byte)num;
                    }
                    ++index1;
                }
                result.Add(bytes);
            }

            return result;
        }

        public static string PrepareTilesFileContent(List<byte[]> tiles)
        {
            var sb = new StringBuilder();

            for (var tile = 0; tile < tiles.Count; ++tile)
            {
                var tileBytes = tiles[tile];
                sb.Append($"tile{tile}:");
                for (var y = 0; y < 8; y++)
                {
                    sb.Append("\t.db ");
                    for (var t = 0; t < 2; t++)
                    {
                        byte b = tileBytes[y * 2 + t];
                        for (var i = 0; i < 8; i++)
                        {
                            sb.Append((b >> (7 - i) & 1) == 1 ? '1' : '0');
                        }
                        sb.Append("b");
                        if (t == 0)
                            sb.Append(", ");
                    }
                    sb.AppendLine();
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static void DrawTile(Graphics graph, int palette, byte[] bytes, int x = 0, int y = 0, int scale = 2)
        {
            for (var ty = 0; ty < 8; ++ty)
            {
                var num0 = bytes[ty * 2];
                var num1 = bytes[ty * 2 + 1];
                for (var tx = 0; tx < 8; ++tx)
                {
                    int colorIndex = compute_color_index(palette, ((num0 >> tx) & 1) * 2 + ((num1 >> tx) & 1));
                    var color = Color.FromArgb(r[colorIndex], g[colorIndex], b[colorIndex]);
                    using (var brush = new SolidBrush(color))
                    {
                        graph.FillRectangle(brush, x + tx * scale, y + ty * scale, scale, scale);
                    }
                }
            }
        }

        public static void DrawScreen(
            Map map,
            Graphics graph,
            Font font,
            Brush brush,
            int screen,
            bool showNumbers,
            int palette,
            int scale = 2)
        {
            var tiles = Program._tiles;

            for (var y = 0; y < Map.ScreenHeight; ++y)
            {
                for (var x = 0; x < Map.ScreenWidth; ++x)
                {
                    int tile = map.GetTile(screen, x, y);
                    if (tile >= tiles.Count)
                        tile = -1;
                    if (tile >= 0)
                        Tools.DrawTile(graph, palette, tiles[tile], x * 8 * scale, y * 8 * scale, scale);
                    else
                        graph.FillRectangle(new SolidBrush(Color.Gray), x * 8 * scale, y * 8 * scale, 8 * scale, 8 * scale);
                    if (showNumbers)
                        graph.DrawString(tile.ToString(), font, brush, x * 8 * scale, y * 8 * scale);
                }
            }
        }

        public static Bitmap CreateTileBitmap(int palette, byte[] bytes, int scale = 2)
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
