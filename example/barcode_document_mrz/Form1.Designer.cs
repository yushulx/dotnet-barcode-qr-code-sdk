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
            buttonFile = new Button();
            buttonCamera = new Button();
            pictureBoxDest = new PictureBox();
            richTextBoxInfo = new RichTextBox();
            label1 = new Label();
            menuStrip1 = new MenuStrip();
            menuStrip2 = new MenuStrip();
            enterLicenseKeyToolStripMenuItem = new ToolStripMenuItem();
            setTemplatesToolStripMenuItem = new ToolStripMenuItem();
            dDNToolStripMenuItem = new ToolStripMenuItem();
            dBRToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            listBox1 = new ListBox();
            label2 = new Label();
            checkBoxDdn = new CheckBox();
            checkBoxDbr = new CheckBox();
            checkBoxMrz = new CheckBox();
            buttonSave = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSrc).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDest).BeginInit();
            menuStrip2.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBoxSrc
            // 
            pictureBoxSrc.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxSrc.Location = new Point(6, 24);
            pictureBoxSrc.Margin = new Padding(4);
            pictureBoxSrc.Name = "pictureBoxSrc";
            pictureBoxSrc.Size = new Size(600, 769);
            pictureBoxSrc.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxSrc.TabIndex = 0;
            pictureBoxSrc.TabStop = false;
            // 
            // buttonFile
            // 
            buttonFile.AllowDrop = true;
            buttonFile.AutoEllipsis = true;
            buttonFile.Font = new Font("Microsoft Sans Serif", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonFile.Location = new Point(1218, 25);
            buttonFile.Margin = new Padding(4);
            buttonFile.Name = "buttonFile";
            buttonFile.Size = new Size(280, 49);
            buttonFile.TabIndex = 1;
            buttonFile.Text = "Load File";
            buttonFile.UseVisualStyleBackColor = true;
            buttonFile.Click += buttonFile_Click;
            // 
            // buttonCamera
            // 
            buttonCamera.AllowDrop = true;
            buttonCamera.AutoEllipsis = true;
            buttonCamera.Font = new Font("Microsoft Sans Serif", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonCamera.Location = new Point(1218, 85);
            buttonCamera.Margin = new Padding(4);
            buttonCamera.Name = "buttonCamera";
            buttonCamera.Size = new Size(280, 54);
            buttonCamera.TabIndex = 1;
            buttonCamera.Text = "Camera Scan";
            buttonCamera.UseVisualStyleBackColor = true;
            buttonCamera.Click += buttonCamera_Click;
            // 
            // pictureBoxDest
            // 
            pictureBoxDest.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxDest.Location = new Point(613, 24);
            pictureBoxDest.Name = "pictureBoxDest";
            pictureBoxDest.Size = new Size(600, 769);
            pictureBoxDest.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxDest.TabIndex = 7;
            pictureBoxDest.TabStop = false;
            // 
            // richTextBoxInfo
            // 
            richTextBoxInfo.Location = new Point(1218, 221);
            richTextBoxInfo.Name = "richTextBoxInfo";
            richTextBoxInfo.Size = new Size(280, 190);
            richTextBoxInfo.TabIndex = 8;
            richTextBoxInfo.Tag = "";
            richTextBoxInfo.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1218, 194);
            label1.Name = "label1";
            label1.Size = new Size(44, 15);
            label1.TabIndex = 9;
            label1.Text = "Results";
            // 
            // menuStrip1
            // 
            menuStrip1.Location = new Point(0, 24);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1500, 24);
            menuStrip1.TabIndex = 10;
            menuStrip1.Text = "menuStrip1";
            // 
            // menuStrip2
            // 
            menuStrip2.Items.AddRange(new ToolStripItem[] { enterLicenseKeyToolStripMenuItem, setTemplatesToolStripMenuItem });
            menuStrip2.Location = new Point(0, 0);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Size = new Size(1500, 24);
            menuStrip2.TabIndex = 11;
            menuStrip2.Text = "menuStrip2";
            // 
            // enterLicenseKeyToolStripMenuItem
            // 
            enterLicenseKeyToolStripMenuItem.Name = "enterLicenseKeyToolStripMenuItem";
            enterLicenseKeyToolStripMenuItem.Size = new Size(110, 20);
            enterLicenseKeyToolStripMenuItem.Text = "Enter License Key";
            enterLicenseKeyToolStripMenuItem.Click += enterLicenseKeyToolStripMenuItem_Click;
            // 
            // setTemplatesToolStripMenuItem
            // 
            setTemplatesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { dDNToolStripMenuItem, dBRToolStripMenuItem });
            setTemplatesToolStripMenuItem.Name = "setTemplatesToolStripMenuItem";
            setTemplatesToolStripMenuItem.Size = new Size(91, 20);
            setTemplatesToolStripMenuItem.Text = "Set Templates";
            // 
            // dDNToolStripMenuItem
            // 
            dDNToolStripMenuItem.Name = "dDNToolStripMenuItem";
            dDNToolStripMenuItem.Size = new Size(180, 22);
            dDNToolStripMenuItem.Text = "DDN";
            dDNToolStripMenuItem.Click += dDNToolStripMenuItem_Click;
            // 
            // dBRToolStripMenuItem
            // 
            dBRToolStripMenuItem.Name = "dBRToolStripMenuItem";
            dBRToolStripMenuItem.Size = new Size(180, 22);
            dBRToolStripMenuItem.Text = "DBR";
            dBRToolStripMenuItem.Click += dBRToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 828);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1500, 22);
            statusStrip1.TabIndex = 12;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(118, 17);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // listBox1
            // 
            listBox1.AllowDrop = true;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(1219, 546);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(279, 244);
            listBox1.TabIndex = 14;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            listBox1.DragDrop += listBox1_DragDrop;
            listBox1.DragEnter += listBox1_DragEnter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(1218, 525);
            label2.Name = "label2";
            label2.Size = new Size(66, 15);
            label2.TabIndex = 15;
            label2.Text = "Image Files";
            // 
            // checkBoxDdn
            // 
            checkBoxDdn.AutoSize = true;
            checkBoxDdn.Checked = true;
            checkBoxDdn.CheckState = CheckState.Checked;
            checkBoxDdn.Location = new Point(1233, 148);
            checkBoxDdn.Name = "checkBoxDdn";
            checkBoxDdn.Size = new Size(51, 19);
            checkBoxDdn.TabIndex = 17;
            checkBoxDdn.Text = "DDN";
            checkBoxDdn.UseVisualStyleBackColor = true;
            checkBoxDdn.CheckedChanged += checkBox_CheckedChanged;
            // 
            // checkBoxDbr
            // 
            checkBoxDbr.AutoSize = true;
            checkBoxDbr.Checked = true;
            checkBoxDbr.CheckState = CheckState.Checked;
            checkBoxDbr.Location = new Point(1316, 148);
            checkBoxDbr.Name = "checkBoxDbr";
            checkBoxDbr.Size = new Size(51, 19);
            checkBoxDbr.TabIndex = 18;
            checkBoxDbr.Text = "DBR ";
            checkBoxDbr.UseVisualStyleBackColor = true;
            checkBoxDbr.CheckedChanged += checkBox_CheckedChanged;
            // 
            // checkBoxMrz
            // 
            checkBoxMrz.AutoSize = true;
            checkBoxMrz.Checked = true;
            checkBoxMrz.CheckState = CheckState.Checked;
            checkBoxMrz.Location = new Point(1404, 148);
            checkBoxMrz.Name = "checkBoxMrz";
            checkBoxMrz.Size = new Size(51, 19);
            checkBoxMrz.TabIndex = 19;
            checkBoxMrz.Text = "MRZ";
            checkBoxMrz.UseVisualStyleBackColor = true;
            checkBoxMrz.CheckedChanged += checkBox_CheckedChanged;
            // 
            // buttonSave
            // 
            buttonSave.AllowDrop = true;
            buttonSave.AutoEllipsis = true;
            buttonSave.Font = new Font("Microsoft Sans Serif", 20.25F, FontStyle.Bold, GraphicsUnit.Point);
            buttonSave.Location = new Point(1218, 450);
            buttonSave.Margin = new Padding(4);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(280, 54);
            buttonSave.TabIndex = 20;
            buttonSave.Text = "Save Image";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1500, 850);
            Controls.Add(buttonSave);
            Controls.Add(checkBoxMrz);
            Controls.Add(checkBoxDbr);
            Controls.Add(checkBoxDdn);
            Controls.Add(label2);
            Controls.Add(listBox1);
            Controls.Add(statusStrip1);
            Controls.Add(label1);
            Controls.Add(richTextBoxInfo);
            Controls.Add(pictureBoxDest);
            Controls.Add(buttonFile);
            Controls.Add(buttonCamera);
            Controls.Add(pictureBoxSrc);
            Controls.Add(menuStrip1);
            Controls.Add(menuStrip2);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4);
            MaximizeBox = false;
            Name = "Form1";
            Text = ".NET Dynamsoft Capture Vision Demo";
            ((System.ComponentModel.ISupportInitialize)pictureBoxSrc).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDest).EndInit();
            menuStrip2.ResumeLayout(false);
            menuStrip2.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxSrc;
        private Button buttonFile;
        private Button buttonCamera;
        private PictureBox pictureBoxDest;
        private RichTextBox richTextBoxInfo;
        private Label label1;
        private MenuStrip menuStrip1;
        private MenuStrip menuStrip2;
        private ToolStripMenuItem enterLicenseKeyToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ListBox listBox1;
        private Label label2;
        private CheckBox checkBoxDdn;
        private CheckBox checkBoxDbr;
        private CheckBox checkBoxMrz;
        private ToolStripMenuItem setTemplatesToolStripMenuItem;
        private ToolStripMenuItem dDNToolStripMenuItem;
        private ToolStripMenuItem dBRToolStripMenuItem;
        private Button buttonSave;
    }
}

