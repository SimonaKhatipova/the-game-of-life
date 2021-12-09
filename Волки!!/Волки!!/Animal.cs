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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ВолкииЁлки
{
    // это общий класс для зайцев и волков
    [Serializable]
    public abstract class Animal
    {
        static Random rnd = new Random();
        // пусть жизни и пол будут у всех (если потом понадобятся)
        // зайчики могут есть травку
        int hp;
        // если true, то самец
        bool gender;
        // определяет возможность размножения
        // пока не будет больше определённого значения, размножение не получится
        int reproducePoints;
        // само изображение
        PictureBox image = new PictureBox();
        public static Image rabImg = Image.FromFile("зайка.png"),
            wolfImg = Image.FromFile("волк.png"),
            wolf_fe_Img = Image.FromFile("волчица.png"),
            reprImg = Image.FromFile("бабочка для разм.png"),
            AttImg = Image.FromFile("для охоты.png");

        //конструктор
      
        public Animal(int x, int y)
        {
            image.Width = Main.cellSize;
            image.Height = Main.cellSize;
           
            // надо сделать задний фон прозрачным, поэтому цвет совпадает с цветом ячейки
            image.BackColor = Color.Green;
            // положение изображения
            image.Location = new Point(x * Main.cellSize + Main.marginLeft, y * Main.cellSize + Main.marginTop);
            // картинка будет уменьшаться по размеру клетки. Лучше подбирать близкие к квадратным
            image.SizeMode = PictureBoxSizeMode.Zoom;
            // изначально все особи способны размножаться
            ReproducePoints = 0;// будет увеличиваться с количеством ходов, когда достигнет требуемого, животное сможет размножаться
        }

        // свойства //всего 30 здоровья мало!! надо 200
        public int HP
        {
            get { return hp; }
            set
            { 
                hp = value;//200 и 50
                
            }
        }
        public int ReproducePoints
        {
            get { return reproducePoints; }
            set { reproducePoints = value; }
        }
        [XmlElement("Gender")]
        public bool Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        [XmlElement("Img")]
        public  PictureBox Img
        {
            get { return image; }
            set { image = value; }
        }


   
    // АБСТРАКТНЫй, т.к.зайцы свободно передвига.ncz
    public abstract void Move();

        // метод размножения немного отличается
        // если был создан ребёнок, то вернёт true
        public abstract bool Reproduce();

        //удаление мертвых особей в ACT() при просмотре массива животных

        //серия волков
        static public void SerializeToXML(List<Wolf> dr)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Wolf>));
            TextWriter textWriter = new StreamWriter(@"wolves.xml");
            serializer.Serialize(textWriter, dr);
            textWriter.Close();
        }
        //дисерия волков
        static public List<Wolf> DeserializeFromXML()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<Wolf>));
            TextReader textReader = new StreamReader(@"wolves.xml");
            List<Wolf> wol;
            wol = (List<Wolf>)deserializer.Deserialize(textReader);
            textReader.Close();
            return wol;
        }
        //серия зайцев
        static public void SerializeToXML_R(List<Rabbit> dr)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Rabbit>));
            TextWriter textWriter = new StreamWriter(@"rabbiits.xml");
            serializer.Serialize(textWriter, dr);
            textWriter.Close();
        }
        //дисерия зайцев
        static public List<Rabbit> DeserializeFromXML_R()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<Rabbit>));
            TextReader textReader = new StreamReader(@"rabbits.xml");
            List<Rabbit> wol;
            wol = (List<Rabbit>)deserializer.Deserialize(textReader);
            textReader.Close();
            return wol;
        }

    }
}
