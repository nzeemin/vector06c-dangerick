using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace GulImage
{
    public partial class Form1 : Form
    {
        private static byte[] b = new byte[8] { 0, 192, 0, 192, 0, 192, 0, byte.MaxValue };
        private static byte[] g = new byte[8] { 0, 0, 192, 192, 0, 0, 192, byte.MaxValue };
        private static byte[] r = new byte[8] { 0, 0, 0, 0, 192, 192, 192, byte.MaxValue };

        private const int ImgSize = 16;

        private bool _isSelectionMode = false;
        private Rectangle _selection = Rectangle.Empty;
        private Bitmap _image;
        private Point _highLighted;
        private readonly Bitmap _mapImage;
        private readonly Graphics _graph;
        private int[,] _buffer;

        private static readonly Font _mapFont = new Font("Tahoma", 6f, FontStyle.Bold);
        private static readonly Brush _mapBrush = new SolidBrush(Color.White);

        public Form1()
        {
            InitializeComponent();
            
            AllowDrop = true;
            listView1.AllowDrop = true;
            panel1.AllowDrop = true;

            _image = new Bitmap(248, 248);
            _mapImage = new Bitmap(32 * ImgSize, 31 * ImgSize);
            _graph = Graphics.FromImage(_mapImage);
            RefreshImage();
            pictureBox2.Image = _mapImage;

            RefreshColors();
            RefreshMap();
        }

        private Map CurrentMap => Program._maps[(int)numericUpDown2.Value];

        private int CurrentPalNum => (int)palNum.Value;

        private void RefreshColors()
        {
            pCurCol.BackColor = GetColor(0, CurrentPalNum);
            pCol1.BackColor = GetColor(0, CurrentPalNum);
            pCol2.BackColor = GetColor(1, CurrentPalNum);
            pCol3.BackColor = GetColor(2, CurrentPalNum);
            pCol4.BackColor = GetColor(3, CurrentPalNum);

            LoadImages();
        }

        private static Color GetColor(int index, int pal)
        {
            return Color.FromArgb(
                r[Tools.compute_color_index(pal, index)], g[Tools.compute_color_index(pal, index)], b[Tools.compute_color_index(pal, index)]);
        }

        private void LoadImages()
        {
            imageList1.Images.Clear();
            listView1.Items.Clear();
            for (int index1 = 0; index1 < Program._tiles.Count; ++index1)
            {
                var bytes = Program._tiles[index1];
                var bitmap = Tools.PrepareTileBitmap(CurrentPalNum, bytes);

                imageList1.Images.Add(bitmap);
                ListView.ListViewItemCollection items = listView1.Items;
                int count1 = listView1.Items.Count;
                string key = count1.ToString();
                count1 = listView1.Items.Count;
                string text = count1.ToString();
                int count2 = listView1.Items.Count;
                items.Add(key, text, count2);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            byte[] colors = new byte[4];
            for (int color = 0; color < 4; ++color)
                colors[color] = Tools.compute_color_index(CurrentPalNum, color);
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            Bitmap bitmap1 = new Bitmap(openFileDialog.FileName);
            Bitmap bitmap2 = new Bitmap(bitmap1.Width, bitmap1.Height);
            for (int y = 0; y < bitmap2.Height; ++y)
            {
                for (int x = 0; x < bitmap2.Width; ++x)
                {
                    Color color = GetColor(GetColor(bitmap1.GetPixel(x, y), colors), CurrentPalNum);
                    bitmap2.SetPixel(x, y, color);
                }
            }
            pictureBox1.Width = bitmap2.Width * 3;
            pictureBox1.Height = bitmap2.Height * 3;
            _image = bitmap2;
            RefreshImage();
        }

        private void RefreshImage()
        {
            Bitmap bitmap = new Bitmap(_image.Width * 3, _image.Height * 3);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.DrawImage(_image, 0, 0, _image.Width * 3, _image.Height * 3);
                for (int index = 0; index < _image.Width / 8; ++index)
                {
                    graphics.DrawLine(Pens.White, 0, index * 24 + 23, _image.Width * 3, index * 24 + 23);
                    graphics.DrawLine(Pens.White, index * 24 + 23, 0, index * 24 + 23, _image.Height * 3);
                }
            }
            pictureBox1.Image = bitmap;
        }

        private void splitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int index1 = 0; index1 < _image.Height / 8; ++index1)
            {
                for (int index2 = 0; index2 < _image.Width / 8; ++index2)
                {
                    imageList1.Images.Add(_image.Clone(new Rectangle(index2 * 8, index1 * 8, 8, 8), PixelFormat.Undefined));
                    ListView.ListViewItemCollection items = listView1.Items;
                    int count1 = listView1.Items.Count;
                    string key = count1.ToString();
                    count1 = listView1.Items.Count;
                    string text = count1.ToString();
                    int count2 = listView1.Items.Count;
                    items.Add(key, text, count2);
                }
            }
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

        //NOTE: Changed for Vector-06C video
        private byte[] ParseOriginalImage(Bitmap originalImage)
        {
            byte[] colors = new byte[4];
            for (int color = 0; color < 4; ++color)
                colors[color] = Tools.compute_color_index(CurrentPalNum, color);
            List<byte> byteList = new List<byte>();
            byte num1 = 0, num2 = 0;
            for (int y = 0; y < originalImage.Height; ++y)
            {
                for (int x = 0; x < originalImage.Width; ++x)
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

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void tmiSave_Click(object sender, EventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int index1 = 0; index1 < listView1.Items.Count; ++index1)
            {
                var tileImage = imageList1.Images[listView1.Items[index1].ImageIndex];
                Bitmap OriginalImage = new Bitmap(tileImage, 8, 8);
                byte[] originalImage = ParseOriginalImage(OriginalImage);
                stringBuilder.Append("tile" + index1 + ":");
                for (int index2 = 0; index2 < OriginalImage.Height; ++index2)
                {
                    stringBuilder.Append("\t.db ");
                    for (int index3 = 0; index3 < OriginalImage.Width / 4; ++index3)
                    {
                        BitArray bitArray = new BitArray(new byte[1]
                        {
                            originalImage[index3 + index2 * OriginalImage.Width / 4]
                        });
                        for (int index4 = 0; index4 < 8; ++index4)
                        {
                            stringBuilder.Append(bitArray[7 - index4] ? "1" : "0");
                        }
                        stringBuilder.Append("b, ");
                    }
                    stringBuilder.Remove(stringBuilder.Length - 2, 2);
                    stringBuilder.Append(Environment.NewLine);
                }
                stringBuilder.Append(Environment.NewLine);
            }
            File.WriteAllText("tiles.txt", stringBuilder.ToString());

            Program.SaveMaps();
        }

        private void RefreshMap()
        {
            CurrentMap.RefreshMap(
                _graph, _mapFont, _mapBrush,
                (int)numericUpDown1.Value, imageList1.Images, showCodesToolStripMenuItem.Checked);
            pictureBox2.Image = _mapImage;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SizeForm sizeForm = new SizeForm();
                if (sizeForm.ShowDialog() != DialogResult.OK)
                    return;
                _image = new Bitmap(sizeForm.GetX(), sizeForm.GetY());
                using (Graphics graphics = Graphics.FromImage(_image))
                    graphics.FillRectangle(new SolidBrush(pCurCol.BackColor), 0, 0, _image.Width, _image.Height);
                RefreshImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (_image == null || _image.Width <= e.X / 3 || _image.Height <= e.Y / 3)
                return;
            _image.SetPixel(e.X / 3, e.Y / 3, pCurCol.BackColor);
            RefreshImage();
        }

        private void pCol1_Click(object sender, EventArgs e)
        {
            pCurCol.BackColor = ((Control)sender).BackColor;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = "png";
            saveFileDialog.Filter = "png|*.png";
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;
            _image.Save(saveFileDialog.FileName, ImageFormat.Png);
        }

        private void button1_DragDrop(object sender, DragEventArgs e)
        {
            listView1.Items.Remove((ListViewItem)e.Data.GetData(typeof(ListViewItem)));
        }

        private void button1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            RefreshMap();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            RefreshMap();
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isSelectionMode && (e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                _selection.Width = e.X / ImgSize - _selection.X;
                _selection.Height = e.Y / ImgSize - _selection.Y;
                RefreshMap();
                _graph.DrawRectangle(Pens.Gray, _selection.X * ImgSize, _selection.Y * ImgSize, _selection.Width * ImgSize, _selection.Height * ImgSize);
            }
            else
            {
                if (_isSelectionMode && _selection != Rectangle.Empty)
                    return;
                Point point = new Point(e.X / ImgSize, e.Y / ImgSize);
                if (_highLighted != point)
                {
                    _highLighted = point;
                    RefreshMap();
                    _graph.DrawRectangle(Pens.Gray, _highLighted.X * ImgSize, _highLighted.Y * ImgSize, ImgSize, ImgSize);
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
                Point point = new Point(e.X / ImgSize, e.Y / ImgSize);
                ListViewItem selectedItem = listView1.SelectedItems[0];
                Graphics.FromImage(pictureBox2.Image).DrawImage(imageList1.Images[selectedItem.ImageIndex], point.X * ImgSize, point.Y * ImgSize);
                CurrentMap.SetPoint((int)numericUpDown1.Value, point.X, point.Y, selectedItem.ImageIndex);
                pictureBox2.Refresh();
            }
            else
                _selection = !(_selection != Rectangle.Empty) ? new Rectangle(e.X / ImgSize, e.Y / ImgSize, 0, 0) : Rectangle.Empty;
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

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left || (!_isSelectionMode || _selection == Rectangle.Empty))
                return;
            _selection.Width = e.X / ImgSize - _selection.X;
            _selection.Height = e.Y / ImgSize - _selection.Y;
            RefreshMap();
            _graph.DrawRectangle(Pens.Gray, _selection.X * ImgSize, _selection.Y * ImgSize, _selection.Width * ImgSize, _selection.Height * ImgSize);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _buffer = new int[_selection.Width, _selection.Height];
            for (int index1 = 0; index1 < _buffer.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < _buffer.GetLength(1); ++index2)
                    _buffer[index1, index2] = CurrentMap.GetPoint((int)numericUpDown1.Value, _selection.X + index1, _selection.Y + index2);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int index1 = 0; index1 < _buffer.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < _buffer.GetLength(1); ++index2)
                    CurrentMap.SetPoint((int)numericUpDown1.Value, _highLighted.X + index1, _highLighted.Y + index2, _buffer[index1, index2]);
            }
            RefreshMap();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            copyToolStripMenuItem.Enabled = _selection != Rectangle.Empty;
            pasteToolStripMenuItem.Enabled = _buffer != null;
        }

        private void showCodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showCodesToolStripMenuItem.Checked = !showCodesToolStripMenuItem.Checked;
            RefreshMap();
        }

        private void palNum_ValueChanged(object sender, EventArgs e)
        {
            RefreshColors();
            RefreshMap();
        }

        private void packToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.PackMaps();
        }
    }
}
