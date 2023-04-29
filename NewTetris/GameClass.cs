using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using NewTetris;
namespace TetrisGame
{
    class Tetris : Form
    {
        private Image[] img = new Image[14];
        private Image[,] bg = new Image[17, 24];
        private Image backg, holdImg, scoreSheet, bcgr, minom1img, restart, restart_Gray, exit, exit_Gray;
        private Mino m;
        private Game gam;
        private Label scoreLabel;
        private Label retryLabel;
        private Label exitLabel;
        Random rn = new Random();
        private int scene = 3;//1:None 2:gameinit 3:game 
        private int timer = 0;
        private int rc = 0;
        private int rct = 0;
        private int count = 0;
        private int timerCount = 0;
        private bool keysUp = false;
        private bool retryMouseEnter = false;
        private bool exitMouseEnter = false;
        private bool gotoTitle = false;
        private int[] a = { 1, 2, 3, 4, 5, 5, 4, 3, 2, 1 };
        System.Windows.Forms.Timer tm = new System.Windows.Forms.Timer();

        public Tetris()
        {
            loadImage();
            init();
            this.Text = "faketetris";
            this.ClientSize = new Size(1280, 720);
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Paint += new PaintEventHandler(fm_Paint);
            this.KeyDown += new KeyEventHandler(fm_KeyDown);
            tm.Tick += new EventHandler(tm_Tick);
        }

        public void loadImage()
        {
            minom1img = Image.FromFile(".\\resources\\mino_-1.png");
            scoreSheet = Image.FromFile(".\\resources\\scoreInd.png");
            backg = Image.FromFile(".\\resources\\background.png");
            bcgr = Image.FromFile(".\\resources\\background0.png");
            holdImg = Image.FromFile(".\\resources\\hold.png");
            restart = Image.FromFile(".\\resources\\Restart.png");
            restart_Gray = Image.FromFile(".\\resources\\Restart_gray.png");
            exit = Image.FromFile(".\\resources\\EXIT.png");
            exit_Gray = Image.FromFile(".\\resources\\EXIT_gray.png");
            for (int i = 0; i <= 13; i++)
            {
                img[i] = Image.FromFile(".\\resources\\mino_" + i + ".png");
            }
        }
        public void fm_Paint(Object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Point flm = m.fallingMino;
            g.DrawImage(bcgr, 0, 0, 1280, 720);
            for (int j = 0; j < 24; j++)
            {
                for (int i = 0; i < 16; i++)
                {
                    g.DrawImage(bg[i, j], 560 + i * 30, j * 30 - 30 * 3, 32, 32);
                }
            }

            g.DrawImage(scoreSheet, 30, 30, 600, 300);
            g.DrawImage(holdImg, 500, 200, 128, 128);    
            
            string scoreStr;
            scoreStr = (gam.combo == 0) ? "\n" : "";
            scoreStr += "SCORE:" + gam.score.ToString();
            scoreStr += (gam.score == 0) ? "" : "0";
            scoreStr += "\n\nLINE(S):" + gam.deletedRow.ToString() + "\n\nLEVEL:" + gam.level.ToString();
            scoreStr += (gam.combo == 0) ? "" : "\n\nCOMBO:" + gam.combo.ToString();
            scoreLabel.Text = scoreStr;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    g.DrawImage(m.minoImageHold[j, i], 535 + j * 15, 245 + i * 15, 16, 16);
                }
            }

