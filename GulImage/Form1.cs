using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GulImage
{
    public partial class Form1 : Form
    {
        private static byte[] b = new byte[8] { 0, 192, 0, 192, 0, 192, 0, byte.MaxValue };
        private static byte[] g = new byte[8] { 0, 0, 192, 192, 0, 0, 192, byte.MaxValue };
        private static byte[] r = new byte[8] { 0, 0, 0, 0, 192, 192, 192, byte.MaxValue };

        private const int ScreenScale = 2;

        private bool _isSelectionMode = false;
        private Rectangle _selection = Rectangle.Empty;
        private Point _highLighted;
        private readonly Bitmap _screenImage;
        private readonly Graphics _graph;
        private int[,] _buffer;  // Copy/paste buffer
        private int _currentLevel;

        private static readonly Font _mapFont = new Font("Tahoma", 6f, FontStyle.Bold);
        private static readonly Brush _mapBrush = new SolidBrush(Color.White);

        public Form1()
        {
            InitializeComponent();
            
            AllowDrop = true;
            listView1.AllowDrop = true;
            panel1.AllowDrop = true;

            pictureBox2.Width = Map.ScreenWidth * 8 * ScreenScale;
            pictureBox2.Height = Map.ScreenHeight * 8 * ScreenScale;

            _screenImage = new Bitmap(Map.ScreenWidth * 8 * ScreenScale, Map.ScreenHeight * 8 * ScreenScale);
            _graph = Graphics.FromImage(_screenImage);
            pictureBox2.Image = _screenImage;

            comboBoxLevel.SelectedIndex = 0;

            RefreshColors();
            RefreshScreen();
        }

        private Map CurrentMap => Program._maps[_currentLevel];

        private int CurrentPalNum => (int)palNum.Value;

        private int CurrentScreen => (int)numericUpDownScreen.Value;

        private void RefreshColors()
        {
            RefreshTiles();
        }

        private void RefreshTiles()
        {
            imageList1.Images.Clear();
            listView1.Items.Clear();
            for (var tile = 0; tile < Program._tiles.Count; ++tile)
            {
                var bytes = Program._tiles[tile];
                var bitmap = Tools.CreateTileBitmap(CurrentPalNum, bytes, 1);

                imageList1.Images.Add(bitmap);
                var items = listView1.Items;
                int count1 = listView1.Items.Count;
                string key = count1.ToString();
                count1 = listView1.Items.Count;
                string text = count1.ToString();
                int count2 = listView1.Items.Count;
                items.Add(key, text, count2);
            }
        }

        private void RefreshScreen()
        {
            if (_graph == null)
                return;

            Tools.DrawScreen(
                CurrentMap,
                _graph, _mapFont, _mapBrush,
                CurrentScreen, showCodesToolStripMenuItem.Checked,
                CurrentPalNum, ScreenScale);
            pictureBox2.Image = _screenImage;
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Link);
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            Point client = listView1.PointToClient(new Point(e.X, e.Y));
            ListViewItem itemAt = listView1.GetItemAt(client.X, client.Y);
            ListViewItem data = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
            listView1.Items.Remove(data);
            if (itemAt != null)
                listView1.Items.Insert(itemAt.Index, data);
            else
                listView1.Items.Add(data);
            listView1.Alignment = ListViewAlignment.Default;
            listView1.Alignment = ListViewAlignment.Top;
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private byte[] ParseOriginalImage(Bitmap originalImage)
        {
            var colors = new byte[4];
            for (var color = 0; color < 4; ++color)
                colors[color] = Tools.compute_color_index(CurrentPalNum, color);
            var byteList = new List<byte>();
            byte num1 = 0, num2 = 0;
            for (var y = 0; y < originalImage.Height; ++y)
            {
                for (var x = 0; x < originalImage.Width; ++x)
                {
                    num1 = (byte)(num1 << 1);
                    num2 = (byte)(num2 << 1);

                    byte c = GetColor(originalImage.GetPixel(x, y), colors);
                    num1 = (byte)(num1 | ((c >> 1) & 1));
                    num2 = (byte)(num2 | (c & 1));

                    if (x % 8 == 7)
                    {
                        byteList.Add(num1);
                        byteList.Add(num2);
                        num1 = 0;
                        num2 = 0;
                    }
                }
            }
            return byteList.ToArray();
        }

        // Returns color index 0..3
        private static byte GetColor(Color lCol, byte[] colors)
        {
            int num1 = int.MaxValue;
            byte num2 = 0;
            for (byte index = 0; index < 4; ++index)
            {
                byte color = colors[index];
                int num3 = Math.Abs(lCol.R - r[color]) + Math.Abs(lCol.G - g[color]) + Math.Abs(lCol.B - b[color]);
                if (num3 < num1)
                {
                    num1 = num3;
                    num2 = index;
                }
            }
            return num2;
        }

        // Save tiles and all the maps
        private void tmiSave_Click(object sender, EventArgs e)
        {
            var tileCount = listView1.Items.Count;
            var tiles = new List<byte[]>(tileCount);
            for (var tile = 0; tile < tileCount; ++tile)
            {
                var tileImage = imageList1.Images[listView1.Items[tile].ImageIndex];
                var originalImage = new Bitmap(tileImage, 8, 8);
                tiles.Add(ParseOriginalImage(originalImage));
            }
            Program._tiles = tiles;
            Program.SaveTiles();

            Program.SaveMaps();
        }

        private void packToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.PackMaps();
        }

        private void numericUpDownScreen_ValueChanged(object sender, EventArgs e)
        {
            RefreshScreen();
        }

        private void comboBoxLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentLevel = comboBoxLevel.SelectedIndex;

            RefreshScreen();
        }

        private void palNum_ValueChanged(object sender, EventArgs e)
        {
            RefreshColors();
            RefreshScreen();
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isSelectionMode && (e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                _selection.Width = e.X / (8 * ScreenScale) - _selection.X;
                _selection.Height = e.Y / (8 * ScreenScale) - _selection.Y;
                RefreshScreen();
                _graph.DrawRectangle(Pens.Gray, _selection.X * 8 * ScreenScale, _selection.Y * 8 * ScreenScale, _selection.Width * 8 * ScreenScale, _selection.Height * 8 * ScreenScale);
            }
            else
            {
                if (_isSelectionMode && _selection != Rectangle.Empty)
                    return;
                var point = new Point(e.X / (8 * ScreenScale), e.Y / (8 * ScreenScale));
                if (_highLighted != point)
                {
                    _highLighted = point;
                    RefreshScreen();
                    _graph.DrawRectangle(Pens.Gray, _highLighted.X * 8 * ScreenScale, _highLighted.Y * 8 * ScreenScale, 8 * ScreenScale, 8 * ScreenScale);
                }
            }
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
                return;
            if (!_isSelectionMode)
            {
                if (listView1.SelectedItems.Count <= 0)
                    return;
                Point point = new Point(e.X / (8 * ScreenScale), e.Y / (8 * ScreenScale));
                ListViewItem selectedItem = listView1.SelectedItems[0];
                Graphics.FromImage(pictureBox2.Image).DrawImage(imageList1.Images[selectedItem.ImageIndex], point.X * 8 * ScreenScale, point.Y * 8 * ScreenScale);
                CurrentMap.SetTile(CurrentScreen, point.X, point.Y, selectedItem.ImageIndex);
                pictureBox2.Refresh();
            }
            else
                _selection = !(_selection != Rectangle.Empty) ? new Rectangle(e.X / (8 * ScreenScale), e.Y / (8 * ScreenScale), 0, 0) : Rectangle.Empty;
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left || (!_isSelectionMode || _selection == Rectangle.Empty))
                return;
            _selection.Width = e.X / (8 * ScreenScale) - _selection.X;
            _selection.Height = e.Y / (8 * ScreenScale) - _selection.Y;
            RefreshScreen();
            _graph.DrawRectangle(Pens.Gray, _selection.X * 8 * ScreenScale, _selection.Y * 8 * ScreenScale, _selection.Width * 8 * ScreenScale, _selection.Height * 8 * ScreenScale);
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _isSelectionMode = true;
            _selection = Rectangle.Empty;
            selectToolStripMenuItem.Checked = _isSelectionMode;
            drawToolStripMenuItem.Checked = !_isSelectionMode;
        }

        private void drawToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _isSelectionMode = false;
            selectToolStripMenuItem.Checked = _isSelectionMode;
            drawToolStripMenuItem.Checked = !_isSelectionMode;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _buffer = new int[_selection.Width, _selection.Height];
            for (int index1 = 0; index1 < _buffer.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < _buffer.GetLength(1); ++index2)
                    _buffer[index1, index2] = CurrentMap.GetTile(CurrentScreen, _selection.X + index1, _selection.Y + index2);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int index1 = 0; index1 < _buffer.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < _buffer.GetLength(1); ++index2)
                    CurrentMap.SetTile(CurrentScreen, _highLighted.X + index1, _highLighted.Y + index2, _buffer[index1, index2]);
            }
            RefreshScreen();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            copyToolStripMenuItem.Enabled = _selection != Rectangle.Empty;
            pasteToolStripMenuItem.Enabled = _buffer != null;
        }

        private void showCodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showCodesToolStripMenuItem.Checked = !showCodesToolStripMenuItem.Checked;
            RefreshScreen();
        }
    }
}
