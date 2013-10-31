namespace WaveAnalysis
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audioFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.musicXMLFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearMarksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllNotesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.levelScrollBar = new System.Windows.Forms.HScrollBar();
            this.OpenFD = new System.Windows.Forms.OpenFileDialog();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.noteSpectr_but = new System.Windows.Forms.Button();
            this.onset_but = new System.Windows.Forms.Button();
            this.onset_contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pitchDetectionModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spectralDifferenceModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewNote_but = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.onset_contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1419, 31);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(47, 27);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.audioFileToolStripMenuItem,
            this.musicXMLFileToolStripMenuItem});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(122, 28);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // audioFileToolStripMenuItem
            // 
            this.audioFileToolStripMenuItem.Name = "audioFileToolStripMenuItem";
            this.audioFileToolStripMenuItem.Size = new System.Drawing.Size(189, 28);
            this.audioFileToolStripMenuItem.Text = "Audio File";
            this.audioFileToolStripMenuItem.Click += new System.EventHandler(this.audioFileToolStripMenuItem_Click);
            // 
            // musicXMLFileToolStripMenuItem
            // 
            this.musicXMLFileToolStripMenuItem.Name = "musicXMLFileToolStripMenuItem";
            this.musicXMLFileToolStripMenuItem.Size = new System.Drawing.Size(189, 28);
            this.musicXMLFileToolStripMenuItem.Text = "Music XML file";
            this.musicXMLFileToolStripMenuItem.Click += new System.EventHandler(this.musicXMLFileToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.clearMarksToolStripMenuItem,
            this.clearAllNotesToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(51, 27);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(229, 28);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // clearMarksToolStripMenuItem
            // 
            this.clearMarksToolStripMenuItem.Name = "clearMarksToolStripMenuItem";
            this.clearMarksToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.clearMarksToolStripMenuItem.Size = new System.Drawing.Size(229, 28);
            this.clearMarksToolStripMenuItem.Text = "Clear Marks";
            this.clearMarksToolStripMenuItem.Click += new System.EventHandler(this.clearMarksToolStripMenuItem_Click);
            // 
            // clearAllNotesToolStripMenuItem
            // 
            this.clearAllNotesToolStripMenuItem.Name = "clearAllNotesToolStripMenuItem";
            this.clearAllNotesToolStripMenuItem.Size = new System.Drawing.Size(229, 28);
            this.clearAllNotesToolStripMenuItem.Text = "Clear All Notes";
            this.clearAllNotesToolStripMenuItem.Click += new System.EventHandler(this.clearAllNotesToolStripMenuItem_Click);
            // 
            // levelScrollBar
            // 
            this.levelScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.levelScrollBar.LargeChange = 1;
            this.levelScrollBar.Location = new System.Drawing.Point(411, 759);
            this.levelScrollBar.Margin = new System.Windows.Forms.Padding(30, 14, 30, 14);
            this.levelScrollBar.Maximum = 0;
            this.levelScrollBar.Name = "levelScrollBar";
            this.levelScrollBar.Size = new System.Drawing.Size(998, 24);
            this.levelScrollBar.TabIndex = 2;
            this.levelScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.levelScrollBar_Scroll);
            // 
            // OpenFD
            // 
            this.OpenFD.FileName = "openFileDialog1";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chart1);
            this.tabPage2.Controls.Add(this.pictureBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 27);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(1411, 693);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Spectrogram";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chart1.BorderlineColor = System.Drawing.Color.Black;
            this.chart1.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisX.ScaleView.Zoomable = false;
            chartArea1.AxisX.ScrollBar.Enabled = false;
            chartArea1.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.IsMarginVisible = false;
            chartArea1.AxisY.LabelStyle.Enabled = false;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea1.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea1.AxisY.MajorTickMark.Enabled = false;
            chartArea1.CursorX.IsUserEnabled = true;
            chartArea1.CursorX.IsUserSelectionEnabled = true;
            chartArea1.CursorY.IsUserEnabled = true;
            chartArea1.CursorY.IsUserSelectionEnabled = true;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 100F;
            chartArea1.Position.Width = 100F;
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Location = new System.Drawing.Point(3, 517);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(1402, 178);
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
            this.pictureBox2.Size = new System.Drawing.Size(1401, 518);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.SizeChanged += new System.EventHandler(this.pictureBox2_SizeChanged);
            this.pictureBox2.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox2_Paint);
            this.pictureBox2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox2_MouseClick);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(1411, 693);
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
            this.pictureBox1.Size = new System.Drawing.Size(1403, 685);
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
            this.tabControl1.Location = new System.Drawing.Point(0, 32);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1419, 724);
            this.tabControl1.TabIndex = 5;
            this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.pictureBox3);
            this.tabPage3.Location = new System.Drawing.Point(4, 27);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1411, 693);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Waveform";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox3.Location = new System.Drawing.Point(3, 3);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(1405, 687);
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.SizeChanged += new System.EventHandler(this.pictureBox3_SizeChanged);
            // 
            // noteSpectr_but
            // 
            this.noteSpectr_but.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.noteSpectr_but.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.noteSpectr_but.Location = new System.Drawing.Point(9, 758);
            this.noteSpectr_but.Name = "noteSpectr_but";
            this.noteSpectr_but.Size = new System.Drawing.Size(124, 30);
            this.noteSpectr_but.TabIndex = 11;
            this.noteSpectr_but.Text = "Note Spectrum";
            this.noteSpectr_but.UseVisualStyleBackColor = false;
            this.noteSpectr_but.Click += new System.EventHandler(this.noteSpectr_but_Click);
            // 
            // onset_but
            // 
            this.onset_but.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.onset_but.Location = new System.Drawing.Point(139, 759);
            this.onset_but.Name = "onset_but";
            this.onset_but.Size = new System.Drawing.Size(139, 29);
            this.onset_but.TabIndex = 12;
            this.onset_but.Text = "Onset Detect";
            this.onset_but.UseVisualStyleBackColor = true;
            this.onset_but.Click += new System.EventHandler(this.onset_but_Click);
            // 
            // onset_contextMenu
            // 
            this.onset_contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pitchDetectionModeToolStripMenuItem,
            this.spectralDifferenceModeToolStripMenuItem});
            this.onset_contextMenu.Name = "onset_contextMenu";
            this.onset_contextMenu.Size = new System.Drawing.Size(273, 60);
            // 
            // pitchDetectionModeToolStripMenuItem
            // 
            this.pitchDetectionModeToolStripMenuItem.Name = "pitchDetectionModeToolStripMenuItem";
            this.pitchDetectionModeToolStripMenuItem.Size = new System.Drawing.Size(272, 28);
            this.pitchDetectionModeToolStripMenuItem.Text = "Pitch Detection Mode";
            this.pitchDetectionModeToolStripMenuItem.Click += new System.EventHandler(this.pitchDetectionModeToolStripMenuItem_Click);
            // 
            // spectralDifferenceModeToolStripMenuItem
            // 
            this.spectralDifferenceModeToolStripMenuItem.Name = "spectralDifferenceModeToolStripMenuItem";
            this.spectralDifferenceModeToolStripMenuItem.Size = new System.Drawing.Size(272, 28);
            this.spectralDifferenceModeToolStripMenuItem.Text = "Spectral Difference Mode";
            this.spectralDifferenceModeToolStripMenuItem.Click += new System.EventHandler(this.spectralDifferenceModeToolStripMenuItem_Click);
            // 
            // viewNote_but
            // 
            this.viewNote_but.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.viewNote_but.Location = new System.Drawing.Point(284, 760);
            this.viewNote_but.Name = "viewNote_but";
            this.viewNote_but.Size = new System.Drawing.Size(123, 28);
            this.viewNote_but.TabIndex = 13;
            this.viewNote_but.Text = "View Notes";
            this.viewNote_but.UseVisualStyleBackColor = true;
            this.viewNote_but.Click += new System.EventHandler(this.viewNote_but_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1419, 791);
            this.Controls.Add(this.viewNote_but);
            this.Controls.Add(this.onset_but);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.noteSpectr_but);
            this.Controls.Add(this.levelScrollBar);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(898, 669);
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
            this.onset_contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.HScrollBar levelScrollBar;
        private System.Windows.Forms.OpenFileDialog OpenFD;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ToolStripMenuItem audioFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem musicXMLFileToolStripMenuItem;
        private System.Windows.Forms.Button noteSpectr_but;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearMarksToolStripMenuItem;
        private System.Windows.Forms.Button onset_but;
        private System.Windows.Forms.ContextMenuStrip onset_contextMenu;
        private System.Windows.Forms.ToolStripMenuItem pitchDetectionModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spectralDifferenceModeToolStripMenuItem;
        private System.Windows.Forms.Button viewNote_but;
        private System.Windows.Forms.ToolStripMenuItem clearAllNotesToolStripMenuItem;
    }
}

