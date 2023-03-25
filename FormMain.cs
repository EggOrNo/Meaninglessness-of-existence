using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading;

namespace MyDatabase
{
    public partial class FormMain : Form
    {
        private static SQLiteConnection dbConn;
        private SQLiteCommand sqlCmd;
        private SQLiteDataAdapter dbAdapter;
        private DataTable dTable;
        private static AutoCompleteStringCollection productSource;
        private static AutoCompleteStringCollection shopSource;
        private static List<ForProductSource> forProductSource;
        private string dbFileName;

        public static SQLiteConnection DBConn { get { return dbConn; } }
        public static AutoCompleteStringCollection ProductSource { get { return productSource; } }
        public static AutoCompleteStringCollection ShopSource { get { return shopSource; } }
        public static string[] stringsForProductSource(int number)
        {
            return forProductSource[number].returnStrings();
        }
        public class ForProductSource
        {
            string price;
            string type;
            string shop;
            string sale;
            public ForProductSource(string price, string type, string shop, string sale)
            {
                this.price = price;
                this.type = type;
                this.shop = shop;
                this.sale = sale;
            }
            public string[] returnStrings()
            {
                return new string[4] { price, type, shop, sale };
            }
        }

        private Random random;

        public FormMain()
        {
            InitializeComponent();
        }

        private void UpdateAutoCompleteSource()
        {
            productSource = new AutoCompleteStringCollection();
            shopSource = new AutoCompleteStringCollection();
            forProductSource = new List<ForProductSource>();

            sqlCmd.CommandText = "SELECT name, price, type, shop, sale FROM Products ORDER BY date DESC";
            using (SQLiteDataReader reader = sqlCmd.ExecuteReader())
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        string temp = reader.GetValue(0).ToString();
                        if (productSource.Contains(temp))
                            continue;
                        productSource.Add(temp);
                        forProductSource.Add(new ForProductSource(reader.GetValue(1).ToString(), reader.GetValue(2).ToString(),
                            reader.GetValue(3).ToString(), reader.GetValue(4).ToString()));
                    }
            sqlCmd.CommandText = "SELECT DISTINCT shop FROM Products";
            using (SQLiteDataReader reader = sqlCmd.ExecuteReader())
                if (reader.HasRows)
                    while (reader.Read())
                        shopSource.Add(reader.GetValue(0).ToString());

            textBoxGraphAdd.AutoCompleteCustomSource = productSource;
            // Сюда следует вставить поля для полей списков... Если всё же будут поля для фильтрации
        }

        private void UpdateUpdatesDropDownList()
        {
            comboBoxUpdatesOfVersion.Items.Clear();
            sqlCmd.CommandText = "SELECT DISTINCT version FROM Updates ORDER BY date DESC";
            using (SQLiteDataReader reader = sqlCmd.ExecuteReader())
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        comboBoxUpdatesOfVersion.Items.Add(reader.GetString(0));
                    }
            if (comboBoxUpdatesOfVersion.Items.Count > 0)
                comboBoxUpdatesOfVersion.SelectedIndex = 0;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            random = new Random();
            comboBoxWhereColumn.SelectedIndex = 0;
            
            dbConn = new SQLiteConnection();
            sqlCmd = new SQLiteCommand();
            dbFileName = "usersdata.db";

            dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
            dbConn.Open();
            sqlCmd.Connection = dbConn;
            UpdateAutoCompleteSource();
            UpdateUpdatesDropDownList();
            UpdateLastDate();
            radioButtonPeriod_CheckedChanged(sender, e);
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            dbConn.Close();
        }




        // ****************************************************************************************************
        // ЗДЕСЬ НАХОДЯТСЯ ФУНКЦИИ ПЕРВОЙ ВКЛАДКИ (ПРОДУКТЫ)
        // ****************************************************************************************************
        
        private bool isAutoChanging = false;

        private string GetCheckedTypes()
        {
            string result = string.Empty;
            if (checkBox1.Checked)
                result += "'0 Обяз.еда', ";
            if (checkBox2.Checked)
                result += "'1 Эксп.еда', ";
            if (checkBox3.Checked)
                result += "'2 К чаю', ";
            if (checkBox4.Checked)
                result += "'3 Вкусн.', ";
            if (checkBox5.Checked)
                result += "'4 Прочее', ";
            if (checkBox6.Checked)
                result += "'5 Трансп.', ";
            if (checkBox7.Checked)
                result += "'6 Одежда', ";
            if (checkBox8.Checked)
                result += "'7 Кафе', ";
            if (checkBox9.Checked)
                result += "'8 Техника', ";
            if (checkBox10.Checked)
                result += "'9 Развлеч.', ";
            return result == string.Empty ? result : result.Substring(0, result.Length - 2);
        }
        private void LoadProducts(string sqlWhere)
        {
            dTable = new DataTable();
            string sqlQuery = "SELECT name, price, number, ROUND((price-sale)*number,2) as value, type, date, " +
                $"shop, sale FROM Products WHERE type IN ({GetCheckedTypes()}) AND date BETWEEN " +
                $"'{dateTimePickerFrom.Value.ToString("yyyy.MM.dd")}' AND '{dateTimePickerTo.Value.ToString("yyyy.MM.dd")}'";
            if (sqlWhere != string.Empty)
                sqlQuery += " AND " + sqlWhere;
            sqlQuery += " ORDER BY date DESC, type ASC";
            try
            {
                dbAdapter = new SQLiteDataAdapter(sqlQuery, dbConn);
                dbAdapter.Fill(dTable);
                dgvMain.Rows.Clear();

                if (dTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        dgvMain.Rows.Add(dTable.Rows[i].ItemArray);
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            if (new FormNew().ShowDialog() == DialogResult.Cancel)
                return;
            updateSourcesAfterNew(sender, e);
        }
        private void pictureBoxQRcode_Click(object sender, EventArgs e)
        {
            if (new FormQR().ShowDialog() == DialogResult.Cancel)
                return;
            updateSourcesAfterNew(sender, e);
        }
        private void updateSourcesAfterNew(object sender, EventArgs e)
        {
            UpdateAutoCompleteSource();
            string lastlastDate = lastestDate;
            UpdateLastDate();
            if (DateTime.Parse(lastlastDate) < DateTime.Parse(lastestDate))
            {
                lastCheckedPeriod = string.Empty;
                radioButtonPeriod_CheckedChanged(sender, e);
            }
            else
                buttonFiltration_Click(sender, e);
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (checkBoxGrouping.Checked)
                editForGroup(sender, e);
            else
                editForOne(sender, e);
        }
        private void editForOne(object sender, EventArgs e)
        {
            if (new FormNew(dgvMain.CurrentRow.Cells).ShowDialog() == DialogResult.Cancel)
                return;
            UpdateAutoCompleteSource();
            string lastlastDate = lastestDate;
            UpdateLastDate();
            if (dateTimePickerFrom.Value == DateTime.Parse(lastlastDate) && DateTime.Parse(lastlastDate) < DateTime.Parse(lastestDate))
            {
                lastCheckedPeriod = string.Empty;
                radioButtonPeriod_CheckedChanged(sender, e);
            }
            else
                buttonFiltration_Click(sender, e);
        }
        private void editForGroup(object sender, EventArgs e)
        {
            isAutoChanging = true;
            string type = dgvMain.CurrentRow.Cells[2].Value.ToString();
            string date = dgvMain.CurrentRow.Cells[3].Value.ToString();

            DateTime datefrom, dateto;
            if (!DateTime.TryParse(date, out datefrom))
                if (date.Length > 0)
                    datefrom = new DateTime(int.Parse(date), 1, 1);
                else
                    datefrom = dateTimePickerFrom.Value;
            if (datefrom < dateTimePickerFrom.Value)
                datefrom = dateTimePickerFrom.Value;

            if (date.Length == 10)
                dateto = datefrom;
            else if (date.Length == 7)
                dateto = datefrom.AddMonths(1).AddDays(-1);
            else if (date.Length == 4)
                dateto = datefrom.AddYears(1).AddDays(-1);
            else
                dateto = dateTimePickerTo.Value;

            if (dateto > dateTimePickerTo.Value)
                dateto = dateTimePickerTo.Value;


            checkBoxGrouping.Checked = false;
            radioButtonPeriod.Checked = true;
            dateTimePickerFrom.Value = datefrom;
            dateTimePickerTo.Value = dateto;

            if (type != "все" && !checkBoxOnlyOne.Checked)
            {
                checkBoxOnlyOne.Checked = true;
                switch (type)
                {
                    case "1 Эксп.еда":
                        checkBox2.Checked = true;
                        break;
                    case "2 К чаю":
                        checkBox3.Checked = true;
                        break;
                    case "3 Вкусн.":
                        checkBox4.Checked = true;
                        break;
                    case "4 Прочее":
                        checkBox5.Checked = true;
                        break;
                    case "5 Трансп.":
                        checkBox6.Checked = true;
                        break;
                    case "6 Одежда":
                        checkBox7.Checked = true;
                        break;
                    case "7 Кафе":
                        checkBox8.Checked = true;
                        break;
                    case "8 Техника":
                        checkBox9.Checked = true;
                        break;
                    case "9 Развлеч.":
                        checkBox10.Checked = true;
                        break;
                    default:
                        checkBox1.Checked = true;
                        break;
                }
            }
            isAutoChanging = false;
            buttonFiltration_Click(sender, e);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (checkBoxGrouping.Checked)
                deleteForGroup(sender, e);
            else
                deleteForOne(sender, e);
        }

        private void deleteForOne(object sender, EventArgs e)
        {
            string name = dgvMain.CurrentRow.Cells[0].Value.ToString();
            string date = dgvMain.CurrentRow.Cells[5].Value.ToString();
            if (DialogResult.Cancel == MessageBox.Show($"Вы уверены, что хотите удалить \"{name}\" за {date}?", "Подтверждение", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
                return;

            sqlCmd.CommandText = "DELETE FROM Products WHERE name=@name AND date=@date";
            sqlCmd.Parameters.AddWithValue("@name", name);
            sqlCmd.Parameters.AddWithValue("@date", date);
            sqlCmd.ExecuteNonQuery();
            sqlCmd.Parameters.Clear();
            UpdateAutoCompleteSource();
            UpdateLastDate();
            if (dateTimePickerFrom.Value > DateTime.Parse(lastestDate))
            {
                lastCheckedPeriod = string.Empty;
                radioButtonPeriod_CheckedChanged(sender, e);
            }
            else
                buttonFiltration_Click(sender, e);
        }

        private void deleteForGroup(object sender, EventArgs e)
        {
            string num = dgvMain.CurrentRow.Cells[0].Value.ToString();
            string type = dgvMain.CurrentRow.Cells[2].Value.ToString();
            string date = dgvMain.CurrentRow.Cells[3].Value.ToString();
            if (DialogResult.Cancel == MessageBox.Show($"Вы уверены, что хотите удалить ГРУППУ продуктов из {num} строк(и) за {date}?", "Подтверждение", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) )
                return;

            DateTime datefrom, dateto;
            if (!DateTime.TryParse(date, out datefrom))
                if (date.Length > 0)
                    datefrom = new DateTime(int.Parse(date), 1, 1);
                else
                    datefrom = dateTimePickerFrom.Value;
            if (datefrom < dateTimePickerFrom.Value)
                datefrom = dateTimePickerFrom.Value;

            if (date.Length == 10)
                dateto = datefrom;
            else
            {
                if (date.Length == 7)
                    dateto = datefrom.AddMonths(1).AddDays(-1);
                else if (date.Length == 4)
                    dateto = datefrom.AddYears(1).AddDays(-1);
                else
                    dateto = dateTimePickerTo.Value;

                if (dateto > dateTimePickerTo.Value)
                    dateto = dateTimePickerTo.Value;
            }
            if (DialogResult.Cancel == MessageBox.Show($"Вы ТОЧНО уверены, что хотите удалить ГРУППУ продуктов из {num} строк(и) по типу \"{type}\" за даты от {datefrom.ToString("yyyy.MM.dd")} до {dateto.ToString("yyyy.MM.dd")}?", "Повторное подтверждение", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
                return;

            if (type=="все")
                sqlCmd.CommandText = "DELETE FROM Products WHERE date BETWEEN @datefrom AND @dateto";
            else
                sqlCmd.CommandText = "DELETE FROM Products WHERE type = @type AND date BETWEEN @datefrom AND @dateto ";
            sqlCmd.Parameters.AddWithValue("@datefrom", datefrom.ToString("yyyy.MM.dd"));
            sqlCmd.Parameters.AddWithValue("@dateto", dateto.ToString("yyyy.MM.dd"));
            sqlCmd.Parameters.AddWithValue("@type", type);
            sqlCmd.ExecuteNonQuery();
            sqlCmd.Parameters.Clear();
            UpdateAutoCompleteSource();
            UpdateLastDate();
            buttonFiltration_Click(sender, e);
        }

        private void UpdateLastDate()
        {
            sqlCmd.CommandText = "SELECT date FROM Products ORDER BY date DESC LIMIT 1";
            var temp = sqlCmd.ExecuteScalar();
            if (temp == null)
                lastestDate = DateTime.Today.ToString("yyyy.MM.dd");
            else
                lastestDate = temp.ToString();

            sqlCmd.CommandText = "SELECT date FROM Products ORDER BY date ASC LIMIT 1";
            temp = sqlCmd.ExecuteScalar();
            if (temp == null)
                earliestDate = DateTime.Today.ToString("yyyy.MM.dd");
            else
                earliestDate = temp.ToString();
        }
        private string earliestDate = string.Empty;
        private string lastestDate = string.Empty;
        private string lastCheckedPeriod = string.Empty;
        private void radioButtonPeriod_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonDay.Checked)
            {
                if (lastCheckedPeriod == "Day")
                    return;
                lastCheckedPeriod = "Day";
                dateTimePickerFrom.Value = DateTime.Parse(lastestDate);
                dateTimePickerTo.Value = dateTimePickerFrom.Value;
                dateTimePickerFrom.Enabled = true;
                dateTimePickerTo.Enabled = false;
            }
            else if (radioButtonMonth.Checked)
            {
                if (lastCheckedPeriod == "Month")
                    return;
                lastCheckedPeriod = "Month";
                DateTime date = DateTime.Parse(lastestDate);
                date = date.AddDays(1 - date.Day);
                dateTimePickerFrom.Value = date;
                dateTimePickerTo.Value = date.AddMonths(1).AddDays(-1);
                dateTimePickerFrom.Enabled = true;
                dateTimePickerTo.Enabled = false;
            }
            else if (radioButtonYear.Checked)
            {
                if (lastCheckedPeriod == "Year")
                    return;
                lastCheckedPeriod = "Year";
                int year = DateTime.Parse(lastestDate).Year;
                dateTimePickerFrom.Value = new DateTime(year, 1, 1);
                dateTimePickerTo.Value = new DateTime(year, 12, 31);
                dateTimePickerFrom.Enabled = true;
                dateTimePickerTo.Enabled = false;
            }
            else if (radioButtonAllTime.Checked)
            {
                if (lastCheckedPeriod == "AllTime")
                    return;
                lastCheckedPeriod = "AllTime";
                dateTimePickerFrom.Value = DateTime.Parse(earliestDate);
                dateTimePickerTo.Value = DateTime.Parse(lastestDate);
                dateTimePickerFrom.Enabled = false;
                dateTimePickerTo.Enabled = false;
            }
            else
            {
                lastCheckedPeriod = "Period";
                dateTimePickerFrom.Enabled = true;
                dateTimePickerTo.Enabled = true;
            }
            buttonFiltration_Click(sender, e);
        }

        private bool WhereColumnIsText() 
        { 
            return new string[] { "Название", "Тип", "Дата", "Магазин" }.Contains(comboBoxWhereColumn.Text);
        }
        private void comboBoxWhereColumn_TextChanged(object sender, EventArgs e)
        {
            comboBoxWhereOperation.Items.Clear();
            comboBoxWhereOperation.Items.Add("=");
            comboBoxWhereOperation.Items.Add("<>");
            if (WhereColumnIsText())
            {
                comboBoxWhereOperation.Items.Add("LIKE");
                comboBoxWhereOperation.SelectedIndex = 2; // Выбор "LIKE" по умолчанию
            }
            else
            {
                comboBoxWhereOperation.Items.Add(">");
                comboBoxWhereOperation.Items.Add("<");
                comboBoxWhereOperation.SelectedIndex = 0; // Выбор "=" по умолчанию
            }
            comboBoxWhereOperation.Items.Add("BETWEEN");
            comboBoxWhereOperation.Items.Add("IN");
        }
        private void comboBoxWhereOperation_TextChanged(object sender, EventArgs e)
        {
            textBoxWhere2.Hide();
            textBoxWhere.Size = new Size(210, 20);
            switch (comboBoxWhereOperation.Text)
            {
                case "LIKE":
                    labelWhereLeft.Text = "%";
                    labelWhereRight.Text = "%";
                    break;

                case "BETWEEN":
                    if (WhereColumnIsText())
                    {
                        labelWhereLeft.Text = "  '";
                        labelWhereRight.Text = "'";
                    }
                    else
                    {
                        labelWhereLeft.Text = "";
                        labelWhereRight.Text = "";
                    }
                    textBoxWhere.Size = new Size(90, 20);
                    textBoxWhere2.Show();
                    break;

                case "IN":
                    labelWhereLeft.Text = "  (";
                    labelWhereRight.Text = ")";
                    break;

                default:
                    if (WhereColumnIsText())
                    {
                        labelWhereLeft.Text = "  '";
                        labelWhereRight.Text = "'";
                    }
                    else
                    {
                        labelWhereLeft.Text = "";
                        labelWhereRight.Text = "";
                    }
                    break;
            }
        }
        private string GetFiltrationString()
        {
            string sqlWhere = textBoxWhere.Text.Trim();
            if (sqlWhere == string.Empty && comboBoxWhereOperation.Text == "=")
                sqlWhere = " ";
            if (sqlWhere != string.Empty)
            {
                var columsDict = new Dictionary<string, string>()
                {
                    {"Название", "name"},
                    {"Цена", "price"},
                    {"Кол-во", "number"},
                    {"Итог", "value"},
                    {"Тип", "type"},
                    {"Дата", "date"},
                    {"Магазин", "shop"},
                    {"Скидка", "sale"},
                };
                switch (comboBoxWhereOperation.Text)
                {
                    case "LIKE":
                        sqlWhere = $"{columsDict[comboBoxWhereColumn.Text]} LIKE '%{sqlWhere}%'";
                        break;

                    case "BETWEEN":
                        if (WhereColumnIsText())
                            sqlWhere = $"{columsDict[comboBoxWhereColumn.Text]} BETWEEN '{textBoxWhere.Text}'" +
                                $" AND '{textBoxWhere2.Text}'";
                        else
                            sqlWhere = $"{columsDict[comboBoxWhereColumn.Text]} BETWEEN {textBoxWhere.Text}" +
                                $" AND {textBoxWhere2.Text}";
                        break;

                    case "IN":
                        if (WhereColumnIsText())
                        {
                            string[] temp = sqlWhere.Split(',');
                            sqlWhere = string.Empty;
                            foreach (string line in temp)
                                sqlWhere += $"'{line.Trim()}',";
                            sqlWhere = sqlWhere.Substring(0, sqlWhere.Length - 1);
                        }
                        sqlWhere = $"{columsDict[comboBoxWhereColumn.Text]} IN ({sqlWhere})";
                        break;

                    default:
                        if (WhereColumnIsText())
                            sqlWhere = $"'{sqlWhere}'";
                        sqlWhere = $"{columsDict[comboBoxWhereColumn.Text]} {comboBoxWhereOperation.Text} {sqlWhere}";
                        break;
                }
            }
            return sqlWhere;
        }
        private void buttonFiltration_Click(object sender, EventArgs e)
        {
            if (isAutoChanging)
                return;
            if (!checkBoxGrouping.Checked)
            {
                LoadProducts(GetFiltrationString());
            }
            else if (radioButtonOnDays.Checked)
            {
                LoadProductGroups(10, GetFiltrationString());
            }
            else if (radioButtonOnMonths.Checked)
            {
                LoadProductGroups(7, GetFiltrationString());
            }
            else if (radioButtonOnYears.Checked)
            {
                LoadProductGroups(4, GetFiltrationString());
            }
            else
            {
                LoadProductGroups(0, GetFiltrationString());
            }
            labelNumRows.Text = dTable.Rows.Count.ToString();
        }

        private void buttonResetFiltration_Click(object sender, EventArgs e)
        {
            textBoxWhere.Text = string.Empty;
            textBoxWhere2.Text = string.Empty;
            comboBoxWhereColumn.SelectedIndex = 0;
            checkBoxGrouping.Checked = false;
            checkBoxAllGroup.Checked = true;
            if (checkBoxOnlyOne.Checked)
                checkBoxOnlyOne.Checked = false;
            else
                checkBoxOnlyOne_CheckedChanged(sender, e);
        }

        private void LoadProductGroups(int dateType, string sqlWhere)
        {
            string type = "type";
            if (checkBoxAllGroup.Checked)
                type = "'все'";
            dTable = new DataTable();
            string sqlQuery = $"SELECT SUM(CEILING(number)), ROUND(SUM((price-sale)*number),2) as value, {type}, " +
                $"SUBSTR(date, 1, {dateType}) as newdate, SUM(sale) FROM Products WHERE " +
                $"type IN ({GetCheckedTypes()}) AND date BETWEEN " +
                $"'{dateTimePickerFrom.Value.ToString("yyyy.MM.dd")}' AND '{dateTimePickerTo.Value.ToString("yyyy.MM.dd")}'";
            if (sqlWhere != string.Empty)
                sqlQuery += " AND " + sqlWhere;

            if (checkBoxAllGroup.Checked)
                sqlQuery += " GROUP BY newdate ORDER BY newdate DESC";
            else
                sqlQuery += " GROUP BY type, newdate ORDER BY newdate DESC, type ASC";
            
            try
            {
                dbAdapter = new SQLiteDataAdapter(sqlQuery, dbConn);
                dbAdapter.Fill(dTable);
                dgvMain.Rows.Clear();

                if (dTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        dgvMain.Rows.Add(dTable.Rows[i].ItemArray);
                    }
                }
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void textBoxWhere_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonFiltration_Click(sender, e);
            }
        }

        private void dateTimePickerFrom_ValueChanged(object sender, EventArgs e)
        {
            if (radioButtonDay.Checked)
            {
                dateTimePickerTo.Value = dateTimePickerFrom.Value.AddHours(1);
            }
            else if (radioButtonMonth.Checked)
            {
                dateTimePickerTo.Value = dateTimePickerFrom.Value.AddMonths(1).AddDays(-1);
            }
            else if (radioButtonYear.Checked)
            {
                dateTimePickerTo.Value = dateTimePickerFrom.Value.AddYears(1).AddDays(-1);
            }
            buttonFiltration_Click(sender, e);
        }

        private void checkBoxGrouping_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxGrouping.Enabled = checkBoxGrouping.Checked;
            dgvMain.Columns.Clear();
            if (checkBoxGrouping.Checked)
            {
                dgvMain.Columns.AddRange(new DataGridViewColumn[] {
                    dgvColumnCatNumber,
                    dgvColumnCatValue,
                    dgvColumnCatType,
                    dgvColumnCatDate,
                    dgvColumnCatSale});
                buttonEdit.Text = "Раскрыть";
            }
            else
            {
                dgvMain.Columns.AddRange(new DataGridViewColumn[] {
                    dgvColumnName,
                    dgvColumnPrice,
                    dgvColumnNumber,
                    dgvColumnValue,
                    dgvColumnType,
                    dgvColumnDate,
                    dgvColumnShop,
                    dgvColumnSale});
                buttonEdit.Text = "Изменить";
            }
            buttonFiltration_Click(sender, e);
        }

        private string lastCheckedGroup = string.Empty;
        private void radioButtonGrouping_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonOnDays.Checked)
            {
                if (lastCheckedGroup == "Days")
                    return;
                lastCheckedGroup = "Days";
            }
            else if (radioButtonOnMonths.Checked)
            {
                if (lastCheckedGroup == "Months")
                    return;
                lastCheckedGroup = "Months";
            }
            else if (radioButtonOnYears.Checked)
            {
                if (lastCheckedGroup == "Years")
                    return;
                lastCheckedGroup = "Years";
            }
            else
                lastCheckedGroup = "AllTime";
            buttonFiltration_Click(sender, e);
        }

        private void checkBoxTypes_CheckedChanged(object sender, EventArgs e)
        {
            if (onlyOneInProcess)
                return;
            if (checkBoxOnlyOne.Checked)
            {
                checkBoxOnlyOne_CheckedChanged(sender, e);
                return;
            }
            if ( !(checkBox1.Checked || checkBox2.Checked || checkBox3.Checked || checkBox4.Checked || checkBox5.Checked
                || checkBox6.Checked || checkBox7.Checked || checkBox8.Checked || checkBox9.Checked || checkBox10.Checked) )
                checkBox1.Checked = true;
            buttonFiltration_Click(sender, e);
        }

        private bool onlyOneInProcess = false;
        private CheckBox lastCheckBox = null;
        private void checkBoxOnlyOne_CheckedChanged(object sender, EventArgs e)
        {
            onlyOneInProcess = true;
            CheckBox[] checkBoxes = {checkBox1, checkBox2, checkBox3, checkBox4, checkBox5, checkBox6,
                checkBox7, checkBox8, checkBox9, checkBox10};

            if (checkBoxOnlyOne.Checked && lastCheckBox == null)
            {
                foreach (CheckBox checkBox in checkBoxes)
                    checkBox.Checked = false;

                checkBoxes[0].Checked = true;
                lastCheckBox = checkBoxes[0];
            }
            else if (checkBoxOnlyOne.Checked)
            {
                foreach (CheckBox checkBox in checkBoxes)
                    if (checkBox.Checked && checkBox == lastCheckBox)
                    {
                        checkBox.Checked = false;
                        break;
                    }

                if (checkBoxes.Any(checkBox => checkBox.Checked))
                {
                    foreach (CheckBox checkBox in checkBoxes)
                        if (checkBox.Checked)
                        {
                            lastCheckBox = checkBox;
                            break;
                        }
                }
                else
                {
                    lastCheckBox.Checked = true;
                }
            }
            else
            {
                foreach (CheckBox checkBox in checkBoxes)
                    checkBox.Checked = true;
                lastCheckBox = null;
            }
            buttonFiltration_Click(sender, e);
            onlyOneInProcess = false;
        }

        private void dgvMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonEdit_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Delete)
            {
                buttonDelete_Click(sender, e);
            }
        }

        


        // ****************************************************************************************************
        // ЗДЕСЬ НАХОДЯТСЯ ФУНКЦИИ ВТОРОЙ ВКЛАДКИ (СТАТИСТИКА)
        // ****************************************************************************************************

        private void buttonGraphColor_Click(object sender, EventArgs e)
        {
            if (!radioButtonGraphProducts.Checked)
                return;
            if (colorDialogGraph.ShowDialog() == DialogResult.Cancel)
                return;
            buttonGraphColor.ForeColor = colorDialogGraph.Color;
        }

        private void buttonGraphAdd_Click(object sender, EventArgs e)
        {
            if (!radioButtonGraphProducts.Checked)
                return;
            if (chartMain.Series.Any(t => t.Name == textBoxGraphAdd.Text))
            {
                MessageBox.Show("Введённый продукт уже присутствует на графике.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!productSource.Contains(textBoxGraphAdd.Text))
            {
                MessageBox.Show("Введённый продукт отсутствует в базе данных.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Series newSeries = new Series(textBoxGraphAdd.Text)
            {
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.Date,
                YValueType = ChartValueType.Double,
                BorderWidth = 3,
                ChartArea = "ChartAreaMain",
                MarkerSize = 10,
                MarkerStyle = MarkerStyle.Circle,
                MarkerBorderWidth = 1,
                //MarkerBorderColor = Color.DarkGray,
            };
            if (buttonGraphColor.ForeColor != Color.Black &&
                chartMain.Series.All(t => t.Color != buttonGraphColor.ForeColor))
            {
                newSeries.Color = buttonGraphColor.ForeColor;
                newSeries.MarkerColor = buttonGraphColor.ForeColor;
            }

            sqlCmd.CommandText = @"SELECT name, date, price, (price-sale) as value FROM Products WHERE name=@name ORDER BY date";
            sqlCmd.Parameters.AddWithValue("@name", textBoxGraphAdd.Text);
            using (SQLiteDataReader reader = sqlCmd.ExecuteReader())
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        newSeries.Points.AddXY(
                            DateTime.Parse(reader.GetValue(1).ToString()), 
                            double.Parse(reader.GetValue(2).ToString())
                            );
                    }
                }
                else
                {
                    MessageBox.Show("По неизвестным причинам введённый продукт отсутствует в базе данных.", "Ошибка ввода?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            sqlCmd.Parameters.Clear();
            chartMain.Series.Add(newSeries);
            buttonGraphColor.ForeColor = Color.Black;
            textBoxGraphChoice.Items.Add(textBoxGraphAdd.Text);
            textBoxGraphChoice.SelectedIndex = textBoxGraphChoice.Items.Count - 1;
        }

        private void buttonGraphDelete_Click(object sender, EventArgs e)
        {
            if (!radioButtonGraphProducts.Checked)
                return;
            if (textBoxGraphChoice.Text == string.Empty)
            {
                MessageBox.Show("Введённый продукт отсутствует в списке графиков.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            chartMain.Series.Remove(chartMain.Series.FindByName(textBoxGraphChoice.Text));
            textBoxGraphChoice.Items.Remove(textBoxGraphChoice.Text);
            textBoxGraphChoice.SelectedIndex = textBoxGraphChoice.Items.Count - 1;
        }

        private void textBoxGraphAdd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonGraphAdd_Click(sender, e);
            }
        }
        private void textBoxGraphChoice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonGraphDelete_Click(sender, e);
            }
        }

        private void chartMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (chartMain.Series.Count == 0)
                return;
            chartMain.ChartAreas[0].CursorX.SetCursorPixelPosition(new Point(e.X, e.Y), true);
            chartMain.ChartAreas[0].CursorY.SetCursorPixelPosition(new Point(e.X, e.Y), true);

            labelPositionX.Text = new DateTime(1900, 1, 1).AddDays(chartMain.ChartAreas[0].CursorX.Position).ToString("d");
            labelPositionY.Text = chartMain.ChartAreas[0].CursorY.Position.ToString();
        }

        private Series[] lastGraphSeries = null;
        private bool graphCategoryIsUsed = false;
        private void radioButtonGraphProducts_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonGraphProducts.Checked)
            {
                panelGraphProducts.Visible = true;
                panelGraphCategory.Visible = false;
                panelGraphPeriod.Visible = false;
                chartMain.Series.Clear();
                if (lastGraphSeries == null)
                    return;
                foreach (Series series in lastGraphSeries)
                    chartMain.Series.Add(series);
                lastGraphSeries = null;
            }
            else
            {
                panelGraphProducts.Visible = false;
                panelGraphCategory.Visible = true;
                panelGraphPeriod.Visible = true;
                if (!graphCategoryIsUsed)
                {
                    dateTimePickerGraphFrom.Value = DateTime.Parse(earliestDate);
                    dateTimePickerGraphTo.Value = DateTime.Parse(lastestDate);
                    graphCategoryIsUsed = true;
                }
                if (chartMain.Series.Count > 0)
                {
                    lastGraphSeries = new Series[chartMain.Series.Count];
                    chartMain.Series.CopyTo(lastGraphSeries, 0);
                }
                chartMain.Series.Clear();
                CreateCategoryGraph();
            }
        }

        private string GetCheckedGraphTypes()
        {
            string result = string.Empty;
            if (checkBoxGraph1.Checked)
                result += "'0 Обяз.еда', ";
            if (checkBoxGraph2.Checked)
                result += "'1 Эксп.еда', ";
            if (checkBoxGraph3.Checked)
                result += "'2 К чаю', ";
            if (checkBoxGraph4.Checked)
                result += "'3 Вкусн.', ";
            if (checkBoxGraph5.Checked)
                result += "'4 Прочее', ";
            if (checkBoxGraph6.Checked)
                result += "'5 Трансп.', ";
            if (checkBoxGraph7.Checked)
                result += "'6 Одежда', ";
            if (checkBoxGraph8.Checked)
                result += "'7 Кафе', ";
            if (checkBoxGraph9.Checked)
                result += "'8 Техника', ";
            if (checkBoxGraph10.Checked)
                result += "'9 Развлеч.', ";
            return result == string.Empty ? result : result.Substring(0, result.Length - 2);
        }

        private void CreateCategoryGraph()
        {
            if (!graphCategoryIsUsed)
                return;
            chartMain.Series.Clear();
            int datetype = radioButtonGraphDay.Checked ? 10 : radioButtonGraphMonth.Checked ? 7 : 4;
            string checkedtypes = GetCheckedGraphTypes();
            string command = string.Empty;

            if (checkedtypes != string.Empty)
            {
                command = $"SELECT ROUND(SUM((price-sale)*number), 2) as value, type, " +
                    $"SUBSTR(date, 1, {datetype}) as newdate FROM Products WHERE " +
                    $"type IN ({checkedtypes}) AND date BETWEEN " +
                    $"'{dateTimePickerGraphFrom.Value.ToString("yyyy.MM.dd")}' AND '{dateTimePickerGraphTo.Value.ToString("yyyy.MM.dd")}'" +
                    $" GROUP BY type, newdate";
            }
            if (checkBoxGraphAll.Checked)
            {
                if (command != string.Empty)
                    command += " UNION ALL ";
                command += $"SELECT ROUND(SUM((price-sale)*number),2) as value, 'все' as type, " +
                    $"SUBSTR(date, 1, {datetype}) as newdate FROM Products WHERE date BETWEEN " +
                    $"'{dateTimePickerGraphFrom.Value.ToString("yyyy.MM.dd")}' AND '{dateTimePickerGraphTo.Value.ToString("yyyy.MM.dd")}'" +
                    $" GROUP BY newdate";
            }
            if (command == string.Empty)
                return;

            Series newSeries = null;
            sqlCmd.CommandText = $"{command} ORDER BY type ASC, newdate DESC";
            using (SQLiteDataReader reader = sqlCmd.ExecuteReader())
                if (reader.HasRows)
                {
                    string lastSeries = string.Empty;
                    while (reader.Read())
                    {
                        double sum = double.Parse(reader.GetValue(0).ToString());
                        string type = reader.GetValue(1).ToString();
                        if (!DateTime.TryParse(reader.GetValue(2).ToString(), out DateTime date))
                            date = new DateTime(int.Parse(reader.GetValue(2).ToString()), 1, 1);
                        if (lastSeries != type)
                        {
                            lastSeries = type;
                            if (newSeries != null)
                                chartMain.Series.Add(newSeries);
                            Color color = ColorForCategoryGraph(type);
                            newSeries = new Series(type)
                            {
                                ChartType = SeriesChartType.Line,
                                XValueType = ChartValueType.Date,
                                YValueType = ChartValueType.Double,
                                BorderWidth = 3,
                                ChartArea = "ChartAreaMain",
                                MarkerSize = 10,
                                MarkerStyle = MarkerStyle.Circle,
                                MarkerBorderWidth = 1,
                                Color = color,
                                MarkerColor = color,
                            };
                        }
                        newSeries.Points.AddXY(date, sum);
                    }
                    chartMain.Series.Add(newSeries);
                }
                else
                {
                    MessageBox.Show("По неизвестным причинам введённый продукт отсутствует в базе данных.", "Ошибка ввода?", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

        }

        private Color ColorForCategoryGraph(string type)
        {
            switch (type)
            {
                case "0 Обяз.еда":
                    return Color.Red;
                case "1 Эксп.еда":
                    return Color.DarkGoldenrod;
                case "2 К чаю":
                    return Color.Green;
                case "3 Вкусн.":
                    return Color.Aqua;
                case "4 Прочее":
                    return Color.Gray;
                case "5 Трансп.":
                    return Color.Orange;
                case "6 Одежда":
                    return Color.Brown;
                case "7 Кафе":
                    return Color.Purple;
                case "8 Техника":
                    return Color.Blue;
                case "9 Развлеч.":
                    return Color.DarkGray;
                case "все":
                    return Color.Black;
                default:
                    return Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
            }
        }

        private void checkBoxGraph_CheckedChanged(object sender, EventArgs e)
        {
            CreateCategoryGraph();
        }

        private RadioButton lastRadioButtonGraph = null;
        private void radioButtonGraph_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonGraphDay.Checked)
            {
                if (lastRadioButtonGraph == radioButtonGraphDay)
                    return;
                lastRadioButtonGraph = radioButtonGraphDay;
            }
            else if (radioButtonGraphMonth.Checked)
            {
                if (lastRadioButtonGraph == radioButtonGraphMonth)
                    return;
                lastRadioButtonGraph = radioButtonGraphMonth;
            }
            else if (radioButtonGraphYear.Checked)
            {
                if (lastRadioButtonGraph == radioButtonGraphYear)
                    return;
                lastRadioButtonGraph = radioButtonGraphYear;
            }
            else
                MessageBox.Show("Что-то действительно пошло не так.", "Что-то пошло не так", MessageBoxButtons.OK, MessageBoxIcon.Error);
            CreateCategoryGraph();
        }

        private void dateTimePickerGraphFrom_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value > dateTimePickerGraphTo.Value)
                dateTimePickerFrom.Value = dateTimePickerGraphTo.Value;
            CreateCategoryGraph();
        }

        private void dateTimePickerGraphTo_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value > dateTimePickerGraphTo.Value)
                dateTimePickerTo.Value = dateTimePickerGraphFrom.Value;
            CreateCategoryGraph();
        }




        // ****************************************************************************************************
        // ЗДЕСЬ НАХОДЯТСЯ ФУНКЦИИ ТРЕТЬЕЙ ВКЛАДКИ (АДМИНИСТРИРОВАНИЕ)
        // ****************************************************************************************************

        private void buttonSQL_Click(object sender, EventArgs e)
        {
            int number = 0;
            labelSQL.Enabled = false;
            if (textBoxSQL.Text.ToUpper().StartsWith("SELECT"))
            {
                dbAdapter = new SQLiteDataAdapter(textBoxSQL.Text, dbConn);
                DataSet ds = new DataSet();
                dbAdapter.Fill(ds);
                dataGridViewAdmin.DataSource = ds.Tables[0];
                number = ds.Tables[0].Rows.Count;
            }
            else if ( textBoxSQL.Text.ToUpper().StartsWith("INSERT") || textBoxSQL.Text.ToUpper().StartsWith("UPDATE") 
                        || textBoxSQL.Text.ToUpper().StartsWith("DELETE") )
            {
                sqlCmd.CommandText = textBoxSQL.Text;
                number = sqlCmd.ExecuteNonQuery();
                if (textBoxSQL.Text.Contains("Updates"))
                    UpdateUpdatesDropDownList();
            }
            else
            {
                MessageBox.Show("Введена неизвестная команда.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Thread.Sleep(100);
            labelSQL.Enabled = true;
            labelSQL.Text = $"Затронуто {number} строк";
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            Excel.Application ex = new Excel.Application();
            Excel.Workbook workbook;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                ofd.FilterIndex = 1;
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }
                string filePath = ofd.FileName;
                workbook = ex.Workbooks.Open(filePath);
            }

            ImportingData(workbook);

            CloseExcel(ex);
            UpdateLastDate();
            UpdateUpdatesDropDownList();
            this.Activate();
        }

        private void buttonImportWithRemove_Click(object sender, EventArgs e)
        {
            Excel.Application ex = new Excel.Application();
            ex.DisplayAlerts = false;
            Excel.Workbook workbook;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                ofd.FilterIndex = 1;
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }
                string filePath = ofd.FileName;
                workbook = ex.Workbooks.Open(filePath);
            }

            sqlCmd.CommandText = "DELETE FROM Products";
            sqlCmd.ExecuteNonQuery();
            sqlCmd.CommandText = "DELETE FROM Updates";
            sqlCmd.ExecuteNonQuery();
            sqlCmd.CommandText = "DELETE FROM ShopChain";
            sqlCmd.ExecuteNonQuery();

            ImportingData(workbook);

            CloseExcel(ex);
            UpdateLastDate();
            UpdateUpdatesDropDownList();
            this.Activate();
        }

        private void ImportingData(Excel.Workbook workbook)
        {
            TimeSpan time = new TimeSpan(DateTime.Now.Ticks);
            Excel.Worksheet sheet1 = workbook.Worksheets.get_Item(1);
            Excel.Worksheet sheet2 = workbook.Worksheets.get_Item(2);
            Excel.Worksheet sheet3 = workbook.Worksheets.get_Item(3);

            int findMax(Excel.Worksheet sheet)
            {
                int maxrow;
                for (maxrow = 16384; sheet.Cells[maxrow, 1].Value != null; maxrow += 16384) ;
                for (int step = 8192; step > 0; step /= 2)
                    if (sheet.Cells[maxrow, 1].Value != null)
                    {
                        if (sheet.Cells[maxrow + 1, 1].Value == null)
                            break;
                        maxrow += step;
                    }
                    else
                        maxrow -= step;
                return maxrow;
            }
            int maxrow1 = findMax(sheet1);
            int maxrow2 = findMax(sheet2);
            int maxrow3 = findMax(sheet3);


            string commandtext = "INSERT INTO Products (name, price, number, sale, type, date, shop) VALUES";
            int maxvalue = maxrow1 + maxrow2 + maxrow3 - 3;
            labelProgressBar.Text = $"{0} / {maxvalue}";
            labelProgressBar.ForeColor = Color.Black;
            labelProgressBar.Visible = true;
            int forstep = 102;
            for (int i = 2; i <= maxrow1; i++)
            {
                if (i == forstep)
                {
                    labelProgressBar.Text = $"{forstep - 2} / {maxvalue}";
                    forstep += 100;
                }
                sqlCmd.Parameters.AddWithValue($"@date{i}", sheet1.Cells[i, 1].Value);
                sqlCmd.Parameters.AddWithValue($"@type{i}", sheet1.Cells[i, 2].Value);
                sqlCmd.Parameters.AddWithValue($"@name{i}", sheet1.Cells[i, 3].Value);
                sqlCmd.Parameters.AddWithValue($"@price{i}", sheet1.Cells[i, 4].Value);
                sqlCmd.Parameters.AddWithValue($"@number{i}", sheet1.Cells[i, 5].Value);
                sqlCmd.Parameters.AddWithValue($"@sale{i}", sheet1.Cells[i, 6].Value);
                sqlCmd.Parameters.AddWithValue($"@shop{i}", sheet1.Cells[i, 7].Value);
                commandtext += $" (@name{i}, @price{i}, @number{i}, @sale{i}, @type{i}, @date{i}, @shop{i}),";
            }
            sqlCmd.CommandText = commandtext.TrimEnd(new char[] { ',' });
            sqlCmd.ExecuteNonQuery();
            sqlCmd.Parameters.Clear();


            commandtext = "INSERT INTO Updates (version, date, text) VALUES";
            for (int i = 2; i <= maxrow2; i++)
            {
                if (i + maxrow1 - 1 == forstep)
                {
                    labelProgressBar.Text = $"{forstep - 2} / {maxvalue}";
                    forstep += 100;
                }
                sqlCmd.Parameters.AddWithValue($"@version{i}", sheet2.Cells[i, 1].Value);
                sqlCmd.Parameters.AddWithValue($"@date{i}", sheet2.Cells[i, 2].Value);
                sqlCmd.Parameters.AddWithValue($"@text{i}", sheet2.Cells[i, 3].Value);
                commandtext += $" (@version{i}, @date{i}, @text{i}),";
            }
            sqlCmd.CommandText = commandtext.TrimEnd(new char[] { ',' });
            sqlCmd.ExecuteNonQuery();
            sqlCmd.Parameters.Clear();


            commandtext = "INSERT INTO ShopChain (name_place, my_name) VALUES";
            for (int i = 2; i <= maxrow3; i++)
            {
                if (i + maxrow1 + maxrow2 - 2 == forstep)
                {
                    labelProgressBar.Text = $"{forstep - 2} / {maxvalue}";
                    forstep += 100;
                }
                sqlCmd.Parameters.AddWithValue($"@name_place{i}", sheet3.Cells[i, 1].Value);
                sqlCmd.Parameters.AddWithValue($"@my_name{i}", sheet3.Cells[i, 2].Value);
                commandtext += $" (@name_place{i}, @my_name{i}),";
            }

            labelProgressBar.Text = $"{maxvalue} / {maxvalue}";
            labelProgressBar.ForeColor = Color.Green;

            sqlCmd.CommandText = commandtext.TrimEnd(new char[] { ',' });
            sqlCmd.ExecuteNonQuery();
            sqlCmd.Parameters.Clear();

            MessageBox.Show($"Было вынесено несколько строк - {maxvalue}.\nВремя выполнения - {new TimeSpan(DateTime.Now.Ticks) - time}.",
                "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            labelProgressBar.Visible = false;
            labelProgressBar.ForeColor = Color.Black;
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            string filePath;
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.FileName = $"Продукты {DateTime.Today.ToString("yyyy.MM.dd")}";
                sfd.Filter = "xlsx files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                sfd.FilterIndex = 1;
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }
                filePath = sfd.FileName;
            }

            TimeSpan time = new TimeSpan(DateTime.Now.Ticks);

            sqlCmd.CommandText = "SELECT COUNT(name) FROM Products LIMIT 1";
            int maxrow1 = int.Parse(sqlCmd.ExecuteScalar().ToString());
            sqlCmd.CommandText = "SELECT COUNT(version) FROM Updates LIMIT 1";
            int maxrow2 = int.Parse(sqlCmd.ExecuteScalar().ToString());
            sqlCmd.CommandText = "SELECT COUNT(name_place) FROM ShopChain LIMIT 1";
            int maxrow3 = int.Parse(sqlCmd.ExecuteScalar().ToString());
            int maxvalue = maxrow1 + maxrow2 + maxrow3;

            Excel.Application ex = new Excel.Application();
            ex.SheetsInNewWorkbook = 3;
            Excel.Workbook workBook = ex.Workbooks.Add();
            ex.DisplayAlerts = false;

            Excel.Worksheet sheet = ex.Worksheets.get_Item(1);
            sheet.Name = "Products";
            sheet.Cells[1, 1] = "Дата";
            sheet.Cells[1, 2] = "Тип";
            sheet.Cells[1, 3] = "Название";
            sheet.Cells[1, 4] = "Цена";
            sheet.Cells[1, 5] = "Кол-во";
            sheet.Cells[1, 6] = "Скидка";
            sheet.Cells[1, 7] = "Магазин";
            sqlCmd.CommandText = "SELECT date, type, name, price, number, sale, shop FROM Products";

            labelProgressBar.Text = $"{0} / {maxvalue}";
            labelProgressBar.ForeColor = Color.Black;
            labelProgressBar.Visible = true;
            int rownum;
            int forstep = 102;
            using (SQLiteDataReader reader = sqlCmd.ExecuteReader())
            {
                for (rownum = 2; reader.Read(); rownum++)
                {
                    if (rownum == forstep)
                    {
                        labelProgressBar.Text = $"{forstep - 2} / {maxvalue}";
                        forstep += 100;
                    }
                    for (int j = 0; j < 7; j++)
                        sheet.Cells[rownum, j + 1] = reader.GetValue(j);
                }
            }

            sheet = ex.Worksheets.get_Item(2);
            sheet.Name = "Updates";
            sheet.Cells[1, 1] = "Версия";
            sheet.Cells[1, 2] = "Дата";
            sheet.Cells[1, 3] = "Описание";
            sqlCmd.CommandText = "SELECT version, date, text FROM Updates";
            using (SQLiteDataReader reader = sqlCmd.ExecuteReader())
            {
                for (int i = 2; reader.Read(); i++)
                {
                    if (i + maxrow1 - 1 == forstep)
                    {
                        labelProgressBar.Text = $"{forstep - 2} / {maxvalue}";
                        forstep += 100;
                    }
                    for (int j = 0; j < 3; j++)
                        sheet.Cells[i, j + 1] = reader.GetValue(j);
                }
            }

            sheet = ex.Worksheets.get_Item(3);
            sheet.Name = "ShopChain";
            sheet.Cells[1, 1] = "name_place";
            sheet.Cells[1, 2] = "my_name";
            sqlCmd.CommandText = "SELECT name_place, my_name FROM ShopChain";
            using (SQLiteDataReader reader = sqlCmd.ExecuteReader())
            {
                for (int i = 2; reader.Read(); i++)
                {
                    if (i + maxrow1 + maxrow2 - 2 == forstep)
                    {
                        labelProgressBar.Text = $"{forstep - 2} / {maxvalue}";
                        forstep += 100;
                    }
                    for (int j = 0; j < 2; j++)
                        sheet.Cells[i, j + 1] = reader.GetValue(j);
                }
            }

            labelProgressBar.Text = $"{maxvalue} / {maxvalue}";
            labelProgressBar.ForeColor = Color.Green;

            try
            {
                ex.ActiveWorkbook.SaveAs(filePath, Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                    false, false, Excel.XlSaveAsAccessMode.xlNoChange, Excel.XlSaveConflictResolution.xlLocalSessionChanges);
            }
            catch
            {
                MessageBox.Show($"Экспорт данных был прерван.", "Отмена!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CloseExcel(ex);
                return;
            }
            CloseExcel(ex);

            MessageBox.Show($"Было вынесено несколько строк - {maxvalue}.\nВремя выполнения - {new TimeSpan(DateTime.Now.Ticks) - time}.",
                "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            labelProgressBar.Visible = false;
            labelProgressBar.ForeColor = Color.Black;
            this.Activate();
        }
        private void CloseExcel(Excel.Application ex)
        {
            ex.Workbooks.Close();
            ex.Quit();
            Process[] List = Process.GetProcessesByName("EXCEL");
            foreach (Process proc in List)
                proc.Kill();
        }

        private void buttonSelectUpdateDate_Click(object sender, EventArgs e)
        {
            sqlCmd.Parameters.AddWithValue("@version", comboBoxUpdatesOfVersion.Text);
            sqlCmd.CommandText = @"SELECT date FROM Updates WHERE version=@version ORDER BY date ASC LIMIT 1";
            string mindate = sqlCmd.ExecuteScalar().ToString();
            sqlCmd.CommandText = @"SELECT date FROM Updates WHERE version=@version ORDER BY date DESC LIMIT 1";
            string maxdate = sqlCmd.ExecuteScalar().ToString();
            string dates;
            if (mindate == maxdate)
                dates = mindate;
            else
                dates = $"{mindate} - {maxdate}";

            sqlCmd.CommandText = @"SELECT text FROM Updates WHERE version=@version";
            string updates = $"Обновление v{comboBoxUpdatesOfVersion.Text} ({dates}):\n";
            using (SQLiteDataReader reader = sqlCmd.ExecuteReader())
                if (reader.HasRows)
                    while (reader.Read())
                    {
                        updates += $" - {reader.GetString(0)};\n";
                    }
            sqlCmd.Parameters.Clear();
            MessageBox.Show($"{updates.Substring(0, updates.Length - 2)}.", "Описание обновлений",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonAddUpdate_Click(object sender, EventArgs e)
        {
            textBoxSQL.Text = $"INSERT INTO Updates (version, text, date) VALUES" +
                $" ('{comboBoxUpdatesOfVersion.Items[0]}', ' text ', '{DateTime.Today.ToString("yyyy.MM.dd")}')";
        }



    }
}