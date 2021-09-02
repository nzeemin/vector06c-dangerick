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
    public class Form1 : Form
    {
        private static byte[] b = new byte[8] { 0, 192, 0, 192, 0, 192, 0, byte.MaxValue };
        private static byte[] g = new byte[8] { 0, 0, 192, 192, 0, 0, 192, byte.MaxValue };
        private static byte[] r = new byte[8] { 0, 0, 0, 0, 192, 192, 192, byte.MaxValue };

        private List<string> _tiles = new List<string>();
        private List<Map> _maps = new List<Map>();
        private int _imgSize = 16;
        private bool _isSelectionMode = false;
        private Rectangle _selection = Rectangle.Empty;
        private IContainer components = null;
        private Bitmap _image;
        private Point _highLighted;
        private Bitmap map;
        private Graphics graph;
        private int[,] _buffer;
        private PictureBox pictureBox1;
        private ListView listView1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem imageToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem mapToolStripMenuItem;
        private ToolStripMenuItem tmiSave;
        private ToolStripMenuItem splitToolStripMenuItem;
        private PictureBox pictureBox2;
        private ImageList imageList1;
        private Panel panel1;
        private Panel panel2;
        private Panel pCurCol;
        private Panel pCol1;
        private Panel pCol2;
        private Panel pCol3;
        private Panel pCol4;
        private NumericUpDown numericUpDown1;
        private ToolStripMenuItem saveToolStripMenuItem;
        private Button button1;
        private NumericUpDown numericUpDown2;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem drawToolStripMenuItem;
        private ToolStripMenuItem selectToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private Label label1;
        private Label label3;
        private ColumnHeader columnHeader1;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem showCodesToolStripMenuItem;
        private Label label2;
        private NumericUpDown palNum;
        private ToolStripMenuItem packToolStripMenuItem;
        private SplitContainer splitContainer1;

        public Form1()
        {
            InitializeComponent();
            AllowDrop = true;
            listView1.AllowDrop = true;
            panel1.AllowDrop = true;
            _image = new Bitmap(248, 248);
            map = new Bitmap(32 * _imgSize, 31 * _imgSize);
            graph = Graphics.FromImage(map);
            RefreshImage();
            pictureBox2.Image = map;
            RefreshColors();
            LoadMaps();
        }

        private void LoadMaps()
        {
            _maps.Add(new Map("maps1.txt"));
            _maps.Add(new Map("maps2.txt"));
            _maps.Add(new Map("maps3.txt"));
            _maps.Add(new Map("maps4.txt"));
            RefrshMap();
        }

        private void RefreshColors()
        {
            pCurCol.BackColor = GetColor(0, (int)palNum.Value);
            pCol1.BackColor = GetColor(0, (int)palNum.Value);
            pCol2.BackColor = GetColor(1, (int)palNum.Value);
            pCol3.BackColor = GetColor(2, (int)palNum.Value);
            pCol4.BackColor = GetColor(3, (int)palNum.Value);
            if (!File.Exists("tiles.txt"))
                return;
            LoadImages();
        }

        private Color GetColor(int index, int pal)
        {
            return Color.FromArgb(
                r[Tools.compute_color_index(pal, index)], g[Tools.compute_color_index(pal, index)], b[Tools.compute_color_index(pal, index)]);
        }

        public void LoadImages()
        {
            _tiles.Clear();
            imageList1.Images.Clear();
            listView1.Items.Clear();
            _tiles.AddRange(File.ReadAllLines("tiles.txt"));
            for (int index1 = 0; index1 < _tiles.Count; ++index1)
            {
                var bitmap = new Bitmap(16, 16);
                for (int index2 = 0; index2 < 8; ++index2)
                {
                    string str1 = _tiles[index1]
                        .Split(new string[1] { "\t.db " }, StringSplitOptions.None)[1]
                        .Replace("b", string.Empty);
                    ++index1;
                    string[] strArray = str1.Split(',');
                    var num = new int[2];
                    for (int index3 = 0; index3 < 2; ++index3)
                    {
                        string str2 = strArray[index3].Trim();
                        num[index3] = 0;
                        for (int index4 = 0; index4 < 8; ++index4)
                            num[index3] |=  (str2[index4] != '1' ? 0 : 1) << index4;
                    }
                    for (int index4 = 0; index4 < 8; ++index4)
                    {
                        int colorIndex = Tools.compute_color_index((int)palNum.Value, ((num[0] >> index4) & 1) * 2 + ((num[1] >> index4) & 1));
                        var color = Color.FromArgb(r[colorIndex], g[colorIndex], b[colorIndex]);
                        bitmap.SetPixel(index4 * 2, index2 * 2, color);
                        bitmap.SetPixel(index4 * 2 + 1, index2 * 2, color);
                        bitmap.SetPixel(index4 * 2, index2 * 2 + 1, color);
                        bitmap.SetPixel(index4 * 2 + 1, index2 * 2 + 1, color);
                    }
                }
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
                colors[color] = Tools.compute_color_index((int)palNum.Value, color);
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            Bitmap bitmap1 = new Bitmap(openFileDialog.FileName);
            Bitmap bitmap2 = new Bitmap(bitmap1.Width, bitmap1.Height);
            for (int y = 0; y < bitmap2.Height; ++y)
            {
                for (int x = 0; x < bitmap2.Width; ++x)
                {
                    Color color = GetColor(GetColor(bitmap1.GetPixel(x, y), colors), (int)palNum.Value);
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
            int num = (int)DoDragDrop(e.Item, DragDropEffects.Link);
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
        private byte[] ParseOriginalImage(Bitmap OriginalImage)
        {
            byte[] colors = new byte[4];
            for (int color = 0; color < 4; ++color)
                colors[color] = Tools.compute_color_index((int)palNum.Value, color);
            List<byte> byteList = new List<byte>();
            byte num1 = 0, num2 = 0;
            for (int y = 0; y < OriginalImage.Height; ++y)
            {
                for (int x = 0; x < OriginalImage.Width; ++x)
                {
                    num1 = (byte)(num1 << 1);
                    num2 = (byte)(num2 << 1);

                    byte c = GetColor(OriginalImage.GetPixel(x, y), colors);
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
                Form1 form1 = this;
                Bitmap OriginalImage = new Bitmap(form1.imageList1.Images[listView1.Items[index1].ImageIndex], 8, 8);
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
                            if (bitArray[7 - index4])
                                stringBuilder.Append("1");
                            else
                                stringBuilder.Append("0");
                        }
                        stringBuilder.Append("b, ");
                    }
                    stringBuilder.Remove(stringBuilder.Length - 2, 2);
                    stringBuilder.Append(Environment.NewLine);
                }
                stringBuilder.Append(Environment.NewLine);
            }
            File.WriteAllText("tiles.txt", stringBuilder.ToString());
            foreach (Map map in _maps)
                map.Save(listView1);
        }

        private void RefrshMap()
        {
            _maps[(int)numericUpDown2.Value].RefreshMap(graph, (int)numericUpDown1.Value, imageList1.Images, showCodesToolStripMenuItem.Checked);
            pictureBox2.Image = map;
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
                int num = (int)MessageBox.Show(ex.Message);
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
            RefrshMap();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            RefrshMap();
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isSelectionMode && (e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                _selection.Width = e.X / _imgSize - _selection.X;
                _selection.Height = e.Y / _imgSize - _selection.Y;
                RefrshMap();
                graph.DrawRectangle(Pens.Gray, _selection.X * _imgSize, _selection.Y * _imgSize, _selection.Width * _imgSize, _selection.Height * _imgSize);
            }
            else
            {
                if (_isSelectionMode && _selection != Rectangle.Empty)
                    return;
                Point point = new Point(e.X / _imgSize, e.Y / _imgSize);
                if (_highLighted != point)
                {
                    _highLighted = point;
                    RefrshMap();
                    graph.DrawRectangle(Pens.Gray, _highLighted.X * _imgSize, _highLighted.Y * _imgSize, _imgSize, _imgSize);
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
                Point point = new Point(e.X / _imgSize, e.Y / _imgSize);
                ListViewItem selectedItem = listView1.SelectedItems[0];
                Graphics.FromImage(pictureBox2.Image).DrawImage(imageList1.Images[selectedItem.ImageIndex], point.X * _imgSize, point.Y * _imgSize);
                _maps[(int)numericUpDown2.Value].SetPoint((int)numericUpDown1.Value, point.X, point.Y, selectedItem.ImageIndex);
                pictureBox2.Refresh();
            }
            else
                _selection = !(_selection != Rectangle.Empty) ? new Rectangle(e.X / _imgSize, e.Y / _imgSize, 0, 0) : Rectangle.Empty;
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
            _selection.Width = e.X / _imgSize - _selection.X;
            _selection.Height = e.Y / _imgSize - _selection.Y;
            RefrshMap();
            graph.DrawRectangle(Pens.Gray, _selection.X * _imgSize, _selection.Y * _imgSize, _selection.Width * _imgSize, _selection.Height * _imgSize);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _buffer = new int[_selection.Width, _selection.Height];
            for (int index1 = 0; index1 < _buffer.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < _buffer.GetLength(1); ++index2)
                    _buffer[index1, index2] = _maps[(int)numericUpDown2.Value].GetPoint((int)numericUpDown1.Value, _selection.X + index1, _selection.Y + index2);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int index1 = 0; index1 < _buffer.GetLength(0); ++index1)
            {
                for (int index2 = 0; index2 < _buffer.GetLength(1); ++index2)
                    _maps[(int)numericUpDown2.Value].SetPoint((int)numericUpDown1.Value, _highLighted.X + index1, _highLighted.Y + index2, _buffer[index1, index2]);
            }
            RefrshMap();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            copyToolStripMenuItem.Enabled = _selection != Rectangle.Empty;
            pasteToolStripMenuItem.Enabled = _buffer != null;
        }

        private void showCodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showCodesToolStripMenuItem.Checked = !showCodesToolStripMenuItem.Checked;
            RefrshMap();
        }

        private void palNum_ValueChanged(object sender, EventArgs e)
        {
            RefreshColors();
            RefrshMap();
        }

        private void packToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!File.Exists("MegaLZ.exe"))
                return;
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < _maps.Count; ++index)
            {
                stringBuilder.Append("map");
                stringBuilder.Append(index + 1);
                stringBuilder.AppendLine(":");
                stringBuilder.AppendLine(_maps[index].Pack(listView1));
            }
            File.WriteAllText("Map.asm", stringBuilder.ToString());
            File.Copy("tiles.txt", "Tiles.asm", true);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = (IContainer)new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Form1));
            pictureBox1 = new PictureBox();
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            imageList1 = new ImageList(components);
            menuStrip1 = new MenuStrip();
            mapToolStripMenuItem = new ToolStripMenuItem();
            tmiSave = new ToolStripMenuItem();
            packToolStripMenuItem = new ToolStripMenuItem();
            imageToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            splitToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            drawToolStripMenuItem = new ToolStripMenuItem();
            selectToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            showCodesToolStripMenuItem = new ToolStripMenuItem();
            pictureBox2 = new PictureBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            copyToolStripMenuItem = new ToolStripMenuItem();
            pasteToolStripMenuItem = new ToolStripMenuItem();
            panel1 = new Panel();
            panel2 = new Panel();
            pCurCol = new Panel();
            pCol1 = new Panel();
            pCol2 = new Panel();
            pCol3 = new Panel();
            pCol4 = new Panel();
            numericUpDown1 = new NumericUpDown();
            button1 = new Button();
            numericUpDown2 = new NumericUpDown();
            label1 = new Label();
            label3 = new Label();
            label2 = new Label();
            palNum = new NumericUpDown();
            splitContainer1 = new SplitContainer();
            ((ISupportInitialize)pictureBox1).BeginInit();
            menuStrip1.SuspendLayout();
            ((ISupportInitialize)pictureBox2).BeginInit();
            contextMenuStrip1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            numericUpDown1.BeginInit();
            numericUpDown2.BeginInit();
            palNum.BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(768, 768);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.MouseClick += new MouseEventHandler(pictureBox1_MouseClick);
            listView1.Columns.AddRange(new ColumnHeader[1]
            {
        columnHeader1
            });
            listView1.Dock = DockStyle.Fill;
            listView1.LabelWrap = false;
            listView1.LargeImageList = imageList1;
            listView1.Location = new Point(0, 0);
            listView1.Name = "listView1";
            listView1.Size = new Size(181, 236);
            listView1.SmallImageList = imageList1;
            listView1.TabIndex = 1;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.List;
            listView1.ItemDrag += new ItemDragEventHandler(listView1_ItemDrag);
            listView1.DragDrop += new DragEventHandler(listView1_DragDrop);
            listView1.DragEnter += new DragEventHandler(listView1_DragEnter);
            columnHeader1.Width = 40;
            imageList1.ColorDepth = ColorDepth.Depth8Bit;
            imageList1.ImageSize = new Size(16, 16);
            imageList1.TransparentColor = Color.Transparent;
            menuStrip1.Items.AddRange(new ToolStripItem[4]
            {
        (ToolStripItem) mapToolStripMenuItem,
        (ToolStripItem) imageToolStripMenuItem,
        (ToolStripItem) editToolStripMenuItem,
        (ToolStripItem) viewToolStripMenuItem
            });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(567, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            mapToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
            {
        (ToolStripItem) tmiSave,
        (ToolStripItem) packToolStripMenuItem
            });
            mapToolStripMenuItem.Name = "mapToolStripMenuItem";
            mapToolStripMenuItem.Size = new Size(35, 20);
            mapToolStripMenuItem.Text = "File";
            tmiSave.Name = "tmiSave";
            tmiSave.Size = new Size(98, 22);
            tmiSave.Text = "Save";
            tmiSave.Click += new EventHandler(tmiSave_Click);
            packToolStripMenuItem.Name = "packToolStripMenuItem";
            packToolStripMenuItem.Size = new Size(98, 22);
            packToolStripMenuItem.Text = "Pack";
            packToolStripMenuItem.Click += new EventHandler(packToolStripMenuItem_Click);
            imageToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[4]
            {
        (ToolStripItem) newToolStripMenuItem,
        (ToolStripItem) openToolStripMenuItem,
        (ToolStripItem) saveToolStripMenuItem,
        (ToolStripItem) splitToolStripMenuItem
            });
            imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            imageToolStripMenuItem.Size = new Size(49, 20);
            imageToolStripMenuItem.Text = "Image";
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(112, 22);
            newToolStripMenuItem.Text = "New...";
            newToolStripMenuItem.Click += new EventHandler(newToolStripMenuItem_Click);
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(112, 22);
            openToolStripMenuItem.Text = "Open...";
            openToolStripMenuItem.Click += new EventHandler(openToolStripMenuItem_Click);
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(112, 22);
            saveToolStripMenuItem.Text = "Save...";
            saveToolStripMenuItem.Click += new EventHandler(saveToolStripMenuItem_Click);
            splitToolStripMenuItem.Name = "splitToolStripMenuItem";
            splitToolStripMenuItem.Size = new Size(112, 22);
            splitToolStripMenuItem.Text = "Split";
            splitToolStripMenuItem.Click += new EventHandler(splitToolStripMenuItem_Click);
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
            {
        (ToolStripItem) drawToolStripMenuItem,
        (ToolStripItem) selectToolStripMenuItem
            });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(37, 20);
            editToolStripMenuItem.Text = "Edit";
            drawToolStripMenuItem.Checked = true;
            drawToolStripMenuItem.CheckState = CheckState.Checked;
            drawToolStripMenuItem.Name = "drawToolStripMenuItem";
            drawToolStripMenuItem.Size = new Size(103, 22);
            drawToolStripMenuItem.Text = "Draw";
            drawToolStripMenuItem.Click += new EventHandler(drawToolStripMenuItem_Click);
            selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            selectToolStripMenuItem.Size = new Size(103, 22);
            selectToolStripMenuItem.Text = "Select";
            selectToolStripMenuItem.Click += new EventHandler(selectToolStripMenuItem_Click);
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[1]
            {
        (ToolStripItem) showCodesToolStripMenuItem
            });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(41, 20);
            viewToolStripMenuItem.Text = "View";
            showCodesToolStripMenuItem.Name = "showCodesToolStripMenuItem";
            showCodesToolStripMenuItem.Size = new Size(133, 22);
            showCodesToolStripMenuItem.Text = "Show Codes";
            showCodesToolStripMenuItem.Click += new EventHandler(showCodesToolStripMenuItem_Click);
            pictureBox2.ContextMenuStrip = contextMenuStrip1;
            pictureBox2.Location = new Point(0, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(512, 496);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.MouseDown += new MouseEventHandler(pictureBox2_MouseDown);
            pictureBox2.MouseMove += new MouseEventHandler(pictureBox2_MouseMove);
            pictureBox2.MouseUp += new MouseEventHandler(pictureBox2_MouseUp);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[2]
            {
        (ToolStripItem) copyToolStripMenuItem,
        (ToolStripItem) pasteToolStripMenuItem
            });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(102, 48);
            contextMenuStrip1.Opening += new CancelEventHandler(contextMenuStrip1_Opening);
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new Size(101, 22);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += new EventHandler(copyToolStripMenuItem_Click);
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.Size = new Size(101, 22);
            pasteToolStripMenuItem.Text = "Paste";
            pasteToolStripMenuItem.Click += new EventHandler(pasteToolStripMenuItem_Click);
            panel1.AutoScroll = true;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add((Control)pictureBox2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(210, 236);
            panel1.TabIndex = 6;
            panel2.AutoScroll = true;
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add((Control)pictureBox1);
            panel2.Location = new Point(0, 32);
            panel2.Name = "panel2";
            panel2.Size = new Size(166, 223);
            panel2.TabIndex = 7;
            pCurCol.Location = new Point(1, 273);
            pCurCol.Name = "pCurCol";
            pCurCol.Size = new Size(16, 16);
            pCurCol.TabIndex = 8;
            pCol1.BackColor = Color.Black;
            pCol1.Location = new Point(40, 273);
            pCol1.Name = "pCol1";
            pCol1.Size = new Size(16, 16);
            pCol1.TabIndex = 8;
            pCol1.Click += new EventHandler(pCol1_Click);
            pCol2.BackColor = Color.Yellow;
            pCol2.Location = new Point(62, 273);
            pCol2.Name = "pCol2";
            pCol2.Size = new Size(16, 16);
            pCol2.TabIndex = 8;
            pCol2.Click += new EventHandler(pCol1_Click);
            pCol3.BackColor = Color.Lime;
            pCol3.Location = new Point(84, 273);
            pCol3.Name = "pCol3";
            pCol3.Size = new Size(16, 16);
            pCol3.TabIndex = 8;
            pCol3.Click += new EventHandler(pCol1_Click);
            pCol4.BackColor = Color.Magenta;
            pCol4.Location = new Point(106, 273);
            pCol4.Name = "pCol4";
            pCol4.Size = new Size(16, 16);
            pCol4.TabIndex = 8;
            pCol4.Click += new EventHandler(pCol1_Click);
            numericUpDown1.Location = new Point(353, 27);
            numericUpDown1.Maximum = new Decimal(new int[4]
            {
        7,
        0,
        0,
        0
            });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(39, 20);
            numericUpDown1.TabIndex = 9;
            numericUpDown1.ValueChanged += new EventHandler(numericUpDown1_ValueChanged);
            button1.AllowDrop = true;
            button1.Image = (Image)componentResourceManager.GetObject("button1.Image");
            button1.Location = new Point(143, 266);
            button1.Name = "button1";
            button1.Size = new Size(23, 23);
            button1.TabIndex = 10;
            button1.UseVisualStyleBackColor = true;
            button1.DragDrop += new DragEventHandler(button1_DragDrop);
            button1.DragEnter += new DragEventHandler(button1_DragEnter);
            numericUpDown2.Location = new Point(440, 27);
            numericUpDown2.Maximum = new Decimal(new int[4]
            {
        3,
        0,
        0,
        0
            });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(39, 20);
            numericUpDown2.TabIndex = 13;
            numericUpDown2.ValueChanged += new EventHandler(numericUpDown2_ValueChanged);
            label1.AutoSize = true;
            label1.Location = new Point(303, 29);
            label1.Name = "label1";
            label1.Size = new Size(44, 13);
            label1.TabIndex = 15;
            label1.Text = "Screen:";
            label3.AutoSize = true;
            label3.Location = new Point(398, 29);
            label3.Name = "label3";
            label3.Size = new Size(36, 13);
            label3.TabIndex = 16;
            label3.Text = "Level:";
            label2.AutoSize = true;
            label2.Location = new Point(486, 29);
            label2.Name = "label2";
            label2.Size = new Size(25, 13);
            label2.TabIndex = 18;
            label2.Text = "Pal:";
            palNum.Location = new Point(528, 27);
            palNum.Maximum = new Decimal(new int[4]
            {
        (int) byte.MaxValue,
        0,
        0,
        0
            });
            palNum.Name = "palNum";
            palNum.Size = new Size(39, 20);
            palNum.TabIndex = 17;
            palNum.Value = new Decimal(new int[4]
            {
        12,
        0,
        0,
        0
            });
            palNum.ValueChanged += new EventHandler(palNum_ValueChanged);
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(172, 53);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Panel1.Controls.Add((Control)listView1);
            splitContainer1.Panel2.Controls.Add((Control)panel1);
            splitContainer1.Size = new Size(395, 236);
            splitContainer1.SplitterDistance = 181;
            splitContainer1.TabIndex = 1;
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(567, 310);
            Controls.Add((Control)splitContainer1);
            Controls.Add((Control)label2);
            Controls.Add((Control)palNum);
            Controls.Add((Control)label3);
            Controls.Add((Control)label1);
            Controls.Add((Control)numericUpDown2);
            Controls.Add((Control)button1);
            Controls.Add((Control)numericUpDown1);
            Controls.Add((Control)pCol4);
            Controls.Add((Control)pCol3);
            Controls.Add((Control)pCol2);
            Controls.Add((Control)pCol1);
            Controls.Add((Control)pCurCol);
            Controls.Add((Control)panel2);
            Controls.Add((Control)menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = nameof(Form1);
            Text = "Dangerous Rick Editor";
            WindowState = FormWindowState.Maximized;
            ((ISupportInitialize)pictureBox1).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((ISupportInitialize)pictureBox2).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            numericUpDown1.EndInit();
            numericUpDown2.EndInit();
            palNum.EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
