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
    // Next is right click function!!!
    public partial class Form1 : Form
    {
        public Dictionary<int, tile> Grid = new Dictionary<int, tile>();
        public Dictionary<int, Button> GridButtons = new Dictionary<int, Button>();
        public Dictionary<int, PictureBox> GridNums = new Dictionary<int, PictureBox>();
        public Dictionary<int, PictureBox> GridFlags = new Dictionary<int, PictureBox>();
        public Dictionary<(int x, int y), int> pointToNum = new Dictionary<(int x, int y), int>();
        public Dictionary<int, bool> buttonStates = new Dictionary<int, bool>();
        public Dictionary<Point, tile> GridPoint = new Dictionary<Point, tile>();
        public List<int> correctHits = new List<int>();
        public List<int> generalHits = new List<int>();
        public List<int> MineList = new List<int>();
        public int mineCountNum = 0;
        public int mineNum = 0;
        public int mineNumCorrect = 0;
        public Form1()
        {
            var random = new Random();
            var Yes = ShowInputDialog(ref mineCountNum);
            
            mineNum = mineCountNum;
            int boardx = 16, boardy = 16;
            if (mineCountNum == 99) { boardx = 30; boardy = 16; }
            else if (mineCountNum < 99 && mineCountNum > 40) { boardx = 16; boardy = 16; }
            else { boardx = 10; boardy = 10; }

            MessageBox.Show(((boardy + 72) * 16).ToString() + " " + ((boardy + 72) * 16).ToString());

            Size = new Size((boardy + 72) * 16, (boardy + 72) * 16);



            InitializeComponent();
            int xx = 24, yy = 48, counter = 0;
            for (int i = 0; i < boardy; i++)
            {
                for (int j = 0; j < boardx; j++)
                {
                    var picbox = new System.Windows.Forms.PictureBox();
                    var flagbox = new System.Windows.Forms.PictureBox();
                    flagbox.Name = counter.ToString();
                    flagbox.Hide();
                    flagbox.Location = new System.Drawing.Point(xx, yy);
                    flagbox.Size = new System.Drawing.Size(16, 16);
                    flagbox.MouseDown += pic_MouseDown;
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
                    button.MouseDown += button_MouseDown;
                    button.Paint += BackButton_Paint;
                    button.ForeColor = System.Drawing.Color.Black;
                    GridPoint.Add(button.Location, new tile(xx, yy, counter, 'n'));
                    buttonStates.Add(counter, false);
                    GridButtons.Add(counter, button);
                    this.Controls.Add(picbox);
                    this.Controls.Add(flagbox);
                    GridFlags.Add(counter, flagbox);
                    GridNums.Add(counter, picbox);
                    this.Controls.Add(button);
                    pointToNum.Add((xx, yy), counter);
                    xx += 16;
                    counter++;
                }
                xx = 24;
                yy += 16;
            }
            var maxLimit = boardx * boardy;

            var randomList = new List<int>();
            for (int i = 0; i < mineCountNum; i++)
            {
                var rad = random.Next(0, maxLimit);
                if (!randomList.Contains(rad)) randomList.Add(rad);
                else
                {
                    do
                    {
                        var radTry = random.Next(0, maxLimit);
                        rad = radTry;
                    }
                    while (randomList.Contains(rad));
                    randomList.Add(rad);
                }
            }
            foreach (var value in randomList)
            {
                Grid[value].changeType('m');
                MineList.Add(value);
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
        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Right Clicckckckkckckckckckkckckckckc
                var send = sender as PictureBox;
                var buttonNum = Int32.Parse(send.Name);
                handleImage(buttonNum);
            }
        }

        private void button_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var buttonNum = Int32.Parse(sender.ToString().Split(':')[1]);
                if (Grid[buttonNum].type == 'm')
                {
                    foreach (var item in MineList)
                    {
                        GridNums[item].Show();
                        GridNums[item].BringToFront();
                        GridButtons[item].SendToBack();
                    }
                    MessageBox.Show("You hit a mine\nGame Over!!! :(");
                    foreach(var item in Grid)
                        if(item.Value.type == 'm')
                        {
                            // Set the Mine to a red background one to see what they missed XD or not whatevers
                            GridButtons[buttonNum].Hide();
                            GridButtons[buttonNum].SendToBack();
                            //GridNums[buttonNum].Image = 
                            GridNums[buttonNum].Show();
                            GridNums[buttonNum].BringToFront();
                        }
                    Application.Restart();
                }
                else
                {
                    if (GridNums[buttonNum].Image == null)
                    {
                        revealAroundNew(buttonNum);
                    }
                    GridNums[buttonNum].Show();
                    GridNums[buttonNum].BringToFront();
                    GridButtons[buttonNum].SendToBack();
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                var buttonNum = Int32.Parse(sender.ToString().Split(':')[1]);
                buttonStates[buttonNum] = false;
                handleImage(buttonNum);
            }
        }

        private void handleImage(int buttonNum)
        {
            mineNumberBox.Text = (mineNum - generalHits.Count).ToString();
            var current = Grid[buttonNum].disPlay;
            current++;
            if (current > 2) current = 0;
            Grid[buttonNum].disPlay = current;

            if (current == 0)
            {
                GridFlags[buttonNum].Image = global::Minesweeper.Properties.Resources.Mineflag;
                GridFlags[buttonNum].BringToFront();
                GridFlags[buttonNum].Show();
                if (!generalHits.Contains(buttonNum)) generalHits.Add(buttonNum);

                if (Grid[buttonNum].type == 'm')
                {
                    if (!correctHits.Contains(buttonNum))
                    {
                        correctHits.Add(buttonNum);
                    }
                }
                mineNumberBox.Text = (mineNum - generalHits.Count).ToString();
                if(correctHits.Count == mineCountNum)
                {
                    MessageBox.Show("Good Job, you Won!!");
                    Application.Restart();
                }


            }
            else if (current == 1)
            {
                GridFlags[buttonNum].Image = global::Minesweeper.Properties.Resources.Questionmark;
                GridFlags[buttonNum].BringToFront();
                GridFlags[buttonNum].Show();
                if (correctHits.Contains(buttonNum)) correctHits.Remove(buttonNum);
                if (generalHits.Contains(buttonNum)) generalHits.Remove(buttonNum);
            }
            else if (current == 2)
            {
                GridNums[buttonNum].Image = null;
                GridFlags[buttonNum].SendToBack();
                GridFlags[buttonNum].Hide();
                if (generalHits.Contains(buttonNum)) generalHits.Remove(buttonNum);
                if (correctHits.Contains(buttonNum)) correctHits.Remove(buttonNum);
                //Grid[buttonNum].
            }
        

    }

        private void revealAroundNew(int num)
        {
            var xx = Grid[num].x;
            var yy = Grid[num].y;
            var points = new (int x, int y)[] { (xx + 16, yy), (xx - 16, yy), (xx, yy - 16), (xx, yy + 16) };
            foreach (var value in points)
                try
                {
                    int i = value.x, j = value.y;
                    if (Grid[GridPoint[new Point(i, j)].count].type == 'b')
                    {
                        if (Grid[GridPoint[new Point(i, j)].count].revealed)
                        {
                            continue;
                        }
                        Grid[GridPoint[new Point(i, j)].count].revealed = true;
                        GridNums[GridPoint[new Point(i, j)].count].Show();
                        GridNums[GridPoint[new Point(i, j)].count].BringToFront();
                        GridButtons[GridPoint[new Point(i, j)].count].SendToBack();
                        revealAroundNew(GridPoint[new Point(i, j)].count);
                    }
                    else
                    {
                        Grid[GridPoint[new Point(i, j)].count].revealed = true;
                        GridNums[GridPoint[new Point(i, j)].count].Show();
                        GridNums[GridPoint[new Point(i, j)].count].BringToFront();
                        GridButtons[GridPoint[new Point(i, j)].count].SendToBack();
                        Grid[GridPoint[new Point(i, j)].count].revealed = true;
                    }
                }
                catch (Exception)
                {
                    continue;
                    //Ignore because it could be out of bounds and am to lazy
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
        public int disPlay { get; set; }
        public bool revealed { get; set; }
        public bool correct { get; set; }
        public tile(int x, int y, int count, char type)
        {
            this.x = x;
            this.y = y;
            this.count = count;
            this.type = type;
            this.disPlay = 2;
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
