using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private int resolution;

        private bool[,] field;
        private int rows;
        private int columns;

        public Form1()
        {
            InitializeComponent();
        }
        
        private void StartGane()
        {
            if (timer1.Enabled)
                return;

            nudResolution.Enabled = false;
            nudDensity.Enabled = false;

            resolution = (int)nudResolution.Value;
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);

            rows = pictureBox1.Height / resolution;
            columns = pictureBox1.Width / resolution;
            field = new bool[columns, rows];

            Random random = new Random();
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next((int)nudDensity.Value) == 0;
                }
            }

            timer1.Start();
        }
        private void StopGame()
        {
            if (!timer1.Enabled)
                return;
            nudResolution.Enabled = true;
            nudDensity.Enabled = true;
        }

        private void NextGeneration()
        {
            graphics.Clear(Color.Black);
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    if (field[x, y])
                    {
                        graphics.FillRectangle(Brushes.DarkRed, x * resolution, y * resolution, resolution, resolution);
                    }
                }
            }
            pictureBox1.Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneration();
        }

        private void bStart_Click(object sender, EventArgs e)
        {
            StartGane();
        }

        private void bStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
