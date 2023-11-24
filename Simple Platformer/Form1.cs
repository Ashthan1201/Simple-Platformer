using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;


namespace Simple_Platformer
{
    class DrawingControl
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;
        public static void SuspendDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }
        public static void ResumeDrawing (Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            parent.Refresh();
        }
    }
    public partial class PlatformGameForm : Form
    {

        bool goLeft, goRight, jumping, isGameOver;

        int jumpSpeed;
        int force;
        int score = 0;
        int playerSpeed = 7;

        int horizontalSpeed = 5;
        int verticalSpeed = 3;

        int enemyOneSpeed = 5;
        int enemyTwoSpeed = 3;
        int enemyThreeSpeed = 4;
        int enemyFourSpeed = 3;

        public object Enviroment { get; private set; }

        public PlatformGameForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            DrawingControl.SuspendDrawing(this);
            txtScore.Text = "Score: " + score;

            Player.Top += jumpSpeed;

            if (goLeft == true)
            {
                Player.Left -= playerSpeed;
            }

            if (goRight == true)
            {
                Player.Left += playerSpeed;
            }

            if (jumping == true && force < 0)
            {
                jumping = false;
            }

            if (jumping == true)
            {
                jumpSpeed = -8;
                force -= 1;
            }
            else
            {
                jumpSpeed = 10;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {

                    if ((string)x.Tag == "platform")
                    {
                        if (Player.Bounds.IntersectsWith(x.Bounds))
                        {
                            force = 8;
                            Player.Top = x.Top - Player.Height;

                            if ((string)x.Name == "HorizontalPlatform" && goLeft == false || (string)x.Name == "HorizontalPlatform" && goRight == false)
                            {
                                Player.Left -= horizontalSpeed;
                            }
                        }
                    }

                   
                }

                if ((string)x.Tag == "coin")
                {
                    if (Player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                    {
                        x.Visible = false;
                        score++;
                    }
                }

                if ((string)x.Tag == "enemy")
                {
                    if (Player.Bounds.IntersectsWith(x.Bounds))
                    {
                        gameTimer.Stop();
                        isGameOver = true;
                        txtScore.Text = "Score: " + score + Environment.NewLine + " Lol You died, Get good scrub";

                    }
                }
            }

            HorizontalPlatform.Left -= horizontalSpeed;

            if (HorizontalPlatform.Left < 0 || HorizontalPlatform.Left >243)
            {
                horizontalSpeed = -horizontalSpeed;
            }

            verticalPlatform.Top += verticalSpeed;

            if (verticalPlatform.Top <159 || verticalPlatform.Top >276)
            {
                verticalSpeed = -verticalSpeed;
            }

            enemyOne.Left -= enemyOneSpeed;

            if (enemyOne.Left < pictureBox7.Left || enemyOne.Left + enemyOne.Width > pictureBox7.Left + pictureBox7.Width)
            {
                enemyOneSpeed = -enemyOneSpeed;
            }

            enemyTwo.Left += enemyTwoSpeed;

            if (enemyTwo.Left < pictureBox6.Left || enemyTwo.Left + enemyTwo.Width > pictureBox6.Left + pictureBox6.Width)
            {
                enemyTwoSpeed = -enemyTwoSpeed;
            }

            enemyThree.Left -= enemyThreeSpeed;

            if (enemyThree.Left < pictureBox11.Left || enemyThree.Left + enemyThree.Width > pictureBox11.Left + pictureBox11.Width)
            {
                enemyThreeSpeed = -enemyThreeSpeed;
            }

            enemyFour.Left -= enemyFourSpeed;

            if (enemyFour.Left < pictureBox5.Left || enemyFour.Left + enemyFour.Width > pictureBox5.Left + pictureBox5.Width)
            {
                enemyFourSpeed = -enemyFourSpeed;
            }

            if (Player.Top + Player.Height > this.ClientSize.Height + 20)
            {
                gameTimer.Stop();
                isGameOver = true;
                txtScore.Text = "Score: " + score + Environment.NewLine + "haha you fell Try again scrub";
            }

            if (Player.Bounds.IntersectsWith(door.Bounds) && score == 37)
            {
                gameTimer.Stop();
                isGameOver = true;
                txtScore.Text = "Score: " + score + Environment.NewLine + "You are not Useless! Well Done!";
            }
            else
            {
                txtScore.Text = "Score: " + score + Environment.NewLine + "Nuh Uhh, Get the coins Scrub";
            }

            DrawingControl.ResumeDrawing(this);
        }


        

                
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Space && jumping == false)
            {
                jumping = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight= false;
            }
            if (jumping == true)
            {
                jumping = false;
            }

            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                RestartGame();
            }

        }

        private void RestartGame()
        {

            jumping = false;
            goLeft = false;
            goRight = false;
            isGameOver = false;
            score = 0;

            txtScore.Text = "Score: " + score;

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Visible == false)
                {
                    x.Visible = true;
                }
            }

            Player.Left = 12;
            Player.Top = 759;

            enemyOne.Left = 513;
            enemyTwo.Left = 85;
            enemyThree.Left = 501;
            enemyFour.Left = 443;

            HorizontalPlatform.Left = 22;
            verticalPlatform.Top = 169;

            gameTimer.Start();
        


        }
    }
}
