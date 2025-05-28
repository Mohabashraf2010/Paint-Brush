using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace paint_brush
{
    public partial class Form1 : Form
    {
        List<shape> shapes = new List<shape>();
        shape temp = new shape();
        int shapetype = 0; // 1 for line, 2 for rectangle
        bool isDrawing = false; // flag to indicate if the mouse is being dragged

        Color currcolor = Color.Black; // current color of the shape


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;

            if (shapetype == 1) // line
            {
                temp.x1 = e.X; // starting point of the line
                temp.y1 = e.Y;
                temp.x2 = e.X; // ending point of the line
                temp.y2 = e.Y;
            }
            else if (shapetype == 2) // rectangle
            {
                temp.x1 = e.X; // starting point of the rectangle
                temp.y1 = e.Y;
                temp.x2 = e.X; // ending point of the rectangle
                temp.y2 = e.Y;
            }
        }

        class shape
        {
            public Color color = Color.Black;
            public int x1, y1, x2, y2; // starting and ending points of the shape
        }
        class Line : shape
        {
            
        }

        class myrectangle : shape
        {
            
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {

            if (isDrawing)
            {
                Graphics beta3 = this.CreateGraphics();  //gives me a graphics handle for the form
                Pen pen = new Pen(currcolor);
                Pen erasePen = new Pen(this.BackColor); // create a pen with the background color
                if (shapetype == 1) // line
                {
                    beta3.DrawLine(erasePen, temp.x1, temp.y1, temp.x2, temp.y2); // erase the previous line
                    temp.x2 = e.X; // update the ending point of the line
                    temp.y2 = e.Y;
                    beta3.DrawLine(pen, temp.x1, temp.y1, temp.x2, temp.y2); // draw the new line
                }
                else if (shapetype == 2) // rectangle
                {
                    beta3.DrawRectangle(erasePen, Math.Min(temp.x1, e.X), Math.Min(temp.y1, e.Y), Math.Abs(temp.x1 - e.X), Math.Abs(temp.y1 - e.Y)); // erase the previous rectangle
                                                                                                                                                     // Instead of erasing and redrawing the rectangle on the form's surface, simply invalidate the form to trigger a repaint.
                                                                                                                                                     // Replace the selected lines in Form1_MouseMove with:
                    if (isDrawing)
                    {
                        temp.x2 = e.X;
                        temp.y2 = e.Y;
                        this.Invalidate(); // triggers Form1_Paint, which calls restore()
                    }
                    beta3.DrawRectangle(pen, temp.x1, temp.y1, temp.x2 - temp.x1, temp.y2 - temp.y1); // draw the new rectangle
                }
            }
            restore(); // restore the shapes on the form
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false; // stop drawing
            Graphics beta3 = this.CreateGraphics();  //gives me a graphics handle for the form
            Pen pen = new Pen(Color.Black);
            beta3.DrawLine(pen, temp.x1, temp.y1, temp.x2, temp.y2);

            if (shapetype == 1) // line
            {
                Line newLine = new Line();
                newLine.x1 = temp.x1;
                newLine.y1 = temp.y1;
                newLine.x2 = temp.x2;
                newLine.y2 = temp.y2;
                newLine.color = currcolor;
                shapes.Add(newLine);
            }
            else if (shapetype == 2) // rectangle
            {
                myrectangle newRectangle = new myrectangle();
                newRectangle.x1 = temp.x1;
                newRectangle.y1 = temp.y1;
                newRectangle.x2 = temp.x2;
                newRectangle.y2 = temp.y2;
                newRectangle.color = currcolor;
                shapes.Add(newRectangle);
            }
        }

        void restore()
        {
            Graphics beta3 = this.CreateGraphics();  //gives me a graphics handle for the form
            foreach (shape x in shapes)
            {
                Pen pen1 = new Pen(x.color);

                if (x is Line)
                {
                    Line line = (Line)x;
                    beta3.DrawLine(pen1, line.x1, line.y1, line.x2, line.y2);
                }
                else if (x is myrectangle)
                {
                    myrectangle rectangle = (myrectangle)x;
                    beta3.DrawRectangle(pen1, Math.Min(rectangle.x1, rectangle.x2), Math.Min(rectangle.y1, rectangle.y2), Math.Abs(rectangle.x1 - rectangle.x2), Math.Abs(rectangle.y1 - rectangle.y2));
                }

                pen1.Dispose();
            }
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            restore();
        }

        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shapetype = 1; // set the shape type to line
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            shapetype = 2;
            restore();
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
             currcolor = Color.Red; // set the current color to red
        }

        private void blackToolStripMenuItem_Click(object sender, EventArgs e)
        {
             currcolor = Color.Black;
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
             currcolor = Color.Green;
        }

        // Replace the incorrect block in saveToolStripMenuItem_Click with the following:

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.IO.StreamWriter myFile = new System.IO.StreamWriter("c:/test/shapes.txt");
            foreach (shape x in shapes)
            {
                if (x is Line)
                {
                    myFile.WriteLine("Line");
                }
                else if (x is myrectangle)
                {
                    myFile.WriteLine("Rectangle");
                }
            }
            myFile.Close();
        }
    }
}
