namespace MyDatabase
{
    partial class FormNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNew));
            this.label1 = new System.Windows.Forms.Label();
            this.buttonConfirm = new System.Windows.Forms.Button();
            this.inputName = new System.Windows.Forms.TextBox();
            this.inputPrice = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.inputNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.inputSale = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.inputType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.inputDate = new System.Windows.Forms.DateTimePicker();
            this.inputShop = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.outputValue = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBoxPercent = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Название";
            // 
            // buttonConfirm
            // 
            this.buttonConfirm.Location = new System.Drawing.Point(199, 37);
            this.buttonConfirm.Name = "buttonConfirm";
            this.buttonConfirm.Size = new System.Drawing.Size(75, 23);
            this.buttonConfirm.TabIndex = 16;
            this.buttonConfirm.Text = "Добавить";
            this.buttonConfirm.UseVisualStyleBackColor = true;
            this.buttonConfirm.Click += new System.EventHandler(this.buttonConfirm_Click);
            // 
            // inputName
            // 
            this.inputName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.inputName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.inputName.Location = new System.Drawing.Point(69, 11);
            this.inputName.Name = "inputName";
            this.inputName.Size = new System.Drawing.Size(200, 20);
            this.inputName.TabIndex = 2;
            this.inputName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inputEnter_KeyDown);
            this.inputName.Leave += new System.EventHandler(this.inputName_Leave);
            // 
            // inputPrice
            // 
            this.inputPrice.Location = new System.Drawing.Point(69, 39);
            this.inputPrice.Name = "inputPrice";
            this.inputPrice.Size = new System.Drawing.Size(80, 20);
            this.inputPrice.TabIndex = 4;
            this.inputPrice.TextChanged += new System.EventHandler(this.inputValue_TextChanged);
            this.inputPrice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inputEnter_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Цена";
            // 
            // inputNumber
            // 
            this.inputNumber.Location = new System.Drawing.Point(69, 66);
            this.inputNumber.Name = "inputNumber";
            this.inputNumber.Size = new System.Drawing.Size(80, 20);
            this.inputNumber.TabIndex = 6;
            this.inputNumber.Text = "1";
            this.inputNumber.TextChanged += new System.EventHandler(this.inputValue_TextChanged);
            this.inputNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inputEnter_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Кол-во";
            // 
            // inputSale
            // 
            this.inputSale.Location = new System.Drawing.Point(199, 66);
            this.inputSale.Name = "inputSale";
            this.inputSale.Size = new System.Drawing.Size(75, 20);
            this.inputSale.TabIndex = 8;
            this.inputSale.Text = "0";
            this.inputSale.TextChanged += new System.EventHandler(this.inputValue_TextChanged);
            this.inputSale.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inputEnter_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(149, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Скидка";
            // 
            // inputType
            // 
            this.inputType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputType.DropDownWidth = 80;
            this.inputType.Items.AddRange(new object[] {
            "0 Обяз.еда",
            "1 Эксп.еда",
            "2 К чаю",
            "3 Вкусн.",
            "4 Прочее",
            "5 Трансп.",
            "6 Одежда",
            "7 Кафе",
            "8 Техника",
            "9 Развлеч."});
            this.inputType.Location = new System.Drawing.Point(69, 93);
            this.inputType.Name = "inputType";
            this.inputType.Size = new System.Drawing.Size(78, 21);
            this.inputType.TabIndex = 9;
            this.inputType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inputEnter_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Тип";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 124);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Дата";
            // 
            // inputDate
            // 
            this.inputDate.Location = new System.Drawing.Point(69, 120);
            this.inputDate.Name = "inputDate";
            this.inputDate.Size = new System.Drawing.Size(139, 20);
            this.inputDate.TabIndex = 13;
            this.inputDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inputEnter_KeyDown);
            // 
            // inputShop
            // 
            this.inputShop.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.inputShop.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.inputShop.Location = new System.Drawing.Point(69, 148);
            this.inputShop.Name = "inputShop";
            this.inputShop.Size = new System.Drawing.Size(200, 20);
            this.inputShop.TabIndex = 15;
            this.inputShop.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inputEnter_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 151);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Магазин";
            // 
            // outputValue
            // 
            this.outputValue.Location = new System.Drawing.Point(199, 94);
            this.outputValue.Name = "outputValue";
            this.outputValue.ReadOnly = true;
            this.outputValue.Size = new System.Drawing.Size(75, 20);
            this.outputValue.TabIndex = 16;
            this.outputValue.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(162, 97);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Итог";
            // 
            // checkBoxPercent
            // 
            this.checkBoxPercent.AutoSize = true;
            this.checkBoxPercent.Location = new System.Drawing.Point(256, 68);
            this.checkBoxPercent.Name = "checkBoxPercent";
            this.checkBoxPercent.Size = new System.Drawing.Size(34, 17);
            this.checkBoxPercent.TabIndex = 18;
            this.checkBoxPercent.TabStop = false;
            this.checkBoxPercent.Text = "%";
            this.checkBoxPercent.UseVisualStyleBackColor = true;
            this.checkBoxPercent.CheckedChanged += new System.EventHandler(this.checkBoxProcent_CheckedChanged);
            // 
            // FormNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 181);
            this.Controls.Add(this.checkBoxPercent);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.outputValue);
            this.Controls.Add(this.inputShop);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.inputDate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.inputType);
            this.Controls.Add(this.inputSale);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.inputNumber);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.inputPrice);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.inputName);
            this.Controls.Add(this.buttonConfirm);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNew";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Добавление покупки";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonConfirm;
        private System.Windows.Forms.TextBox inputName;
        private System.Windows.Forms.TextBox inputPrice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox inputNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox inputSale;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox inputType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker inputDate;
        private System.Windows.Forms.TextBox inputShop;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox outputValue;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox checkBoxPercent;
    }
}