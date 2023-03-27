using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using DocumentFormat.OpenXml.Vml;
using WpfApp3.Classes;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, double> _shows;
        private double _selectedShowPrice;
        private double _zoneMultiplier;

        public MainWindow()
        {
            InitializeComponent();
            InitializeShows();
        }

        private void TicketCountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Разрешить только числовые символы
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            return !new Regex("[^0-9]+").IsMatch(text);
        }

        private void TicketCountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Удаление минуса, если пользователь вводит его
            TicketCountTextBox.Text = TicketCountTextBox.Text.Replace("-", "");
        }

        private void InitializeShows()
        {
            //Цена на представления
            _shows = new Dictionary<string, double>
            {
                {"Красная шапочка", 1000},
                {"Летучий корабль", 1200},
                {"Лебединое озеро", 1500},
                {"Донкихот", 1800},
                {"Алые паруса", 2000},
                {"Щелкунчик", 2500}
            };

            ShowComboBox.ItemsSource = _shows.Keys;
        }

        private void ShowSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ShowComboBox.SelectedIndex < 0) return;

            _selectedShowPrice = _shows[ShowComboBox.SelectedItem.ToString()];

            //Загрузка картинки для выбранного представления
            string showName = ShowComboBox.SelectedItem.ToString();
            string imagePath = $"C:\\Users\\123\\Desktop\\экзамен пример\\WpfApp3\\WpfApp3\\Images\\{showName}.jpg";
            ShowImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));

        }

        private void ZoneRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            RadioButton checkedZone = sender as RadioButton;
            //Рассчет стоимости при выборе места
            switch (checkedZone.Content.ToString())
            {
                case "VIP":
                    _zoneMultiplier = 1.5;
                    break;
                case "Партер":
                    _zoneMultiplier = 1.07;
                    break;
                case "Балкон":
                    _zoneMultiplier = 1.2;
                    break;
                default:
                    _zoneMultiplier = 1.0;
                    break;
            }
        }

        private double CalculateDiscount(int ticketsCount)
        {
            //Рассчет стоимости при выборе билетов
            if (ticketsCount > 30)
            {
                return 0.25;
            }
            else if (ticketsCount > 20)
            {
                return 0.1;
            }
            else if (ticketsCount > 15)
            {
                return 0.07;
            }
            else if (ticketsCount > 10)
            {
                return 0.05;
            }
            return 0.0;
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на выбор представления
            if (ShowComboBox.SelectedIndex < 0)
            {
                MessageBox.Show("Пожалуйста, выберите представление.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка на выбор места
            if (VIPRadioButton.IsChecked == false && OrchestraRadioButton.IsChecked == false && BalconyRadioButton.IsChecked == false)
            {
                MessageBox.Show("Пожалуйста, выберите место.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка на пустое поле
            if (string.IsNullOrWhiteSpace(TicketCountTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите количество билетов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int.TryParse(TicketCountTextBox.Text, out int ticketsCount);

            double discount = CalculateDiscount(ticketsCount);
            double totalPrice = _selectedShowPrice * _zoneMultiplier * ticketsCount * (1 - discount);

            string selectedZone = "";
            if (VIPRadioButton.IsChecked == true)
                selectedZone = "VIP";
            else if (OrchestraRadioButton.IsChecked == true)
                selectedZone = "Партер";
            else if (BalconyRadioButton.IsChecked == true)
                selectedZone = "Балкон";

            string showName = ShowComboBox.SelectedItem.ToString();
            string purchaseInfo = $"Вы купили {ticketsCount} билетов на представление \"{showName}\" в зоне {selectedZone}. Общая стоимость: {totalPrice} ₽.";
            PurchaseInfoTextBlock.Text = purchaseInfo;

            string imagePath = $"C:\\Users\\123\\Desktop\\экзамен пример\\WpfApp3\\WpfApp3\\Images\\{showName}.JPEG"; // Замените расширение файла на то, которое используется для изображений
            ShowImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));

            // Отображение картинки
            ShowImage.Visibility = Visibility.Visible;

            Random rnd1 = new Random();
            int c = rnd1.Next(0, 100000);
            // Создание чека
            var helper = new WordHelper("чек.docx");
            {
                var items = new Dictionary<string, string>
                {
                        {"{итог}", $"{totalPrice}" },
                        {"{Товар}",$"Представление  {ticketsCount} билетов на представление \"{showName}\" в зоне {selectedZone}"},
                        {"{дата}", Convert.ToString(DateTime.Now)},
                        {"{Уникальный_номер}", Convert.ToString(c)}
                                                                    };
                helper.Process(items);
            }
        }

        private void ReceiptButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Квитанция оформлена");
        }
    }
}
