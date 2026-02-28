namespace Test
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
            pictureBoxSrc = new PictureBox();
            tableLayoutRight = new TableLayoutPanel();
            listBox1 = new ListBox();
            label2 = new Label();
            buttonSave = new Button();
            richTextBoxInfo = new RichTextBox();
            label1 = new Label();
            groupBoxMode = new GroupBox();
            radioButtonDdn = new RadioButton();
            radioButtonMrz = new RadioButton();
            radioButtonDbr = new RadioButton();
            buttonCamera = new Button();
            buttonFile = new Button();
            menuStrip1 = new MenuStrip();
            enterLicenseKeyToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSrc).BeginInit();
            tableLayoutRight.SuspendLayout();
            groupBoxMode.SuspendLayout();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBoxSrc
            // 
            pictureBoxSrc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBoxSrc.BackColor = SystemColors.ControlDark;
            pictureBoxSrc.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxSrc.Location = new Point(12, 27);
            pictureBoxSrc.Name = "pictureBoxSrc";
            pictureBoxSrc.Size = new Size(634, 510);
            pictureBoxSrc.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxSrc.TabIndex = 0;
            pictureBoxSrc.TabStop = false;
            // 
            // tableLayoutRight
            // 
            tableLayoutRight.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            tableLayoutRight.ColumnCount = 1;
            tableLayoutRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutRight.Controls.Add(buttonFile, 0, 0);
            tableLayoutRight.Controls.Add(buttonCamera, 0, 1);
            tableLayoutRight.Controls.Add(groupBoxMode, 0, 2);
            tableLayoutRight.Controls.Add(label1, 0, 3);
            tableLayoutRight.Controls.Add(richTextBoxInfo, 0, 4);
            tableLayoutRight.Controls.Add(buttonSave, 0, 5);
            tableLayoutRight.Controls.Add(label2, 0, 6);
            tableLayoutRight.Controls.Add(listBox1, 0, 7);
            tableLayoutRight.Location = new Point(658, 27);
            tableLayoutRight.Name = "tableLayoutRight";
            tableLayoutRight.RowCount = 8;
            tableLayoutRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            tableLayoutRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutRight.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));
            tableLayoutRight.Size = new Size(260, 510);
            tableLayoutRight.TabIndex = 1;
            tableLayoutRight.Padding = new Padding(3, 3, 3, 3);
            // 
            // buttonFile
            // 
            buttonFile.Dock = DockStyle.Fill;
            buttonFile.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            buttonFile.Margin = new Padding(0, 0, 0, 3);
            buttonFile.Name = "buttonFile";
            buttonFile.TabIndex = 1;
            buttonFile.Text = "Load File";
            buttonFile.UseVisualStyleBackColor = true;
            buttonFile.Click += buttonFile_Click;
            // 
            // buttonCamera
            // 
            buttonCamera.Dock = DockStyle.Fill;
            buttonCamera.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            buttonCamera.Margin = new Padding(0, 0, 0, 3);
            buttonCamera.Name = "buttonCamera";
            buttonCamera.TabIndex = 2;
            buttonCamera.Text = "Camera Scan";
            buttonCamera.UseVisualStyleBackColor = true;
            buttonCamera.Click += buttonCamera_Click;
            // 
            // groupBoxMode
            // 
            groupBoxMode.Controls.Add(radioButtonDdn);
            groupBoxMode.Controls.Add(radioButtonMrz);
            groupBoxMode.Controls.Add(radioButtonDbr);
            groupBoxMode.Dock = DockStyle.Fill;
            groupBoxMode.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBoxMode.Margin = new Padding(0, 0, 0, 3);
            groupBoxMode.Name = "groupBoxMode";
            groupBoxMode.Padding = new Padding(5);
            groupBoxMode.TabIndex = 3;
            groupBoxMode.TabStop = false;
            groupBoxMode.Text = "Detection Mode";
            // 
            // radioButtonDdn
            // 
            radioButtonDdn.AutoSize = true;
            radioButtonDdn.Location = new Point(170, 22);
            radioButtonDdn.Name = "radioButtonDdn";
            radioButtonDdn.Size = new Size(50, 19);
            radioButtonDdn.TabIndex = 2;
            radioButtonDdn.Text = "DDN";
            radioButtonDdn.UseVisualStyleBackColor = true;
            radioButtonDdn.CheckedChanged += radioButton_CheckedChanged;
            // 
            // radioButtonMrz
            // 
            radioButtonMrz.AutoSize = true;
            radioButtonMrz.Location = new Point(90, 22);
            radioButtonMrz.Name = "radioButtonMrz";
            radioButtonMrz.Size = new Size(52, 19);
            radioButtonMrz.TabIndex = 1;
            radioButtonMrz.Text = "MRZ";
            radioButtonMrz.UseVisualStyleBackColor = true;
            radioButtonMrz.CheckedChanged += radioButton_CheckedChanged;
            // 
            // radioButtonDbr
            // 
            radioButtonDbr.AutoSize = true;
            radioButtonDbr.Checked = true;
            radioButtonDbr.Location = new Point(10, 22);
            radioButtonDbr.Name = "radioButtonDbr";
            radioButtonDbr.Size = new Size(55, 19);
            radioButtonDbr.TabIndex = 0;
            radioButtonDbr.TabStop = true;
            radioButtonDbr.Text = "DBR";
            radioButtonDbr.UseVisualStyleBackColor = true;
            radioButtonDbr.CheckedChanged += radioButton_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = false;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Margin = new Padding(0, 2, 0, 0);
            label1.Name = "label1";
            label1.TabIndex = 4;
            label1.Text = "Results:";
            label1.TextAlign = ContentAlignment.BottomLeft;
            // 
            // richTextBoxInfo
            // 
            richTextBoxInfo.Dock = DockStyle.Fill;
            richTextBoxInfo.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            richTextBoxInfo.Margin = new Padding(0, 0, 0, 3);
            richTextBoxInfo.Name = "richTextBoxInfo";
            richTextBoxInfo.ReadOnly = true;
            richTextBoxInfo.TabIndex = 5;
            richTextBoxInfo.Text = "";
            // 
            // buttonSave
            // 
            buttonSave.Dock = DockStyle.Fill;
            buttonSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            buttonSave.Margin = new Padding(0, 0, 0, 3);
            buttonSave.Name = "buttonSave";
            buttonSave.TabIndex = 6;
            buttonSave.Text = "Save Image";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // label2
            // 
            label2.AutoSize = false;
            label2.Dock = DockStyle.Fill;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Margin = new Padding(0, 2, 0, 0);
            label2.Name = "label2";
            label2.TabIndex = 7;
            label2.Text = "Image Files:";
            label2.TextAlign = ContentAlignment.BottomLeft;
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Margin = new Padding(0, 0, 0, 0);
            listBox1.Name = "listBox1";
            listBox1.TabIndex = 8;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            listBox1.DragDrop += listBox1_DragDrop;
            listBox1.DragEnter += listBox1_DragEnter;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { enterLicenseKeyToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(930, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // enterLicenseKeyToolStripMenuItem
            // 
            enterLicenseKeyToolStripMenuItem.Name = "enterLicenseKeyToolStripMenuItem";
            enterLicenseKeyToolStripMenuItem.Size = new Size(110, 20);
            enterLicenseKeyToolStripMenuItem.Text = "Enter License Key";
            enterLicenseKeyToolStripMenuItem.Click += enterLicenseKeyToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Dock = DockStyle.Bottom;
            statusStrip1.Location = new Point(0, 539);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(930, 22);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(118, 17);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(930, 561);
            Controls.Add(pictureBoxSrc);
            Controls.Add(tableLayoutRight);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(700, 500);
            Name = "Form1";
            Text = "Dynamsoft Capture Vision Demo";
            ((System.ComponentModel.ISupportInitialize)pictureBoxSrc).EndInit();
            tableLayoutRight.ResumeLayout(false);
            tableLayoutRight.PerformLayout();
            groupBoxMode.ResumeLayout(false);
            groupBoxMode.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxSrc;
        private TableLayoutPanel tableLayoutRight;
        private Button buttonFile;
        private Button buttonCamera;
        private RichTextBox richTextBoxInfo;
        private Label label1;
        private GroupBox groupBoxMode;
        private RadioButton radioButtonDdn;
        private RadioButton radioButtonMrz;
        private RadioButton radioButtonDbr;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem enterLicenseKeyToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ListBox listBox1;
        private Label label2;
        private Button buttonSave;
    }
}