            switch (scene)
            {
                case 3:
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            g.DrawImage(m.minoImage1[j, i], m.point.X + j * 30, (flm.Y + i + fallShadow() - 4) * 30, 32, 32);
                        }
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            g.DrawImage(m.minoImage0[j, i], m.point.X + j * 30, m.point.Y + i * 30, 32, 32);
                        }
                    }
                    break;
                case 5:
                    int transp1 = retryMouseEnter ? 8 : 1;
                    int transp2 = exitMouseEnter ? 9 : 2;
                    for (int j = 0; j < 5; j++)
                    {
                        g.DrawImage(img[transp1], j * 60 + 600, 240, 64, 64);
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        g.DrawImage(img[transp2], j * 55 + 640, 480, 60, 60);

                    }
                    break;
            }
        }
        public void fm_KeyDown(Object sender, KeyEventArgs e)
        {
            if(scene == 3)
            {
                if ((e.KeyCode == Keys.Right) & (rightOk()))
                {
                    Point flm = m.fallingMino;
                    flm.X += 1;
                    m.fallingMino = flm;
                    Point point = m.point;
                    point.X += 30;
                    m.point = point;
                }
                else if ((e.KeyCode == Keys.Left) & (leftOk()))
                {
                    Point flm = m.fallingMino;
                    flm.X -= 1;
                    m.fallingMino = flm;
                    Point point = m.point;
                    point.X -= 30;
                    m.point = point;
                }
                else if (e.KeyCode == Keys.Up)
                {
                    keysUp = true;
                    while (lookBottom() == true)
                    {
                        fall(0);
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (fallOk() == true)
                    {
                        gam.veryfast = true;
                        fall(0);
                        gam.veryfast = false;
                    }
                }
                else if ((e.KeyCode == Keys.Z) & (rotaterightOk()))
                {
                    if (m.deg == 3)
                    {
                        m.deg = 0;
                    }
                    else
                    {
                        m.deg++;
                    }
                    minoDeg();
                }
                else if ((e.KeyCode == Keys.X) & (rotateleftOk()))
                {
                    if (m.deg == 0)
                    {
                        m.deg = 3;
                    }
                    else
                    {
                        m.deg--;
                    }
                    minoDeg();
                }
                else if (e.KeyCode == Keys.C)
                {
                    if (holdOk())
                    {
                        if (gam.hold == -3)
                        {
                            gam.hold = gam.kind;
                            gam.kind = gam.hidHold;
                        }
                        else
                        {
                            int temp;
                            temp = gam.hold;
                            gam.hold = gam.kind;
                            gam.kind = temp;
                        }
                        if (gam.holded == false)
                        {
                            m.fallingMino = new Point(6, 0);
                            Point minop = new Point(740, -90);
                            m.point = minop;
                            gam.holded = true;
                        }
                        minoDeg();
                    }
                }
            }
        }
        public void retry_MouseEnter(Object sender, EventArgs e)
        {
            retryMouseEnter = true;
        }
        public void retry_MouseLeave(Object sender, EventArgs e)
        {
            retryMouseEnter = false;
        }
        public void retry_MouseClick(Object sender, EventArgs e)
        {
            scene = 1;
        }
        public void exit_MouseEnter(Object sender, EventArgs e)
        {
            exitMouseEnter = true;
        }
        public void exit_MouseLeave(Object sender, EventArgs e)
        {
            exitMouseEnter = false;
        }
        public void exit_MouseClick(Object sender, EventArgs e)
        {
            if (!gotoTitle)
            {
                gotoTitle = true;
                TitleClass.Title tit = new TitleClass.Title();
                tit.timer_Start();
                tit.Show();
                this.Dispose();
            }
        }
        public void gameDispose()
        {
            scoreLabel.Dispose();
            exitLabel.Dispose();
            retryLabel.Dispose();
        }
        public void init()
        {
            tm.Interval = 20;
            m = new Mino();
            gam = new Game();
            int[,] initMino = {{-2, -2, -2, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -2, -2, -2},
                               {-2, -2, -2, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -2, -2, -2},
                               {-2, -2, -2, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -2, -2, -2},
                               {-2, -2, -2, -3, -3, -3, -3, -3, -3, -3, -3, -3, -3, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -2, -2, -2},
                               {-2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2},
                               {-2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2},
                               {-2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2}};
            int d = 0;
            int k = rn.Next(7);
            m.deg = d;
            m.fallingMino = new Point(6, 0);
            Point minop = new Point(740, -90);
            m.point = minop;
            gam.placedMino = initMino;
            gam.kind = rn.Next(7);
            gam.hidHold = rn.Next(7);
            gam.hold = -3;
            gam.veryfast = false;
            gam.score = 0;
            gam.deletedRow = 0;
            gam.level = 0;
            gam.combo = 0;
            gam.holded = false;
            gam.tickCount = 0;
            gam.fallTick = 0;
            int[] initDRN = { -1, -1, -1, -1 };
            gam.deletedRowNum = initDRN;
            scoreLabel = new Label();
            scoreLabel.Font = new Font("MS UI Gothic", 30);
            scoreLabel.Size = new Size(500, 300);
            scoreLabel.Location = new Point(30, 30);
            scoreLabel.BackColor = Color.Transparent;
            scoreLabel.Parent = this;
            bgPaint();
            minoDeg();
            tm.Start();
        }
        public void bgPaint()
        {
            for (int j = 0; j < 24; j++)
            {
                for (int i = 0; i < 16; i++)
                {
                    switch (gam.placedMino[j, i])
                    {
                        case -3:
                            bg[i, j] = minom1img;
                            break;
                        case -2:
                            bg[i, j] = minom1img;
                            break;
                        case -1:
                            bg[i, j] = backg;
                            break;
                        default:
                            bg[i, j] = img[gam.placedMino[j, i]];
                            break;
                    }
                }
            }
        }

        public void minoDeg()
        {
            int d;
            d = m.deg;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    d = m.deg;
                    m.minoImage0[j, i] = (m.minoShape[gam.kind, d, i, j] == 1) ? img[gam.kind] : minom1img;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    d = m.deg;
                    m.minoImage1[j, i] = (m.minoShape[gam.kind, d, i, j] == 1) ? img[gam.kind + 7] : minom1img;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    d = m.deg;
                    if (gam.hold == -3)
                    {
                        m.minoImageHold[j, i] = backg;
                    }
                    else
                    {
                        m.minoImageHold[j, i] = (m.minoShape[gam.hold, d, i, j] == 1) ? img[gam.hold] : backg;
                    }

                }
            }
        }

        public void gameLoop()
        {
            if (gam.tickCount != 2)
            {
                gam.tickCount += 1;
            }
            if (lookBottom() == true)
            {
                gam.level = (gam.deletedRow < 140) ? gam.deletedRow / 10 : 14;
                /*
                if (gam.deletedRow < 10) gam.level = 0;
                else if (gam.deletedRow < 20) gam.level = 1;
                else if (gam.deletedRow < 30) gam.level = 2;
                else if (gam.deletedRow < 40) gam.level = 3;
                else if (gam.deletedRow < 50) gam.level = 4;
                else if (gam.deletedRow < 60) gam.level = 5;
                else if (gam.deletedRow < 70) gam.level = 6;
                else if (gam.deletedRow < 80) gam.level = 7;
                else if (gam.deletedRow < 90) gam.level = 8;
                else if (gam.deletedRow < 100) gam.level = 9;
                else if (gam.deletedRow < 110) gam.level = 10;
                else if (gam.deletedRow < 120) gam.level = 11;
                else if (gam.deletedRow < 130) gam.level = 12;
                else if (gam.deletedRow < 140) gam.level = 13;
                else gam.level = 14;
                */
                switch (gam.level)
                {
                    case 0: gam.fallTick = 20; break;
                    case 1: gam.fallTick = 15; break;
                    case 2: gam.fallTick = 10; break;
                    case 3: gam.fallTick = 5; break;
                    case 4: gam.fallTick = 0; break;
                    case 5: gam.veryfast = true; gam.fallTick = 9; break;
                    case 6: gam.veryfast = true; gam.fallTick = 8; break;
                    case 7: gam.veryfast = true; gam.fallTick = 7; break;
                    case 8: gam.veryfast = true; gam.fallTick = 6; break;
                    case 9: gam.veryfast = true; gam.fallTick = 5; break;
                    case 10: gam.veryfast = true; gam.fallTick = 4; break;
                    case 11: gam.veryfast = true; gam.fallTick = 3; break;
                    case 12: gam.veryfast = true; gam.fallTick = 2; break;
                    case 13: gam.veryfast = true; gam.fallTick = 1; break;
                    case 14: gam.veryfast = true; gam.fallTick = 0; break;
                }
                fall(gam.fallTick);
            }
            else
            {
                if ((timerCount >= gam.fallTick) || (keysUp))
                {
                    keysUp = false;
                    timerCount = 0;
                    Parallel.For(0, 8, id =>
                    {
                        Point flm = m.fallingMino;
                        for (int j = 0; j < 4; j++)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                if (m.minoShape[gam.kind, m.deg, j, i] == 1)
                                {
                                    gam.placedMino[flm.Y + j, flm.X + i] = gam.kind;
                                }

                            }
                        }
                    });
                    gam.holded = false;
                    bgPaint();
                    
                    rct = deleteRow();
                    rc = rct;
                    while (!(rct == 0))
                    {
                        rct = deleteRow();
                        rc += rct;
                    }
                    scoreCal(rc, gam.combo);
                    if (!(rc == 0)) gam.combo += 1; else gam.combo = 0;
                    if (gameOver()) scene = 4;
                    gam.kind = rn.Next(7);
                    m.fallingMino = new Point(6, 0);
                    m.point = new Point(740, -90);
                    m.deg = 0;
                    minoDeg();
                }
                else
                {
                    timerCount++;
                }


            }

            bgPaint();
        }
        public void tm_Tick(Object sender, EventArgs e)
        {
            switch (scene)
            {
                case 1:
                    gameDispose();
                    scene = 2;
                    break;
                case 2:
                    init();
                    scene = 3;
                    break;
                case 3:
                    gameLoop();
                    Invalidate();
                    break;
                case 4:
                    gameOverInit();
                    scene = 5;
                    break;
                case 5:
                    gameOverLoop();
                    Invalidate();
                    break;
            }
            
        }
        public void gameOverInit()
        {
            retryLabel = new Label();
            retryLabel.Font = new Font("MS UI Gothic", 50);
            retryLabel.Size = new Size(400, 100);
            retryLabel.Location = new Point(600, 240);
            retryLabel.BackColor = Color.Transparent;
            retryLabel.Parent = this;
            retryLabel.Text = "R E T R Y";
            exitLabel = new Label();
            exitLabel.Font = new Font("MS UI Gothic", 50);
            exitLabel.Size = new Size(400, 100);
            exitLabel.Location = new Point(600, 480);
            exitLabel.BackColor = Color.Transparent;
            exitLabel.Parent = this;
            exitLabel.Text = "  E X I T ";
        }
        public void gameOverLoop()
        {
            exitLabel.MouseEnter += new EventHandler(exit_MouseEnter);
            exitLabel.MouseLeave += new EventHandler(exit_MouseLeave);
            exitLabel.MouseClick += new MouseEventHandler(exit_MouseClick);
            retryLabel.MouseEnter += new EventHandler(retry_MouseEnter);
            retryLabel.MouseLeave += new EventHandler(retry_MouseLeave);
            retryLabel.MouseClick += new MouseEventHandler(retry_MouseClick);
        }
        public bool gameOver()
        {
            for (int i = 3; i < 13; i++)
            {
                if (!((gam.placedMino[4, i] == -1)||(gam.placedMino[4, i] == -3))) return true;
            }
            return false;
        }
        public void scoreCal(int rc, int ratio)
        {
            gam.deletedRow += rc;
            ratio++;
            switch (rc)
            {
                case 1:
                    gam.score += 1 * ratio;
                    break;
                case 2:
                    gam.score += 5 * ratio;
                    break;
                case 3:
                    gam.score += 10 * ratio;
                    break;
                case 4:
                    gam.score += 50 * ratio;
                    break;
            }

        }
        public bool rightOk()
        {
            Point flm = m.fallingMino;
            bool rightOk = true;
            int d = m.deg;
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (m.minoShape[gam.kind, d, j, i] == 1)
                    {
                        if (!(gam.placedMino[flm.Y + j, flm.X + i + 1] == -1 || gam.placedMino[flm.Y + j, flm.X + i + 1] == -3))
                        {
                            rightOk = false;
                        }
                    }

                }
            }
            return rightOk;
        }

        public bool leftOk()
        {
            Point flm = m.fallingMino;
            bool leftOk = true;
            int d = m.deg;
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (m.minoShape[gam.kind, d, j, i] == 1)
                    {
                        if (!(gam.placedMino[flm.Y + j, flm.X + i - 1] == -1 || gam.placedMino[flm.Y + j, flm.X + i - 1] == -3))
                        {
                            leftOk = false;
                        }
                    }

                }
            }
            return leftOk;
        }

        public bool rotaterightOk()
        {
            Point flm = m.fallingMino;
            bool rotateok = true;
            int d = m.deg;
            d = d == 3 ? 0 : d++;
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (m.minoShape[gam.kind, d, j, i] == 1)
                    {
                        if (!(gam.placedMino[flm.Y + j + 1, flm.X + i] == -1 || gam.placedMino[flm.Y + j + 1, flm.X + i] == -3))
                        {
                            rotateok = false;
                        }
                    }

                }
            }
            return rotateok;
        }
        public bool rotateleftOk()
        {
            Point flm = m.fallingMino;
            bool rotateok = true;
            int d = m.deg;
            d = d == 0 ? 3 : d--;
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (m.minoShape[gam.kind, d, j, i] == 1)
                    {
                        if (!(gam.placedMino[flm.Y + j + 1, flm.X + i] == -1 || gam.placedMino[flm.Y + j + 1, flm.X + i] == -3))
                        {
                            rotateok = false;
                        }
                    }

                }
            }
            return rotateok;
        }
        public bool lookBottom()
        {
            Point flm = m.fallingMino;
            bool fallok = true;
            Parallel.For(0, 4, id =>
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (m.minoShape[gam.kind, m.deg, j, i] == 1)
                        {
                            if (!(gam.placedMino[flm.Y + j + 1, flm.X + i] == -1 || gam.placedMino[flm.Y + j + 1, flm.X + i] == -3))
                            {
                                fallok = false;
                            }
                        }

                    }
                }
            });
            return fallok;
        }
        public bool holdOk()
        {
            Point flm = m.fallingMino;
            bool fallok = true;
            Parallel.For(0, 4, id =>
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (m.minoShape[(gam.hold == -3) ? gam.hidHold : gam.hold, m.deg, j, i] == 1)
                        {
                            if (!(gam.placedMino[flm.Y + j, flm.X + i] == -1 || gam.placedMino[flm.Y + j, flm.X + i] == -3))
                            {
                                fallok = false;
                            }
                        }

                    }
                }
            });
            return fallok;
        }
        public int fallShadow()
        {
            bool fallok = true;
            int fallS = 0;
            do
            {
                Point flm = m.fallingMino;
                Parallel.For(0, 4, id =>
                {
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (m.minoShape[gam.kind, m.deg, j, i] == 1)
                            {
                                if (!(gam.placedMino[flm.Y + j + 1 + fallS, flm.X + i] == -1 || gam.placedMino[flm.Y + j + 1 + fallS, flm.X + i] == -3))
                                {
                                    fallok = false;
                                }
                            }
                        }
                    }
                });
                fallS++;
            } while (fallok);
            return fallS;
        }
        public bool fallOk()
        {
            Point flm = m.fallingMino;
            bool fallok = true;
            Parallel.For(0, 4, id =>
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (m.minoShape[gam.kind, m.deg, j, i] == 1)
                        {
                            if (!(gam.placedMino[flm.Y + j + 2, flm.X + i] == -1 || gam.placedMino[flm.Y + j + 2, flm.X + i] == -3))
                            {
                                fallok = false;
                            }
                        }

                    }
                }
            });
            return fallok;
        }
        public void fall(int time)
        {
            Point minop = m.point;
            Point flMino = m.fallingMino;
            timer += 1;
            //gam.veryfast = true;
            //timer = time;
            if (gam.veryfast == true)
            {
                if (timer >= time)
                {
                    flMino.Y += 1;
                    minop.Y += 30;
                    m.fallingMino = flMino;
                    timer = 0;
                }
            }
            else
            {
                if (timer >= time)
                {
                    minop.Y += a[count];
                    count += 1;
                    if (count >= 10)
                    {
                        flMino.Y += 1;
                        m.fallingMino = flMino;
                        timer = 0;
                        count = 0;
                    }
                }
            }
            m.point = minop;
        }
        public bool deleteRowOK(int j)
        {
            bool OK = true;
            Parallel.For(0, 4, id =>
            {
                for (int i = 3; i <= 12; i++)
                {
                    if (gam.placedMino[j + 3, i] == -1)
                    {
                        OK = false;
                    }
                }
            });
            return OK;
        }
        public int deleteRow()
        {
            int rowCount = 0;
            for (int i = 20; i >= 0; i--)
            {
                if (deleteRowOK(i))
                {
                    deleteRowR(i);
                    rowCount++;
                }
            }
            rowCount -= 1;
            return rowCount;
        }

        public int deleteRowR(int j)
        {
            if (j == 0)
            {
                for (int i = 0; i < 16; i++)
                {
                    if (gam.placedMino[4, i] == -1 || gam.placedMino[4, i] == -3)
                    {
                        gam.placedMino[4, i] = ((i >= 3) & (i <= 12)) ? -1 : -2;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 16; i++)
                {
                    gam.placedMino[j + 3, i] = gam.placedMino[j + 2, i];
                }
            }
            return j == 0 ? 0 : deleteRowR(j - 1);
        }
    }
}