using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FloydAlg
{
    public partial class Form1 : Form
    {
        private Bitmap inBm;
        private Bitmap outBm;
        private int width;
        private int height;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void loadImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)

            {

                inBm = new Bitmap(Image.FromFile(openFileDialog1.FileName));

                width = inBm.Width;

                height = inBm.Height;

                pictureBox1.Size = new Size(width, height);

                pictureBox1.Image = inBm;

                textBox1.Visible = true;
                label1.Visible = true;
                


            }
        }

        private void grayScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < width; i++)

                for (int j = 0; j < height; j++)

                {

                    int c = inBm.GetPixel(i, j).R;

                    Color r = Color.FromArgb(c, c, c);

                    inBm.SetPixel(i, j, r);

                }

            pictureBox1.Image = inBm;
        }

        private int Approximate(int c)

        {

            if (c >= 0 && c < 63)

                return 0;

            if (c >= 63 && c < 127)

                return 63;

            if (c >= 127 && c < 191)

                return 127;

            if (c >= 191 && c < 255)

                return 191;

            else

                return 255;

        }

        private void quantizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            outBm = new Bitmap(width, height);

            for (int i = 0; i < width; i++)

                for (int j = 0; j < height; j++)

                {

                    int c = inBm.GetPixel(i, j).R;

                    int d = Approximate(c);

                    outBm.SetPixel(i, j, Color.FromArgb(d, d, d));

                }

            pictureBox1.Image = outBm;
        }

        private void floydSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int r, r1, r2, r3, r4;
            int error;
            outBm = new Bitmap(width, height);

            string mString = textBox1.Text;
            int m = Int32.Parse(mString);


            for (int i = width - 2; i > 0; i--)
                for (int j = 1; j < height - 1; j++)
                {
                    int c = inBm.GetPixel(i, j).R;
                    r = Approximate(c);

                    if(r==63 || r==127 || r==191 || r==255)
                    {
                        Pen pen = new Pen(Color.FromArgb(r, r, r));
                        using(var graphics = Graphics.FromImage(outBm))
                        {
                            graphics.DrawLine(pen,
                            i,
                            j,
                            i + m,
                            j + m);
                        }

                        error = c - r - (m-1);
                    }
                    else
                    {
                        error = c;
                    }
                    outBm.SetPixel(i, j, Color.FromArgb(r, r, r));
                    //error = c - r;

                    r1 = inBm.GetPixel(i, j + 1).R + 7 * error / 16;
                    r2 = inBm.GetPixel(i + 1, j - 1).R + 3 * error / 16;
                    r3 = inBm.GetPixel(i + 1, j).R + 5 * error / 16;
                    r4 = inBm.GetPixel(i + 1, j + 1).R + error / 16;

                    if (r1 >= 255) r1 = 255;
                    if (r2 >= 255) r2 = 255;
                    if (r3 >= 255) r3 = 255;
                    if (r4 >= 255) r4 = 255;

                    if (r1 < 0) r1 = 0;
                    if (r2 < 0) r2 = 0;
                    if (r3 < 0) r3 = 0;
                    if (r4 < 0) r4 = 0;

                    outBm.SetPixel(i, j + 1, Color.FromArgb(r1, r1, r1));
                    outBm.SetPixel(i + 1, j - 1, Color.FromArgb(r2, r2, r2));
                    outBm.SetPixel(i + 1, j, Color.FromArgb(r3, r3, r3));
                    outBm.SetPixel(i + 1, j + 1, Color.FromArgb(r4, r4, r4));
                }
            pictureBox1.Image = outBm;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
