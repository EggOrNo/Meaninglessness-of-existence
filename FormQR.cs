using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using ZXing;
using AForge.Video;
using AForge.Video.DirectShow;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

using System.Data.SQLite;
using System.Linq;
using System.IO;
using System.Threading;

namespace MyDatabase
{
    public partial class FormQR : Form
    {

        private SQLiteConnection dbConn;
        public FormQR()
        {
            InitializeComponent();
            dbConn = FormMain.DBConn;
        }

        private void FormQR_Load(object sender, EventArgs e)
        {
            UploadCameraList();
            dateTimePicker.Value = DateTime.Today;
        }

        private void FormQR_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoCaptureDevice != null && videoCaptureDevice.IsRunning)
                videoCaptureDevice.Stop();
            if (driver != null)
                driver.Quit();
        }

        private void buttonManualInput_Click(object sender, EventArgs e)
        {
            FormReceipt form;
            if (textBoxQRcode.Text.Length < 20 || textBoxQRcode.Text.Split('&').Length < 6)
            {
                form = new FormReceipt();
            }
            else
            {
                form = new FormReceipt(textBoxQRcode.Text);
            }
            if (DialogResult.Cancel == form.ShowDialog())
                return;
            textBoxQRcode.Text = FormReceipt.QRCode;
            buttonSearch_Click(sender, e);
        }

        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;
        private void UploadCameraList()
        {
            comboBoxCamera.Items.Clear();
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
                comboBoxCamera.Items.Add(filterInfo.Name);
            if (comboBoxCamera.Items.Count > 0)
                comboBoxCamera.SelectedIndex = 0;
            else
                comboBoxCamera.Text = string.Empty;
        }

        private void buttonCameraUpload_Click(object sender, EventArgs e)
        {
            if (!timerAnimationCamera.Enabled)
            {
                timerAnimationCamera.Interval = periodUpdate;
                timerAnimationCamera.Start();
            }
            UploadCameraList();
        }
        private int currenttime = 0;
        private int periodUpdate = 20;
        private int maxtime = 600;
        private void timerAnimation_Tick(object sender, EventArgs e)
        {
            currenttime += periodUpdate;
            if (currenttime > maxtime)
            {
                currenttime = 0;
                timerAnimationCamera.Stop();
            }
            Image img = Properties.Resources.reload;
            float newangle = 360f / (float)Math.Pow(maxtime, 3) * (float)Math.Pow(currenttime, 2) * (3 * maxtime - 2 * currenttime);
            buttonCameraUpload.Image = RotateImage(img, new PointF(img.Width / 2f, img.Height / 2f), newangle);
            img.Dispose();
        }
        public static Bitmap RotateImage(Image image, PointF offset, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            //create a new empty bitmap to hold rotated image
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //make a graphics object from the empty bitmap
            Graphics g = Graphics.FromImage(rotatedBmp);

            //Put the rotation point in the center of the image
            g.TranslateTransform(offset.X, offset.Y);

            //rotate the image
            g.RotateTransform(angle);

            //move the image back
            g.TranslateTransform(-offset.X, -offset.Y);

            //draw passed in image onto graphics object
            g.DrawImage(image, new PointF(0, 0));

            return rotatedBmp;
        }

        private void buttonCamera_Click(object sender, EventArgs e)
        {
            string lastSelected = comboBoxCamera.Text;
            UploadCameraList();
            if (lastSelected != string.Empty)
                comboBoxCamera.SelectedItem = lastSelected;
            if (comboBoxCamera.Items.Count == 0)
            {
                MessageBox.Show("Камера не найдена.", "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (!videoIsRunning)
                    return;
            }
            ChangeCameraState();
        }

        private int scale = 2;
        private void CaptureDevice_NewFrame(object sender, NewFrameEventArgs e)
        {
            if (e.Frame.Width <= pictureBoxQR.Width * scale && e.Frame.Height <= pictureBoxQR.Height * scale)
            {
                pictureBoxQR.Image = (Bitmap)e.Frame.Clone();
                return;
            }
            int width = e.Frame.Width;
            int height = e.Frame.Height;
            if (width > pictureBoxQR.Width * scale)
                width = pictureBoxQR.Width * scale;
            if (height > pictureBoxQR.Height * scale)
                height = pictureBoxQR.Height * scale;
            Rectangle rect = new Rectangle((e.Frame.Width - width) / 2, (e.Frame.Height - height) / 2, width, height);
            pictureBoxQR.Image = (Bitmap)e.Frame.Clone(rect, e.Frame.PixelFormat);
        }

        private void timerCamera_Tick(object sender, EventArgs e)
        {
            if (pictureBoxQR.Image == null)
                return;
            IBarcodeReader reader = new BarcodeReader();
            Result result = reader.Decode((Bitmap)pictureBoxQR.Image);
            if (result == null)
                return;
            textBoxQRcode.Text = result.ToString();
            timerCamera.Stop();
            ChangeCameraState();
            buttonSearch_Click(sender, e);
        }

        bool videoIsRunning = false;
        private void ChangeCameraState()
        {
            pictureBoxQR.Image = null;
            if (videoCaptureDevice == null || !videoIsRunning)
            {
                videoIsRunning = true;
                videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[comboBoxCamera.SelectedIndex].MonikerString);
                videoCaptureDevice.NewFrame += CaptureDevice_NewFrame;
                videoCaptureDevice.Start();
                timerCamera.Start();
                buttonCamera.Text = "Выключить камеру";
            }
            else
            {
                videoIsRunning = false;
                videoCaptureDevice.Stop();
                timerCamera.Stop();
                buttonCamera.Text = "Включить камеру";
            }
            pictureBoxQR.Image = null;
        }

        private void OpenQRcode()
        {
            string imageFormat = "*.gif;*.jpg;*.jpeg;*.bmp;*.wmf;*.png";
            Image image = null;

            // Проверка в буфере изображения
            if (Clipboard.ContainsImage() && DialogResult.OK == MessageBox.Show("В буфере обмена было обнаружено изображение.\nЗагрузить его?",
                    "Обнаружено изображение", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            {
                image = Clipboard.GetImage();
            }
            // Проверка в буфере файла изображения
            else if (Clipboard.ContainsFileDropList()) 
            {
                foreach (var fileName in Clipboard.GetFileDropList())
                {
                    FileInfo fileInfo = new FileInfo(fileName);
                    if (fileInfo.Exists && imageFormat.Contains(fileInfo.Extension)
                        && DialogResult.OK == MessageBox.Show("В буфере обмена было обнаружено изображение.\nЗагрузить его?",
                        "Обнаружено изображение", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                    {
                        image = Image.FromFile(fileName);
                        break;
                    }
                }
            }

            if (image == null) // Открытие изображения через OpenFileDialog
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = $"image files ({imageFormat})|{imageFormat}";
                    //ofd.RestoreDirectory = true;
                    if (ofd.ShowDialog() == DialogResult.Cancel)
                        return;
                    image = Image.FromFile(ofd.FileName);
                }
            }
            pictureBoxQR.Image = image;
            IBarcodeReader reader = new BarcodeReader();
            Result result = reader.Decode((Bitmap)image);
            if (result == null)
                textBoxQRcode.Text = string.Empty;
            else
                textBoxQRcode.Text = result.Text;
        }

        private void pictureBoxQR_DoubleClick(object sender, EventArgs e)
        {
            OpenQRcode();
        }

        private void buttonUploadQR_Click(object sender, EventArgs e)
        {
            OpenQRcode();
        }




        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (!ToParseCom())
            {
                if (DialogResult.Cancel == MessageBox.Show("Повторить попытку на другом сайте?", "Повтор попытки",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) || !ToParseRu())
                    return;
            }
            ShopChain();
            CreateDynamicInputs();
        }

        private List<string> names;
        private List<double> prices;
        private List<double> numbers;
        private DateTime date;
        private string shop;
        private string firstshop;
        private IWebDriver driver;
        private bool ToParseCom()
        {
            if (textBoxQRcode.Text == string.Empty)
            {
                MessageBox.Show("Поле пустое.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            driver = new ChromeDriver(service);
            try
            {
                driver.Navigate().GoToUrl($"https://proverkacheka.com");
            }
            catch (Exception ex)
            {
                driver.Quit();
                driver.Dispose();
                Console.WriteLine(ex.Message);
                MessageBox.Show("Нет интернет-соединения. Провейте подключение или сообщите провайдеру.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            if (!WaitForStart(driver, wait, "//a[@href='#b-checkform_tab-qrraw']"))
                return false;

            if (driver.FindElements(By.XPath("//a[@href='#b-checkform_tab-qrraw']")).Count == 0)
            {
                driver.Quit();
                driver.Dispose();
                MessageBox.Show("Ошибка ввода или сервер недоступен.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            Actions action = new Actions(driver);
            action.ScrollByAmount(0, 400).Perform();

            IWebElement foundElement = driver.FindElement(By.XPath("//a[@href='#b-checkform_tab-qrraw']"));
            foundElement.Click();
            if (!WaitForNothing(driver, wait, "//div[@class='b-checkform_tab-qrraw tab-pane fade active in']"))
                return false;

            foundElement = driver.FindElement(By.XPath("//*[@id='b-checkform_qrraw']"));
            foundElement.SendKeys(textBoxQRcode.Text);
            foundElement = driver.FindElement(By.XPath("//*[@id='b-checkform_tab-qrraw']/div/div/div/form/div[2]/div/button"));
            foundElement.Click();
            Thread.Sleep(100);

            if (!WaitForNothing(driver, wait, "//div[@class='b-check_place']"))
                return false;

            Thread.Sleep(100);
            if (driver.FindElements(By.XPath("//div[@class='b-check_place hidden']")).Count > 0)
            {
                MessageBox.Show("Чек не был найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            names = new List<string>();
            var elements = driver.FindElements(By.XPath("//tr[@class='b-check_item']/td[2]"));
            if (elements.Count == 0)
            {
                MessageBox.Show("Обработать чек не вышло.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            foreach (IWebElement element in elements)
                names.Add(element.Text);

            prices = new List<double>();
            elements = driver.FindElements(By.XPath("//tr[@class='b-check_item']/td[3]"));
            foreach (IWebElement element in elements)
                prices.Add(double.Parse(element.Text.Replace('.', ',')));

            numbers = new List<double>();
            elements = driver.FindElements(By.XPath("//tr[@class='b-check_item']/td[4]"));
            foreach (IWebElement element in elements)
                numbers.Add(double.Parse(element.Text.Replace('.', ',')));

            date = DateTime.Parse(driver.FindElement(By.XPath("//tr[5][@class='b-check_vblock-middle b-check_center']/td")).Text);
            firstshop = driver.FindElement(By.XPath("//tr[1][@class='b-check_vblock-middle b-check_center']/td")).Text;

            driver.Quit();
            driver.Dispose();
            return true;
        }
        private bool WaitForStart(IWebDriver driver, WebDriverWait wait, string xPath)
        {
            try
            {
                wait.Until(d => driver.FindElements(By.XPath(xPath)).Count > 0);
            }
            catch (OpenQA.Selenium.NoSuchWindowException ex) // Не обрабатывается, даже в общем Exception
            {
                Console.WriteLine(ex.Message);
                driver.Quit();
                driver.Dispose();
                return false;
            }
            catch (OpenQA.Selenium.WebDriverTimeoutException ex)
            {
                Console.WriteLine(ex.Message);
                driver.Quit();
                driver.Dispose();
                MessageBox.Show("Время выполнения запроса превысило крайнее значение. Повторите попытку позже.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                driver.Quit();
                driver.Dispose();
                if (!ex.Message.Contains("no such window: target window already closed")) // Повторная попытка обработать NoSuchWindowException
                    MessageBox.Show($"Была получена неизвестная ошибка:/n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private bool WaitForNothing(IWebDriver driver, WebDriverWait wait, string xPath)
        {
            try
            {
                wait.Until(d => driver.FindElements(By.XPath(xPath)).Count > 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                driver.Quit();
                driver.Dispose();
                MessageBox.Show($"Была получена неизвестная ошибка:/n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private bool ToParseRu()
        {
            if (textBoxQRcode.Text == string.Empty)
            {
                MessageBox.Show("Поле пустое.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            driver = new ChromeDriver(service);
            try
            {
                driver.Navigate().GoToUrl($"https://proverkacheka.ru/receipt/{textBoxQRcode.Text}");
            }
            catch (Exception ex)
            {
                driver.Quit();
                driver.Dispose();
                Console.WriteLine(ex.Message);
                MessageBox.Show("Нет интернет-соединения. Провейте подключение или сообщите провайдеру.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            if (!WaitForStart(driver, wait, "//a[@href='#b-checkform_tab-qrraw']"))
                return false;

            if (driver.FindElements(By.XPath("//div[@class='MuiTypography-root MuiTypography-body1 css-9l3uo3']")).Count < 10)
            {
                driver.Quit();
                driver.Dispose();
                MessageBox.Show("Ошибка ввода или сервер недоступен.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            names = new List<string>();
            var elements = driver.FindElements(By.XPath("/html/body/div[1]/div/div/main/div/div[3]/div/div/div[2]/div[1]/div"));
            if (elements.Count == 0)
            {
                MessageBox.Show("Обработать чек не вышло.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            foreach (IWebElement element in elements)
                names.Add(element.Text);

            prices = new List<double>();
            numbers = new List<double>();
            elements = driver.FindElements(By.XPath("/html/body/div[1]/div/div/main/div/div[3]/div/div/div[2]/div[3]/div[1]"));
            foreach (IWebElement element in elements)
            {
                string[] temp = element.Text.Split('x');
                if (temp.Length < 2)
                    continue;
                prices.Add(double.Parse(temp[0].Substring(0, temp[0].Length - 3)));
                numbers.Add(double.Parse(temp[1].Replace('.', ',')));
            }

            date = DateTime.Parse(driver.FindElement(By.XPath("/html/body/div[1]/div/div/main/div/div[2]/div[2]/div")).Text);
            firstshop = driver.FindElements(By.XPath("//div[@class='MuiTypography-root MuiTypography-body1 css-9l3uo3']")).Last().Text;
            driver.Quit();
            driver.Dispose();
            return true;
        }


        private void ShopChain()
        {
            SQLiteCommand command = new SQLiteCommand();
            command.Connection = dbConn;

            command.CommandText = "SELECT my_name FROM ShopChain WHERE name_place = @name_place LIMIT 1";
            command.Parameters.AddWithValue("@name_place", firstshop);
            var result = command.ExecuteScalar();
            command.Parameters.Clear();
            if (result != null)
            {
                shop = result.ToString();
                labelNewShop.Visible = false;
                return;
            }
            shop = firstshop;
            labelNewShop.Visible = !FormMain.ShopSource.Contains(shop);
        }

        private void CreateDynamicInputs()
        {
            if (names == null || names.Count == 0)
            {
                MessageBox.Show("Что-то действительно пошло не так...", "Что-то пошло нет так", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            double value = 0.0;
            panelNewProducts.Controls.Clear();
            for (int i = 0; i < names.Count; i++)
            {
                Panel newpanel = CreateNewPanel(i);
                newpanel.Location = new Point(3, 33 * i + 3);
                panelNewProducts.Controls.Add(newpanel);
                value += Math.Round(prices[i] * numbers[i], 2);
            }
            dateTimePicker.Value = date;
            textBoxShop.Text = shop;
            textBoxAllValue.Text = value.ToString();
        }

        private Panel CreateNewPanel(int num)
        {
            Panel panel = new Panel();
            panel.Name = $"NewPanel{num}";
            panel.BorderStyle = BorderStyle.Fixed3D;
            panel.Size = new Size(573, 30);


            PictureBox pictureBoxDelete = new PictureBox();
            pictureBoxDelete.Name = $"Delete{num}";
            pictureBoxDelete.Image = Properties.Resources.black_cross;
            pictureBoxDelete.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxDelete.Size = new Size(16, 16);
            pictureBoxDelete.Location = new Point(3, 5);
            pictureBoxDelete.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxDelete.Click += new System.EventHandler(buttonDeletePanel_Click);
            panel.Controls.Add(pictureBoxDelete);


            TextBox textBoxNewName = new TextBox();
            textBoxNewName.Name = "newName";
            textBoxNewName.Size = new Size(200, 20);
            textBoxNewName.Location = new Point(24, 3);
            textBoxNewName.Text = names[num];
            panel.Controls.Add(textBoxNewName);


            int index = FormMain.ProductSource.IndexOf(names[num]);
            if (index == -1)
            {
                Label labelNew = new Label();
                labelNew.Name = "newLabel";
                labelNew.Text = "new";
                labelNew.Font = new Font(labelNew.Font, FontStyle.Bold);
                labelNew.Size = new Size(30, 20);
                labelNew.Location = new Point(225, 5);
                panel.Controls.Add(labelNew);
            }


            ComboBox comboBoxNewType = new ComboBox();
            comboBoxNewType.Name = "newType";
            comboBoxNewType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxNewType.Items.AddRange(new object[] {"0 Обяз.еда", "1 Эксп.еда", "2 К чаю", "3 Вкусн.", "4 Прочее",
                "5 Трансп.", "6 Одежда", "7 Кафе", "8 Техника", "9 Развлеч."});
            comboBoxNewType.Size = new Size(80, 20);
            comboBoxNewType.Location = new Point(258, 3);
            
            if (index == -1)
                comboBoxNewType.SelectedIndex = 0;
            else
                comboBoxNewType.Text = FormMain.stringsForProductSource(index)[1];
            panel.Controls.Add(comboBoxNewType);


            TextBox textBoxNewPrice = new TextBox();
            textBoxNewPrice.Name = "newPrice";
            textBoxNewPrice.Size = new Size(70, 20);
            textBoxNewPrice.Location = new Point(344, 3);
            textBoxNewPrice.Text = prices[num].ToString();
            panel.Controls.Add(textBoxNewPrice);


            TextBox textBoxNewNumber = new TextBox();
            textBoxNewNumber.Name = "newNumber";
            textBoxNewNumber.Size = new Size(70, 20);
            textBoxNewNumber.Location = new Point(420, 3);
            textBoxNewNumber.Text = numbers[num].ToString();
            panel.Controls.Add(textBoxNewNumber);


            TextBox textBoxNewValue = new TextBox();
            textBoxNewValue.Name = "newValue";
            textBoxNewValue.Size = new Size(70, 20);
            textBoxNewValue.Location = new Point(496, 3);
            textBoxNewValue.ReadOnly = true;
            textBoxNewValue.Text = (prices[num] * numbers[num]).ToString("0.00");
            panel.Controls.Add(textBoxNewValue);

            return panel;
        }
        private void buttonDeletePanel_Click(object sender, EventArgs e)
        {
            string name = ((Control)sender).Name;
            int indexDeleted = 0;

            for (int i = 0; i < panelNewProducts.Controls.Count; i++)
            {
                Panel panel = panelNewProducts.Controls[i] as Panel;
                if (panel.Controls.Find(name, false).Length > 0)
                {
                    if (DialogResult.Cancel == MessageBox.Show($"Вы хотите удалить \"{panel.Controls.Find("newName", false)[0].Text}\" из списка?", "Подтверждение действия", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                        return;

                    panelNewProducts.Controls.RemoveAt(i);
                    indexDeleted = int.Parse(panel.Name.Substring(8));
                    panel.Dispose();
                    break;
                }
            }
            foreach (Control control in panelNewProducts.Controls)
                if (int.Parse(control.Name.Substring(8)) > indexDeleted)
                {
                    control.Location = new Point(control.Location.X, control.Location.Y - 33);
                }
            UpdateValues();
        }

        private void buttonAddAll_Click(object sender, EventArgs e)
        {
            if (panelNewProducts.Controls.Count == 0)
            {
                MessageBox.Show("Поля для добавления не были обозначены в списке.","Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SQLiteCommand command = new SQLiteCommand();
            command.Connection = dbConn;

            if (textBoxShop.Text != shop && shop == firstshop)
            {
                command.CommandText = "INSERT INTO ShopChain (name_place, my_name) VALUES (@name_place, @my_name)";
                command.Parameters.AddWithValue("@name_place", firstshop);
                command.Parameters.AddWithValue("@my_name", textBoxShop.Text);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }
            else if (textBoxShop.Text != shop && shop != firstshop)
            {
                command.CommandText = "UPDATE ShopChain SET my_name = @my_name WHERE name_place = @name_place";
                command.Parameters.AddWithValue("@name_place", firstshop);
                command.Parameters.AddWithValue("@my_name", textBoxShop.Text);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }

            string commandtext = "INSERT INTO Products (name, price, number, sale, type, date, shop) VALUES";
            for (int i = 0; i < panelNewProducts.Controls.Count; i++)
            {
                foreach (Control control in panelNewProducts.Controls[i].Controls)
                    switch (control.Name)
                    {
                        case "newName":
                            if (control.Text == string.Empty)
                            {
                                MessageBox.Show($"В поле #{i + 1} название не должно быть пустым.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            command.Parameters.AddWithValue($"@name{i}", control.Text);
                            break;
                        case "newType":
                            command.Parameters.AddWithValue($"@type{i}", control.Text);
                            break;
                        case "newPrice":
                            if (!double.TryParse(control.Text, out double price))
                            {
                                MessageBox.Show($"В поле #{i+1} цена указана неверно.","Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            command.Parameters.AddWithValue($"@price{i}", price);
                            break;
                        case "newNumber":
                            if (!double.TryParse(control.Text, out double number))
                            {
                                MessageBox.Show($"В поле #{i + 1} количество указано неверно.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            command.Parameters.AddWithValue($"@number{i}", number);
                            break;
                        default:
                            break;
                    }
                command.Parameters.AddWithValue($"@date{i}", dateTimePicker.Value.ToString("yyyy.MM.dd"));
                command.Parameters.AddWithValue($"@sale{i}", 0);
                command.Parameters.AddWithValue($"@shop{i}", textBoxShop.Text);
                commandtext += $" (@name{i}, @price{i}, @number{i}, @sale{i}, @type{i}, @date{i}, @shop{i}),";
            }
            command.CommandText = commandtext.Substring(0, commandtext.Length - 1);
            command.ExecuteNonQuery();
            command.Parameters.Clear();
            
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonUpdateValues_Click(object sender, EventArgs e)
        {
            if (!timerAnimationValue.Enabled)
            {
                timerAnimationValue.Interval = periodUpdate;
                timerAnimationValue.Start();
            }
            UpdateValues();
        }
        private void UpdateValues()
        {
            double value = 0.0;
            for (int i = 0; i < panelNewProducts.Controls.Count; i++)
            {
                double price = 0;
                double number = 0;
                int indexValue = 0;
                foreach (Control control in panelNewProducts.Controls[i].Controls)
                    switch (control.Name)
                    {
                        case "newPrice":
                            price = double.Parse(control.Text);
                            break;
                        case "newNumber":
                            number = double.Parse(control.Text);
                            break;
                        case "newValue":
                            indexValue = panelNewProducts.Controls[i].Controls.IndexOf(control);
                            break;
                        default:
                            break;
                    }
                double productValue = Math.Round(price * number, 2);
                panelNewProducts.Controls[i].Controls[indexValue].Text = productValue.ToString();
                value += productValue;
            }
            textBoxAllValue.Text = value.ToString();
        }
        private int currenttimeValue = 0;
        private void timerAnimationValue_Tick(object sender, EventArgs e)
        {
            currenttimeValue += periodUpdate;
            if (currenttimeValue > maxtime)
            {
                currenttimeValue = 0;
                timerAnimationValue.Stop();
            }
            Image img = Properties.Resources.reload;
            float newangle = 360f / (float)Math.Pow(maxtime, 3) * (float)Math.Pow(currenttimeValue, 2) * (3 * maxtime - 2 * currenttimeValue);
            buttonUpdateValues.Image = RotateImage(img, new PointF(img.Width / 2f, img.Height / 2f), newangle);
            img.Dispose();
        }


    }
}
