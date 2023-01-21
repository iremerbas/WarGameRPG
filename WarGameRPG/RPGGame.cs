using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarGameRPG.DataAccesLayer;
using WarGameRPG.Models;
using WarGameRPG.Repository;

namespace WarGameRPG
{
    public partial class RPGGame : Form
    { //zombiList
        Character character = new Character();
        dataContext db = new dataContext();
        CharacterRepository characterRepository = new CharacterRepository();
        public int iksir = 0;
        public int heart = 0;
        public int power = 20;
        public int durability = 20;
        bool goLeft, goRight, goUp, goDown, gameOver;
        string facing = "up";
        public double playerHealth = 10000; // Can 100 olacak
        public int speed = 7; // Hızı
        int ammo = 999; // Kurşun sayısı
        int zombieSpeed = 3; // Canavar hızı
        Random randNum = new Random();
        public int score;
        int zombiedead;
        public string level;

       
        List<PictureBox> zombiesList = new List<PictureBox>();

        public RPGGame()
        {
            InitializeComponent();
            RestartGame();
            lblWin.Visible = false;
           
        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            if (playerHealth > 1)
            {
                healthBar.Value = Convert.ToInt32(playerHealth);
            }
            else
            {
                gameOver = true;
                player.Image = Properties.Resources.dead;
                lblWin.Text = "Game Over";
                lblWin.BackColor = Color.Black;
                lblWin.Visible = true;
                GameTimer.Stop();
            }

            txtAmmo.Text = "Ammo: " + ammo;
            txtScore.Text = "Score: " + score;

            if (goLeft == true && player.Left > 0)
            {
                player.Left -= speed;
            }
            if (goRight == true && player.Left + player.Width < this.ClientSize.Width)
            {
                player.Left += speed;
            }
            if (goUp == true && player.Top > 45)
            {
                player.Top -= speed;
            }
            if (goDown == true && player.Top + player.Height < this.ClientSize.Height)
            {
                player.Top += speed;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "ammo")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        ammo += 5;
                    }
                }
                if (x is PictureBox && (string)x.Tag == "potion")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        if (healthBar.Value < 75)
                        {
                            playerHealth += 25;
                        }
                        else
                        {
                            healthBar.Value = 100;
                        }
                    }
                }

                //zombiler karakteri takip etsin
                if (x is PictureBox && (string)x.Tag == "zombie")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        if (lbl_Level.Text == "Level : 1")
                        { playerHealth -= 1; }
                        else if (lbl_Level.Text == "Level : 2")
                        { playerHealth -= 0.9; }
                        else if (lbl_Level.Text == "Level : 3")
                        { playerHealth -= 0.8; }
                        else if (lbl_Level.Text == "Level : 4")
                        { playerHealth -= 0.7; }
                        else if (lbl_Level.Text == "Level : 5")
                        { playerHealth -= 0.6; }
                        else if (lbl_Level.Text == "Level : 6")
                        { playerHealth -= 0.5; }
                        else if (lbl_Level.Text == "Level : 7")
                        { playerHealth -= 0.4; }
                        else if (lbl_Level.Text == "Level : 8")
                        { playerHealth -= 0.3; }
                        else if (lbl_Level.Text == "Level : 9")
                        { playerHealth -= 0.2; }
                        else if (lbl_Level.Text == "Level : 10")
                        { playerHealth -= 0.1; }
                    }

                    if (x.Left > player.Left)
                    {
                        x.Left -= zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zleft;
                    }
                    if (x.Left < player.Left)
                    {
                        x.Left += zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zright;
                    }
                    if (x.Top > player.Top)
                    {
                        x.Top -= zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zup;
                    }
                    if (x.Top < player.Top)
                    {
                        x.Top += zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zdown;

                    }
                }
                //zombilere vurup vurmadığını kontrol eden kısım
                foreach (Control j in this.Controls)
                {
                    if (j is PictureBox && (string)j.Tag == "bullet" && x is PictureBox && (string)x.Tag == "zombie")
                    {
                        // zombiesList
                        if (x.Bounds.IntersectsWith(j.Bounds))
                        {
                            zombiedead += 1;
                            //zombi death 10 olunca bitti ekranı gelsind evam ederse lvl 2 den devam eder.
                            //zombiler canı 0 olunca ölecek
                            score += 10;
                            iksir = randNum.Next(1, 25);
                            if (iksir == 3)
                            {
                                PictureBox iksir = new PictureBox();
                                iksir.Image = Properties.Resources.iksir;
                                iksir.SizeMode = PictureBoxSizeMode.AutoSize;
                                iksir.BackColor = Color.Transparent;
                                iksir.Left = randNum.Next(10, this.ClientSize.Width - iksir.Width);
                                iksir.Top = randNum.Next(60, this.ClientSize.Height - iksir.Height);

                                iksir.Tag = "potion";
                                this.Controls.Add(iksir);

                                iksir.BringToFront();
                                player.BringToFront();
                            }
                           
                            if (score < 100)
                            {
                                zombieSpeed = 1;
                                speed = 7;
                                lbl_Level.Text = "Level : 1";
                                level = "Level : 1";
                               ;
                                  
                                //db.Character.Add(character);
                            }
                            else if (score >= 100 && score < 200)
                            {
                                BackgroundImage = Properties.Resources.orman;
                                zombieSpeed = 2;
                                speed = 8;
                                lbl_Level.Text = "Level : 2";
                                level = "Level : 2";
                            }
                            else if (score >= 200 && score < 400)
                            {
                                BackgroundImage = Properties.Resources.level3;
                                zombieSpeed = 3;
                                speed = 9;
                                lbl_Level.Text = "Level : 3";
                                level = "Level : 3";
                            }
                            else if (score >= 400 && score < 1000)
                            {
                                BackgroundImage = Properties.Resources.level4;
                                zombieSpeed = 4;
                                speed = 10;
                                lbl_Level.Text = "Level : 4";
                                level = "Level : 4";
                            }
                            else if (score >= 1000 && score < 2500)
                            {
                                BackgroundImage = Properties.Resources.level5;
                                zombieSpeed = 5;
                                speed = 11;
                                lbl_Level.Text = "Level : 5";
                                level = "Level : 5";
                            }
                            else if (score >= 2500 && score < 5000)
                            {
                                BackgroundImage = Properties.Resources.level6jfif;
                                zombieSpeed = 6;
                                speed = 12;
                                lbl_Level.Text = "Level : 6";
                                level = "Level : 6";
                            }
                            else if (score >= 5000 && score < 12500)
                            {
                                BackgroundImage = Properties.Resources.level7;
                                zombieSpeed = 7;
                                speed = 13;
                                lbl_Level.Text = "Level : 7";
                                level = "Level : 7";
                            }
                            else if (score >= 12500 && score < 30000)
                            {
                                BackgroundImage = Properties.Resources.level8;
                                zombieSpeed = 8;
                                speed = 14;
                                lbl_Level.Text = "Level : 8";
                                level = "Level : 8";
                            }
                            else if (score >= 30000 && score < 100000)
                            {
                                BackgroundImage = Properties.Resources.level9;
                                zombieSpeed = 9;
                                speed = 15;
                                lbl_Level.Text = "Level : 9";
                                level = "Level : 9";
                            }
                            else if (score >= 100000 && score < 250000)
                            {
                                BackgroundImage = Properties.Resources.level10;
                                zombieSpeed = 10;
                                speed = 16;
                                lbl_Level.Text = "Level : 10";
                                level = "Level : 10";
                                GameTimer.Stop();
                                lblWin.Visible = true;
                            }
                            this.Controls.Remove(j);
                            ((PictureBox)j).Dispose();
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                            zombiesList.Remove(((PictureBox)x));
                            if (zombiedead == 10)
                            {
                                power++;
                                speed++;
                                durability++;
                                zombiedead = 0;

                                // System.Threading.Thread.Sleep(10000);

                                for (int i = 0; i < 10; i++)
                                {
                                    MakeZombies();

                                    //     Thread.Sleep(1000);
                                    //10 adet güçlendirilmiş zombi getirecek.
                                }
                            }
                        }
                    }
                }
            }

            if (lbl_Level.Text == "Level : 1")
            {
                this.power = 20;
                this.durability = 20;
                this.lblpower.Text =power.ToString();
                this.lblDurability.Text =durability.ToString();
            }
            else if (lbl_Level.Text == "Level : 2")
            {
                this.power = 21;
                this.durability = 21;
                this.lblpower.Text = power.ToString();
                this.lblDurability.Text =  durability.ToString();
            }
            else if (lbl_Level.Text == "Level : 3")
            {
                this.power = 22;
                this.durability = 22;
                this.lblpower.Text =  power.ToString();
                this.lblDurability.Text = durability.ToString();
            }
            else if (lbl_Level.Text == "Level : 4")
            {

                power = 23;
                durability = 23;
                lblpower.Text =  power.ToString();
                lblDurability.Text =  durability.ToString();
            }
            else if (lbl_Level.Text == "Level : 5")
            {
                power = 24;
                durability = 24;
                lblpower.Text =  power.ToString();
                lblDurability.Text =  durability.ToString();
            }
            else if (lbl_Level.Text == "Level : 6")
            {
                power = 25;
                durability = 25;
                lblpower.Text =  power.ToString();
                lblDurability.Text =  durability.ToString();
            }
            else if (lbl_Level.Text == "Level : 7")
            {
                this.power = 26;
                this.durability = 26;
                lblpower.Text =  power.ToString();
                lblDurability.Text = durability.ToString();
            }
            else if (lbl_Level.Text == "Level : 8")
            {
                power = 27;
                durability = 27;
                lblpower.Text =  power.ToString();
                lblDurability.Text = durability.ToString();
            }
            else if (lbl_Level.Text == "Level : 9")
            {
                power = 28;
                durability = 28;
                lblpower.Text =  power.ToString();
                lblDurability.Text =  durability.ToString();
            }
            else if (lbl_Level.Text == "Level : 10")
            {
                power = 29;
                durability = 29;
                lblpower.Text =  power.ToString();
                lblDurability.Text =  durability.ToString();
            }

        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {

            if (gameOver == true)
            {
                return;
            }

            if (e.KeyCode == Keys.Left) //sola tıklandığında resim gelsin
            {
                goLeft = true;
                facing = "left";
                player.Image = Properties.Resources.left;
            }

            if (e.KeyCode == Keys.Right) //sağa tıklandığında resim gelsin
            {
                goRight = true;
                facing = "right";
                player.Image = Properties.Resources.right;
            }

            if (e.KeyCode == Keys.Up) // yukarı tıklandığında resim gelsin
            {
                goUp = true;
                facing = "up";
                player.Image = Properties.Resources.up;
            }

            if (e.KeyCode == Keys.Down) // aşağı tıklandığında resim gelsin
            {
                goDown = true;
                facing = "down";
                player.Image = Properties.Resources.down;
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
                goRight = false;
            }

            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }

            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }

            if (e.KeyCode == Keys.Space && ammo > 0 && gameOver == false)
            {
                ammo--;
                ShootBullet(facing);


                if (ammo <= randNum.Next(0, 2))
                {
                    DropAmmo();
                }

            }

            if (e.KeyCode == Keys.Enter && gameOver == true)
            {
                RestartGame();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void player_Click(object sender, EventArgs e)
        {

        }

        private void powerlbl_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void RPGGame_Load(object sender, EventArgs e)
        {
           
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {

           
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            GameTimer.Stop();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            GameTimer.Start();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            character = new Character();
            character.Name = "Player";
            character.Power = power;
            character.Durability = durability;
            character.Speed = speed;
            character.Health = playerHealth;
            character.Level = lbl_Level.Text;
            character.Score = score;
            characterRepository.Add(character);

            MessageBox.Show("Ekleme Başarılı");

            //  db.Character.Add(character);
        }


        private void ShootBullet(string direction)
        {
            Bullet shootBullet = new Bullet();
            shootBullet.direction = direction;
            shootBullet.bulletLeft = player.Left + (player.Width / 2);
            shootBullet.bulletTop = player.Top + (player.Height / 2);
            shootBullet.MakeBullet(this);
        }

        private void MakeZombies()
        {
            PictureBox zombie = new PictureBox();
            zombie.Tag = "zombie";
            zombie.Image = Properties.Resources.zdown;
            zombie.Left = randNum.Next(0, 100);
            zombie.Top = randNum.Next(0, 100);
            zombie.BackColor = Color.Transparent;

            zombie.SizeMode = PictureBoxSizeMode.AutoSize;

            zombiesList.Add(zombie);
            this.Controls.Add(zombie);
            player.BringToFront();
        }

        private void DropAmmo()
        {
            for (int i = 0; i < randNum.Next(1, 3); i++)
            {
                PictureBox ammo = new PictureBox();
                ammo.Image = Properties.Resources.ammo_Image;
                ammo.SizeMode = PictureBoxSizeMode.AutoSize;

                ammo.Left = randNum.Next(10, this.ClientSize.Width - ammo.Width);
                ammo.Top = randNum.Next(60, this.ClientSize.Height - ammo.Height);

                ammo.Tag = "ammo";
                this.Controls.Add(ammo);

                ammo.BringToFront();
                player.BringToFront();
            }
        }

        private void RestartGame()//karakter özellikleri ve zombi özellikleri 0 level değerine geri döndürelecek.
        {
            player.Image = Properties.Resources.up;

            foreach (PictureBox i in zombiesList)
            {
                this.Controls.Remove(i);
            }

            zombiesList.Clear();

            //Thread thread = new Thread();
            for (int i = 0; i < 10; i++)
            {
                MakeZombies();

            }

            goUp = false;
            goDown = false;
            goLeft = false;
            goRight = false;
            gameOver = false;

            playerHealth = 100;
            score = 0;
            ammo = 999;

            GameTimer.Start();
        }

    }
}
