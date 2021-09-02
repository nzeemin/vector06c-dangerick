// Decompiled with JetBrains decompiler
// Type: GulImage.SizeForm
// Assembly: GulImage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 367B3184-2483-40A5-B8C3-9858C62B2ADB
// Assembly location: D:\Work\MyProjects-old\vector06c-rick\lviv\src\Resources\GulImage.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GulImage
{
    public class SizeForm : Form
    {
        private IContainer components = (IContainer)null;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private Label label1;
        private Label label2;
        private Button button1;

        public SizeForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        public int GetX()
        {
            return (int)numericUpDown1.Value;
        }

        public int GetY()
        {
            return (int)numericUpDown2.Value;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            numericUpDown1 = new NumericUpDown();
            numericUpDown2 = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            button1 = new Button();
            numericUpDown1.BeginInit();
            numericUpDown2.BeginInit();
            SuspendLayout();
            numericUpDown1.Location = new Point(12, 25);
            numericUpDown1.Maximum = new Decimal(new int[4]
            {
        24,
        0,
        0,
        0
            });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(47, 20);
            numericUpDown1.TabIndex = 0;
            numericUpDown1.Value = new Decimal(new int[4]
            {
        8,
        0,
        0,
        0
            });
            numericUpDown2.Location = new Point(85, 25);
            numericUpDown2.Maximum = new Decimal(new int[4]
            {
        24,
        0,
        0,
        0
            });
            numericUpDown2.Minimum = new Decimal(new int[4]
            {
        8,
        0,
        0,
        0
            });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(46, 20);
            numericUpDown2.TabIndex = 1;
            numericUpDown2.Value = new Decimal(new int[4]
            {
        8,
        0,
        0,
        0
            });
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(35, 13);
            label1.TabIndex = 2;
            label1.Text = "Width";
            label2.AutoSize = true;
            label2.Location = new Point(82, 9);
            label2.Name = "label2";
            label2.Size = new Size(38, 13);
            label2.TabIndex = 2;
            label2.Text = "Height";
            button1.Location = new Point(32, 51);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 3;
            button1.Text = "OK";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new EventHandler(button1_Click);
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(143, 84);
            Controls.Add((Control)button1);
            Controls.Add((Control)label2);
            Controls.Add((Control)label1);
            Controls.Add((Control)numericUpDown2);
            Controls.Add((Control)numericUpDown1);
            Name = nameof(SizeForm);
            Text = "Image Size";
            numericUpDown1.EndInit();
            numericUpDown2.EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
