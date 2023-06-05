namespace Minesweeper
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mineCount = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.mineNumberBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Custom = new System.Windows.Forms.Button();
            this.Hard = new System.Windows.Forms.Button();
            this.Medium = new System.Windows.Forms.Button();
            this.Easy = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mineCount
            // 
            this.mineCount.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.mineCount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mineCount.Enabled = false;
            this.mineCount.Font = new System.Drawing.Font("MingLiU_HKSCS-ExtB", 15.85714F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mineCount.Location = new System.Drawing.Point(268, 12);
            this.mineCount.Multiline = true;
            this.mineCount.Name = "mineCount";
            this.mineCount.Size = new System.Drawing.Size(256, 16);
            this.mineCount.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(32, 800);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(32, 32);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(249, 60);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "Mines: ";
            // 
            // mineNumberBox
            // 
            this.mineNumberBox.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.mineNumberBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mineNumberBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mineNumberBox.Location = new System.Drawing.Point(105, 12);
            this.mineNumberBox.Name = "mineNumberBox";
            this.mineNumberBox.Size = new System.Drawing.Size(100, 32);
            this.mineNumberBox.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Custom);
            this.panel1.Controls.Add(this.Hard);
            this.panel1.Controls.Add(this.Medium);
            this.panel1.Controls.Add(this.Easy);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Location = new System.Drawing.Point(192, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(93, 173);
            this.panel1.TabIndex = 4;
            // 
            // Custom
            // 
            this.Custom.Location = new System.Drawing.Point(0, 134);
            this.Custom.Name = "Custom";
            this.Custom.Size = new System.Drawing.Size(93, 36);
            this.Custom.TabIndex = 9;
            this.Custom.Text = "Custom";
            this.Custom.UseVisualStyleBackColor = true;
            // 
            // Hard
            // 
            this.Hard.Location = new System.Drawing.Point(0, 100);
            this.Hard.Name = "Hard";
            this.Hard.Size = new System.Drawing.Size(93, 36);
            this.Hard.TabIndex = 8;
            this.Hard.Text = "Hard";
            this.Hard.UseVisualStyleBackColor = true;
            // 
            // Medium
            // 
            this.Medium.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Medium.Location = new System.Drawing.Point(0, 65);
            this.Medium.Name = "Medium";
            this.Medium.Size = new System.Drawing.Size(93, 36);
            this.Medium.TabIndex = 7;
            this.Medium.Text = "Medium";
            this.Medium.UseVisualStyleBackColor = true;
            // 
            // Easy
            // 
            this.Easy.Location = new System.Drawing.Point(0, 33);
            this.Easy.Name = "Easy";
            this.Easy.Size = new System.Drawing.Size(93, 36);
            this.Easy.TabIndex = 6;
            this.Easy.Text = "Easy";
            this.Easy.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(0, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(93, 35);
            this.button2.TabIndex = 5;
            this.button2.Text = "Options";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(880, 600);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.mineNumberBox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mineCount);
            this.Name = "Form1";
            this.Text = "MINESWEEPER";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox mineCount;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox mineNumberBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button Custom;
        private System.Windows.Forms.Button Hard;
        private System.Windows.Forms.Button Medium;
        private System.Windows.Forms.Button Easy;
        private System.Windows.Forms.Timer timer1;
    }
}

