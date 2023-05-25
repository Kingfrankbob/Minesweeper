using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        public Dictionary<int, tile> Grid = new Dictionary<int, tile>();
        public Dictionary<int, Button> GridButtons = new Dictionary<int, Button>();
        public Dictionary<int, PictureBox> GridNums = new Dictionary<int, PictureBox>();
        public Dictionary<int, bool> buttonStates = new Dictionary<int, bool>();
        public Dictionary<Point, tile> GridPoint = new Dictionary<Point, tile>();
        public List<Point> checkedPoints = new List<Point>();
        public List<Point> needToCheck = new List<Point>();
        public int mineCountNum = 0;
        public int mineNum = 0;
        public int mineNumCorrect = 0;
        public Form1()
        {
            var random = new Random();
            var Yes = ShowInputDialog(ref mineCountNum);
            InitializeComponent();
            int xx = 24, yy = 48, counter = 0;
            for (int i = 0; i < 23; i++)
            {
                for (int j = 0; j < 23; j++)
                {
                    var picbox = new System.Windows.Forms.PictureBox();
                    picbox.Name = counter.ToString();
                    picbox.Hide();
                    picbox.Location = new System.Drawing.Point(xx, yy);
                    picbox.Size = new System.Drawing.Size(16, 16);
                    Grid.Add(counter, new tile(xx, yy, counter, 'n'));
                    var button = new System.Windows.Forms.Button();
                    button.Name = counter.ToString();
                    button.Show();
                    button.Location = new System.Drawing.Point(xx, yy);
                    button.Size = new System.Drawing.Size(16, 16);
                    button.TabIndex = 2;
                    button.Text = counter.ToString();
                    button.UseVisualStyleBackColor = true;
                    button.MouseDown += BackButton_MouseDown;
                    button.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.857143F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    button.Margin = new System.Windows.Forms.Padding(0);
                    button.Click += button_Click;
                    button.Paint += BackButton_Paint;
                    button.ForeColor = System.Drawing.Color.Black;
                    GridPoint.Add(button.Location, new tile(xx, yy, counter, 'n'));
                    buttonStates.Add(counter, false);
                    GridButtons.Add(counter, button);
                    this.Controls.Add(picbox);
                    GridNums.Add(counter, picbox);
                    this.Controls.Add(button);
                    xx += 16;
                    counter++;
                }
                xx = 24;
                yy += 16;
            }

            var randomList = new List<int>();
            for (int i = 0; i < mineCountNum; i++)
            {
                var rad = random.Next(0, 528);
                if (!randomList.Contains(rad)) randomList.Add(rad);
                else
                {
                    do
                    {
                        var radTry = random.Next(0, 528);
                        rad = radTry;
                    }
                    while (randomList.Contains(rad));
                    randomList.Add(rad);
                }
            }
            foreach (var value in randomList)
            {
                Grid[value].changeType('m');
                int xxx = Grid[value].x;
                int yyy = Grid[value].y;
                GridPoint[new Point(xxx, yyy)].changeType('m');
            }
            for (int i = 0; i < Grid.Count() - 1; i++)
            {
                if (Grid[i].type == 'm') continue;
                byte total = 0;
                xx = Grid[i].x;
                yy = Grid[i].y;
                // 1 2 3 
                // 4 P 5
                // 6 7 8
                try { if (GridPoint[new Point(xx - 16, yy - 16)].type == 'm') total++; } catch { }
                try { if (GridPoint[new Point(xx, yy - 16)].type == 'm') total++; } catch { }
                try { if (GridPoint[new Point(xx + 16, yy - 16)].type == 'm') total++; } catch { }
                try { if (GridPoint[new Point(xx - 16, yy)].type == 'm') total++; } catch { }
                try { if (GridPoint[new Point(xx + 16, yy)].type == 'm') total++; } catch { }
                try { if (GridPoint[new Point(xx - 16, yy + 16)].type == 'm') total++; } catch { }
                try { if (GridPoint[new Point(xx, yy + 16)].type == 'm') total++; } catch { }
                try { if (GridPoint[new Point(xx + 16, yy + 16)].type == 'm') total++; } catch { }
                Grid[i].disNum = total;
            }
            counter = 0;
            foreach (var picbox in GridNums)
            {
                switch (Grid[counter].disNum)
                {
                    case 1: picbox.Value.Image = global::Minesweeper.Properties.Resources._1; Grid[counter].changeType('n'); break;
                    case 2: picbox.Value.Image = global::Minesweeper.Properties.Resources._2; Grid[counter].changeType('n'); break;
                    case 3: picbox.Value.Image = global::Minesweeper.Properties.Resources._3; Grid[counter].changeType('n'); break;
                    case 4: picbox.Value.Image = global::Minesweeper.Properties.Resources._4; Grid[counter].changeType('n'); break;
                    case 5: picbox.Value.Image = global::Minesweeper.Properties.Resources._5; Grid[counter].changeType('n'); break;
                    case 0: if (Grid[counter].type != 'm') { picbox.Value.Image = null; Grid[counter].changeType('b'); } else { picbox.Value.Image = global::Minesweeper.Properties.Resources.mine; } break;
                    default: if (Grid[counter].type != 'm') { picbox.Value.Image = null; Grid[counter].changeType('b'); } else { picbox.Value.Image = global::Minesweeper.Properties.Resources.mine; } break;
                }
                counter++;
            }


        }
        private void BackButton_Paint(object sender, PaintEventArgs e)
        {
            var blnButtonDown = buttonStates[Int32.Parse(sender.ToString().Split(':')[1])];
            if (blnButtonDown == false)
            {
                ControlPaint.DrawBorder(e.Graphics, (sender as System.Windows.Forms.Button).ClientRectangle,
                    System.Drawing.SystemColors.ControlLightLight, 20, ButtonBorderStyle.Outset,
                    System.Drawing.SystemColors.ControlLightLight, 20, ButtonBorderStyle.Outset,
                    System.Drawing.SystemColors.ControlLightLight, 20, ButtonBorderStyle.Outset,
                    System.Drawing.SystemColors.ControlLightLight, 20, ButtonBorderStyle.Outset);
            }
            else
            {
                ControlPaint.DrawBorder(e.Graphics, (sender as System.Windows.Forms.Button).ClientRectangle,
                    System.Drawing.SystemColors.ControlLightLight, 20, ButtonBorderStyle.Inset,
                    System.Drawing.SystemColors.ControlLightLight, 20, ButtonBorderStyle.Inset,
                    System.Drawing.SystemColors.ControlLightLight, 20, ButtonBorderStyle.Inset,
                    System.Drawing.SystemColors.ControlLightLight, 20, ButtonBorderStyle.Inset);
            }
        }
        private void BackButton_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            buttonStates[Int32.Parse(sender.ToString().Split(':')[1])] = true;
        }
        private void BackButton_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            buttonStates[Int32.Parse(sender.ToString().Split(':')[1])] = false;
        }
        private void button_Click(object sender, EventArgs e)
        {
            
        }

        private void button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var buttonNum = Int32.Parse(sender.ToString().Split(':')[1]);
                // MessageBox.Show(Grid[buttonNum].x.ToString() + "<-X Y->" + Grid[buttonNum].y.ToString() + " Num->" + buttonNum + " SenderToString->" + sender.ToString() + " Mines->" + Grid[buttonNum].disNum);
                if (Grid[buttonNum].type == 'm') MessageBox.Show("You hit a mine");
                else
                {
                    if (GridNums[buttonNum].Image == null)
                    {
                        revealAroundBlank(buttonNum);
                        do
                        {
                            var next = needToCheck[0];
                            needToCheck.RemoveAt(0);
                            checkedPoints.Add(next);
                            revealAroundBlank(next.X, next.Y);
                        } while (needToCheck.Count != 0);
                    }
                    GridNums[buttonNum].Show();
                    GridNums[buttonNum].BringToFront();
                    GridButtons[buttonNum].SendToBack();
                }
            }
            if(e.Button == MouseButtons.Right)
            {

            }
        }
        private void revealAroundBlank(int number)
        {
            var xx = Grid[number].x;
            var yy = Grid[number].y;
            try
            {
                if (Grid[GridPoint[new Point(xx - 16, yy - 16)].count].type == 'b' && !checkedPoints.Contains(new Point(xx - 16, yy - 16)) && !needToCheck.Contains(new Point(xx - 16, yy - 16)))
                {
                    checkedPoints.Add(new Point(xx - 16, yy - 16));
                    GridNums[GridPoint[new Point(xx - 16, yy - 16)].count].Show();
                    GridNums[GridPoint[new Point(xx - 16, yy - 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx - 16, yy - 16)].count].SendToBack();
                    needToCheck.Add(new Point(xx - 16, yy - 16));
                }
                else
                {
                    checkedPoints.Add(new Point(xx - 16, yy - 16));
                    GridNums[GridPoint[new Point(xx - 16, yy - 16)].count].Show();
                    GridNums[GridPoint[new Point(xx - 16, yy - 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx - 16, yy - 16)].count].SendToBack();

                }
            }
            catch (Exception e) { MessageBox.Show(e.ToString() + " <-Error"); }
            try
            {
                if (Grid[GridPoint[new Point(xx, yy - 16)].count].type == 'b' && !checkedPoints.Contains(new Point(xx, yy - 16)) && !needToCheck.Contains(new Point(xx, yy - 16)))
                {
                    checkedPoints.Add(new Point(xx, yy - 16));
                    GridNums[GridPoint[new Point(xx, yy - 16)].count].Show();
                    GridNums[GridPoint[new Point(xx, yy - 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx, yy - 16)].count].SendToBack();
                    needToCheck.Add(new Point(xx, yy - 16));
                }
                else
                {
                    checkedPoints.Add(new Point(xx, yy - 16));
                    GridNums[GridPoint[new Point(xx, yy - 16)].count].Show();
                    GridNums[GridPoint[new Point(xx, yy - 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx, yy - 16)].count].SendToBack();
                }
            }
            catch { }
            try
            {
                if (Grid[GridPoint[new Point(xx + 16, yy - 16)].count].type == 'b' && !checkedPoints.Contains(new Point(xx + 16, yy - 16)) && !needToCheck.Contains(new Point(xx + 16, yy - 16)))
                {
                    checkedPoints.Add(new Point(xx + 16, yy - 16));
                    GridNums[GridPoint[new Point(xx + 16, yy - 16)].count].Show();
                    GridNums[GridPoint[new Point(xx + 16, yy - 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx + 16, yy - 16)].count].SendToBack();
                    needToCheck.Add(new Point(xx + 16, yy - 16));
                }
                else
                {
                    checkedPoints.Add(new Point(xx + 16, yy - 16));
                    GridNums[GridPoint[new Point(xx + 16, yy - 16)].count].Show();
                    GridNums[GridPoint[new Point(xx + 16, yy - 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx + 16, yy - 16)].count].SendToBack();
                }
            }
            catch { }
            try
            {
                if (Grid[GridPoint[new Point(xx - 16, yy)].count].type == 'b' && !checkedPoints.Contains(new Point(xx - 16, yy)) && !needToCheck.Contains(new Point(xx - 16, yy)))
                {
                    checkedPoints.Add(new Point(xx - 16, yy));
                    GridNums[GridPoint[new Point(xx - 16, yy)].count].Show();
                    GridNums[GridPoint[new Point(xx - 16, yy)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx - 16, yy)].count].SendToBack();
                    needToCheck.Add(new Point(xx - 16, yy));
                }
                else
                {
                    checkedPoints.Add(new Point(xx - 16, yy));
                    GridNums[GridPoint[new Point(xx - 16, yy)].count].Show();
                    GridNums[GridPoint[new Point(xx - 16, yy)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx - 16, yy)].count].SendToBack();
                }
            }
            catch { }
            try
            {
                if (Grid[GridPoint[new Point(xx + 16, yy)].count].type == 'b' && !checkedPoints.Contains(new Point(xx + 16, yy)) && !needToCheck.Contains(new Point(xx + 16, yy)))
                {
                    checkedPoints.Add(new Point(xx + 16, yy));
                    GridNums[GridPoint[new Point(xx + 16, yy)].count].Show();
                    GridNums[GridPoint[new Point(xx + 16, yy)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx + 16, yy)].count].SendToBack();
                    needToCheck.Add(new Point(xx + 16, yy));
                }
                else
                {
                    checkedPoints.Add(new Point(xx + 16, yy));
                    GridNums[GridPoint[new Point(xx + 16, yy)].count].Show();
                    GridNums[GridPoint[new Point(xx + 16, yy)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx + 16, yy)].count].SendToBack();
                }
            }
            catch { }
            try
            {
                if (Grid[GridPoint[new Point(xx - 16, yy + 16)].count].type == 'b' && !checkedPoints.Contains(new Point(xx - 16, yy + 16)) && !needToCheck.Contains(new Point(xx - 16, yy + 16)))
                {
                    checkedPoints.Add(new Point(xx - 16, yy + 16));
                    GridNums[GridPoint[new Point(xx - 16, yy + 16)].count].Show();
                    GridNums[GridPoint[new Point(xx - 16, yy + 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx - 16, yy + 16)].count].SendToBack();
                    needToCheck.Add(new Point(xx - 16, yy + 16));
                }
                else
                {
                    checkedPoints.Add(new Point(xx - 16, yy + 16));
                    GridNums[GridPoint[new Point(xx - 16, yy + 16)].count].Show();
                    GridNums[GridPoint[new Point(xx - 16, yy + 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx - 16, yy + 16)].count].SendToBack();
                }
            }
            catch { }
            try
            {
                if (Grid[GridPoint[new Point(xx, yy + 16)].count].type == 'b' && !checkedPoints.Contains(new Point(xx, yy + 16)) && !needToCheck.Contains(new Point(xx, yy + 16)))
                {
                    checkedPoints.Add(new Point(xx, yy + 16));
                    GridNums[GridPoint[new Point(xx, yy + 16)].count].Show();
                    GridNums[GridPoint[new Point(xx, yy + 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx, yy + 16)].count].SendToBack();
                    needToCheck.Add(new Point(xx, yy + 16));
                }
                else
                {
                    checkedPoints.Add(new Point(xx, yy + 16));
                    GridNums[GridPoint[new Point(xx, yy + 16)].count].Show();
                    GridNums[GridPoint[new Point(xx, yy + 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx, yy + 16)].count].SendToBack();
                }
            }
            catch { }
            try
            {
                if (Grid[GridPoint[new Point(xx + 16, yy + 16)].count].type == 'b' && !checkedPoints.Contains(new Point(xx + 16, yy + 16)) && !needToCheck.Contains(new Point(xx + 16, yy + 16)))

                {
                    checkedPoints.Add(new Point(xx + 16, yy + 16));
                    GridNums[GridPoint[new Point(xx + 16, yy + 16)].count].Show();
                    GridNums[GridPoint[new Point(xx + 16, yy + 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx + 16, yy + 16)].count].SendToBack();
                    needToCheck.Add(new Point(xx + 16, yy + 16));
                }
                else
                {
                    checkedPoints.Add(new Point(xx + 16, yy + 16));
                    GridNums[GridPoint[new Point(xx + 16, yy + 16)].count].Show();
                    GridNums[GridPoint[new Point(xx + 16, yy + 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx + 16, yy + 16)].count].SendToBack();
                }
            }
            catch { }
        }







        private void revealAroundBlank(int xx, int yy)
        {
            try
            {
                if (Grid[GridPoint[new Point(xx - 16, yy - 16)].count].type == 'b' && !checkedPoints.Contains(new Point(xx - 16, yy - 16)) && !needToCheck.Contains(new Point(xx - 16, yy - 16)))
                {
                    checkedPoints.Add(new Point(xx - 16, yy - 16));
                    GridNums[GridPoint[new Point(xx - 16, yy - 16)].count].Show();
                    GridNums[GridPoint[new Point(xx - 16, yy - 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx - 16, yy - 16)].count].SendToBack();
                    needToCheck.Add(new Point(xx - 16, yy - 16));
                }
                else
                {
                    checkedPoints.Add(new Point(xx - 16, yy - 16));
                    GridNums[GridPoint[new Point(xx - 16, yy - 16)].count].Show();
                    GridNums[GridPoint[new Point(xx - 16, yy - 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx - 16, yy - 16)].count].SendToBack();
                }
            }
            catch { }
            try
            {
                if (Grid[GridPoint[new Point(xx, yy - 16)].count].type == 'b' && !checkedPoints.Contains(new Point(xx, yy - 16)) && !needToCheck.Contains(new Point(xx, yy - 16)))
                {
                    checkedPoints.Add(new Point(xx, yy - 16));
                    GridNums[GridPoint[new Point(xx, yy - 16)].count].Show();
                    GridNums[GridPoint[new Point(xx, yy - 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx, yy - 16)].count].SendToBack();
                    needToCheck.Add(new Point(xx, yy - 16));
                }
                else
                {
                    checkedPoints.Add(new Point(xx, yy - 16));
                    GridNums[GridPoint[new Point(xx, yy - 16)].count].Show();
                    GridNums[GridPoint[new Point(xx, yy - 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx, yy - 16)].count].SendToBack();
                }
            }
            catch { }
            try
            {
                if (Grid[GridPoint[new Point(xx + 16, yy - 16)].count].type == 'b' && !checkedPoints.Contains(new Point(xx + 16, yy - 16)) && !needToCheck.Contains(new Point(xx + 16, yy - 16)))
                {
                    checkedPoints.Add(new Point(xx + 16, yy - 16));
                    GridNums[GridPoint[new Point(xx + 16, yy - 16)].count].Show();
                    GridNums[GridPoint[new Point(xx + 16, yy - 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx + 16, yy - 16)].count].SendToBack();
                    needToCheck.Add(new Point(xx + 16, yy - 16));
                }
                else
                {
                    checkedPoints.Add(new Point(xx + 16, yy - 16));
                    GridNums[GridPoint[new Point(xx + 16, yy - 16)].count].Show();
                    GridNums[GridPoint[new Point(xx + 16, yy - 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx + 16, yy - 16)].count].SendToBack();
                }
            }
            catch { }
            try
            {
                if (Grid[GridPoint[new Point(xx - 16, yy)].count].type == 'b' && !checkedPoints.Contains(new Point(xx - 16, yy)) && !needToCheck.Contains(new Point(xx - 16, yy)))
                {
                    checkedPoints.Add(new Point(xx - 16, yy));
                    GridNums[GridPoint[new Point(xx - 16, yy)].count].Show();
                    GridNums[GridPoint[new Point(xx - 16, yy)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx - 16, yy)].count].SendToBack();
                    needToCheck.Add(new Point(xx - 16, yy));
                }
                else
                {
                    checkedPoints.Add(new Point(xx - 16, yy));
                    GridNums[GridPoint[new Point(xx - 16, yy)].count].Show();
                    GridNums[GridPoint[new Point(xx - 16, yy)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx - 16, yy)].count].SendToBack();
                }
            }
            catch { }
            try
            {
                if (Grid[GridPoint[new Point(xx + 16, yy)].count].type == 'b' && !checkedPoints.Contains(new Point(xx + 16, yy)) && !needToCheck.Contains(new Point(xx + 16, yy)))
                {
                    checkedPoints.Add(new Point(xx + 16, yy));
                    GridNums[GridPoint[new Point(xx + 16, yy)].count].Show();
                    GridNums[GridPoint[new Point(xx + 16, yy)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx + 16, yy)].count].SendToBack();
                    needToCheck.Add(new Point(xx + 16, yy));
                }
                else
                {
                    checkedPoints.Add(new Point(xx + 16, yy));
                    GridNums[GridPoint[new Point(xx + 16, yy)].count].Show();
                    GridNums[GridPoint[new Point(xx + 16, yy)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx + 16, yy)].count].SendToBack();
                }
            }
            catch { }
            try
            {
                if (Grid[GridPoint[new Point(xx - 16, yy + 16)].count].type == 'b' && !checkedPoints.Contains(new Point(xx - 16, yy + 16)) && !needToCheck.Contains(new Point(xx - 16, yy + 16)))
                {
                    checkedPoints.Add(new Point(xx - 16, yy + 16));
                    GridNums[GridPoint[new Point(xx - 16, yy + 16)].count].Show();
                    GridNums[GridPoint[new Point(xx - 16, yy + 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx - 16, yy + 16)].count].SendToBack();
                    needToCheck.Add(new Point(xx - 16, yy + 16));
                }
                else
                {
                    checkedPoints.Add(new Point(xx - 16, yy + 16));
                    GridNums[GridPoint[new Point(xx - 16, yy + 16)].count].Show();
                    GridNums[GridPoint[new Point(xx - 16, yy + 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx - 16, yy + 16)].count].SendToBack();
                }
            }
            catch { }
            try
            {
                if (Grid[GridPoint[new Point(xx, yy + 16)].count].type == 'b' && !checkedPoints.Contains(new Point(xx, yy + 16)) && !needToCheck.Contains(new Point(xx, yy + 16)))
                {
                    checkedPoints.Add(new Point(xx, yy + 16));
                    GridNums[GridPoint[new Point(xx, yy + 16)].count].Show();
                    GridNums[GridPoint[new Point(xx, yy + 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx, yy + 16)].count].SendToBack();
                    needToCheck.Add(new Point(xx, yy + 16));
                }
                else
                {
                    checkedPoints.Add(new Point(xx, yy + 16));
                    GridNums[GridPoint[new Point(xx, yy + 16)].count].Show();
                    GridNums[GridPoint[new Point(xx, yy + 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx, yy + 16)].count].SendToBack();
                }
            }
            catch { }
            try
            {
                if (Grid[GridPoint[new Point(xx + 16, yy + 16)].count].type == 'b' && !checkedPoints.Contains(new Point(xx + 16, yy + 16)) && !needToCheck.Contains(new Point(xx + 16, yy + 16)))
                {
                    checkedPoints.Add(new Point(xx + 16, yy + 16));
                    GridNums[GridPoint[new Point(xx + 16, yy + 16)].count].Show();
                    GridNums[GridPoint[new Point(xx + 16, yy + 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx + 16, yy + 16)].count].SendToBack();
                    needToCheck.Add(new Point(xx + 16, yy + 16));
                }
                else
                {
                    checkedPoints.Add(new Point(xx + 16, yy + 16));
                    GridNums[GridPoint[new Point(xx + 16, yy + 16)].count].Show();
                    GridNums[GridPoint[new Point(xx + 16, yy + 16)].count].BringToFront();
                    GridButtons[GridPoint[new Point(xx + 16, yy + 16)].count].SendToBack();
                }
            }
            catch { }
            return;
        }
        private static DialogResult ShowInputDialog(ref int input)
        {
            System.Drawing.Size size = new System.Drawing.Size(200, 70);
            Form inputBox = new Form();

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = "MineNum";

            System.Windows.Forms.TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.Location = new System.Drawing.Point(5, 5);
            textBox.Text = input.ToString();
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = Int32.Parse(textBox.Text);
            return result;
        }

    }
    public class tile
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public int count { get; private set; }
        public char type { get; private set; }
        public int disNum { get; set; }
        public tile(int x, int y, int count, char type)
        {
            this.x = x;
            this.y = y;
            this.count = count;
            this.type = type;

        }
        public tile(int x, int y, int count)
        {
            this.x = x;
            this.y = y;
            this.count = count;
        }
        public void changeType(char type)
        {
            this.type = type;
        }

    }
}
