using SimpleCalculatorMVVM.Commands;
using SimpleCalculatorMVVM.Models;
using System.Windows.Input;

namespace SimpleCalculatorMVVM.ViewModels
{
    public class CalculatorViewModel : ViewModelBase
    {
        private double _firstOperand = 0;
        private string _operator = "";
        private bool _newInput = true;
        private string _display = "0";
        private string _exprDisplay = "";

        public string Display
        {
            get => _display;
            set => SetProperty(ref _display, value);
        }

        public string ExprDisplay
        {
            get => _exprDisplay;
            set => SetProperty(ref _exprDisplay, value);
        }

        // Команды для цифр и операций
        public ICommand DigitCommand { get; }
        public ICommand OperatorCommand { get; }
        public ICommand EqualsCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand DotCommand { get; }
        public ICommand NegateCommand { get; }
        public ICommand PercentCommand { get; }

        public CalculatorViewModel()
        {
            DigitCommand = new RelayCommand(OnDigit);
            OperatorCommand = new RelayCommand(OnOperator);
            EqualsCommand = new RelayCommand(OnEquals);
            ClearCommand = new RelayCommand(OnClear);
            DotCommand = new RelayCommand(OnDot);
            NegateCommand = new RelayCommand(OnNegate);
            PercentCommand = new RelayCommand(OnPercent);
        }

        private void AppendDigit(string digit)
        {
            if (_newInput)
            {
                Display = digit == "0" ? "0" : digit;
                _newInput = false;
            }
            else
            {
                Display = Display == "0" ? digit : Display + digit;
            }
        }

        private void AppendDot()
        {
            if (_newInput)
            {
                Display = "0.";
                _newInput = false;
                return;
            }
            if (!Display.Contains('.'))
                Display += '.';
        }

        private void SetOperator(string op)
        {
            if (!_newInput && _operator != "")
                Calculate();

            _firstOperand = double.Parse(Display.Replace(',', '.'));
            _operator = op;
            _newInput = true;

            string opSymbol = op switch
            {
                "+" => "+",
                "-" => "−",
                "*" => "×",
                "/" => "÷",
                _ => op
            };
            ExprDisplay = $"{_firstOperand} {opSymbol}";
        }

        private void DoEquals()
        {
            if (_operator == "") return;
            ExprDisplay = $"{_firstOperand} {_operator} {Display} =";
            Calculate();
            _operator = "";
        }

        private void Clear()
        {
            Display = "0";
            ExprDisplay = "";
            _firstOperand = 0;
            _operator = "";
            _newInput = true;
        }

        private void Negate()
        {
            if (double.TryParse(Display, out double val))
                Display = (-val).ToString();
        }

        private void Percent()
        {
            if (double.TryParse(Display, out double val))
                Display = (val / 100).ToString();
        }

        private void Calculate()
        {
            if (!double.TryParse(Display, out double second)) return;

            double result = _operator switch
            {
                "+" => _firstOperand + second,
                "-" => _firstOperand - second,
                "*" => _firstOperand * second,
                "/" => second != 0 ? _firstOperand / second : double.NaN,
                _ => second
            };

            Display = double.IsNaN(result) ? "Ошибка" : result.ToString();
            _firstOperand = result;
            _newInput = true;
        }

        // Команды
        private void OnDigit(object? parameter)
        {
            if (parameter is string digit)
                AppendDigit(digit);
        }

        private void OnOperator(object? parameter)
        {
            if (parameter is string op)
                SetOperator(op);
        }

        private void OnEquals(object? parameter)
        {
            DoEquals();
        }

        private void OnDot(object? parameter)
        {
            AppendDot();
        }

        private void OnClear(object? parameter)
        {
            Clear();
        }

        private void OnNegate(object? parameter)
        {
            Negate();
        }

        private void OnPercent(object? parameter)
        {
            Percent();
        }
    }
}
