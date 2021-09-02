using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GulImage
{
    public class Map
    {
        private const int ScreenWidth = 32;
        private const int ScreenHeight = 31;

        private readonly int[] _maps;

        public Map(string filename)
        {
            _maps = new int[8 * ScreenWidth * ScreenHeight];
            Filename = filename;
            if (!File.Exists(filename))
            {
                for (int index = 0; index < _maps.Length; ++index)
                    _maps[index] = -1;
            }
            else
            {
                string[] strArray1 = File.ReadAllLines(filename);
                int index1 = 0;
                for (var index2 = 0; index2 < ScreenHeight; ++index2)
                {
                    for (var index3 = 0; index3 < 8; ++index3)
                    {
                        string[] strArray2 = strArray1[index1].Split(new[] { "\t.db " }, StringSplitOptions.None)[1].Split(',');
                        for (int index4 = 0; index4 < ScreenWidth; ++index4)
                            _maps[index3 * 32 + index2 * ScreenWidth * 8 + index4] = int.Parse(strArray2[index4]);
                        ++index1;
                    }
                    ++index1;
                }
            }
        }

        public string Filename { get; }

        public void RefreshMap(
            Graphics graph,
            Font font,
            Brush brush,
            int screen,
            ImageList.ImageCollection images,
            bool showNumbers)
        {
            for (int index1 = 0; index1 < ScreenHeight; ++index1)
            {
                for (int index2 = 0; index2 < 32; ++index2)
                {
                    int index3 = _maps[screen * 32 + index1 * 32 * 8 + index2];
                    if (index3 >= images.Count)
                        index3 = -1;
                    if (index3 >= 0)
                        graph.DrawImage(images[index3], index2 * 16, index1 * 16);
                    else
                        graph.FillRectangle(new SolidBrush(Color.Gray), index2 * 16, index1 * 16, 16, 16);
                    if (showNumbers)
                        graph.DrawString(index3.ToString(), font, brush, index2 * 16, index1 * 16);
                }
            }
        }

        public void SetPoint(int screen, int x, int y, int value)
        {
            _maps[screen * 32 + y * 32 * 8 + x] = value;
        }

        public int GetPoint(int screen, int x, int y)
        {
            return _maps[screen * 32 + y * 32 * 8 + x];
        }

        public byte[] GetBytes()
        {
            var byteList = new List<byte>();
            for (int index = 0; index < _maps.Length; ++index)
            {
                var tile = _maps[index];
                if (tile == -1)
                    byteList.Add(byte.MaxValue);
                else
                    byteList.Add((byte)tile);
            }
            return byteList.ToArray();
        }
    }
}
