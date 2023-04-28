using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
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
        Label titleLabel = new Label();
        
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
            this.img = Image.FromFile(".\\Resources\\background0.png");
            this.Paint += new PaintEventHandler(form_Paint);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 15;
            titleLabel = new Label();
            titleLabel.Font = new Font("コーポレート・ロゴ ver3 Bold", 50);
            titleLabel.Size = new Size(1280, 400);
            titleLabel.Location = new Point(300, 100);
            titleLabel.BackColor = Color.Transparent;
            titleLabel.Parent = this;
            titleLabel.Text = " PCの性能がいいほど\n難易度が上がるテトリス";
            Invalidate();
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
            
        }
        public void form_Paint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(img, 0, 0, 1280, 720);
        }        

    }
}
