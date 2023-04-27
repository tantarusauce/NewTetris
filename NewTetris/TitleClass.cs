using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TitleClass
{
    public class Title : Form
    {
        private Image img;
        public bool next;
        private int i = 0;

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        public Title()
        {
            this.next = false;
            this.Text = "Tetris";
            this.ClientSize = new Size(1280, 720);
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.img = Image.FromFile(".\\resources\\background0.png");
            this.Paint += new PaintEventHandler(form_Paint);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 15;
        }
        public void timer_Start()
        {
            timer.Start();
        }
        public void timer_Tick(Object sender, EventArgs e)
        {
            i++;
            if(i > 500)
            {
                next = true;
            }
            Invalidate();
        }
        public void form_Paint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(img, 0, 0, 1280, 720);
        }        

    }
}
