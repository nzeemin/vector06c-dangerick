using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace GulImage
{
    public class Map
    {
        private static Font _font = new Font("Tahoma", 6f, FontStyle.Bold);
        private static Brush _brush = new SolidBrush(Color.White);
        private int width = 32;
        private int heigth = 31;
        private int[] _maps;
        private string _filename;

        public Map(string filename)
        {
            _maps = new int[8 * width * heigth];
            _filename = filename;
            if (!File.Exists(filename))
            {
                for (int index = 0; index < _maps.Length; ++index)
                    _maps[index] = -1;
            }
            else
            {
                string[] strArray1 = File.ReadAllLines(filename);
                int index1 = 0;
                for (int index2 = 0; index2 < heigth; ++index2)
                {
                    for (int index3 = 0; index3 < 8; ++index3)
                    {
                        string[] strArray2 = strArray1[index1].Split(new string[1] { "\t.db " }, StringSplitOptions.None)[1].Split(',');
                        for (int index4 = 0; index4 < width; ++index4)
                            _maps[index3 * 32 + index2 * width * 8 + index4] = int.Parse(strArray2[index4]);
                        ++index1;
                    }
                    ++index1;
                }
            }
        }

        public void RefreshMap(
            Graphics graph,
            int screen,
            ImageList.ImageCollection images,
            bool showNumbers)
        {
            for (int index1 = 0; index1 < heigth; ++index1)
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
                        graph.DrawString(index3.ToString(), Map._font, Map._brush, index2 * 16, index1 * 16);
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

        private byte[] GetBytes(ListView listView)
        {
            List<byte> byteList = new List<byte>();
            for (int index = 0; index < _maps.Length; ++index)
            {
                int map = _maps[index];
                if (map == -1)
                    byteList.Add(byte.MaxValue);
                else
                    byteList.Add((byte)listView.Items[map.ToString()].Index);
            }
            return byteList.ToArray();
        }

        private string PrepareString(byte[] bytes, int bytesInRow, int rowsInChapter)
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

        public void Save(ListView listView)
        {
            File.WriteAllText(_filename, PrepareString(GetBytes(listView), 32, 8));
        }

        public string Pack(ListView listView)
        {
            File.WriteAllBytes("temp.bin", GetBytes(listView));
            Process.Start("MegaLZ.exe", "temp.bin").WaitForExit();
            byte[] bytes = File.ReadAllBytes("temp.bin.mlz");
            File.Delete("temp.bin");
            File.Delete("temp.bin.mlz");
            return PrepareString(bytes, 32, 8);
        }
    }
}
