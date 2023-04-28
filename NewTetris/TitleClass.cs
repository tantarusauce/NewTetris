using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TetrisGame;

namespace TitleClass
{
    public class Title : Form
    {
        Images im = new Images();
        Label titleLabel = new Label();
        PictureBox startClick = new PictureBox();
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private bool game_In_Progress = false;
        public Title()
        {
            this.Visible = true;
            this.Text = "Tetris";
            this.ClientSize = new Size(1280, 720);
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Paint += new PaintEventHandler(paint);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 1;
            startClick = new PictureBox();
            startClick.Image = im.tp;
            startClick.Size = new Size(500, 100);
            startClick.Location = new Point(390, 500);
            startClick.BackColor = Color.Transparent;
            startClick.Parent = this;
            titleLabel = new Label();
            titleLabel.Font = new Font("コーポレート・ロゴ ver3 Bold", 50);
            titleLabel.Size = new Size(1280, 400);
            titleLabel.Location = new Point(320, 100);
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
            //イベントハンドラ
            startClick.MouseEnter += new EventHandler(mouse_Enter);
            startClick.MouseLeave += new EventHandler(mouse_Leave);
            startClick.MouseClick += new MouseEventHandler(mouse_Click);
            this.KeyDown += new KeyEventHandler(key_Down);
        }
        public void paint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(im.img, 0, 0, 1280, 720);
            if (im.mouse_In)
            {
                //スタートボタンにマウスカーソルが乗ったとき
                g.DrawImage(im.start_Gray, 390, 500, 500, 100);
            }
            else
            {
                //スタートボタンにマウスカーソルが載っていないとき
                g.DrawImage(im.start, 390, 500, 500, 100);
            }
        }
        public void mouse_Click(Object sender, MouseEventArgs e)
        {
            next();
        }
        public void mouse_Enter(Object sender, EventArgs e)
        {
            im.mouse_In = true;
            Invalidate();
        }
        public void mouse_Leave(Object sender, EventArgs e)
        {
            im.mouse_In = false;
            Invalidate();
        }
        public void key_Down(Object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space)
            {
                next();
            }
        }
        public void next()
        {
            if (!game_In_Progress)
            {
                game_In_Progress = true;
                //ゲーム画面への遷移
                startClick.Dispose();
                titleLabel.Dispose();
                this.Visible = false;
                timer.Stop();
                TetrisGame.Tetris tetris = new TetrisGame.Tetris();
                tetris.Show();
                this.Dispose();
            }
        }
    }
    public class Images
    {
        public Image img, start, start_Gray, tp;
        public bool mouse_In;
        public Images()
        {
            this.img = Image.FromFile(".\\Resources\\background0.png");
            this.start = Image.FromFile(".\\Resources\\START.png");
            this.start_Gray = Image.FromFile(".\\Resources\\START_gray.png");
            this.tp = Image.FromFile(".\\Resources\\mino_-1.png");
        }
    }
}
