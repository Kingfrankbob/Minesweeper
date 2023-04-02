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
        //public Dictionary<int, byte> GridNums = new Dictionary<int, byte>();
        public Dictionary<Point, tile> GridPoint = new Dictionary<Point, tile>();
        public Dictionary<int, Button> GridButtons = new Dictionary<int, Button>();
        public Dictionary<int, bool> buttonStates = new Dictionary<int, bool>();
        public int mineCountNum = 0;
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
                    button.MouseUp += BackButton_MouseUp;
                    button.Click += button_Click;
                    button.Paint += BackButton_Paint;
                    button.ForeColor = System.Drawing.Color.White;
                    GridPoint.Add(button.Location, new tile(xx, yy, counter, 'n'));
                    buttonStates.Add(counter, false);
                    GridButtons.Add(counter, button);
                    //MessageBox.Show(button.Location.ToString());
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
                //GridButtons[i].Text = GridButtons[i].Text + ' ' + total.ToString();
            }

        }
        private void BackButton_Paint(object sender, PaintEventArgs e)
        {
            var blnButtonDown = buttonStates[Int32.Parse(sender.ToString().Split(':')[1])];
            if (Grid[Int32.Parse(sender.ToString().Split(':')[1])].type == 'm')
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
            var buttonNum = Int32.Parse(sender.ToString().Split(':')[1]);
            MessageBox.Show(Grid[buttonNum].x.ToString() + ' ' + Grid[buttonNum].y.ToString() + ' ' + buttonNum + ' ' + sender.ToString() + ' ' + Grid[buttonNum].disNum);
            if (Grid[buttonNum].type == 'm') MessageBox.Show("You hit a mine");
            else
            {
                //int xx = Grid[buttonNum].x, yy = Grid[buttonNum].y, total = 0;
                //try { if (GridPoint[new Point(xx - 16, yy - 16)].type == 'm') total++; } catch { }
                //try { if (GridPoint[new Point(xx, yy - 16)].type == 'm') total++; } catch { }
                //try { if (GridPoint[new Point(xx + 16, yy - 16)].type == 'm') total++;} catch { }
                //try { if (GridPoint[new Point(xx - 16, yy)].type == 'm') total++;} catch { }
                //try { if (GridPoint[new Point(xx + 16, yy)].type == 'm') total++;  } catch { }
                //try { if (GridPoint[new Point(xx - 16, yy + 16)].type == 'm') total++; } catch { }
                //try { if (GridPoint[new Point(xx, yy + 16)].type == 'm') total++; } catch { }
                //try { if (GridPoint[new Point(xx + 16, yy + 16)].type == 'm') total++; } catch { }

            }
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
