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
        private Dictionary<int, tile> _Grid = new Dictionary<int, tile>();
        private Dictionary<int, Button> _GridButtons = new Dictionary<int, Button>();
        private Dictionary<int, PictureBox> _GridNums = new Dictionary<int, PictureBox>();
        private Dictionary<int, PictureBox> _GridFlags = new Dictionary<int, PictureBox>();
        private Dictionary<(int x, int y), int> _pointToNum = new Dictionary<(int x, int y), int>();
        private Dictionary<int, bool> _buttonStates = new Dictionary<int, bool>();
        private Dictionary<Point, tile> _GridPoint = new Dictionary<Point, tile>();
        private List<int> _correctHits = new List<int>();
        private List<int> _generalHits = new List<int>();
        private List<int> _MineList = new List<int>();
        private int _mineCountNum = 0;
        private int _mineNum = 0;
        private bool _isCollapsed = true;
        public Form1()
        {
            var random = new Random();
            _mineCountNum = 30;
            InitializeComponent();
            createBoard(random);
        }
        private void clearEverything()
        {
            _Grid.Clear();
            foreach (var item in _GridButtons)
            {
                item.Value.Hide();
                item.Value.Enabled = false;
                this.Controls.Remove(item.Value);
            }
            foreach (var item in _GridFlags)
            {
                item.Value.Hide();
                item.Value.Enabled = false;
                this.Controls.Remove(item.Value);
            }
            foreach (var item in _GridNums)
            {
                item.Value.Hide();
                item.Value.Enabled = false;
                this.Controls.Remove(item.Value);
            }
            _GridButtons.Clear(); _GridFlags.Clear(); _GridNums.Clear(); _pointToNum.Clear(); _buttonStates.Clear(); _GridPoint.Clear(); _correctHits.Clear(); _generalHits.Clear(); _MineList.Clear(); _mineNum = 0;

        }
        private void createBoard(Random random)
        {
            clearEverything();
            _mineNum = _mineCountNum;
            int boardx = 16, boardy = 16;
            if (_mineCountNum == 99) { boardx = 30; boardy = 16; }
            else if (_mineCountNum < 99 && _mineCountNum > 40) { boardx = 16; boardy = 16; }
            else if (_mineCountNum > 20 && _mineCountNum <= 40) { boardx = _mineCountNum / 2; boardy = _mineCountNum / 2; }
            else if (_mineCountNum <= 20 && _mineCountNum > 5) { boardx = 10; boardy = 10; }
            else if (_mineCountNum != 1) { boardx = 3; boardy = 3; }
            else { boardx = 1; boardy = 1; }

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
                    _Grid.Add(counter, new tile(xx, yy, counter, 'n'));
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
                    _GridPoint.Add(button.Location, new tile(xx, yy, counter, 'n'));
                    _buttonStates.Add(counter, false);
                    _GridButtons.Add(counter, button);
                    this.Controls.Add(picbox);
                    this.Controls.Add(flagbox);
                    _GridFlags.Add(counter, flagbox);
                    _GridNums.Add(counter, picbox);
                    this.Controls.Add(button);
                    _pointToNum.Add((xx, yy), counter);
                    xx += 16;
                    counter++;
                }
                xx = 24;
                yy += 16;
            }
            var maxLimit = boardx * boardy;

            var randomList = new List<int>();
            for (int i = 0; i < _mineCountNum; i++)
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
                _Grid[value].changeType('m');
                _MineList.Add(value);
                int xxx = _Grid[value].x;
                int yyy = _Grid[value].y;
                _GridPoint[new Point(xxx, yyy)].changeType('m');
            }
            for (int i = 0; i < _Grid.Count() - 1; i++)
            {
                if (_Grid[i].type == 'm') continue;
                byte total = 0;
                xx = _Grid[i].x;
                yy = _Grid[i].y;
                // 1 2 3 
                // 4 P 5
                // 6 7 8
                try { if (_GridPoint[new Point(xx - 16, yy - 16)].type == 'm') total++; } catch { }
                try { if (_GridPoint[new Point(xx, yy - 16)].type == 'm') total++; } catch { }
                try { if (_GridPoint[new Point(xx + 16, yy - 16)].type == 'm') total++; } catch { }
                try { if (_GridPoint[new Point(xx - 16, yy)].type == 'm') total++; } catch { }
                try { if (_GridPoint[new Point(xx + 16, yy)].type == 'm') total++; } catch { }
                try { if (_GridPoint[new Point(xx - 16, yy + 16)].type == 'm') total++; } catch { }
                try { if (_GridPoint[new Point(xx, yy + 16)].type == 'm') total++; } catch { }
                try { if (_GridPoint[new Point(xx + 16, yy + 16)].type == 'm') total++; } catch { }
                _Grid[i].disNum = total;
            }
            counter = 0;
            foreach (var picbox in _GridNums)
            {
                switch (_Grid[counter].disNum)
                {
                    case 1: picbox.Value.Image = global::Minesweeper.Properties.Resources._1; _Grid[counter].changeType('n'); break;
                    case 2: picbox.Value.Image = global::Minesweeper.Properties.Resources._2; _Grid[counter].changeType('n'); break;
                    case 3: picbox.Value.Image = global::Minesweeper.Properties.Resources._3; _Grid[counter].changeType('n'); break;
                    case 4: picbox.Value.Image = global::Minesweeper.Properties.Resources._4; _Grid[counter].changeType('n'); break;
                    case 5: picbox.Value.Image = global::Minesweeper.Properties.Resources._5; _Grid[counter].changeType('n'); break;
                    //case 6: picbox.Value.Image = global::Minesweeper.Properties.Resources._6; _Grid[counter].changeType('n'); break;
                    //case 7: picbox.Value.Image = global::Minesweeper.Properties.Resources._7; _Grid[counter].changeType('n'); break;
                    //case 8: picbox.Value.Image = global::Minesweeper.Properties.Resources._8; _Grid[counter].changeType('n'); break;
                    case 0: if (_Grid[counter].type != 'm') { picbox.Value.Image = null; _Grid[counter].changeType('b'); } else { picbox.Value.Image = global::Minesweeper.Properties.Resources.mine; } break;
                    default: if (_Grid[counter].type != 'm') { picbox.Value.Image = null; _Grid[counter].changeType('b'); } else { picbox.Value.Image = global::Minesweeper.Properties.Resources.mine; } break;
                }
                counter++;
            }
            mineNumberBox.Text = _mineCountNum.ToString();
            Size = new Size((boardx * 16) + 72, (boardy * 16) + 112);
            this.Size = Size;
        }
        private void BackButton_Paint(object sender, PaintEventArgs e)
        {
            var blnButtonDown = _buttonStates[Int32.Parse(sender.ToString().Split(':')[1])];
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
            _buttonStates[Int32.Parse(sender.ToString().Split(':')[1])] = true;
        }
        private void BackButton_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            _buttonStates[Int32.Parse(sender.ToString().Split(':')[1])] = false;
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
                if (_Grid[buttonNum].type == 'm')
                {
                    foreach (var item in _MineList)
                    {
                        _GridNums[item].Show();
                        _GridNums[item].BringToFront();
                        _GridButtons[item].SendToBack();
                    }
                    MessageBox.Show("You hit a mine\nGame Over!!! :(");
                    foreach (var item in _Grid)
                        if (item.Value.type == 'm')
                        {
                            // Set the Mine to a red background one to see what they missed XD or not whatevers
                            _GridButtons[buttonNum].Hide();
                            _GridButtons[buttonNum].SendToBack();
                            _GridNums[buttonNum].Show();
                            _GridNums[buttonNum].BringToFront();
                        }
                    var random = new Random();
                    MessageBox.Show("Restarting Game");
                    createBoard(random);
                }
                else
                {
                    if (_GridNums[buttonNum].Image == null)
                    {
                        revealAroundNew(buttonNum);
                    }
                    _GridNums[buttonNum].Show();
                    _GridNums[buttonNum].BringToFront();
                    _GridButtons[buttonNum].SendToBack();
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                var buttonNum = Int32.Parse(sender.ToString().Split(':')[1]);
                _buttonStates[buttonNum] = false;
                handleImage(buttonNum);
            }
        }
        private void handleImage(int buttonNum)
        {
            mineNumberBox.Text = (_mineNum - _generalHits.Count).ToString();
            var current = _Grid[buttonNum].disPlay;
            current++;
            if (current > 2) current = 0;
            _Grid[buttonNum].disPlay = current;

            if (current == 0)
            {
                _GridFlags[buttonNum].Image = global::Minesweeper.Properties.Resources.Mineflag;
                _GridFlags[buttonNum].BringToFront();
                _GridFlags[buttonNum].Show();
                if (!_generalHits.Contains(buttonNum)) _generalHits.Add(buttonNum);

                if (_Grid[buttonNum].type == 'm')
                {
                    if (!_correctHits.Contains(buttonNum))
                    {
                        _correctHits.Add(buttonNum);
                    }
                }
                mineNumberBox.Text = (_mineNum - _generalHits.Count).ToString();
                if (_correctHits.Count == _mineCountNum && _mineCountNum != 69)
                {
                    MessageBox.Show("Good Job, you Won!!");
                    var random = new Random();
                    MessageBox.Show("Restarting Game");
                    _mineCountNum = _correctHits.Count;
                    createBoard(random);
                }
                else if (_correctHits.Count == _mineCountNum && _mineCountNum == 69)
                {
                    MessageBox.Show("😎 Nice! B)");
                    _mineCountNum = 69;
                    MessageBox.Show("Restarting Game");
                    var random = new Random();
                    createBoard(random);
                }


            }
            else if (current == 1)
            {
                _GridFlags[buttonNum].Image = global::Minesweeper.Properties.Resources.Questionmark;
                _GridFlags[buttonNum].BringToFront();
                _GridFlags[buttonNum].Show();
                if (_correctHits.Contains(buttonNum)) _correctHits.Remove(buttonNum);
                if (_generalHits.Contains(buttonNum)) _generalHits.Remove(buttonNum);
            }
            else if (current == 2)
            {
                _GridNums[buttonNum].Image = null;
                _GridFlags[buttonNum].SendToBack();
                _GridFlags[buttonNum].Hide();
                if (_generalHits.Contains(buttonNum)) _generalHits.Remove(buttonNum);
                if (_correctHits.Contains(buttonNum)) _correctHits.Remove(buttonNum);
                //_Grid[buttonNum].
            }


        }
        private void revealAroundNew(int num)
        {
            var xx = _Grid[num].x;
            var yy = _Grid[num].y;
            var points = new (int x, int y)[] { (xx + 16, yy), (xx - 16, yy), (xx, yy - 16), (xx, yy + 16) };
            foreach (var value in points)
                try
                {
                    int i = value.x, j = value.y;
                    if (_Grid[_GridPoint[new Point(i, j)].count].type == 'b')
                    {
                        if (_Grid[_GridPoint[new Point(i, j)].count].revealed)
                        {
                            continue;
                        }
                        _Grid[_GridPoint[new Point(i, j)].count].revealed = true;
                        _GridNums[_GridPoint[new Point(i, j)].count].Show();
                        _GridNums[_GridPoint[new Point(i, j)].count].BringToFront();
                        _GridButtons[_GridPoint[new Point(i, j)].count].SendToBack();
                        revealAroundNew(_GridPoint[new Point(i, j)].count);
                    }
                    else
                    {
                        _Grid[_GridPoint[new Point(i, j)].count].revealed = true;
                        _GridNums[_GridPoint[new Point(i, j)].count].Show();
                        _GridNums[_GridPoint[new Point(i, j)].count].BringToFront();
                        _GridButtons[_GridPoint[new Point(i, j)].count].SendToBack();
                        _Grid[_GridPoint[new Point(i, j)].count].revealed = true;
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
            int number;
            bool resulll = Int32.TryParse(textBox.Text, out number);
            if (resulll) input = Int32.Parse(textBox.Text);
            else result = 0;

            return result;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            var random = new Random();
            var Yes = ShowInputDialog(ref _mineCountNum);
            if (_mineCountNum == -69)
            {
                MessageBox.Show("B) NICE! 😎\nYou did pretty good!!!");
                _mineCountNum = 69;
            }
            else if (_mineCountNum < 1 || _mineCountNum > 99)
            {
                MessageBox.Show("Invalid number of mines, please try again.");
                do
                {
                    var Yes2 = ShowInputDialog(ref _mineCountNum);
                } while (_mineCountNum < 1 || _mineCountNum > 99 && _mineCountNum != -69);
            }
            createBoard(random);
        }
        private void Hard_Click(object sender, EventArgs e)
        {
            var random = new Random();
            _mineCountNum = 99;
            createBoard(random);
        }
        private void Medium_Click(object sender, EventArgs e)
        {
            var random = new Random();
            _mineCountNum = 55;
            createBoard(random);
        }
        private void Easy_Click(object sender, EventArgs e)
        {
            var random = new Random();
            _mineCountNum = 22;
            createBoard(random);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_isCollapsed)
            {
                panel1.Height += 10;
                if (panel1.Size.Height >= 93) // 88, 131
                {
                    timer1.Stop();
                    _isCollapsed = false;
                }
            }
            else
            {
                panel1.Height -= 10;
                if (panel1.Size.Height <= 22)
                {
                    timer1.Stop();
                    _isCollapsed = true;
                }
            }
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            timer1.Start();
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
