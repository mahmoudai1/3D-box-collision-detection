using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graphics_2_Assignments
{
    public partial class Form1 : Form
    {
        Bitmap off;

        List<_3D_Model> LCubes = new List<_3D_Model>();
        _3D_Model myCube = new _3D_Model();
        List<_3D_Model> LDangerCubes = new List<_3D_Model>();

        Camera cam = new Camera();

        Timer t = new Timer();
        int ctTimer = 0;
        int camSpeed = 2;

        int ctWalk = 0;
        int dir = 1;

        bool gameOver = false;

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
            t.Tick += T_Tick;
            t.Start();
        }

        private void T_Tick(object sender, EventArgs e)
        {
            if(!gameOver && ctTimer % 8 == 0)
            {
                if (ctWalk >= 9) { dir *= -1; ctWalk = 0; }
                for (int i = 0; i < LDangerCubes.Count; i++)
                {
                    for (int j = 0; j < LDangerCubes[i].L_3D_Pts.Count; j++)
                    {
                        if (i % 2 == 0)
                        {
                            LDangerCubes[i].L_3D_Pts[j].Z += 40 * dir;
                        }
                        else
                        {
                            LDangerCubes[i].L_3D_Pts[j].Z -= 40 * dir;
                        }
                    }
                }
                ctWalk++;
                Console.WriteLine(ctWalk);
            }
            checkIfCollided();
            ctTimer++;
            drawDouble(this.CreateGraphics());
        }

        public void checkIfCollided()
        {
            for(int i = 0; i < LDangerCubes.Count; i++)
            {
                for (int j = 0; j < LDangerCubes[i].L_3D_Pts.Count; j++)
                {
                    if (LDangerCubes[i].L_3D_Pts[j].X == myCube.L_3D_Pts[j].X &&
                        LDangerCubes[i].L_3D_Pts[j].Y == myCube.L_3D_Pts[j].Y &&
                        LDangerCubes[i].L_3D_Pts[j].Z == myCube.L_3D_Pts[j].Z)
                    {
                        gameOver = true;
                        break;
                    }
                }
                if (gameOver) break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    cam.cop.X += camSpeed;

                    break;

                case Keys.Left:
                    cam.cop.X -= camSpeed;

                    break;

                case Keys.Up:
                    cam.cop.Z += camSpeed;

                    break;

                case Keys.Down:
                    cam.cop.Z -= camSpeed;

                    break;

                case Keys.Y:
                    cam.cop.Y -= camSpeed;

                    break;

                case Keys.H:
                    cam.cop.Y += camSpeed;
            
                    break;

                case Keys.W:
                    if (myCube.L_3D_Pts[0].Z < LCubes[LCubes.Count - 1].L_3D_Pts[0].Z)
                    {
                        for (int j = 0; j < myCube.L_3D_Pts.Count; j++)
                        {
                            myCube.L_3D_Pts[j].Z += 40;
                        }
                    }
                    break;

                case Keys.S:
                    if (myCube.L_3D_Pts[2].Z > LCubes[1].L_3D_Pts[2].Z)
                    {
                        for (int j = 0; j < myCube.L_3D_Pts.Count; j++)
                        {
                            myCube.L_3D_Pts[j].Z -= 40;
                        }
                    }
                    break;

                case Keys.D:
                    if (myCube.L_3D_Pts[1].X < LCubes[1].L_3D_Pts[1].X)
                    {
                        for (int j = 0; j < myCube.L_3D_Pts.Count; j++)
                        {
                            myCube.L_3D_Pts[j].X += 40;
                        }
                    }
                    break;

                case Keys.A:
                    if (myCube.L_3D_Pts[0].X > LCubes[LCubes.Count - 1].L_3D_Pts[0].X)
                    {
                        for (int j = 0; j < myCube.L_3D_Pts.Count; j++)
                        {
                            myCube.L_3D_Pts[j].X -= 40;
                        }
                    }
                    break;
            }
            cam.BuildNewSystem();
            drawDouble(this.CreateGraphics());
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            drawDouble(e.Graphics);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.Width, this.Height);

            int cx = 0;
            int cy = 0;
            cam.ceneterX = (this.ClientSize.Width / 2);
            cam.ceneterY = (this.ClientSize.Height / 2);
            cam.cxScreen = cx;
            cam.cyScreen = cy;
            cam.BuildNewSystem();

            _3D_Model mainCube = new _3D_Model();
            mainCube.CreateTheObject("MainCube", -100, 0 , -350, Color.White);
            mainCube.cam = cam;
            LCubes.Add(mainCube);

            int vZ = -50;
            for (int i = 0; i < 10; i++)
            {
                _3D_Model cube = new _3D_Model();
                cube.CreateTheObject("SmallCubes", 20, -110, vZ, Color.Cyan);
                cube.cam = cam;
                LCubes.Add(cube);
                vZ += 40;
            }
            vZ = -50;
            for (int i = 0; i < 10; i++)
            {
                _3D_Model cube = new _3D_Model();
                cube.CreateTheObject("SmallCubes", -20, -110, vZ, Color.HotPink);
                cube.cam = cam;
                LCubes.Add(cube);
                vZ += 40;
            }

            vZ = -50;
            int vX = -20;
            for (int i = 0; i < 2; i++)
            {
                _3D_Model cube = new _3D_Model();
                cube.CreateTheObject("SmallCubes", vX, -150, vZ, Color.Red);
                cube.cam = cam;
                LDangerCubes.Add(cube);
                vZ = 310;
                vX = 20;
            }

            myCube.CreateTheObject("SmallCubes", 20, -150, -50, Color.Yellow);
            myCube.cam = cam;
        }

        public void drawScene(Graphics g)
        {
            g.Clear(Color.Black);

            for(int i = 0; i < LCubes.Count; i++)
            {
                LCubes[i].DrawYourSelf(g);
            }

            for (int i = 0; i < LDangerCubes.Count; i++)
            {
                LDangerCubes[i].DrawYourSelf(g);
            }

            myCube.DrawYourSelf(g);
        }

        public void drawDouble(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            drawScene(g2);
            g.DrawImage(off, 0, 0);
        }

    }
}


