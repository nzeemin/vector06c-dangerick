﻿using System;
using System.Collections.Generic;
using System.Drawing;

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

        public void ParseMap(string[] fileContent)
        {
            int index1 = 0;
            for (var row = 0; row < ScreenHeight; ++row)
            {
                for (var screen = 0; screen < 8; ++screen)
                {
                    string[] strArray2 = fileContent[index1].Split(new[] { "\t.db " }, StringSplitOptions.None)[1].Split(',');
                    for (var col = 0; col < ScreenWidth; ++col)
                        _maps[screen * 32 + row * ScreenWidth * 8 + col] = int.Parse(strArray2[col]);
                    ++index1;
                }
                ++index1;
            }
        }

        //TODO: Move to Form1
        public void DrawScreen(
            Graphics graph,
            Font font,
            Brush brush,
            int screen,
            bool showNumbers,
            int palette,
            int scale = 2)
        {
            var tiles = Program._tiles;

            for (var y = 0; y < ScreenHeight; ++y)
            {
                for (var x = 0; x < ScreenWidth; ++x)
                {
                    int tile = GetTile(screen, x, y);
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

        public void SetTile(int screen, int x, int y, int value)
        {
            _maps[screen * ScreenWidth + y * ScreenWidth * 8 + x] = value;
        }

        public int GetTile(int screen, int x, int y)
        {
            return _maps[screen * ScreenWidth + y * ScreenWidth * 8 + x];
        }

        public byte[] GetBytes()
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
