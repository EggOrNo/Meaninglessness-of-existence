using System;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Reflection;

namespace MyDatabase
{
    public partial class FormNew : Form
    {
        private SQLiteConnection dbConn;
        private bool isnew;
        private string oldName;
        private string oldDate;

        static private DateTime lastDate = DateTime.Today;
        static private string lastShop = "Ярче";
        public FormNew()
        {
            InitializeComponent();
            dbConn = FormMain.DBConn;
            inputName.AutoCompleteCustomSource = FormMain.ProductSource;
            inputShop.AutoCompleteCustomSource = FormMain.ShopSource;
            isnew = true;
            inputDate.Value = lastDate;
            inputShop.Text = lastShop;
            inputType.SelectedIndex = 0;
        }
        public FormNew(DataGridViewCellCollection cells)
        {
            InitializeComponent();
            dbConn = FormMain.DBConn;
            inputName.AutoCompleteCustomSource = FormMain.ProductSource;
            inputShop.AutoCompleteCustomSource = FormMain.ShopSource;
            isnew = false;
            inputDate.Value = lastDate;
            inputShop.Text = lastShop;

            inputName.Text = cells[0].Value.ToString();
            inputPrice.Text = cells[1].Value.ToString();
            inputNumber.Text = cells[2].Value.ToString();
            inputSale.Text = cells[7].Value.ToString();
            inputType.Text = cells[4].Value.ToString();
            inputDate.Text = cells[5].Value.ToString();
            inputShop.Text = cells[6].Value.ToString();
            buttonConfirm.Text = "Сохранить";
            oldName = cells[0].Value.ToString();
            oldDate = cells[5].Value.ToString();
        }

        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            SQLiteCommand command = CreateCommand();
            if (command == null)
                return;
            if (isnew)
                command.CommandText = "INSERT INTO Products (name, price, number, sale, type, date, shop)" +
                    " VALUES (@name, @price, @number, @sale, @type, @date, @shop)";
            else
            {
                command.CommandText = "UPDATE Products SET name=@name, price=@price, number=@number, sale=@sale, " +
                    "type=@type, date=@date, shop=@shop WHERE name=@oldName AND date=@oldDate";
                command.Parameters.AddWithValue("@oldName", oldName);
                command.Parameters.AddWithValue("@oldDate", oldDate);
            }
            command.ExecuteNonQuery();
            if (isnew)
            {
                lastDate = inputDate.Value;
                lastShop = inputShop.Text;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private SQLiteCommand CreateCommand()
        {
            SQLiteCommand command = new SQLiteCommand();
            if (inputName.Text == "")
            {
                MessageBox.Show($"Название не указано.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            command.Parameters.AddWithValue("@name", inputName.Text);

            if (inputPrice.Text == "" || !double.TryParse(inputPrice.Text, out double result))
            {
                MessageBox.Show($"Цена указана некорректно.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            command.Parameters.AddWithValue("@price", result);
            double price = result;

            if (inputNumber.Text == "" || !double.TryParse(inputNumber.Text, out result))
            {
                MessageBox.Show($"Количество указано некорректно.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            command.Parameters.AddWithValue("@number", result);

            if (inputSale.Text == "" || !double.TryParse(inputSale.Text, out result))
            {
                MessageBox.Show($"Скидка указана некорректно.", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            command.Parameters.AddWithValue("@sale", checkBoxPercent.Checked ? result * price / 100 : result);
            command.Parameters.AddWithValue("@type", inputType.Text);
            command.Parameters.AddWithValue("@date", inputDate.Value.ToString("yyyy.MM.dd"));

            if (inputShop.Text == "")
                command.Parameters.AddWithValue("@shop", string.Empty);
            else
                command.Parameters.AddWithValue("@shop", inputShop.Text);

            command.Connection = dbConn;
            return command;
        }

        private void inputValue_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(inputPrice.Text, out float price) && float.TryParse(inputNumber.Text, out float number) &&
                float.TryParse(inputSale.Text, out float sale))
            {
                if (checkBoxPercent.Checked)
                    outputValue.Text = (price * (1.0 - sale / 100) * number).ToString("0.00");
                else
                    outputValue.Text = ((price - sale) * number).ToString("0.00");
            }
            else if (inputPrice.Text.Contains("."))
            {
                int index = inputPrice.SelectionStart;
                inputPrice.Text = inputPrice.Text.Replace('.', ',');
                inputPrice.SelectionStart = index;
            }
            else if (inputNumber.Text.Contains("."))
            {
                int index = inputNumber.SelectionStart;
                inputNumber.Text = inputNumber.Text.Replace('.', ',');
                inputNumber.SelectionStart = index;
            }
            else if (inputSale.Text.Contains("."))
            {
                int index = inputSale.SelectionStart;
                inputSale.Text = inputSale.Text.Replace('.', ',');
                inputSale.SelectionStart = index;
            }
        }

        private void inputEnter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonConfirm_Click(sender, e);
            }
        }

        private string lastAutoCompleteName = "";
        private void inputName_Leave(object sender, EventArgs e)
        {
            if (!isnew || inputName.Text == lastAutoCompleteName)
                return;
            lastAutoCompleteName = inputName.Text;
            int number = inputName.AutoCompleteCustomSource.IndexOf(inputName.Text);
            if (number == -1)
                return;
            string[] temp = FormMain.stringsForProductSource(number);
            inputPrice.Text = temp[0];
            inputType.Text = temp[1];
            inputShop.Text = temp[2];
            inputSale.Text = temp[3];
        }

        private void checkBoxProcent_CheckedChanged(object sender, EventArgs e)
        {
            if (!float.TryParse(inputSale.Text, out float sale) || sale == 0)
            {
                sale = 0;
                inputSale.Text = sale.ToString();
            }
            else
            {
                float price = float.Parse(inputPrice.Text);
                if (checkBoxPercent.Checked)
                    sale = sale / price * 100;
                else
                    sale = sale / 100 * price;
                inputSale.Text = sale.ToString("f2");
            }
        }



    }
}
