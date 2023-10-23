using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private int resolution;

        private bool[,] field;
        private int rows;
        private int columns;
        private int currentGen = 0;

        public Form1()
        {
            InitializeComponent();
        }
        
        private void StartGane()
        {
            currentGen = 1;
            if (timer1.Enabled)
                return;

            nudResolution.Enabled = false;
            nudDensity.Enabled = false;

            resolution = (int)nudResolution.Value;

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
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }
        private void StopGame()
        {
            if (!timer1.Enabled)
                return;
            timer1.Stop();
            nudResolution.Enabled = true;
            nudDensity.Enabled = true;
        }

        private void NextGeneration()
        {
            graphics.Clear(Color.Black);

            bool[,] newfield = new bool[columns, rows];

            
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int neighboursCount = CountNeighbours(x, y);
                    bool hasLife = field[x, y];

                    if (!hasLife && neighboursCount == 3)
                    {
                        newfield[x, y] = true;
                    }
                    else if (hasLife && (neighboursCount < 2 || neighboursCount > 3))
                    {
                        newfield[x, y] = false;
                    }
                    else
                    {
                        newfield[x, y] = field[x, y];
                    }

                    if (hasLife)
                    {
                        graphics.FillRectangle(Brushes.DarkRed, x * resolution, y * resolution, resolution, resolution);
                    }
                }
            }
            field = newfield;
            pictureBox1.Refresh();

            currentGen++;
            lblCurrentGen.Text = currentGen.ToString();
        }

        private int CountNeighbours(int x, int y)
        {
            int count = 0;

            for (int i = -1; i < 2; i++)
            {
                for(int j = -1; j < 2; j++) 
                {
                    int col = (x + i + columns) % columns;
                    int row = (y + j + rows) % rows;

                    bool isSelfChecking = col == x && row == y;
                    bool hasLife = field[col, row];

                    if (hasLife && !isSelfChecking)
                    {
                        count++;
                    }
                }
            }
            return count;
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
            Text = "Game of Life v.4";
        }

        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblCurrentGen_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        { 
            if(!timer1.Enabled)
            {
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                int x = e.Location.X / resolution;
                int y = e.Location.Y / resolution;
                if (ValidateMousePosition(x, y))
                {
                    field[x, y] = true;
                }
            }
            else if(e.Button == MouseButtons.Right)
            {
                int x = e.Location.X / resolution;
                int y = e.Location.Y / resolution;
                if (ValidateMousePosition(x, y))
                {
                    field[x, y] = false;
                }
            }
        }

        private bool ValidateMousePosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < columns && y < rows;
        }
    }
}
