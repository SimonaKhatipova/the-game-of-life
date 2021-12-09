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
    // это класс с пустыми клетками
    public class Cell
    {
        public Button cell = new Button();
        public Cell(int x, int y)
        {
            cell.Width = Main.cellSize;
            cell.Height = Main.cellSize;
            cell.BackColor = Color.LightGreen;
            // положение ячейки
            cell.Location = new Point(x * Main.cellSize + Main.marginLeft, y * Main.cellSize + Main.marginTop);
            // задаём рамку
            cell.FlatStyle = FlatStyle.Flat;
            cell.FlatAppearance.BorderColor = Color.Black;
            cell.FlatAppearance.BorderSize = 1;
        }
    }
}
