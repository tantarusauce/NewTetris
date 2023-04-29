using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using static System.Windows.Forms.DataFormats;

namespace Main
{
    public class Window : Form
    {
        private bool constr = true;
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        public static void Main()
        {
            Application.Run(new Window());
        }
        public Window()
        {
            this.Text = "Tetris";
            this.ClientSize = new Size(1280, 720);
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            TitleClass.Title tit = new TitleClass.Title();
            this.Paint += new PaintEventHandler(form_Paint);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 30;
            timer.Start();
            tit.timer_Start();
            tit.Show();
        }
        public void form_Paint(Object sender, PaintEventArgs e)
        {
            
        }
        public void timer_Tick(Object sender, EventArgs e)
        {
            if (constr)
            {
                this.Visible = false;
                constr = false;
            }
            Invalidate();
        }
    }
}