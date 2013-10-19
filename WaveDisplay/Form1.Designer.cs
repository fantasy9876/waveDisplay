namespace WaveDisplay
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audioFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.musicXMLFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.levelScrollBar = new System.Windows.Forms.HScrollBar();
            this.OpenFD = new System.Windows.Forms.OpenFileDialog();
            this.undoBut = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.Auto_but = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.noteSpectr_but = new System.Windows.Forms.Button();
            this.clear = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1261, 36);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 32);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.audioFileToolStripMenuItem,
            this.musicXMLFileToolStripMenuItem});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(132, 32);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // audioFileToolStripMenuItem
            // 
            this.audioFileToolStripMenuItem.Name = "audioFileToolStripMenuItem";
            this.audioFileToolStripMenuItem.Size = new System.Drawing.Size(210, 32);
            this.audioFileToolStripMenuItem.Text = "Audio File";
            this.audioFileToolStripMenuItem.Click += new System.EventHandler(this.audioFileToolStripMenuItem_Click);
            // 
            // musicXMLFileToolStripMenuItem
            // 
            this.musicXMLFileToolStripMenuItem.Name = "musicXMLFileToolStripMenuItem";
            this.musicXMLFileToolStripMenuItem.Size = new System.Drawing.Size(210, 32);
            this.musicXMLFileToolStripMenuItem.Text = "Music XML file";
            this.musicXMLFileToolStripMenuItem.Click += new System.EventHandler(this.musicXMLFileToolStripMenuItem_Click);
            // 
            // levelScrollBar
            // 
            this.levelScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.levelScrollBar.LargeChange = 1;
            this.levelScrollBar.Location = new System.Drawing.Point(289, 675);
            this.levelScrollBar.Margin = new System.Windows.Forms.Padding(27, 12, 27, 12);
            this.levelScrollBar.Maximum = 0;
            this.levelScrollBar.Name = "levelScrollBar";
            this.levelScrollBar.Size = new System.Drawing.Size(964, 24);
            this.levelScrollBar.TabIndex = 2;
            this.levelScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.levelScrollBar_Scroll);
            // 
            // OpenFD
            // 
            this.OpenFD.FileName = "openFileDialog1";
            // 
            // undoBut
            // 
            this.undoBut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.undoBut.BackColor = System.Drawing.Color.Linen;
            this.undoBut.Enabled = false;
            this.undoBut.Location = new System.Drawing.Point(105, 675);
            this.undoBut.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.undoBut.Name = "undoBut";
            this.undoBut.Size = new System.Drawing.Size(60, 25);
            this.undoBut.TabIndex = 6;
            this.undoBut.Text = "Undo";
            this.undoBut.UseVisualStyleBackColor = false;
            this.undoBut.Click += new System.EventHandler(this.undoBut_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.Auto_but);
            this.tabPage2.Controls.Add(this.chart1);
            this.tabPage2.Controls.Add(this.pictureBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(1253, 615);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Spectrogram";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Auto_but
            // 
            this.Auto_but.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Auto_but.BackColor = System.Drawing.Color.LightSkyBlue;
            this.Auto_but.Location = new System.Drawing.Point(24, 469);
            this.Auto_but.Name = "Auto_but";
            this.Auto_but.Size = new System.Drawing.Size(95, 27);
            this.Auto_but.TabIndex = 2;
            this.Auto_but.Text = "Auto Detect";
            this.Auto_but.UseVisualStyleBackColor = false;
            this.Auto_but.Click += new System.EventHandler(this.Auto_but_Click);
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chart1.BorderlineColor = System.Drawing.Color.Black;
            this.chart1.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea3.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea3.AxisX.ScaleView.Zoomable = false;
            chartArea3.AxisX.ScrollBar.Enabled = false;
            chartArea3.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea3.AxisY.IsLabelAutoFit = false;
            chartArea3.AxisY.IsMarginVisible = false;
            chartArea3.AxisY.LabelStyle.Enabled = false;
            chartArea3.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea3.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea3.AxisY.MajorTickMark.Enabled = false;
            chartArea3.CursorY.IsUserEnabled = true;
            chartArea3.CursorY.IsUserSelectionEnabled = true;
            chartArea3.Name = "ChartArea1";
            chartArea3.Position.Auto = false;
            chartArea3.Position.Height = 100F;
            chartArea3.Position.Width = 100F;
            this.chart1.ChartAreas.Add(chartArea3);
            this.chart1.Location = new System.Drawing.Point(3, 461);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(1246, 158);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Location = new System.Drawing.Point(4, 0);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(1245, 462);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.SizeChanged += new System.EventHandler(this.pictureBox2_SizeChanged);
            this.pictureBox2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseClick);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(1253, 615);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Home";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(4, 4);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1245, 607);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.SizeChanged += new System.EventHandler(this.pictureBox1_SizeChanged);
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 28);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1261, 644);
            this.tabControl1.TabIndex = 5;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.pictureBox3);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1253, 615);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Waveform";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox3.Location = new System.Drawing.Point(3, 3);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(1247, 609);
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.SizeChanged += new System.EventHandler(this.pictureBox3_SizeChanged);
            // 
            // noteSpectr_but
            // 
            this.noteSpectr_but.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.noteSpectr_but.BackColor = System.Drawing.Color.PaleGreen;
            this.noteSpectr_but.Location = new System.Drawing.Point(171, 674);
            this.noteSpectr_but.Name = "noteSpectr_but";
            this.noteSpectr_but.Size = new System.Drawing.Size(110, 25);
            this.noteSpectr_but.TabIndex = 11;
            this.noteSpectr_but.Text = "Note Spectrum";
            this.noteSpectr_but.UseVisualStyleBackColor = false;
            this.noteSpectr_but.Click += new System.EventHandler(this.noteSpectr_but_Click);
            // 
            // clear
            // 
            this.clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clear.BackColor = System.Drawing.Color.Salmon;
            this.clear.Location = new System.Drawing.Point(8, 674);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(91, 26);
            this.clear.TabIndex = 12;
            this.clear.Text = "Clear Marks";
            this.clear.UseVisualStyleBackColor = false;
            this.clear.Click += new System.EventHandler(this.clear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1261, 703);
            this.Controls.Add(this.clear);
            this.Controls.Add(this.noteSpectr_but);
            this.Controls.Add(this.undoBut);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.levelScrollBar);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.HScrollBar levelScrollBar;
        private System.Windows.Forms.OpenFileDialog OpenFD;
        private System.Windows.Forms.Button undoBut;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ToolStripMenuItem audioFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem musicXMLFileToolStripMenuItem;
        private System.Windows.Forms.Button noteSpectr_but;
        private System.Windows.Forms.Button clear;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button Auto_but;
    }
}

