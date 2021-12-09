using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ВолкииЁлки
{
    [Serializable]

    public partial class Main : Form
    {


        // ЗДЕСЬ ВЫНЕСЕНЫ ВСЕ ЗНАЧЕНИЯ РАЗМЕРОВ И ОТСТУПОВ. ЕСЛИ МЕНЯЕШЬ ИХ ЗДЕСЬ, ТО ОНИ МЕНЯЮТСЯ ВЕЗДЕ
        // удобно же!   :) 

        //отступы таблицы от левого и верхнего краёв
        public static int marginLeft = 30, marginTop = 35;

        //размер таблицы
        public static int rows = 15, cols = 15;

        //размер ячейки
        public static int cellSize = 50;
        
        // продолжить/остановить
        public static bool isPlaying = false;
        public static bool isQuick = false;

        //список зайчиков
        public static List<Rabbit> rabbits = new List<Rabbit>();

        // список волков
        public static List<Wolf> wolves = new List<Wolf>();

        public static List<Animal> animals= new List<Animal>();

        // если true, то добавляются зайцы. если false, то волки
        // для изменения животного надо добавить какой-нибудь переключатель на форме
        public static bool addRabbit = true;
        //количество на поле животных
        public static int tim = 0;
        public static int speed = 200;
        public static int oneSecond=1000;
        public static Timer timer = new Timer();
        public static Timer timer_act = new Timer();



        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void конфигурацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Волки__.Rules rules = new Волки__.Rules();
            rules.Show();
        }

        //Обработчик нажатия на кнопку "Старт"


        private void Main_Load(object sender, EventArgs e)
        {
            // рисуем таблицу при загрузки формы
            DrawTable();
            Act();
            Time();
           
 

            timer.Stop();
            timer_act.Stop();
            // по умолчанию сначала создаются зайчики
            makeRabbits.Checked = true;
        }

        public Main()
        {
            InitializeComponent();
        }

        //выводим на экран таблицу
        public void DrawTable()
        {
            for (int row = 0; row < rows; row++)
                for (int col = 0; col < cols; col++)
                {
                    Cell newCell = new Cell(col, row);
                    // добавляем обработчик клика для всех клеток
                    newCell.cell.Click += AddAnimal;
                    // добавим кнопку на форму
                    this.Controls.Add(newCell.cell);
                }
        }



        //[XmlElement("")]
        public void AddAnimal(object sender, EventArgs e)
        {
            if (addRabbit)
            {
                // вычисляем столбец и строку
                int x = (((Button)sender).Location.X - marginLeft) / cellSize;
                int y = (((Button)sender).Location.Y - marginTop) / cellSize;
                Rabbit rabbit = new Rabbit(x, y, this);
                this.Controls.Add(rabbit.Img);
                rabbit.Img.BringToFront();
                rabbits.Add(rabbit);
               
            }
            if (!addRabbit)
            {
                // вычисляем столбец и строку
                int x = (((Button)sender).Location.X - marginLeft) / cellSize;
                int y = (((Button)sender).Location.Y - marginTop) / cellSize;
                Wolf wolf = new Wolf(x, y,this);
                this.Controls.Add(wolf.Img);
                wolf.Img.BringToFront();
                wolves.Add(wolf);
               
            }
            count_wolves.Text = wolves.Count.ToString();
            count_rabbits.Text = rabbits.Count.ToString();

        }
        // class Elem
        //{
        //    int x, y;
        //    int type;
        //    int gender;
        //}
        //Elem[,] matr;
        //Elem el = new Elem();
        //matr[]=el;
        // foreach (Wolf w in wolves) Elem.Add(w);
        //    foreach (Rabbit r in rabbits) animals.Add(r);


        public void Act()
        {
                  

            // задержка перед повторением движения
            // увеличь, если хочешь замедлить и наоборот

            timer_act.Interval = speed;

            timer_act.Tick += new EventHandler((o, ev) =>
            {
                try
                {
                    if (wolves.Count.ToString() == rabbits.Count.ToString())
                        if (wolves.Count.ToString() == "0")
                        {
                            End form = new ВолкииЁлки.End();
                            form.Show();
                            isPlaying = false;
                        }
                    foreach (Rabbit r in rabbits)
                    {
                        //приоритет действий:  размножиться -> погулять
                        if (!r.Reproduce())
                            r.Move();
                        if (r.HP <= 0)
                        {
                            // удаляем мёртвого зайца
                            Controls.Remove(r.Img);
                            Main.rabbits.Remove(r);
                        }

                    }
                    foreach (Wolf w in wolves)
                    {
                        //приоритет действий: поесть -> размножиться -> погулять
                        if (!w.AttackRabbit())
                            if (!w.Reproduce()) w.Move();
                        if (w.HP <= 0)
                        {
                            // удаляем мёртвого волка
                            Controls.Remove(w.Img);
                            Main.wolves.Remove(w);
                        }
                    }

                    
                }
                catch { }
                count_wolves.Text = wolves.Count.ToString();
                count_rabbits.Text = rabbits.Count.ToString();
                // прервать анимацию
                if (!isPlaying)
                {
                    Timer t = o as Timer;
                    t.Stop();
                }
                
            });
            timer_act.Start();
        }



       


        // ОБРАБОТЧИКИ НАЖАТИЙ И ДРИГИЕ ВТОРОСТЕПЕННЫЕ ВЕЩИ
        private void btnPlayPause_Click(object sender, EventArgs e)
        {
            if (isPlaying) isPlaying = false;
            else
            {
                isPlaying = true;
                //возобновить анимацию
                //Act();
                //Time();
                timer_act.Start();
                timer.Start();
            }
        }
        public void Time()
        {
            // задержка перед повторением движения
            // увеличь, если хочешь замедлить и наоборот
            timer.Interval = oneSecond;

            timer.Tick += new EventHandler((o, ev) =>
            {
                tim++;
                //var time = TimeSpan.FromSeconds(sec).ToString("HH:mm:ss");
                timer_label.Text = (tim/60).ToString()+" мин. "+ (tim%60).ToString() +" сек.";
                //"Текущее время: T: {0,T}"+ tim; //Convert.ToDateTime(tim.ToString() );//Format(DateTime.tim.ToString("HH:mm:ss");
                // прервать анимацию
                if (!isPlaying)
                {
                    Timer t = o as Timer;
                    t.Stop();
                }
            });
            timer.Start();
        
        }
        private void btnQuick_Click(object sender, EventArgs e)
        {
            if (!isQuick)
            {
                timer_act.Interval = speed / 50;
                timer.Interval /= 50;
                isQuick = true;
                
            } 
           else
            {
                timer_act.Interval = speed  ;
                timer.Interval = oneSecond ;
                isQuick = false ;
                
            }
            // timer1.Enabled = !timer1.enabled;

        }       
        private void timer_label_Click(object sender, EventArgs e)
        {

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

        }

        private void размерToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            foreach (Wolf w in wolves)
                Controls.Remove(w.Img);
            foreach (Rabbit r in rabbits)
                Controls.Remove(r.Img);
            wolves.Clear();
            rabbits.Clear();
            isPlaying = false;
            count_rabbits.Text = "0";
            count_wolves.Text = "0";
            tim = 0;
            timer_label.Text= "0 сек";

           
        }

        private void btnStep_Click(object sender, EventArgs e)
        {

            try
            {
                foreach (Rabbit r in rabbits)
                {
                    //приоритет действий:  размножиться -> погулять
                    if (!r.Reproduce())
                        r.Move();
                    if (r.HP <= 0)
                    {
                        // удаляем мёртвого зайца
                        Controls.Remove(r.Img);
                        Main.rabbits.Remove(r);
                    }

                }
                foreach (Wolf w in wolves)
                {
                    //приоритет действий: поесть -> размножиться -> погулять
                    if (!w.AttackRabbit())
                        if (!w.Reproduce()) w.Move();
                    if (w.HP <= 0)
                    {
                        // удаляем мёртвого волка
                        Controls.Remove(w.Img);
                        Main.wolves.Remove(w);
                    }
                }
            }
            catch { }
           
            count_wolves.Text = wolves.Count.ToString();
            count_rabbits.Text = rabbits.Count.ToString();
            isPlaying = false;
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //OpenFileDialog
            BinaryFormatter bf = new BinaryFormatter();
            List<Wolf> wo;
            wo = Animal.DeserializeFromXML();
            List<Rabbit > ra;
            ra = Animal.DeserializeFromXML_R();


            //Console.WriteLine("Выгружаем объекты из потока");
            FileStream f2 = new FileStream("Demo.bin", FileMode.Open);
            Animal  d = (Animal)bf.Deserialize(f2); // восстановление объекта
            //d.Show();
            Animal x = (Animal)bf.Deserialize(f2); //x.Show();
            Animal[] mas2 = (Animal[])bf.Deserialize(f2);
            foreach (Animal p in mas2)
            {
                Act();
            }
            f2.Close();
            DrawTable();
            
            Time();
            timer.Stop();
            timer_act.Stop();
            // по умолчанию сначала создаются зайчики
            makeRabbits.Checked = true;
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            foreach (Wolf w in wolves) animals.Add(w);
            foreach (Rabbit r in rabbits) animals.Add(r);
            var anim=animals.ToArray();


            //Animal.SerializeToXML(wolves);
            //Animal.SerializeToXML_R(rabbits);

            // string Path = "f";
            // FolderBrowserDialog fbd = new FolderBrowserDialog();
            // if (fbd.ShowDialog() == DialogResult.OK)
            // {
            //     Path = fbd.SelectedPath;
            // }
            // foreach (Wolf w in wolves) animals.Add(w);
            // foreach (Rabbit r in rabbits) animals.Add(r);
            // string content = Serialize(animals);
            //// string content = Control.Serialize(Control.Month);
            // File.WriteAllText(Path + "\\" + "ЖИЗНЬ" + ".txt", content);



            //File.WriteAllText("Животные"+ ".txt", content);



            FileStream f1 = new FileStream("Demo.bin", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(f1, animals); // сохранение объекта в потоке f
            //bf.Serialize(f1, p2); // сохранение объекта в потоке f
            Animal[] mas1 = anim;
            bf.Serialize(f1, mas1);
            f1.Close();
            //Console.WriteLine("Объекты сохранены в поток");



        }


        private void makeWolves_CheckedChanged(object sender, EventArgs e)
        {
            addRabbit = false;
        }
        private void makeRabbits_CheckedChanged(object sender, EventArgs e)
        {
            addRabbit = true;
        }

        //сериализация
        public static string Serialize(List<Animal> tData)
        {
            var serializer = new XmlSerializer(typeof(List<Animal>));
          List<Animal> list = new List<Animal>();
            foreach (Wolf w in wolves) animals.Add(w);
            foreach (Rabbit r in rabbits) animals.Add(r);
            TextWriter writer = new StringWriter();
            serializer.Serialize(writer, tData);

            return writer.ToString();
        }

        //десериализация
        public static List<Animal> Deserialize(string tData)
        {
            var serializer = new XmlSerializer(typeof(List<Animal>));

            TextReader reader = new StringReader(tData);

            return (List<Animal>)serializer.Deserialize(reader);
            
        }

        private void допИнфоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Волки__.Instruction form = new Волки__.Instruction();
            form.Show();
        }

        private void count_wolves_TextChanged(object sender, EventArgs e)
        {

        }


    }
        
}

 
  