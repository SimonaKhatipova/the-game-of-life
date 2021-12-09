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

    // класс для кролика
    public class Rabbit : Animal
    {
     
        Form MainWindow;
        static Random rnd = new Random();
        // сколько очков размножения нужно для начала размножения
        int requiredReproducePoints = 20;
        // максимальное количество очков здоровья
        int maxHP = 200;
        public Rabbit(int x, int y, Form MainWindow) : base(x, y)
        {
            if (rnd.Next(0, 2) == 0)
            {
                Gender = true;
            }
            else
            {
                Gender = false;
            }
            Img.Image = Image.FromFile("зайка.png");
            HP = maxHP;
            this.MainWindow = MainWindow;

        }
        public override bool Reproduce()
        {
            // оглядываемся вокруг и ищем партнёра для размножения
            // возможность размножаться появляется только после 4 шагов
            // аналогично зайцам, но с учётом пола
            if (ReproducePoints > requiredReproducePoints)
            {
                foreach (Rabbit r in Main.rabbits)
                {
                    //не считая самого себя
                    if (r != this)
                    {
                        if ((Math.Abs(Img.Location.X - r.Img.Location.X) <= Main.cellSize && Math.Abs(Img.Location.Y - r.Img.Location.Y) <= Main.cellSize) && r.ReproducePoints > requiredReproducePoints && this.Gender != r.Gender) // разнополые особи
                        {
                            // перемещаем волка на клетку партнёра для начала процесса
                            this.Img.Location = new Point(r.Img.Location.X, r.Img.Location.Y);
                            // создаём нового зайца в той же клетке
                            Rabbit child = new Rabbit((r.Img.Location.X - Main.marginLeft) / Main.cellSize, (r.Img.Location.Y - Main.marginTop) / Main.cellSize, MainWindow);
                            Main.rabbits.Add(child);
                            MainWindow.Controls.Add(child.Img);
                            child.Img.BringToFront();
                            child.Img.Image = reprImg;

                            // оба временно теряют желание размножаться
                            this.ReproducePoints = 0;
                            r.ReproducePoints = 0;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        //от ходов тоже стареет,
        //зайчику неважно где волк, лишь бы не съел, но если видит зайчика другого пола идет
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

            // зайчик стареет, очень сильно страдает из-за движения, и его жизнь уменьшается
            HP--;
            if (HP > maxHP * 0.6) Img.BackColor = Color.Green;
            else if (HP > maxHP * 0.3) Img.BackColor = Color.Yellow;//khaki
            else if (HP>10)Img.BackColor = Color.Tomato;
            else Img.BackColor = Color.Black;

            if (Img.Image != rabImg)
            { 
            Img.Image = rabImg;

            }


            // при движении желание спариваться возрастает
            ReproducePoints++;
        }

    }
    
}
