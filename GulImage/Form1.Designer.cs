namespace GulImage
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = (System.ComponentModel.IContainer)new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            mapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tmiSave = new System.Windows.Forms.ToolStripMenuItem();
            packToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            splitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            drawToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            showCodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pictureBox2 = new System.Windows.Forms.PictureBox();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            panel1 = new System.Windows.Forms.Panel();
            panel2 = new System.Windows.Forms.Panel();
            pCurCol = new System.Windows.Forms.Panel();
            pCol1 = new System.Windows.Forms.Panel();
            pCol2 = new System.Windows.Forms.Panel();
            pCol3 = new System.Windows.Forms.Panel();
            pCol4 = new System.Windows.Forms.Panel();
            numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            button1 = new System.Windows.Forms.Button();
            numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            label1 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            palNum = new System.Windows.Forms.NumericUpDown();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
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
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new System.Drawing.Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(768, 768);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(pictureBox1_MouseClick);
            // 
            listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[1]
            {
        columnHeader1
            });
            listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            listView1.LabelWrap = false;
            listView1.LargeImageList = imageList1;
            listView1.Location = new System.Drawing.Point(0, 0);
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(181, 236);
            listView1.SmallImageList = imageList1;
            listView1.TabIndex = 1;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = System.Windows.Forms.View.List;
            listView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(listView1_ItemDrag);
            listView1.DragDrop += new System.Windows.Forms.DragEventHandler(listView1_DragDrop);
            listView1.DragEnter += new System.Windows.Forms.DragEventHandler(listView1_DragEnter);
            columnHeader1.Width = 40;
            imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageList1.ImageSize = new System.Drawing.Size(16, 16);
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4]
            {
        (System.Windows.Forms.ToolStripItem) mapToolStripMenuItem,
        (System.Windows.Forms.ToolStripItem) imageToolStripMenuItem,
        (System.Windows.Forms.ToolStripItem) editToolStripMenuItem,
        (System.Windows.Forms.ToolStripItem) viewToolStripMenuItem
            });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(567, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            mapToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2]
            {
        (System.Windows.Forms.ToolStripItem) tmiSave,
        (System.Windows.Forms.ToolStripItem) packToolStripMenuItem
            });
            mapToolStripMenuItem.Name = "mapToolStripMenuItem";
            mapToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            mapToolStripMenuItem.Text = "File";
            tmiSave.Name = "tmiSave";
            tmiSave.Size = new System.Drawing.Size(98, 22);
            tmiSave.Text = "Save";
            tmiSave.Click += new System.EventHandler(tmiSave_Click);
            packToolStripMenuItem.Name = "packToolStripMenuItem";
            packToolStripMenuItem.Size = new System.Drawing.Size(98, 22);
            packToolStripMenuItem.Text = "Pack";
            packToolStripMenuItem.Click += new System.EventHandler(packToolStripMenuItem_Click);
            imageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4]
            {
        (System.Windows.Forms.ToolStripItem) newToolStripMenuItem,
        (System.Windows.Forms.ToolStripItem) openToolStripMenuItem,
        (System.Windows.Forms.ToolStripItem) saveToolStripMenuItem,
        (System.Windows.Forms.ToolStripItem) splitToolStripMenuItem
            });
            imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            imageToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            imageToolStripMenuItem.Text = "Image";
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            newToolStripMenuItem.Text = "New...";
            newToolStripMenuItem.Click += new System.EventHandler(newToolStripMenuItem_Click);
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            openToolStripMenuItem.Text = "Open...";
            openToolStripMenuItem.Click += new System.EventHandler(openToolStripMenuItem_Click);
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            saveToolStripMenuItem.Text = "Save...";
            saveToolStripMenuItem.Click += new System.EventHandler(saveToolStripMenuItem_Click);
            splitToolStripMenuItem.Name = "splitToolStripMenuItem";
            splitToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            splitToolStripMenuItem.Text = "Split";
            splitToolStripMenuItem.Click += new System.EventHandler(splitToolStripMenuItem_Click);
            editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2]
            {
        (System.Windows.Forms.ToolStripItem) drawToolStripMenuItem,
        (System.Windows.Forms.ToolStripItem) selectToolStripMenuItem
            });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            editToolStripMenuItem.Text = "Edit";
            drawToolStripMenuItem.Checked = true;
            drawToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            drawToolStripMenuItem.Name = "drawToolStripMenuItem";
            drawToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            drawToolStripMenuItem.Text = "Draw";
            drawToolStripMenuItem.Click += new System.EventHandler(drawToolStripMenuItem_Click);
            selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            selectToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            selectToolStripMenuItem.Text = "Select";
            selectToolStripMenuItem.Click += new System.EventHandler(selectToolStripMenuItem_Click);
            viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1]
            {
        (System.Windows.Forms.ToolStripItem) showCodesToolStripMenuItem
            });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            viewToolStripMenuItem.Text = "View";
            showCodesToolStripMenuItem.Name = "showCodesToolStripMenuItem";
            showCodesToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            showCodesToolStripMenuItem.Text = "Show Codes";
            showCodesToolStripMenuItem.Click += new System.EventHandler(showCodesToolStripMenuItem_Click);
            pictureBox2.ContextMenuStrip = contextMenuStrip1;
            pictureBox2.Location = new System.Drawing.Point(0, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new System.Drawing.Size(512, 496);
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            pictureBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(pictureBox2_MouseDown);
            pictureBox2.MouseMove += new System.Windows.Forms.MouseEventHandler(pictureBox2_MouseMove);
            pictureBox2.MouseUp += new System.Windows.Forms.MouseEventHandler(pictureBox2_MouseUp);
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2]
            {
        (System.Windows.Forms.ToolStripItem) copyToolStripMenuItem,
        (System.Windows.Forms.ToolStripItem) pasteToolStripMenuItem
            });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(102, 48);
            contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(contextMenuStrip1_Opening);
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += new System.EventHandler(copyToolStripMenuItem_Click);
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.Size = new System.Drawing.Size(101, 22);
            pasteToolStripMenuItem.Text = "Paste";
            pasteToolStripMenuItem.Click += new System.EventHandler(pasteToolStripMenuItem_Click);
            panel1.AutoScroll = true;
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel1.Controls.Add((System.Windows.Forms.Control)pictureBox2);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(210, 236);
            panel1.TabIndex = 6;
            panel2.AutoScroll = true;
            panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel2.Controls.Add((System.Windows.Forms.Control)pictureBox1);
            panel2.Location = new System.Drawing.Point(0, 32);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(166, 223);
            panel2.TabIndex = 7;
            pCurCol.Location = new System.Drawing.Point(1, 273);
            pCurCol.Name = "pCurCol";
            pCurCol.Size = new System.Drawing.Size(16, 16);
            pCurCol.TabIndex = 8;
            pCol1.BackColor = System.Drawing.Color.Black;
            pCol1.Location = new System.Drawing.Point(40, 273);
            pCol1.Name = "pCol1";
            pCol1.Size = new System.Drawing.Size(16, 16);
            pCol1.TabIndex = 8;
            pCol1.Click += new System.EventHandler(pCol1_Click);
            pCol2.BackColor = System.Drawing.Color.Yellow;
            pCol2.Location = new System.Drawing.Point(62, 273);
            pCol2.Name = "pCol2";
            pCol2.Size = new System.Drawing.Size(16, 16);
            pCol2.TabIndex = 8;
            pCol2.Click += new System.EventHandler(pCol1_Click);
            pCol3.BackColor = System.Drawing.Color.Lime;
            pCol3.Location = new System.Drawing.Point(84, 273);
            pCol3.Name = "pCol3";
            pCol3.Size = new System.Drawing.Size(16, 16);
            pCol3.TabIndex = 8;
            pCol3.Click += new System.EventHandler(pCol1_Click);
            pCol4.BackColor = System.Drawing.Color.Magenta;
            pCol4.Location = new System.Drawing.Point(106, 273);
            pCol4.Name = "pCol4";
            pCol4.Size = new System.Drawing.Size(16, 16);
            pCol4.TabIndex = 8;
            pCol4.Click += new System.EventHandler(pCol1_Click);
            numericUpDown1.Location = new System.Drawing.Point(353, 27);
            numericUpDown1.Maximum = new System.Decimal(new int[4]
            {
        7,
        0,
        0,
        0
            });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new System.Drawing.Size(39, 20);
            numericUpDown1.TabIndex = 9;
            numericUpDown1.ValueChanged += new System.EventHandler(numericUpDown1_ValueChanged);
            button1.AllowDrop = true;
            button1.Image = (System.Drawing.Image)componentResourceManager.GetObject("button1.Image");
            button1.Location = new System.Drawing.Point(143, 266);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(23, 23);
            button1.TabIndex = 10;
            button1.UseVisualStyleBackColor = true;
            button1.DragDrop += new System.Windows.Forms.DragEventHandler(button1_DragDrop);
            button1.DragEnter += new System.Windows.Forms.DragEventHandler(button1_DragEnter);
            numericUpDown2.Location = new System.Drawing.Point(440, 27);
            numericUpDown2.Maximum = new System.Decimal(new int[4]
            {
        3,
        0,
        0,
        0
            });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new System.Drawing.Size(39, 20);
            numericUpDown2.TabIndex = 13;
            numericUpDown2.ValueChanged += new System.EventHandler(numericUpDown2_ValueChanged);
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(303, 29);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(44, 13);
            label1.TabIndex = 15;
            label1.Text = "Screen:";
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(398, 29);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(36, 13);
            label3.TabIndex = 16;
            label3.Text = "Level:";
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(486, 29);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(25, 13);
            label2.TabIndex = 18;
            label2.Text = "Pal:";
            palNum.Location = new System.Drawing.Point(528, 27);
            palNum.Maximum = new System.Decimal(new int[4]
            {
        (int) byte.MaxValue,
        0,
        0,
        0
            });
            palNum.Name = "palNum";
            palNum.Size = new System.Drawing.Size(39, 20);
            palNum.TabIndex = 17;
            palNum.Value = new System.Decimal(new int[4]
            {
        12,
        0,
        0,
        0
            });
            palNum.ValueChanged += new System.EventHandler(palNum_ValueChanged);
            splitContainer1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            splitContainer1.Location = new System.Drawing.Point(172, 53);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Panel1.Controls.Add((System.Windows.Forms.Control)listView1);
            splitContainer1.Panel2.Controls.Add((System.Windows.Forms.Control)panel1);
            splitContainer1.Size = new System.Drawing.Size(395, 236);
            splitContainer1.SplitterDistance = 181;
            splitContainer1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 310);
            Controls.Add((System.Windows.Forms.Control)splitContainer1);
            Controls.Add((System.Windows.Forms.Control)label2);
            Controls.Add((System.Windows.Forms.Control)palNum);
            Controls.Add((System.Windows.Forms.Control)label3);
            Controls.Add((System.Windows.Forms.Control)label1);
            Controls.Add((System.Windows.Forms.Control)numericUpDown2);
            Controls.Add((System.Windows.Forms.Control)button1);
            Controls.Add((System.Windows.Forms.Control)numericUpDown1);
            Controls.Add((System.Windows.Forms.Control)pCol4);
            Controls.Add((System.Windows.Forms.Control)pCol3);
            Controls.Add((System.Windows.Forms.Control)pCol2);
            Controls.Add((System.Windows.Forms.Control)pCol1);
            Controls.Add((System.Windows.Forms.Control)pCurCol);
            Controls.Add((System.Windows.Forms.Control)panel2);
            Controls.Add((System.Windows.Forms.Control)menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = nameof(Form1);
            this.Text = "Dangerous Rick Editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            numericUpDown1.EndInit();
            numericUpDown2.EndInit();
            palNum.EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tmiSave;
        private System.Windows.Forms.ToolStripMenuItem splitToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pCurCol;
        private System.Windows.Forms.Panel pCol1;
        private System.Windows.Forms.Panel pCol2;
        private System.Windows.Forms.Panel pCol3;
        private System.Windows.Forms.Panel pCol4;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem drawToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showCodesToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown palNum;
        private System.Windows.Forms.ToolStripMenuItem packToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}