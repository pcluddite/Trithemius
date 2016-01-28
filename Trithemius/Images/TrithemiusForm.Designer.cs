namespace Trithemius
{
    partial class TrithemiusForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrithemiusForm));
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
            this.availableSizeBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.decodeRadio = new System.Windows.Forms.RadioButton();
            this.encodeRadio = new System.Windows.Forms.RadioButton();
            this.seedBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.requiredSizeBox = new System.Windows.Forms.TextBox();
            this.requiredSizeLabel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.bitsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.randomButton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.invertBox = new System.Windows.Forms.CheckBox();
            this.msgSaveDialog = new System.Windows.Forms.SaveFileDialog();
            this.decodeWorker = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bitsNumericUpDown)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(68, 47);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(149, 20);
            this.passwordBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
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
            this.pixelValueComboBox.Location = new System.Drawing.Point(72, 19);
            this.pixelValueComboBox.Name = "pixelValueComboBox";
            this.pixelValueComboBox.Size = new System.Drawing.Size(66, 21);
            this.pixelValueComboBox.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Password:";
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
            this.label4.Location = new System.Drawing.Point(27, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Seed:";
            // 
            // encodeButton
            // 
            this.encodeButton.Location = new System.Drawing.Point(214, 45);
            this.encodeButton.Name = "encodeButton";
            this.encodeButton.Size = new System.Drawing.Size(79, 23);
            this.encodeButton.TabIndex = 1;
            this.encodeButton.Text = "&Encode";
            this.encodeButton.UseVisualStyleBackColor = true;
            this.encodeButton.Click += new System.EventHandler(this.encodeButton_Click);
            // 
            // fileRadioButton
            // 
            this.fileRadioButton.AutoSize = true;
            this.fileRadioButton.Location = new System.Drawing.Point(113, 22);
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
            this.textRadioButton.Location = new System.Drawing.Point(21, 22);
            this.textRadioButton.Name = "textRadioButton";
            this.textRadioButton.Size = new System.Drawing.Size(86, 17);
            this.textRadioButton.TabIndex = 1;
            this.textRadioButton.TabStop = true;
            this.textRadioButton.Text = "Encode Text";
            this.textRadioButton.UseVisualStyleBackColor = true;
            // 
            // writeTextButton
            // 
            this.writeTextButton.Location = new System.Drawing.Point(207, 20);
            this.writeTextButton.Name = "writeTextButton";
            this.writeTextButton.Size = new System.Drawing.Size(79, 23);
            this.writeTextButton.TabIndex = 3;
            this.writeTextButton.Text = "Write Text";
            this.writeTextButton.UseVisualStyleBackColor = true;
            this.writeTextButton.Click += new System.EventHandler(this.writeTextButton_Click);
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(260, 17);
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
            this.pathTextbox.Location = new System.Drawing.Point(44, 19);
            this.pathTextbox.Name = "pathTextbox";
            this.pathTextbox.ReadOnly = true;
            this.pathTextbox.Size = new System.Drawing.Size(210, 20);
            this.pathTextbox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Path:";
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
            this.groupBox1.Controls.Add(this.availableSizeBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.decodeRadio);
            this.groupBox1.Controls.Add(this.encodeRadio);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.pathTextbox);
            this.groupBox1.Controls.Add(this.browseButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(306, 77);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image";
            // 
            // availableSizeBox
            // 
            this.availableSizeBox.Location = new System.Drawing.Point(85, 45);
            this.availableSizeBox.Name = "availableSizeBox";
            this.availableSizeBox.ReadOnly = true;
            this.availableSizeBox.Size = new System.Drawing.Size(79, 20);
            this.availableSizeBox.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Available Size:";
            // 
            // decodeRadio
            // 
            this.decodeRadio.AutoSize = true;
            this.decodeRadio.Location = new System.Drawing.Point(238, 47);
            this.decodeRadio.Name = "decodeRadio";
            this.decodeRadio.Size = new System.Drawing.Size(63, 17);
            this.decodeRadio.TabIndex = 8;
            this.decodeRadio.Text = "Decode";
            this.decodeRadio.UseVisualStyleBackColor = true;
            // 
            // encodeRadio
            // 
            this.encodeRadio.AutoSize = true;
            this.encodeRadio.Checked = true;
            this.encodeRadio.Location = new System.Drawing.Point(170, 47);
            this.encodeRadio.Name = "encodeRadio";
            this.encodeRadio.Size = new System.Drawing.Size(62, 17);
            this.encodeRadio.TabIndex = 7;
            this.encodeRadio.TabStop = true;
            this.encodeRadio.Text = "Encode";
            this.encodeRadio.UseVisualStyleBackColor = true;
            this.encodeRadio.CheckedChanged += new System.EventHandler(this.encodeRadio_CheckedChanged);
            // 
            // seedBox
            // 
            this.seedBox.Location = new System.Drawing.Point(68, 21);
            this.seedBox.MaxLength = 10;
            this.seedBox.Name = "seedBox";
            this.seedBox.Size = new System.Drawing.Size(149, 20);
            this.seedBox.TabIndex = 3;
            this.seedBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.seedBox_KeyPress);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.requiredSizeBox);
            this.groupBox2.Controls.Add(this.requiredSizeLabel);
            this.groupBox2.Controls.Add(this.textRadioButton);
            this.groupBox2.Controls.Add(this.fileRadioButton);
            this.groupBox2.Controls.Add(this.writeTextButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 95);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(306, 78);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Message";
            // 
            // requiredSizeBox
            // 
            this.requiredSizeBox.Location = new System.Drawing.Point(85, 45);
            this.requiredSizeBox.Name = "requiredSizeBox";
            this.requiredSizeBox.ReadOnly = true;
            this.requiredSizeBox.Size = new System.Drawing.Size(79, 20);
            this.requiredSizeBox.TabIndex = 10;
            // 
            // requiredSizeLabel
            // 
            this.requiredSizeLabel.AutoSize = true;
            this.requiredSizeLabel.Location = new System.Drawing.Point(6, 48);
            this.requiredSizeLabel.Name = "requiredSizeLabel";
            this.requiredSizeLabel.Size = new System.Drawing.Size(76, 13);
            this.requiredSizeLabel.TabIndex = 9;
            this.requiredSizeLabel.Text = "Required Size:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Enabled = false;
            this.label10.Location = new System.Drawing.Point(144, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(27, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Bits:";
            // 
            // bitsNumericUpDown
            // 
            this.bitsNumericUpDown.Enabled = false;
            this.bitsNumericUpDown.Location = new System.Drawing.Point(174, 19);
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
            this.bitsNumericUpDown.ValueChanged += new System.EventHandler(this.bitsNumericUpDown_ValueChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.randomButton);
            this.groupBox3.Controls.Add(this.passwordBox);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.seedBox);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(324, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(299, 77);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Security";
            // 
            // randomButton
            // 
            this.randomButton.Location = new System.Drawing.Point(223, 19);
            this.randomButton.Name = "randomButton";
            this.randomButton.Size = new System.Drawing.Size(70, 23);
            this.randomButton.TabIndex = 4;
            this.randomButton.Text = "Random";
            this.randomButton.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.invertBox);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.encodeButton);
            this.groupBox4.Controls.Add(this.bitsNumericUpDown);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.pixelValueComboBox);
            this.groupBox4.Location = new System.Drawing.Point(324, 95);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(299, 78);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Encoding";
            // 
            // invertBox
            // 
            this.invertBox.AutoSize = true;
            this.invertBox.Location = new System.Drawing.Point(216, 21);
            this.invertBox.Name = "invertBox";
            this.invertBox.Size = new System.Drawing.Size(72, 17);
            this.invertBox.TabIndex = 9;
            this.invertBox.Text = "Invert bits";
            this.invertBox.UseVisualStyleBackColor = true;
            // 
            // msgSaveDialog
            // 
            this.msgSaveDialog.Filter = "All Files (*.*)|*.*";
            this.msgSaveDialog.Title = "Save";
            // 
            // decodeWorker
            // 
            this.decodeWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.decodeWorker_DoWork);
            // 
            // TrithemiusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 179);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TrithemiusForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trithemius";
            this.Load += new System.EventHandler(this.TrithemiusForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox seedBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button randomButton;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown bitsNumericUpDown;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox invertBox;
        private System.Windows.Forms.TextBox availableSizeBox;
        private System.Windows.Forms.RadioButton encodeRadio;
        private System.Windows.Forms.RadioButton decodeRadio;
        private System.Windows.Forms.TextBox requiredSizeBox;
        private System.Windows.Forms.Label requiredSizeLabel;
        private System.Windows.Forms.SaveFileDialog msgSaveDialog;
        private System.ComponentModel.BackgroundWorker decodeWorker;
    }
}