namespace Trithemius
{
    partial class EncodeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EncodeForm));
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pixelValueComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.encodeButton = new System.Windows.Forms.Button();
            this.fileRadioButton = new System.Windows.Forms.RadioButton();
            this.textRadioButton = new System.Windows.Forms.RadioButton();
            this.writeTextButton = new System.Windows.Forms.Button();
            this.browseButton = new System.Windows.Forms.Button();
            this.pathTextbox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.encodeWorker = new System.ComponentModel.BackgroundWorker();
            this.imageOpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.imageSaveDialog = new System.Windows.Forms.SaveFileDialog();
            this.msgOpenDialog = new System.Windows.Forms.OpenFileDialog();
            this.changesSaveDialog = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.refreshButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.seedBox = new System.Windows.Forms.TextBox();
            this.seedSize = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.bitsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.randomButton = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seedSize)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bitsNumericUpDown)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(58, 19);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(143, 20);
            this.passwordBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Pixel Value:";
            // 
            // pixelValueComboBox
            // 
            this.pixelValueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pixelValueComboBox.FormattingEnabled = true;
            this.pixelValueComboBox.Items.AddRange(new object[] {
            "Red",
            "Green",
            "Blue"});
            this.pixelValueComboBox.Location = new System.Drawing.Point(73, 42);
            this.pixelValueComboBox.Name = "pixelValueComboBox";
            this.pixelValueComboBox.Size = new System.Drawing.Size(66, 21);
            this.pixelValueComboBox.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Encrypt:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Seed:";
            // 
            // encodeButton
            // 
            this.encodeButton.Location = new System.Drawing.Point(67, 32);
            this.encodeButton.Name = "encodeButton";
            this.encodeButton.Size = new System.Drawing.Size(75, 23);
            this.encodeButton.TabIndex = 1;
            this.encodeButton.Text = "&Encode";
            this.encodeButton.UseVisualStyleBackColor = true;
            this.encodeButton.Click += new System.EventHandler(this.encodeButton_Click);
            // 
            // fileRadioButton
            // 
            this.fileRadioButton.AutoSize = true;
            this.fileRadioButton.Location = new System.Drawing.Point(97, 19);
            this.fileRadioButton.Name = "fileRadioButton";
            this.fileRadioButton.Size = new System.Drawing.Size(81, 17);
            this.fileRadioButton.TabIndex = 2;
            this.fileRadioButton.Text = "Encode File";
            this.fileRadioButton.UseVisualStyleBackColor = true;
            this.fileRadioButton.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // textRadioButton
            // 
            this.textRadioButton.AutoSize = true;
            this.textRadioButton.Checked = true;
            this.textRadioButton.Location = new System.Drawing.Point(6, 19);
            this.textRadioButton.Name = "textRadioButton";
            this.textRadioButton.Size = new System.Drawing.Size(86, 17);
            this.textRadioButton.TabIndex = 1;
            this.textRadioButton.TabStop = true;
            this.textRadioButton.Text = "Encode Text";
            this.textRadioButton.UseVisualStyleBackColor = true;
            // 
            // writeTextButton
            // 
            this.writeTextButton.Location = new System.Drawing.Point(184, 16);
            this.writeTextButton.Name = "writeTextButton";
            this.writeTextButton.Size = new System.Drawing.Size(94, 22);
            this.writeTextButton.TabIndex = 3;
            this.writeTextButton.Text = "Write Text";
            this.writeTextButton.UseVisualStyleBackColor = true;
            this.writeTextButton.Click += new System.EventHandler(this.writeTextButton_Click);
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(259, 12);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(40, 23);
            this.browseButton.TabIndex = 2;
            this.browseButton.Text = "...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // pathTextbox
            // 
            this.pathTextbox.BackColor = System.Drawing.SystemColors.Window;
            this.pathTextbox.Location = new System.Drawing.Point(60, 14);
            this.pathTextbox.Name = "pathTextbox";
            this.pathTextbox.ReadOnly = true;
            this.pathTextbox.Size = new System.Drawing.Size(193, 20);
            this.pathTextbox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Picture:";
            // 
            // encodeWorker
            // 
            this.encodeWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.encodeWorker_DoWork);
            this.encodeWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.encodeWorker_RunWorkerCompleted);
            // 
            // imageOpenDialog
            // 
            this.imageOpenDialog.Filter = "Image Files (*.jpg; *.bmp; *.png; *.gif; *.tiff)|*.jpg; *.jpeg; *.bmp;*.png;*.gif" +
    ";*.tiff|All Files (*.*)|*.*";
            this.imageOpenDialog.Title = "Open";
            // 
            // imageSaveDialog
            // 
            this.imageSaveDialog.Filter = "PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp|GIF (*.gif)|*.gif|All Files (*.*)|*.*";
            this.imageSaveDialog.Title = "Save";
            // 
            // msgOpenDialog
            // 
            this.msgOpenDialog.Filter = "All Files (*.*)|*.*";
            this.msgOpenDialog.Title = "Open";
            // 
            // changesSaveDialog
            // 
            this.changesSaveDialog.Filter = "Text File (*.txt)|*.txt|All Files (*.*)|*.*";
            this.changesSaveDialog.Title = "Save Pixel Change Log";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.refreshButton);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(14, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(285, 62);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Picture Statistics";
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(203, 22);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 24);
            this.refreshButton.TabIndex = 2;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Space Required:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Maximum Avalible Size:";
            // 
            // seedBox
            // 
            this.seedBox.Location = new System.Drawing.Point(58, 47);
            this.seedBox.MaxLength = 10;
            this.seedBox.Name = "seedBox";
            this.seedBox.Size = new System.Drawing.Size(143, 20);
            this.seedBox.TabIndex = 3;
            this.seedBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.seedBox_KeyPress);
            // 
            // seedSize
            // 
            this.seedSize.Location = new System.Drawing.Point(149, 78);
            this.seedSize.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.seedSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seedSize.Name = "seedSize";
            this.seedSize.Size = new System.Drawing.Size(52, 20);
            this.seedSize.TabIndex = 6;
            this.seedSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(113, 81);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Size:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.bitsNumericUpDown);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.textRadioButton);
            this.groupBox2.Controls.Add(this.fileRadioButton);
            this.groupBox2.Controls.Add(this.writeTextButton);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.pixelValueComboBox);
            this.groupBox2.Location = new System.Drawing.Point(14, 108);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(285, 75);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Message";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Enabled = false;
            this.label10.Location = new System.Drawing.Point(230, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(19, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Bit";
            // 
            // bitsNumericUpDown
            // 
            this.bitsNumericUpDown.Enabled = false;
            this.bitsNumericUpDown.Location = new System.Drawing.Point(194, 42);
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
            this.bitsNumericUpDown.TabIndex = 7;
            this.bitsNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.bitsNumericUpDown.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Enabled = false;
            this.label9.Location = new System.Drawing.Point(165, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(26, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Use";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.randomButton);
            this.groupBox3.Controls.Add(this.passwordBox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.seedBox);
            this.groupBox3.Controls.Add(this.seedSize);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(315, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(210, 112);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Security";
            // 
            // randomButton
            // 
            this.randomButton.Location = new System.Drawing.Point(32, 76);
            this.randomButton.Name = "randomButton";
            this.randomButton.Size = new System.Drawing.Size(75, 23);
            this.randomButton.TabIndex = 4;
            this.randomButton.Text = "Random";
            this.randomButton.UseVisualStyleBackColor = true;
            this.randomButton.Click += new System.EventHandler(this.randomButton_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(44, 15);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(118, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Create Change Log";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.encodeButton);
            this.groupBox4.Controls.Add(this.checkBox1);
            this.groupBox4.Location = new System.Drawing.Point(316, 126);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(209, 61);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Encode";
            // 
            // EncodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 199);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.pathTextbox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "EncodeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Encoder";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seedSize)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bitsNumericUpDown)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox pixelValueComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button encodeButton;
        private System.Windows.Forms.RadioButton fileRadioButton;
        private System.Windows.Forms.RadioButton textRadioButton;
        private System.Windows.Forms.Button writeTextButton;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.TextBox pathTextbox;
        private System.Windows.Forms.Label label5;
        private System.ComponentModel.BackgroundWorker encodeWorker;
        private System.Windows.Forms.OpenFileDialog imageOpenDialog;
        private System.Windows.Forms.SaveFileDialog imageSaveDialog;
        private System.Windows.Forms.OpenFileDialog msgOpenDialog;
        private System.Windows.Forms.SaveFileDialog changesSaveDialog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox seedBox;
        private System.Windows.Forms.NumericUpDown seedSize;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button randomButton;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown bitsNumericUpDown;
        private System.Windows.Forms.Label label9;
    }
}