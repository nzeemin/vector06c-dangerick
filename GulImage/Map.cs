using System;
using System.Collections.Generic;

namespace GulImage
{
    public class Map
    {
        public const int ScreenWidth = 32;
        public const int ScreenHeight = 31;

        private readonly int[] _maps;

        public Map()
        {
            _maps = new int[8 * ScreenWidth * ScreenHeight];
            for (int index = 0; index < _maps.Length; ++index)
                _maps[index] = -1;
        }

        public void SetTile(int screen, int x, int y, int value)
        {
            _maps[screen * ScreenWidth + y * ScreenWidth * 8 + x] = value;
        }

        public int GetTile(int screen, int x, int y)
        {
            return _maps[screen * ScreenWidth + y * ScreenWidth * 8 + x];
        }

        public void ParseMap(string[] fileContent)
        {
            var index1 = 0;
            for (var row = 0; row < ScreenHeight; ++row)
            {
                for (var screen = 0; screen < 8; ++screen)
                {
                    var strArray2 = fileContent[index1]
                        .Split(new[] { "\t.db " }, StringSplitOptions.None)[1]
                        .Split(',');
                    for (var col = 0; col < ScreenWidth; ++col)
                        SetTile(screen, col, row, int.Parse(strArray2[col]));
                    ++index1;
                }
                ++index1;
            }
        }

        public byte[] ToByteArray()
        {
            var byteList = new List<byte>();
            for (var index = 0; index < _maps.Length; ++index)
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
