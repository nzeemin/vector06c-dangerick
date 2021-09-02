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
        public static List<Map> _maps = new List<Map>();
        public static List<byte[]> _tiles;

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _maps.Add(new Map("maps1.txt"));
            _maps.Add(new Map("maps2.txt"));
            _maps.Add(new Map("maps3.txt"));
            _maps.Add(new Map("maps4.txt"));

            var fileContent = File.ReadAllLines("tiles.txt");
            _tiles = Tools.ParseTiles(fileContent);

            Application.Run(new Form1());
        }

        public static void SaveMaps()
        {
            foreach (var map in _maps)
            {
                File.WriteAllText(map.Filename, Tools.PrepareAsmDbStrings(map.GetBytes(), 32, 8));
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
