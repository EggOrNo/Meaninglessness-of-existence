namespace MyDatabase
{
    partial class FormReceipt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormReceipt));
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxSum = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxFN = new System.Windows.Forms.MaskedTextBox();
            this.textBoxFD = new System.Windows.Forms.MaskedTextBox();
            this.textBoxFP = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ФН";
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(59, 142);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonSearch.TabIndex = 17;
            this.buttonSearch.Text = "Поиск";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker.Location = new System.Drawing.Point(53, 116);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(127, 20);
            this.dateTimePicker.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "ФД";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "ФП/ФПД";
            // 
            // textBoxSum
            // 
            this.textBoxSum.Location = new System.Drawing.Point(68, 90);
            this.textBoxSum.Name = "textBoxSum";
            this.textBoxSum.Size = new System.Drawing.Size(112, 20);
            this.textBoxSum.TabIndex = 15;
            this.textBoxSum.TextChanged += new System.EventHandler(this.textBoxSum_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Сумма";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Дата";
            // 
            // textBoxFN
            // 
            this.textBoxFN.Location = new System.Drawing.Point(68, 12);
            this.textBoxFN.Mask = "000000000000000000";
            this.textBoxFN.Name = "textBoxFN";
            this.textBoxFN.Size = new System.Drawing.Size(112, 20);
            this.textBoxFN.TabIndex = 12;
            this.textBoxFN.ValidatingType = typeof(int);
            // 
            // textBoxFD
            // 
            this.textBoxFD.Location = new System.Drawing.Point(68, 38);
            this.textBoxFD.Mask = "000000000000000000";
            this.textBoxFD.Name = "textBoxFD";
            this.textBoxFD.Size = new System.Drawing.Size(112, 20);
            this.textBoxFD.TabIndex = 13;
            this.textBoxFD.ValidatingType = typeof(int);
            // 
            // textBoxFP
            // 
            this.textBoxFP.Location = new System.Drawing.Point(68, 64);
            this.textBoxFP.Mask = "000000000000000000";
            this.textBoxFP.Name = "textBoxFP";
            this.textBoxFP.Size = new System.Drawing.Size(112, 20);
            this.textBoxFP.TabIndex = 14;
            this.textBoxFP.ValidatingType = typeof(int);
            // 
            // FormReceipt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(190, 174);
            this.Controls.Add(this.textBoxFP);
            this.Controls.Add(this.textBoxFD);
            this.Controls.Add(this.textBoxFN);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxSum);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormReceipt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormReceipt";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxSum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MaskedTextBox textBoxFN;
        private System.Windows.Forms.MaskedTextBox textBoxFD;
        private System.Windows.Forms.MaskedTextBox textBoxFP;
    }
}