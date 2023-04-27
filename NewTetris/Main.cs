using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;

namespace Main
{
    class Window : Form
    {
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        public static void Main()
        {
            Application.Run(new Window());
        }
        Window()
        {
            this.Text = "Tetris";
            this.ClientSize = new Size(1280, 720);
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Paint += new PaintEventHandler(form_Paint);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 15;
            timer.Start();
        }
        public void form_Paint(Object sender, PaintEventArgs e)
        {

        }
        public void timer_Tick(Object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}