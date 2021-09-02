using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace GulImage
{
    internal static class Program
    {
        public static readonly List<Map> _maps = new List<Map>();
        public static List<byte[]> _tiles;

        private static readonly string[] MapFileNames = { "maps1.txt", "maps2.txt", "maps3.txt", "maps4.txt" };

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            foreach (var filename in MapFileNames)
            {
                var map = new Map();
                if (File.Exists(filename))
                {
                    string[] mapFileContent = File.ReadAllLines(filename);
                    map.ParseMap(mapFileContent);
                }
                _maps.Add(map);
            }

            var fileContent = File.ReadAllLines("tiles.txt");
            _tiles = Tools.ParseTiles(fileContent);

            Application.Run(new Form1());
        }

        public static void SaveMaps()
        {
            for (var i = 0; i < _maps.Count; i++)
            {
                var map = _maps[i];
                File.WriteAllText(
                    MapFileNames[i],
                    Tools.PrepareAsmDbStrings(map.GetBytes(), 32, 8));
            }
        }

        public static void PackMaps()
        {
            if (!File.Exists("MegaLZ.exe"))
                return; //TODO: Show message

            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < Program._maps.Count; ++index)
            {
                var map = _maps[index];

                File.WriteAllBytes("temp.bin", map.GetBytes());
                Process.Start("MegaLZ.exe", "temp.bin").WaitForExit();
                byte[] bytes = File.ReadAllBytes("temp.bin.mlz");
                File.Delete("temp.bin");
                File.Delete("temp.bin.mlz");
                var asmStrings = Tools.PrepareAsmDbStrings(bytes, 32, 8);

                stringBuilder.Append("map");
                stringBuilder.Append(index + 1);
                stringBuilder.AppendLine(":");
                stringBuilder.AppendLine(asmStrings);
            }

            File.WriteAllText("Map.asm", stringBuilder.ToString());
            File.Copy("tiles.txt", "Tiles.asm", true);
        }
    }
}
