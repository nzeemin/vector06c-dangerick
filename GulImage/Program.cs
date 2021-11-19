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

        private static readonly string[] MapFileNames =
        {
            "maps1.txt", "maps2.txt", "maps3.txt", "maps4.txt"
        };

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ReadMaps();
            ReadTiles();

            Application.Run(new Form1());
        }

        private static void ReadMaps()
        {
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
        }

        public static void SaveMaps()
        {
            for (var n = 0; n < _maps.Count; n++)
            {
                var map = _maps[n];
                //TODO: Move to Map class
                var contents = Tools.PrepareAsmDbStrings(map.GetBytes(), 32, 8);
                File.WriteAllText(MapFileNames[n], contents);
            }
        }

        private static void ReadTiles()
        {
            var fileContent = File.ReadAllLines("tiles.txt");
            _tiles = Tools.ParseTiles(fileContent);
        }

        public static void SaveTiles()
        {
            var fileContent = Tools.PrepareTilesFileContent(_tiles);
            File.WriteAllText("tiles.txt", fileContent);
        }

        public static void PackMaps()
        {
            if (!File.Exists("MegaLZ.exe"))
                return; //TODO: Show message

            var sb = new StringBuilder();
            for (var n = 0; n < Program._maps.Count; ++n)
            {
                var map = _maps[n];

                File.WriteAllBytes("temp.bin", map.GetBytes());
                
                Process.Start("MegaLZ.exe", "temp.bin").WaitForExit();

                byte[] bytes = File.ReadAllBytes("temp.bin.mlz");
                File.Delete("temp.bin");
                File.Delete("temp.bin.mlz");
                var asmStrings = Tools.PrepareAsmDbStrings(bytes, 32, 8);

                sb.AppendLine($"map{n + 1}:");
                sb.AppendLine(asmStrings);
            }

            File.WriteAllText("Map.asm", sb.ToString());
            File.Copy("tiles.txt", "Tiles.asm", true);
        }
    }
}
