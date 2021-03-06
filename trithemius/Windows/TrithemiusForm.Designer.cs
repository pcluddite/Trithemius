﻿namespace Trithemius.Windows
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
            if (disposing && (components != null)) {
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
                this.groupBoxImage        = new System.Windows.Forms.GroupBox();
                this.pictureBox           = new System.Windows.Forms.PictureBox();
                this.groupBoxPath         = new System.Windows.Forms.GroupBox();
                this.pathTextBox          = new System.Windows.Forms.TextBox();
                this.buttonBrowse         = new System.Windows.Forms.Button();
                this.groupBoxDetails      = new System.Windows.Forms.GroupBox();
                this.label3               = new System.Windows.Forms.Label();
                this.label4               = new System.Windows.Forms.Label();
                this.textBoxHeight        = new System.Windows.Forms.TextBox();
                this.textBoxWidth         = new System.Windows.Forms.TextBox();
                this.label2               = new System.Windows.Forms.Label();
                this.label1               = new System.Windows.Forms.Label();
                this.textBoxMaxSize       = new System.Windows.Forms.TextBox();
                this.textBoxFormat        = new System.Windows.Forms.TextBox();
                this.groupBoxEncode       = new System.Windows.Forms.GroupBox();
                this.groupBoxAdvanced     = new System.Windows.Forms.GroupBox();
                this.groupBoxLegacy       = new System.Windows.Forms.GroupBox();
                this.comboBoxVersions     = new System.Windows.Forms.ComboBox();
                this.groupBoxPrefix       = new System.Windows.Forms.GroupBox();
                this.checkBoxPrefixSize   = new System.Windows.Forms.CheckBox();
                this.groupBoxInvert       = new System.Windows.Forms.GroupBox();
                this.checkBoxInvertPrefix = new System.Windows.Forms.CheckBox();
                this.checkBoxInvertData   = new System.Windows.Forms.CheckBox();
                this.groupBoxEndian       = new System.Windows.Forms.GroupBox();
                this.comboBoxEndian       = new System.Windows.Forms.ComboBox();
                this.groupBoxStartPixel   = new System.Windows.Forms.GroupBox();
                this.numericUpDownOffset  = new System.Windows.Forms.NumericUpDown();
                this.groupBoxLsb          = new System.Windows.Forms.GroupBox();
                this.numericUpDownLsb     = new System.Windows.Forms.NumericUpDown();
                this.groupBoxDataType     = new System.Windows.Forms.GroupBox();
                this.radioButtonFile      = new System.Windows.Forms.RadioButton();
                this.radioButtonText      = new System.Windows.Forms.RadioButton();
                this.groupBoxKey          = new System.Windows.Forms.GroupBox();
                this.textBoxKey           = new System.Windows.Forms.TextBox();
                this.groupBoxSeed         = new System.Windows.Forms.GroupBox();
                this.textBoxSeed          = new System.Windows.Forms.TextBox();
                this.groupBoxColors       = new System.Windows.Forms.GroupBox();
                this.checkBlue            = new System.Windows.Forms.CheckBox();
                this.checkGreen           = new System.Windows.Forms.CheckBox();
                this.checkRed             = new System.Windows.Forms.CheckBox();
                this.checkAlpha           = new System.Windows.Forms.CheckBox();
                this.buttonEncode         = new System.Windows.Forms.Button();
                this.buttonDecode         = new System.Windows.Forms.Button();
                this.openFileDialog       = new System.Windows.Forms.OpenFileDialog();
                this.encodeWorker         = new System.ComponentModel.BackgroundWorker();
                this.decodeWorker         = new System.ComponentModel.BackgroundWorker();
                this.saveFileDialog       = new System.Windows.Forms.SaveFileDialog();
                this.msgOpenDialog        = new System.Windows.Forms.OpenFileDialog();
                this.msgSaveDialog        = new System.Windows.Forms.SaveFileDialog();
                this.groupBoxImage.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
                this.groupBoxPath.SuspendLayout();
                this.groupBoxDetails.SuspendLayout();
                this.groupBoxEncode.SuspendLayout();
                this.groupBoxAdvanced.SuspendLayout();
                this.groupBoxLegacy.SuspendLayout();
                this.groupBoxPrefix.SuspendLayout();
                this.groupBoxInvert.SuspendLayout();
                this.groupBoxEndian.SuspendLayout();
                this.groupBoxStartPixel.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOffset)).BeginInit();
                this.groupBoxLsb.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLsb)).BeginInit();
                this.groupBoxDataType.SuspendLayout();
                this.groupBoxKey.SuspendLayout();
                this.groupBoxSeed.SuspendLayout();
                this.groupBoxColors.SuspendLayout();
                this.SuspendLayout();

                // 
                // groupBoxImage
                // 
                this.groupBoxImage.Anchor =
                    ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                           | System.Windows.Forms.AnchorStyles.Left)
                                                          | System.Windows.Forms.AnchorStyles.Right)));
                this.groupBoxImage.Controls.Add(this.pictureBox);
                this.groupBoxImage.Location = new System.Drawing.Point(505, 14);
                this.groupBoxImage.Name     = "groupBoxImage";
                this.groupBoxImage.Size     = new System.Drawing.Size(365, 290);
                this.groupBoxImage.TabIndex = 1;
                this.groupBoxImage.TabStop  = false;
                this.groupBoxImage.Text     = "Preview";

                // 
                // pictureBox
                // 
                this.pictureBox.Anchor =
                    ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                           | System.Windows.Forms.AnchorStyles.Left)
                                                          | System.Windows.Forms.AnchorStyles.Right)));
                this.pictureBox.Location = new System.Drawing.Point(9, 18);
                this.pictureBox.Name     = "pictureBox";
                this.pictureBox.Size     = new System.Drawing.Size(349, 260);
                this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                this.pictureBox.TabIndex = 0;
                this.pictureBox.TabStop  = false;
                // 
                // groupBoxPath
                // 
                this.groupBoxPath.Controls.Add(this.pathTextBox);
                this.groupBoxPath.Controls.Add(this.buttonBrowse);
                this.groupBoxPath.Location = new System.Drawing.Point(14, 14);
                this.groupBoxPath.Name     = "groupBoxPath";
                this.groupBoxPath.Size     = new System.Drawing.Size(484, 54);
                this.groupBoxPath.TabIndex = 0;
                this.groupBoxPath.TabStop  = false;
                this.groupBoxPath.Text     = "Path";

                // 
                // pathTextBox
                // 
                this.pathTextBox.Anchor =
                    ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
                this.pathTextBox.Location    =  new System.Drawing.Point(7, 18);
                this.pathTextBox.Name        =  "pathTextBox";
                this.pathTextBox.Size        =  new System.Drawing.Size(375, 23);
                this.pathTextBox.TabIndex    =  0;
                this.pathTextBox.TextChanged += new System.EventHandler(this.pathTextBox_TextChanged);

                // 
                // buttonBrowse
                // 
                this.buttonBrowse.Anchor =
                    ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
                this.buttonBrowse.Location                =  new System.Drawing.Point(390, 17);
                this.buttonBrowse.Name                    =  "buttonBrowse";
                this.buttonBrowse.Size                    =  new System.Drawing.Size(87, 27);
                this.buttonBrowse.TabIndex                =  1;
                this.buttonBrowse.Text                    =  "Browse";
                this.buttonBrowse.UseVisualStyleBackColor =  true;
                this.buttonBrowse.Click                   += new System.EventHandler(this.buttonBrowse_Click);

                // 
                // groupBoxDetails
                // 
                this.groupBoxDetails.Anchor =
                    ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                                                          | System.Windows.Forms.AnchorStyles.Right)));
                this.groupBoxDetails.Controls.Add(this.label3);
                this.groupBoxDetails.Controls.Add(this.label4);
                this.groupBoxDetails.Controls.Add(this.textBoxHeight);
                this.groupBoxDetails.Controls.Add(this.textBoxWidth);
                this.groupBoxDetails.Controls.Add(this.label2);
                this.groupBoxDetails.Controls.Add(this.label1);
                this.groupBoxDetails.Controls.Add(this.textBoxMaxSize);
                this.groupBoxDetails.Controls.Add(this.textBoxFormat);
                this.groupBoxDetails.Location = new System.Drawing.Point(505, 310);
                this.groupBoxDetails.Name     = "groupBoxDetails";
                this.groupBoxDetails.Size     = new System.Drawing.Size(365, 87);
                this.groupBoxDetails.TabIndex = 2;
                this.groupBoxDetails.TabStop  = false;
                this.groupBoxDetails.Text     = "Details";
                // 
                // label3
                // 
                this.label3.AutoSize = true;
                this.label3.Location = new System.Drawing.Point(210, 54);
                this.label3.Name     = "label3";
                this.label3.Size     = new System.Drawing.Size(46, 15);
                this.label3.TabIndex = 6;
                this.label3.Text     = "Height:";
                // 
                // label4
                // 
                this.label4.AutoSize = true;
                this.label4.Location = new System.Drawing.Point(213, 24);
                this.label4.Name     = "label4";
                this.label4.Size     = new System.Drawing.Size(42, 15);
                this.label4.TabIndex = 4;
                this.label4.Text     = "Width:";
                // 
                // textBoxHeight
                // 
                this.textBoxHeight.Location  = new System.Drawing.Point(265, 51);
                this.textBoxHeight.Name      = "textBoxHeight";
                this.textBoxHeight.ReadOnly  = true;
                this.textBoxHeight.Size      = new System.Drawing.Size(91, 23);
                this.textBoxHeight.TabIndex  = 7;
                this.textBoxHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                // 
                // textBoxWidth
                // 
                this.textBoxWidth.Location  = new System.Drawing.Point(265, 21);
                this.textBoxWidth.Name      = "textBoxWidth";
                this.textBoxWidth.ReadOnly  = true;
                this.textBoxWidth.Size      = new System.Drawing.Size(91, 23);
                this.textBoxWidth.TabIndex  = 5;
                this.textBoxWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                // 
                // label2
                // 
                this.label2.AutoSize = true;
                this.label2.Location = new System.Drawing.Point(19, 54);
                this.label2.Name     = "label2";
                this.label2.Size     = new System.Drawing.Size(80, 15);
                this.label2.TabIndex = 2;
                this.label2.Text     = "Available Bits:";
                // 
                // label1
                // 
                this.label1.AutoSize = true;
                this.label1.Location = new System.Drawing.Point(55, 24);
                this.label1.Name     = "label1";
                this.label1.Size     = new System.Drawing.Size(48, 15);
                this.label1.TabIndex = 0;
                this.label1.Text     = "Format:";
                // 
                // textBoxMaxSize
                // 
                this.textBoxMaxSize.Location  = new System.Drawing.Point(111, 51);
                this.textBoxMaxSize.Name      = "textBoxMaxSize";
                this.textBoxMaxSize.ReadOnly  = true;
                this.textBoxMaxSize.Size      = new System.Drawing.Size(91, 23);
                this.textBoxMaxSize.TabIndex  = 3;
                this.textBoxMaxSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                // 
                // textBoxFormat
                // 
                this.textBoxFormat.Location  = new System.Drawing.Point(111, 21);
                this.textBoxFormat.Name      = "textBoxFormat";
                this.textBoxFormat.ReadOnly  = true;
                this.textBoxFormat.Size      = new System.Drawing.Size(91, 23);
                this.textBoxFormat.TabIndex  = 1;
                this.textBoxFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                // 
                // groupBoxEncode
                // 
                this.groupBoxEncode.Controls.Add(this.groupBoxAdvanced);
                this.groupBoxEncode.Controls.Add(this.groupBoxStartPixel);
                this.groupBoxEncode.Controls.Add(this.groupBoxLsb);
                this.groupBoxEncode.Controls.Add(this.groupBoxDataType);
                this.groupBoxEncode.Controls.Add(this.groupBoxKey);
                this.groupBoxEncode.Controls.Add(this.groupBoxSeed);
                this.groupBoxEncode.Controls.Add(this.groupBoxColors);
                this.groupBoxEncode.Enabled  = false;
                this.groupBoxEncode.Location = new System.Drawing.Point(14, 75);
                this.groupBoxEncode.Name     = "groupBoxEncode";
                this.groupBoxEncode.Size     = new System.Drawing.Size(484, 324);
                this.groupBoxEncode.TabIndex = 1;
                this.groupBoxEncode.TabStop  = false;
                this.groupBoxEncode.Text     = "Encoding";
                // 
                // groupBoxAdvanced
                // 
                this.groupBoxAdvanced.Controls.Add(this.groupBoxLegacy);
                this.groupBoxAdvanced.Controls.Add(this.groupBoxPrefix);
                this.groupBoxAdvanced.Controls.Add(this.groupBoxInvert);
                this.groupBoxAdvanced.Controls.Add(this.groupBoxEndian);
                this.groupBoxAdvanced.Location = new System.Drawing.Point(7, 159);
                this.groupBoxAdvanced.Name     = "groupBoxAdvanced";
                this.groupBoxAdvanced.Size     = new System.Drawing.Size(463, 152);
                this.groupBoxAdvanced.TabIndex = 6;
                this.groupBoxAdvanced.TabStop  = false;
                this.groupBoxAdvanced.Text     = "Advanced";
                // 
                // groupBoxLegacy
                // 
                this.groupBoxLegacy.Controls.Add(this.comboBoxVersions);
                this.groupBoxLegacy.Location = new System.Drawing.Point(227, 89);
                this.groupBoxLegacy.Name     = "groupBoxLegacy";
                this.groupBoxLegacy.Size     = new System.Drawing.Size(224, 57);
                this.groupBoxLegacy.TabIndex = 3;
                this.groupBoxLegacy.TabStop  = false;
                this.groupBoxLegacy.Text     = "Compatability Mode";
                // 
                // comboBoxVersions
                // 
                this.comboBoxVersions.DropDownStyle     = System.Windows.Forms.ComboBoxStyle.DropDownList;
                this.comboBoxVersions.FormattingEnabled = true;
                this.comboBoxVersions.Items.AddRange(new object[] {"None", "Beta 1 Presets", "Beta 2 Presets"});
                this.comboBoxVersions.Location             =  new System.Drawing.Point(10, 22);
                this.comboBoxVersions.Name                 =  "comboBoxVersions";
                this.comboBoxVersions.Size                 =  new System.Drawing.Size(208, 23);
                this.comboBoxVersions.TabIndex             =  0;
                this.comboBoxVersions.SelectedIndexChanged += new System.EventHandler(this.comboBoxVersions_SelectedIndexChanged);
                // 
                // groupBoxPrefix
                // 
                this.groupBoxPrefix.Controls.Add(this.checkBoxPrefixSize);
                this.groupBoxPrefix.Location = new System.Drawing.Point(7, 89);
                this.groupBoxPrefix.Name     = "groupBoxPrefix";
                this.groupBoxPrefix.Size     = new System.Drawing.Size(213, 57);
                this.groupBoxPrefix.TabIndex = 1;
                this.groupBoxPrefix.TabStop  = false;
                this.groupBoxPrefix.Text     = "Prefix";
                // 
                // checkBoxPrefixSize
                // 
                this.checkBoxPrefixSize.AutoSize                = true;
                this.checkBoxPrefixSize.Checked                 = true;
                this.checkBoxPrefixSize.CheckState              = System.Windows.Forms.CheckState.Checked;
                this.checkBoxPrefixSize.Location                = new System.Drawing.Point(13, 22);
                this.checkBoxPrefixSize.Name                    = "checkBoxPrefixSize";
                this.checkBoxPrefixSize.Size                    = new System.Drawing.Size(131, 19);
                this.checkBoxPrefixSize.TabIndex                = 2;
                this.checkBoxPrefixSize.Text                    = "Prefix Data with Size";
                this.checkBoxPrefixSize.UseVisualStyleBackColor = true;
                // 
                // groupBoxInvert
                // 
                this.groupBoxInvert.Controls.Add(this.checkBoxInvertPrefix);
                this.groupBoxInvert.Controls.Add(this.checkBoxInvertData);
                this.groupBoxInvert.Location = new System.Drawing.Point(227, 22);
                this.groupBoxInvert.Name     = "groupBoxInvert";
                this.groupBoxInvert.Size     = new System.Drawing.Size(224, 60);
                this.groupBoxInvert.TabIndex = 2;
                this.groupBoxInvert.TabStop  = false;
                this.groupBoxInvert.Text     = "Invert";
                // 
                // checkBoxInvertPrefix
                // 
                this.checkBoxInvertPrefix.AutoSize                = true;
                this.checkBoxInvertPrefix.Location                = new System.Drawing.Point(98, 24);
                this.checkBoxInvertPrefix.Name                    = "checkBoxInvertPrefix";
                this.checkBoxInvertPrefix.Size                    = new System.Drawing.Size(77, 19);
                this.checkBoxInvertPrefix.TabIndex                = 1;
                this.checkBoxInvertPrefix.Text                    = "Prefix Bits";
                this.checkBoxInvertPrefix.UseVisualStyleBackColor = true;
                // 
                // checkBoxInvertData
                // 
                this.checkBoxInvertData.AutoSize                = true;
                this.checkBoxInvertData.Location                = new System.Drawing.Point(10, 24);
                this.checkBoxInvertData.Name                    = "checkBoxInvertData";
                this.checkBoxInvertData.Size                    = new System.Drawing.Size(72, 19);
                this.checkBoxInvertData.TabIndex                = 0;
                this.checkBoxInvertData.Text                    = "Data Bits";
                this.checkBoxInvertData.UseVisualStyleBackColor = true;
                // 
                // groupBoxEndian
                // 
                this.groupBoxEndian.Controls.Add(this.comboBoxEndian);
                this.groupBoxEndian.Location = new System.Drawing.Point(7, 22);
                this.groupBoxEndian.Name     = "groupBoxEndian";
                this.groupBoxEndian.Size     = new System.Drawing.Size(213, 60);
                this.groupBoxEndian.TabIndex = 0;
                this.groupBoxEndian.TabStop  = false;
                this.groupBoxEndian.Text     = "Endian Mode";
                // 
                // comboBoxEndian
                // 
                this.comboBoxEndian.DropDownStyle     = System.Windows.Forms.ComboBoxStyle.DropDownList;
                this.comboBoxEndian.FormattingEnabled = true;
                this.comboBoxEndian.Items.AddRange(new object[] {"Lowest Bit First (Little Endian)", "Highest Bit First (Big Endian)"});
                this.comboBoxEndian.Location = new System.Drawing.Point(13, 22);
                this.comboBoxEndian.Name     = "comboBoxEndian";
                this.comboBoxEndian.Size     = new System.Drawing.Size(193, 23);
                this.comboBoxEndian.TabIndex = 0;
                // 
                // groupBoxStartPixel
                // 
                this.groupBoxStartPixel.Controls.Add(this.numericUpDownOffset);
                this.groupBoxStartPixel.Location = new System.Drawing.Point(112, 89);
                this.groupBoxStartPixel.Name     = "groupBoxStartPixel";
                this.groupBoxStartPixel.Size     = new System.Drawing.Size(86, 63);
                this.groupBoxStartPixel.TabIndex = 2;
                this.groupBoxStartPixel.TabStop  = false;
                this.groupBoxStartPixel.Text     = "Start Pixel";
                // 
                // numericUpDownOffset
                // 
                this.numericUpDownOffset.Location  = new System.Drawing.Point(20, 24);
                this.numericUpDownOffset.Maximum   = new decimal(new int[] {4, 0, 0, 0});
                this.numericUpDownOffset.Minimum   = new decimal(new int[] {1, 0, 0, 0});
                this.numericUpDownOffset.Name      = "numericUpDownOffset";
                this.numericUpDownOffset.Size      = new System.Drawing.Size(56, 23);
                this.numericUpDownOffset.TabIndex  = 0;
                this.numericUpDownOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
                this.numericUpDownOffset.Value     = new decimal(new int[] {1, 0, 0, 0});
                // 
                // groupBoxLsb
                // 
                this.groupBoxLsb.Controls.Add(this.numericUpDownLsb);
                this.groupBoxLsb.Location = new System.Drawing.Point(7, 89);
                this.groupBoxLsb.Name     = "groupBoxLsb";
                this.groupBoxLsb.Size     = new System.Drawing.Size(98, 63);
                this.groupBoxLsb.TabIndex = 1;
                this.groupBoxLsb.TabStop  = false;
                this.groupBoxLsb.Text     = "Bits Per Color";
                // 
                // numericUpDownLsb
                // 
                this.numericUpDownLsb.Location     =  new System.Drawing.Point(20, 24);
                this.numericUpDownLsb.Maximum      =  new decimal(new int[] {4, 0, 0, 0});
                this.numericUpDownLsb.Minimum      =  new decimal(new int[] {1, 0, 0, 0});
                this.numericUpDownLsb.Name         =  "numericUpDownLsb";
                this.numericUpDownLsb.Size         =  new System.Drawing.Size(56, 23);
                this.numericUpDownLsb.TabIndex     =  0;
                this.numericUpDownLsb.TextAlign    =  System.Windows.Forms.HorizontalAlignment.Right;
                this.numericUpDownLsb.Value        =  new decimal(new int[] {1, 0, 0, 0});
                this.numericUpDownLsb.ValueChanged += new System.EventHandler(this.numericUpDownLsb_ValueChanged);
                // 
                // groupBoxDataType
                // 
                this.groupBoxDataType.Controls.Add(this.radioButtonFile);
                this.groupBoxDataType.Controls.Add(this.radioButtonText);
                this.groupBoxDataType.Location = new System.Drawing.Point(7, 22);
                this.groupBoxDataType.Name     = "groupBoxDataType";
                this.groupBoxDataType.Size     = new System.Drawing.Size(191, 60);
                this.groupBoxDataType.TabIndex = 0;
                this.groupBoxDataType.TabStop  = false;
                this.groupBoxDataType.Text     = "Content Type";
                // 
                // radioButtonFile
                // 
                this.radioButtonFile.AutoSize                = true;
                this.radioButtonFile.Location                = new System.Drawing.Point(122, 24);
                this.radioButtonFile.Name                    = "radioButtonFile";
                this.radioButtonFile.Size                    = new System.Drawing.Size(43, 19);
                this.radioButtonFile.TabIndex                = 1;
                this.radioButtonFile.Text                    = "File";
                this.radioButtonFile.UseVisualStyleBackColor = true;
                // 
                // radioButtonText
                // 
                this.radioButtonText.AutoSize                = true;
                this.radioButtonText.Checked                 = true;
                this.radioButtonText.Location                = new System.Drawing.Point(20, 24);
                this.radioButtonText.Name                    = "radioButtonText";
                this.radioButtonText.Size                    = new System.Drawing.Size(84, 19);
                this.radioButtonText.TabIndex                = 0;
                this.radioButtonText.TabStop                 = true;
                this.radioButtonText.Text                    = "Text (UTF8)";
                this.radioButtonText.UseVisualStyleBackColor = true;
                // 
                // groupBoxKey
                // 
                this.groupBoxKey.Controls.Add(this.textBoxKey);
                this.groupBoxKey.Location = new System.Drawing.Point(205, 89);
                this.groupBoxKey.Name     = "groupBoxKey";
                this.groupBoxKey.Size     = new System.Drawing.Size(177, 63);
                this.groupBoxKey.TabIndex = 4;
                this.groupBoxKey.TabStop  = false;
                this.groupBoxKey.Text     = "Key";
                // 
                // textBoxKey
                // 
                this.textBoxKey.Location              = new System.Drawing.Point(7, 23);
                this.textBoxKey.Name                  = "textBoxKey";
                this.textBoxKey.Size                  = new System.Drawing.Size(163, 23);
                this.textBoxKey.TabIndex              = 0;
                this.textBoxKey.UseSystemPasswordChar = true;
                // 
                // groupBoxSeed
                // 
                this.groupBoxSeed.Controls.Add(this.textBoxSeed);
                this.groupBoxSeed.Location = new System.Drawing.Point(205, 22);
                this.groupBoxSeed.Name     = "groupBoxSeed";
                this.groupBoxSeed.Size     = new System.Drawing.Size(177, 60);
                this.groupBoxSeed.TabIndex = 3;
                this.groupBoxSeed.TabStop  = false;
                this.groupBoxSeed.Text     = "Seed";
                // 
                // textBoxSeed
                // 
                this.textBoxSeed.Location = new System.Drawing.Point(7, 23);
                this.textBoxSeed.Name     = "textBoxSeed";
                this.textBoxSeed.Size     = new System.Drawing.Size(163, 23);
                this.textBoxSeed.TabIndex = 0;
                // 
                // groupBoxColors
                // 
                this.groupBoxColors.Controls.Add(this.checkBlue);
                this.groupBoxColors.Controls.Add(this.checkGreen);
                this.groupBoxColors.Controls.Add(this.checkRed);
                this.groupBoxColors.Controls.Add(this.checkAlpha);
                this.groupBoxColors.Location = new System.Drawing.Point(390, 22);
                this.groupBoxColors.Name     = "groupBoxColors";
                this.groupBoxColors.Size     = new System.Drawing.Size(80, 130);
                this.groupBoxColors.TabIndex = 5;
                this.groupBoxColors.TabStop  = false;
                this.groupBoxColors.Text     = "Colors";
                // 
                // checkBlue
                // 
                this.checkBlue.AutoSize                =  true;
                this.checkBlue.Checked                 =  true;
                this.checkBlue.CheckState              =  System.Windows.Forms.CheckState.Checked;
                this.checkBlue.Location                =  new System.Drawing.Point(7, 104);
                this.checkBlue.Name                    =  "checkBlue";
                this.checkBlue.Size                    =  new System.Drawing.Size(49, 19);
                this.checkBlue.TabIndex                =  3;
                this.checkBlue.Text                    =  "Blue";
                this.checkBlue.UseVisualStyleBackColor =  true;
                this.checkBlue.CheckedChanged          += new System.EventHandler(this.checkBlue_CheckedChanged);
                // 
                // checkGreen
                // 
                this.checkGreen.AutoSize                =  true;
                this.checkGreen.Checked                 =  true;
                this.checkGreen.CheckState              =  System.Windows.Forms.CheckState.Checked;
                this.checkGreen.Location                =  new System.Drawing.Point(7, 77);
                this.checkGreen.Name                    =  "checkGreen";
                this.checkGreen.Size                    =  new System.Drawing.Size(57, 19);
                this.checkGreen.TabIndex                =  2;
                this.checkGreen.Text                    =  "Green";
                this.checkGreen.UseVisualStyleBackColor =  true;
                this.checkGreen.CheckedChanged          += new System.EventHandler(this.checkGreen_CheckedChanged);
                // 
                // checkRed
                // 
                this.checkRed.AutoSize                =  true;
                this.checkRed.Checked                 =  true;
                this.checkRed.CheckState              =  System.Windows.Forms.CheckState.Checked;
                this.checkRed.Location                =  new System.Drawing.Point(7, 51);
                this.checkRed.Name                    =  "checkRed";
                this.checkRed.Size                    =  new System.Drawing.Size(46, 19);
                this.checkRed.TabIndex                =  1;
                this.checkRed.Text                    =  "Red";
                this.checkRed.UseVisualStyleBackColor =  true;
                this.checkRed.CheckedChanged          += new System.EventHandler(this.checkRed_CheckedChanged);
                // 
                // checkAlpha
                // 
                this.checkAlpha.AutoSize                =  true;
                this.checkAlpha.Checked                 =  true;
                this.checkAlpha.CheckState              =  System.Windows.Forms.CheckState.Checked;
                this.checkAlpha.Location                =  new System.Drawing.Point(7, 24);
                this.checkAlpha.Name                    =  "checkAlpha";
                this.checkAlpha.Size                    =  new System.Drawing.Size(57, 19);
                this.checkAlpha.TabIndex                =  0;
                this.checkAlpha.Text                    =  "Alpha";
                this.checkAlpha.UseVisualStyleBackColor =  true;
                this.checkAlpha.CheckedChanged          += new System.EventHandler(this.checkAlpha_CheckedChanged);

                // 
                // buttonEncode
                // 
                this.buttonEncode.Anchor =
                    ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
                this.buttonEncode.Enabled                 =  false;
                this.buttonEncode.Location                =  new System.Drawing.Point(783, 404);
                this.buttonEncode.Name                    =  "buttonEncode";
                this.buttonEncode.Size                    =  new System.Drawing.Size(87, 27);
                this.buttonEncode.TabIndex                =  4;
                this.buttonEncode.Text                    =  "Encode";
                this.buttonEncode.UseVisualStyleBackColor =  true;
                this.buttonEncode.Click                   += new System.EventHandler(this.buttonEncode_Click);

                // 
                // buttonDecode
                // 
                this.buttonDecode.Anchor =
                    ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
                this.buttonDecode.Enabled                 =  false;
                this.buttonDecode.Location                =  new System.Drawing.Point(688, 404);
                this.buttonDecode.Name                    =  "buttonDecode";
                this.buttonDecode.Size                    =  new System.Drawing.Size(87, 27);
                this.buttonDecode.TabIndex                =  3;
                this.buttonDecode.Text                    =  "Decode";
                this.buttonDecode.UseVisualStyleBackColor =  true;
                this.buttonDecode.Click                   += new System.EventHandler(this.buttonDecode_Click);

                // 
                // openFileDialog
                // 
                this.openFileDialog.Filter = "Image Files (*.jpg; *.bmp; *.png; *.gif; *.tiff)|*.jpg; *.jpeg; *.bmp;*.png;*.gif"
                                             + ";*.tiff|All Files (*.*)|*.*";
                this.openFileDialog.Title = "Browse Image";
                // 
                // encodeWorker
                // 
                this.encodeWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.encodeWorker_DoWork);

                this.encodeWorker.RunWorkerCompleted +=
                    new System.ComponentModel.RunWorkerCompletedEventHandler(this.encodeWorker_RunWorkerCompleted);
                // 
                // decodeWorker
                // 
                this.decodeWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.decodeWorker_DoWork);

                this.decodeWorker.RunWorkerCompleted +=
                    new System.ComponentModel.RunWorkerCompletedEventHandler(this.decodeWorker_RunWorkerCompleted);
                // 
                // saveFileDialog
                // 
                this.saveFileDialog.Filter = "PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp|GIF (*.gif)|*.gif|All Files (*.*)|*.*";
                this.saveFileDialog.Title  = "Save Image";
                // 
                // msgOpenDialog
                // 
                this.msgOpenDialog.Filter = "All Files (*.*)|*.*";
                this.msgOpenDialog.Title  = "Open";
                // 
                // msgSaveDialog
                // 
                this.msgSaveDialog.Filter = "All Files (*.*)|*.*";
                this.msgSaveDialog.Title  = "Save";
                // 
                // TrithemiusForm
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
                this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize          = new System.Drawing.Size(885, 440);
                this.Controls.Add(this.buttonDecode);
                this.Controls.Add(this.buttonEncode);
                this.Controls.Add(this.groupBoxEncode);
                this.Controls.Add(this.groupBoxDetails);
                this.Controls.Add(this.groupBoxPath);
                this.Controls.Add(this.groupBoxImage);
                this.Icon          =  ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
                this.MinimumSize   =  new System.Drawing.Size(901, 479);
                this.Name          =  "TrithemiusForm";
                this.StartPosition =  System.Windows.Forms.FormStartPosition.CenterScreen;
                this.Text          =  "Trithemius";
                this.Load          += new System.EventHandler(this.TrithemiusForm_Load);
                this.groupBoxImage.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
                this.groupBoxPath.ResumeLayout(false);
                this.groupBoxPath.PerformLayout();
                this.groupBoxDetails.ResumeLayout(false);
                this.groupBoxDetails.PerformLayout();
                this.groupBoxEncode.ResumeLayout(false);
                this.groupBoxAdvanced.ResumeLayout(false);
                this.groupBoxLegacy.ResumeLayout(false);
                this.groupBoxPrefix.ResumeLayout(false);
                this.groupBoxPrefix.PerformLayout();
                this.groupBoxInvert.ResumeLayout(false);
                this.groupBoxInvert.PerformLayout();
                this.groupBoxEndian.ResumeLayout(false);
                this.groupBoxStartPixel.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOffset)).EndInit();
                this.groupBoxLsb.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLsb)).EndInit();
                this.groupBoxDataType.ResumeLayout(false);
                this.groupBoxDataType.PerformLayout();
                this.groupBoxKey.ResumeLayout(false);
                this.groupBoxKey.PerformLayout();
                this.groupBoxSeed.ResumeLayout(false);
                this.groupBoxSeed.PerformLayout();
                this.groupBoxColors.ResumeLayout(false);
                this.groupBoxColors.PerformLayout();
                this.ResumeLayout(false);
            }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxImage;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.GroupBox groupBoxPath;
        private System.Windows.Forms.TextBox pathTextBox;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.GroupBox groupBoxDetails;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxHeight;
        private System.Windows.Forms.TextBox textBoxWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxMaxSize;
        private System.Windows.Forms.TextBox textBoxFormat;
        private System.Windows.Forms.GroupBox groupBoxEncode;
        private System.Windows.Forms.GroupBox groupBoxKey;
        private System.Windows.Forms.GroupBox groupBoxSeed;
        private System.Windows.Forms.TextBox textBoxSeed;
        private System.Windows.Forms.GroupBox groupBoxColors;
        private System.Windows.Forms.CheckBox checkBlue;
        private System.Windows.Forms.CheckBox checkGreen;
        private System.Windows.Forms.CheckBox checkRed;
        private System.Windows.Forms.CheckBox checkAlpha;
        private System.Windows.Forms.GroupBox groupBoxDataType;
        private System.Windows.Forms.RadioButton radioButtonFile;
        private System.Windows.Forms.RadioButton radioButtonText;
        private System.Windows.Forms.GroupBox groupBoxLsb;
        private System.Windows.Forms.NumericUpDown numericUpDownLsb;
        private System.Windows.Forms.GroupBox groupBoxStartPixel;
        private System.Windows.Forms.NumericUpDown numericUpDownOffset;
        private System.Windows.Forms.GroupBox groupBoxAdvanced;
        private System.Windows.Forms.ComboBox comboBoxEndian;
        private System.Windows.Forms.GroupBox groupBoxInvert;
        private System.Windows.Forms.CheckBox checkBoxInvertPrefix;
        private System.Windows.Forms.CheckBox checkBoxInvertData;
        private System.Windows.Forms.GroupBox groupBoxEndian;
        private System.Windows.Forms.GroupBox groupBoxPrefix;
        private System.Windows.Forms.CheckBox checkBoxPrefixSize;
        private System.Windows.Forms.Button buttonEncode;
        private System.Windows.Forms.Button buttonDecode;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox textBoxKey;
        private System.ComponentModel.BackgroundWorker encodeWorker;
        private System.ComponentModel.BackgroundWorker decodeWorker;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog msgOpenDialog;
        private System.Windows.Forms.SaveFileDialog msgSaveDialog;
        private System.Windows.Forms.GroupBox groupBoxLegacy;
        private System.Windows.Forms.ComboBox comboBoxVersions;
    }
}

