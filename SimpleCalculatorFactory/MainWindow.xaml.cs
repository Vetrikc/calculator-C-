using System.Windows;
using System.Windows.Controls;

namespace SimpleCalculatorFactory
{
    public partial class MainWindow : Window
    {
        private double _firstOperand = 0;
        private string _operator = "";
        private bool _newInput = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Единственный обработчик для всех кнопок.
        // Фабрика создаёт нужный объект по тегу кнопки,
        // затем мы обрабатываем результат Press().
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            string key = ((Button)sender).Tag.ToString()!;

            // Создаём объект кнопки через Factory Method
            CalcButton btn = ButtonFactory.Create(key);
            string pressed = btn.Press();

            switch (pressed)
            {
                case "C":
                    Clear();
                    break;

                case "±":
                    Negate();
                    break;

                case "%":
                    Percent();
                    break;

                case "=":
                    Equals();
                    break;

                case "+":
                case "-":
                case "*":
                case "/":
                    SetOperator(pressed);
                    break;

                case ".":
                    AppendDot();
                    break;

                default:
                    // Цифра
                    AppendDigit(pressed);
                    break;
            }
        }

        // ---------- логика ----------

        private void AppendDigit(string digit)
        {
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

        private void AppendDot()
        {
            if (_newInput)
            {
                Display.Text = "0,";
                _newInput = false;
                return;
            }
            if (!Display.Text.Contains(','))
                Display.Text += ',';
        }

        private void SetOperator(string op)
        {
            if (!_newInput && _operator != "")
                Calculate();

            _firstOperand = double.Parse(Display.Text);
            _operator = op;
            _newInput = true;

            string opSymbol = op switch { "+" => "+", "-" => "−", "*" => "×", "/" => "÷", _ => op };
            ExprDisplay.Text = $"{_firstOperand} {opSymbol}";
        }

        private void Equals()
        {
            if (_operator == "") return;
            ExprDisplay.Text = $"{_firstOperand} {_operator} {Display.Text} =";
            Calculate();
            _operator = "";
        }

        private void Clear()
        {
            Display.Text = "0";
            ExprDisplay.Text = "";
            _firstOperand = 0;
            _operator = "";
            _newInput = true;
        }

        private void Negate()
        {
            if (double.TryParse(Display.Text, out double val))
                Display.Text = (-val).ToString();
        }

        private void Percent()
        {
            if (double.TryParse(Display.Text, out double val))
                Display.Text = (val / 100).ToString();
        }

        private void Calculate()
        {
            if (!double.TryParse(Display.Text, out double second)) return;

            double result = _operator switch
            {
                "+" => _firstOperand + second,
                "-" => _firstOperand - second,
                "*" => _firstOperand * second,
                "/" => second != 0 ? _firstOperand / second : double.NaN,
                _   => second
            };

            Display.Text = double.IsNaN(result) ? "Ошибка" : result.ToString();
            _firstOperand = result;
            _newInput = true;
        }
    }
}
