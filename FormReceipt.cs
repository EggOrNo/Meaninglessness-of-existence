using OpenQA.Selenium;
using System;
using System.Windows.Forms;

namespace MyDatabase
{
    public partial class FormReceipt : Form
    {
        private static string qrCode = string.Empty;
        public static string QRCode { get { return qrCode; } }

        public FormReceipt()
        {
            InitializeComponent();
        }
        public FormReceipt(string text)
        {
            InitializeComponent();
            string[] data = text.Split('&');
            if (data.Length == 6)
            {
                dateTimePicker.Value = DateTime.Parse(data[0].Substring(2).Replace('T', ' ').Insert(11, ":").Insert(6, ".").Insert(4, "."));
                textBoxSum.Text = data[1].Substring(2);
                textBoxFN.Text = data[2].Substring(3);
                textBoxFD.Text = data[3].Substring(2);
                textBoxFP.Text = data[4].Substring(3);
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            double sum = CheckTextBoxes();
            if (sum == double.MinValue)
                return;
            qrCode = $"t={dateTimePicker.Value.ToString("yyyyMMdd")}T{dateTimePicker.Value.ToString("HHmm")}&" +
                $"s={sum.ToString("0.00").Replace(',','.')}&fn={textBoxFN.Text}&i={textBoxFD.Text}&fp={textBoxFP.Text}&n=1";
            DialogResult = DialogResult.OK;
            Close();
        }
        private double CheckTextBoxes()
        {
            if (textBoxFN.Text == "")
            {
                MessageBox.Show($"Поле ФН не заполнено.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return double.MinValue;
            }
            if (textBoxFD.Text == "")
            {
                MessageBox.Show($"Поле ФД не заполнено.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return double.MinValue;
            }
            if (textBoxFP.Text == "")
            {
                MessageBox.Show($"Поле ФП не заполнено.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return double.MinValue;
            }
            if (textBoxSum.Text == "" || !double.TryParse(textBoxSum.Text, out double result))
            {
                MessageBox.Show($"Сумма указана некорректно.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return double.MinValue;
            }
            return result;
        }

        private void textBoxSum_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSum.Text.Contains("."))
            {
                int index = textBoxSum.SelectionStart;
                textBoxSum.Text = textBoxSum.Text.Replace('.', ',');
                textBoxSum.SelectionStart = index;
            }
        }

    }
}
