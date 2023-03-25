namespace MyDatabase
{
    partial class FormQR
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQR));
            this.textBoxQRcode = new System.Windows.Forms.TextBox();
            this.buttonUploadQR = new System.Windows.Forms.Button();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.comboBoxCamera = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCamera = new System.Windows.Forms.Button();
            this.timerCamera = new System.Windows.Forms.Timer(this.components);
            this.panelNewProducts = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxShop = new System.Windows.Forms.TextBox();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxAllValue = new System.Windows.Forms.TextBox();
            this.buttonAddAll = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.timerAnimationCamera = new System.Windows.Forms.Timer(this.components);
            this.buttonCameraUpload = new System.Windows.Forms.PictureBox();
            this.pictureBoxQR = new System.Windows.Forms.PictureBox();
            this.buttonUpdateValues = new System.Windows.Forms.PictureBox();
            this.timerAnimationValue = new System.Windows.Forms.Timer(this.components);
            this.buttonManualInput = new System.Windows.Forms.Button();
            this.labelNewShop = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.buttonCameraUpload)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonUpdateValues)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxQRcode
            // 
            this.textBoxQRcode.Location = new System.Drawing.Point(518, 12);
            this.textBoxQRcode.Name = "textBoxQRcode";
            this.textBoxQRcode.Size = new System.Drawing.Size(401, 20);
            this.textBoxQRcode.TabIndex = 1;
            // 
            // buttonUploadQR
            // 
            this.buttonUploadQR.Location = new System.Drawing.Point(12, 345);
            this.buttonUploadQR.Name = "buttonUploadQR";
            this.buttonUploadQR.Size = new System.Drawing.Size(89, 23);
            this.buttonUploadQR.TabIndex = 2;
            this.buttonUploadQR.Text = "Загрузить QR";
            this.buttonUploadQR.UseVisualStyleBackColor = true;
            this.buttonUploadQR.Click += new System.EventHandler(this.buttonUploadQR_Click);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(427, 10);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(85, 23);
            this.buttonSearch.TabIndex = 4;
            this.buttonSearch.Text = "Поиск чека";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // comboBoxCamera
            // 
            this.comboBoxCamera.FormattingEnabled = true;
            this.comboBoxCamera.Location = new System.Drawing.Point(70, 318);
            this.comboBoxCamera.Name = "comboBoxCamera";
            this.comboBoxCamera.Size = new System.Drawing.Size(215, 21);
            this.comboBoxCamera.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 321);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Камера: ";
            // 
            // buttonCamera
            // 
            this.buttonCamera.Location = new System.Drawing.Point(196, 345);
            this.buttonCamera.Name = "buttonCamera";
            this.buttonCamera.Size = new System.Drawing.Size(116, 23);
            this.buttonCamera.TabIndex = 7;
            this.buttonCamera.Text = "Включить камеру";
            this.buttonCamera.UseVisualStyleBackColor = true;
            this.buttonCamera.Click += new System.EventHandler(this.buttonCamera_Click);
            // 
            // timerCamera
            // 
            this.timerCamera.Interval = 50;
            this.timerCamera.Tick += new System.EventHandler(this.timerCamera_Tick);
            // 
            // panelNewProducts
            // 
            this.panelNewProducts.AutoScroll = true;
            this.panelNewProducts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelNewProducts.Location = new System.Drawing.Point(319, 94);
            this.panelNewProducts.Name = "panelNewProducts";
            this.panelNewProducts.Size = new System.Drawing.Size(600, 275);
            this.panelNewProducts.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(563, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Тип";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(334, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Название";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(461, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "Магазин";
            // 
            // textBoxShop
            // 
            this.textBoxShop.Location = new System.Drawing.Point(512, 40);
            this.textBoxShop.Name = "textBoxShop";
            this.textBoxShop.Size = new System.Drawing.Size(169, 20);
            this.textBoxShop.TabIndex = 26;
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Location = new System.Drawing.Point(319, 40);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(137, 20);
            this.dateTimePicker.TabIndex = 25;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(708, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Сумма";
            // 
            // textBoxAllValue
            // 
            this.textBoxAllValue.Location = new System.Drawing.Point(749, 40);
            this.textBoxAllValue.Name = "textBoxAllValue";
            this.textBoxAllValue.ReadOnly = true;
            this.textBoxAllValue.Size = new System.Drawing.Size(75, 20);
            this.textBoxAllValue.TabIndex = 23;
            this.textBoxAllValue.TabStop = false;
            // 
            // buttonAddAll
            // 
            this.buttonAddAll.Location = new System.Drawing.Point(853, 38);
            this.buttonAddAll.Name = "buttonAddAll";
            this.buttonAddAll.Size = new System.Drawing.Size(66, 23);
            this.buttonAddAll.TabIndex = 22;
            this.buttonAddAll.Text = "Добавить";
            this.buttonAddAll.UseVisualStyleBackColor = true;
            this.buttonAddAll.Click += new System.EventHandler(this.buttonAddAll_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(817, 78);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 30;
            this.label8.Text = "Итог";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(733, 78);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 13);
            this.label9.TabIndex = 29;
            this.label9.Text = "Кол-во";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(649, 78);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(33, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "Цена";
            // 
            // timerAnimationCamera
            // 
            this.timerAnimationCamera.Tick += new System.EventHandler(this.timerAnimation_Tick);
            // 
            // buttonCameraUpload
            // 
            this.buttonCameraUpload.Image = global::MyDatabase.Properties.Resources.reload;
            this.buttonCameraUpload.Location = new System.Drawing.Point(291, 318);
            this.buttonCameraUpload.Name = "buttonCameraUpload";
            this.buttonCameraUpload.Size = new System.Drawing.Size(21, 21);
            this.buttonCameraUpload.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.buttonCameraUpload.TabIndex = 8;
            this.buttonCameraUpload.TabStop = false;
            this.buttonCameraUpload.Click += new System.EventHandler(this.buttonCameraUpload_Click);
            // 
            // pictureBoxQR
            // 
            this.pictureBoxQR.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pictureBoxQR.BackgroundImage = global::MyDatabase.Properties.Resources.QRcodeBackGround1;
            this.pictureBoxQR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxQR.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxQR.Name = "pictureBoxQR";
            this.pictureBoxQR.Size = new System.Drawing.Size(300, 300);
            this.pictureBoxQR.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxQR.TabIndex = 0;
            this.pictureBoxQR.TabStop = false;
            this.pictureBoxQR.DoubleClick += new System.EventHandler(this.pictureBoxQR_DoubleClick);
            // 
            // buttonUpdateValues
            // 
            this.buttonUpdateValues.Image = global::MyDatabase.Properties.Resources.reload;
            this.buttonUpdateValues.Location = new System.Drawing.Point(827, 40);
            this.buttonUpdateValues.Name = "buttonUpdateValues";
            this.buttonUpdateValues.Size = new System.Drawing.Size(21, 21);
            this.buttonUpdateValues.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.buttonUpdateValues.TabIndex = 31;
            this.buttonUpdateValues.TabStop = false;
            this.buttonUpdateValues.Click += new System.EventHandler(this.buttonUpdateValues_Click);
            // 
            // timerAnimationValue
            // 
            this.timerAnimationValue.Tick += new System.EventHandler(this.timerAnimationValue_Tick);
            // 
            // buttonManualInput
            // 
            this.buttonManualInput.Location = new System.Drawing.Point(319, 10);
            this.buttonManualInput.Name = "buttonManualInput";
            this.buttonManualInput.Size = new System.Drawing.Size(102, 23);
            this.buttonManualInput.TabIndex = 34;
            this.buttonManualInput.Text = "Ввести вручную";
            this.buttonManualInput.UseVisualStyleBackColor = true;
            this.buttonManualInput.Click += new System.EventHandler(this.buttonManualInput_Click);
            // 
            // labelNewShop
            // 
            this.labelNewShop.AutoSize = true;
            this.labelNewShop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelNewShop.Location = new System.Drawing.Point(681, 42);
            this.labelNewShop.Name = "labelNewShop";
            this.labelNewShop.Size = new System.Drawing.Size(30, 13);
            this.labelNewShop.TabIndex = 35;
            this.labelNewShop.Text = "new";
            this.labelNewShop.Visible = false;
            // 
            // FormQR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 381);
            this.Controls.Add(this.labelNewShop);
            this.Controls.Add(this.buttonManualInput);
            this.Controls.Add(this.buttonUpdateValues);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxShop);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxAllValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonAddAll);
            this.Controls.Add(this.panelNewProducts);
            this.Controls.Add(this.buttonCameraUpload);
            this.Controls.Add(this.buttonCamera);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxCamera);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.buttonUploadQR);
            this.Controls.Add(this.textBoxQRcode);
            this.Controls.Add(this.pictureBoxQR);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormQR";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormQR";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormQR_FormClosing);
            this.Load += new System.EventHandler(this.FormQR_Load);
            ((System.ComponentModel.ISupportInitialize)(this.buttonCameraUpload)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonUpdateValues)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxQR;
        private System.Windows.Forms.TextBox textBoxQRcode;
        private System.Windows.Forms.Button buttonUploadQR;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.ComboBox comboBoxCamera;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCamera;
        private System.Windows.Forms.Timer timerCamera;
        private System.Windows.Forms.PictureBox buttonCameraUpload;
        private System.Windows.Forms.Panel panelNewProducts;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxShop;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxAllValue;
        private System.Windows.Forms.Button buttonAddAll;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Timer timerAnimationCamera;
        private System.Windows.Forms.PictureBox buttonUpdateValues;
        private System.Windows.Forms.Timer timerAnimationValue;
        private System.Windows.Forms.Button buttonManualInput;
        private System.Windows.Forms.Label labelNewShop;
    }
}