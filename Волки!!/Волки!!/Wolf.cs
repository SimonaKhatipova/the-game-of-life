using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ВолкииЁлки
{
    [Serializable]

    public class Wolf : Animal
        
       
    {
        Form MainWindow;
        int requiredReproducePoints = 15;
        // максимальное количество очков здоровья
        int maxHP = 50;
        // сколько здоровья добавляет заяц
     

        static Random rnd = new Random();
  

        public Wolf(int x, int y,Form MainWindow) : base(x, y)
        {
            if (rnd.Next(0, 2) == 0)
            {
                Gender = true;
                Img.Image = Image.FromFile("волк.png");
            }
            else
            {
                Gender = false;
                Img.Image = Image.FromFile("волчица.png");
            }
            HP = maxHP;
            this.MainWindow = MainWindow;
        }

        public override bool Reproduce()
        {
            // аналогично зайцам, но с учётом пола
            if ((ReproducePoints > requiredReproducePoints)&&(HP>maxHP*0.7))
            {
                foreach (Wolf w in Main.wolves)
                { 
                    //не считая самого себя
                    if (w != this)
                    {
                        if ((Math.Abs(Img.Location.X - w.Img.Location.X) <= Main.cellSize && Math.Abs(Img.Location.Y - w.Img.Location.Y) <= Main.cellSize) && w.ReproducePoints > requiredReproducePoints && this.Gender != w.Gender) // разнополые особи
                        {
                            // перемещаем волка на клетку партнёра для начала процесса
                            this.Img.Location = new Point(w.Img.Location.X, w.Img.Location.Y);
                            // создаём нового волка в той же клетке
                            Wolf child = new Wolf((w.Img.Location.X - Main.marginLeft) / Main.cellSize, (w.Img.Location.Y - Main.marginTop) / Main.cellSize, MainWindow);
                            Main.wolves.Add(child);
                            MainWindow.Controls.Add(child.Img);
                            child.Img.BringToFront();
                            child.Img.Image = reprImg;
                            // оба временно теряют желание размножаться
                            this.ReproducePoints = 0;
                            w.ReproducePoints = 0;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public override void Move()
        {
            // 0: - ; 1: стой на месте; 2: +
            int verticalShift = rnd.Next(0, 3);
            int horizontalShift = rnd.Next(0, 3);

            // смещение по вертикали
            //чтобы не вышел за пределы таблицы надо проверить положение относительно её границ
            if (verticalShift == 0 && Img.Location.Y > Main.marginTop) Img.Location = new Point(Img.Location.X, Img.Location.Y - Main.cellSize);
            else if (verticalShift == 2 && Img.Location.Y < Main.marginTop + (Main.rows - 1) * Main.cellSize) Img.Location = new Point(Img.Location.X, Img.Location.Y + Main.cellSize);

            // смещение по горизонтали
            if (horizontalShift == 0 && Img.Location.X > Main.marginLeft) Img.Location = new Point(Img.Location.X - Main.cellSize, Img.Location.Y);
            else if (horizontalShift == 2 && Img.Location.X < Main.marginLeft + (Main.cols - 1) * Main.cellSize) Img.Location = new Point(Img.Location.X + Main.cellSize, Img.Location.Y);

            // волк очень сильно страдает из-за движения, и его жизнь уменьшается
            HP--;
            if (HP > maxHP * 0.6) Img.BackColor = Color.Green;
            else if (HP > maxHP * 0.3) Img.BackColor = Color.Yellow;//khaki
            else if (HP > 10) Img.BackColor = Color.Red;
            else Img.BackColor = Color.Black;

            //возвращаем картинку после охоты
            if (Gender)                       
                Img.Image = wolfImg;            
            else            
                Img.Image = wolf_fe_Img;

            

            // при движении желание спариваться возрастает
            ReproducePoints++;
        }
        
        public bool AttackRabbit()
        {
            //ReproducePoints++;
            // если уровень здоровья меньше 70% от максимального, то волк начинает охотиться
            if (HP < maxHP * 0.8)
            {
                // ищем зайчика
                foreach (Rabbit r in Main.rabbits)
                {
                    if (Math.Abs(Img.Location.X - r.Img.Location.X) <= Main.cellSize && Math.Abs(Img.Location.Y - r.Img.Location.Y) <= Main.cellSize)
                    {
                        // перемещаем волка на клетку зайца для начала трапезы
                        this.Img.Location = new Point(r.Img.Location.X, r.Img.Location.Y);
                        // волк убивает зайца
                        MainWindow.Controls.Remove(r.Img);
                        Main.rabbits.Remove(r);
                        ReproducePoints++;
                        HP = maxHP;
                        this.Img.Image = AttImg;
                        Img.BringToFront();

                        return true;
                    }
                }
            }
            return false;
        }
    }
}
