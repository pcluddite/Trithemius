namespace Trithemius
{
    partial class DecodeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DecodeForm));
            this.browseButton = new System.Windows.Forms.Button();
            this.pathTextbox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.fileRadioButton = new System.Windows.Forms.RadioButton();
            this.textRadioButton = new System.Windows.Forms.RadioButton();
            this.decodeButton = new System.Windows.Forms.Button();
            this.decodeWorker = new System.ComponentModel.BackgroundWorker();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.imageOpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkSizeButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.seedBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.bitsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.pixelValueComboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bitsNumericUpDown)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(255, 4);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(40, 23);
            this.browseButton.TabIndex = 59;
            this.browseButton.Text = "...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // pathTextbox
            // 
            this.pathTextbox.BackColor = System.Drawing.SystemColors.Window;
            this.pathTextbox.Location = new System.Drawing.Point(61, 6);
            this.pathTextbox.Name = "pathTextbox";
            this.pathTextbox.ReadOnly = true;
            this.pathTextbox.Size = new System.Drawing.Size(188, 20);
            this.pathTextbox.TabIndex = 58;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 60;
            this.label5.Text = "Picture:";
            // 
            // fileRadioButton
            // 
            this.fileRadioButton.AutoSize = true;
            this.fileRadioButton.Location = new System.Drawing.Point(111, 19);
            this.fileRadioButton.Name = "fileRadioButton";
            this.fileRadioButton.Size = new System.Drawing.Size(82, 17);
            this.fileRadioButton.TabIndex = 62;
            this.fileRadioButton.Text = "Decode File";
            this.fileRadioButton.UseVisualStyleBackColor = true;
            // 
            // textRadioButton
            // 
            this.textRadioButton.AutoSize = true;
            this.textRadioButton.Checked = true;
            this.textRadioButton.Location = new System.Drawing.Point(20, 19);
            this.textRadioButton.Name = "textRadioButton";
            this.textRadioButton.Size = new System.Drawing.Size(87, 17);
            this.textRadioButton.TabIndex = 61;
            this.textRadioButton.TabStop = true;
            this.textRadioButton.Text = "Decode Text";
            this.textRadioButton.UseVisualStyleBackColor = true;
            // 
            // decodeButton
            // 
            this.decodeButton.Location = new System.Drawing.Point(67, 21);
            this.decodeButton.Name = "decodeButton";
            this.decodeButton.Size = new System.Drawing.Size(75, 23);
            this.decodeButton.TabIndex = 67;
            this.decodeButton.Text = "D&ecode";
            this.decodeButton.UseVisualStyleBackColor = true;
            this.decodeButton.Click += new System.EventHandler(this.decodeButton_Click);
            // 
            // decodeWorker
            // 
            this.decodeWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.decodeWorker_DoWork);
            this.decodeWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.decodeWorker_RunWorkerCompleted);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "All Files (*.*)|*.*";
            this.saveFileDialog.Title = "Save";
            // 
            // imageOpenDialog
            // 
            this.imageOpenDialog.Filter = "Image Files (*.jpg; *.bmp; *.png; *.gif; *.tiff)|*.jpg; *.jpeg; *.bmp;*.png;*.gif" +
    ";*.tiff|All Files (*.*)|*.*";
            this.imageOpenDialog.Title = "Open";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkSizeButton);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(10, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(285, 48);
            this.groupBox1.TabIndex = 68;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Picture Statistics";
            // 
            // checkSizeButton
            // 
            this.checkSizeButton.Location = new System.Drawing.Point(225, 17);
            this.checkSizeButton.Name = "checkSizeButton";
            this.checkSizeButton.Size = new System.Drawing.Size(54, 23);
            this.checkSizeButton.TabIndex = 72;
            this.checkSizeButton.Text = "&Check";
            this.checkSizeButton.UseVisualStyleBackColor = true;
            this.checkSizeButton.Click += new System.EventHandler(this.checkSizeButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(123, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Detected Message Size:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.passwordBox);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.seedBox);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(301, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(210, 95);
            this.groupBox3.TabIndex = 69;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Security";
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(58, 26);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(143, 20);
            this.passwordBox.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Encrypt:";
            // 
            // seedBox
            // 
            this.seedBox.Location = new System.Drawing.Point(58, 54);
            this.seedBox.MaxLength = 10;
            this.seedBox.Name = "seedBox";
            this.seedBox.Size = new System.Drawing.Size(143, 20);
            this.seedBox.TabIndex = 59;
            this.seedBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.seedBox_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Seed:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.bitsNumericUpDown);
            this.groupBox2.Controls.Add(this.pixelValueComboBox);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.fileRadioButton);
            this.groupBox2.Controls.Add(this.textRadioButton);
            this.groupBox2.Location = new System.Drawing.Point(10, 86);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(285, 75);
            this.groupBox2.TabIndex = 70;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Message";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Enabled = false;
            this.label10.Location = new System.Drawing.Point(232, 46);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(19, 13);
            this.label10.TabIndex = 73;
            this.label10.Text = "Bit";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Pixel Value:";
            // 
            // bitsNumericUpDown
            // 
            this.bitsNumericUpDown.Enabled = false;
            this.bitsNumericUpDown.Location = new System.Drawing.Point(196, 43);
            this.bitsNumericUpDown.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.bitsNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.bitsNumericUpDown.Name = "bitsNumericUpDown";
            this.bitsNumericUpDown.Size = new System.Drawing.Size(34, 20);
            this.bitsNumericUpDown.TabIndex = 74;
            this.bitsNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.bitsNumericUpDown.ValueChanged += new System.EventHandler(this.bitsNumericUpDown_ValueChanged);
            // 
            // pixelValueComboBox
            // 
            this.pixelValueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pixelValueComboBox.FormattingEnabled = true;
            this.pixelValueComboBox.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue"});
            this.pixelValueComboBox.Location = new System.Drawing.Point(85, 42);
            this.pixelValueComboBox.Name = "pixelValueComboBox";
            this.pixelValueComboBox.Size = new System.Drawing.Size(58, 21);
            this.pixelValueComboBox.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Enabled = false;
            this.label9.Location = new System.Drawing.Point(167, 46);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(26, 13);
            this.label9.TabIndex = 72;
            this.label9.Text = "Use";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.decodeButton);
            this.groupBox4.Location = new System.Drawing.Point(301, 105);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(209, 56);
            this.groupBox4.TabIndex = 71;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Decode";
            // 
            // DecodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 175);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.pathTextbox);
            this.Controls.Add(this.label5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DecodeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Decoder";
            this.Activated += new System.EventHandler(this.Form2_Activated);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bitsNumericUpDown)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.TextBox pathTextbox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton fileRadioButton;
        private System.Windows.Forms.RadioButton textRadioButton;
        private System.Windows.Forms.Button decodeButton;
        private System.ComponentModel.BackgroundWorker decodeWorker;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog imageOpenDialog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox seedBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox pixelValueComboBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button checkSizeButton;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown bitsNumericUpDown;
        private System.Windows.Forms.Label label9;
    }
}