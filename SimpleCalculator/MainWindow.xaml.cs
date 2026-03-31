using System;
using System.Windows;
using System.Windows.Controls;

namespace SimpleCalculator
{
    public partial class MainWindow : Window
    {
        private double _firstOperand = 0;
        private string _operator = "";
        private bool _newInput = true;   // ждём новый ввод после оператора

        public MainWindow()
        {
            InitializeComponent();
        }

        // Цифры 0–9
        private void OnDigit(object sender, RoutedEventArgs e)
        {
            string digit = ((Button)sender).Content.ToString();

            if (_newInput)
            {
                Display.Text = digit == "0" ? "0" : digit;
                _newInput = false;
            }
            else
            {
                Display.Text = Display.Text == "0" ? digit : Display.Text + digit;
            }
        }

        // Десятичная точка
        private void OnDot(object sender, RoutedEventArgs e)
        {
            if (_newInput)
            {
                Display.Text = "0,";
                _newInput = false;
                return;
            }

            if (!Display.Text.Contains(","))
                Display.Text += ",";
        }

        // Операторы + - * /
        private void OnOperator(object sender, RoutedEventArgs e)
        {
            // Если оператор уже выбран — посчитаем промежуточный результат
            if (!_newInput && _operator != "")
                Calculate();

            _firstOperand = double.Parse(Display.Text);
            _operator = ((Button)sender).Tag.ToString();
            _newInput = true;
        }

        // Знак числа ±
        private void OnNegate(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Display.Text, out double val))
                Display.Text = (-val).ToString();
        }

        // Процент %
        private void OnPercent(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Display.Text, out double val))
                Display.Text = (val / 100).ToString();
        }

        // Сброс C
        private void OnClear(object sender, RoutedEventArgs e)
        {
            Display.Text = "0";
            _firstOperand = 0;
            _operator = "";
            _newInput = true;
        }

        // Равно =
        private void OnEquals(object sender, RoutedEventArgs e)
        {
            if (_operator == "") return;
            Calculate();
            _operator = "";
        }

        // Вычисление
        private void Calculate()
        {
            if (!double.TryParse(Display.Text, out double secondOperand)) return;

            double result = _operator switch
            {
                "+" => _firstOperand + secondOperand,
                "-" => _firstOperand - secondOperand,
                "*" => _firstOperand * secondOperand,
                "/" => secondOperand != 0 ? _firstOperand / secondOperand : double.NaN,
                _   => secondOperand
            };

            Display.Text = double.IsNaN(result) ? "Ошибка" : result.ToString();
            _firstOperand = result;
            _newInput = true;
        }
    }
}
