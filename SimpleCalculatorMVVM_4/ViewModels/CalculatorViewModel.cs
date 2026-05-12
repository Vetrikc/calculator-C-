using System;
using System.Diagnostics;
using System.Windows.Input;
using SimpleCalculatorMVVM_4.Commands;
using SimpleCalculatorMVVM_4.Models;

namespace SimpleCalculatorMVVM_4.ViewModels
{
    public class CalculatorViewModel : ViewModelBase
    {
        private readonly CalculatorReceiver _receiver;
        private readonly CommandInvoker _invoker;
        
        private string _display = "0";
        private string _exprDisplay = "";
        private double _firstOperand;
        private string _operator = "";
        private bool _newInput = true;

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

        public ICommand DigitCommand { get; }
        public ICommand OperatorCommand { get; }
        public ICommand EqualsCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand DotCommand { get; }
        public ICommand NegateCommand { get; }
        public ICommand PercentCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        public CalculatorViewModel()
        {
            _receiver = new CalculatorReceiver();
            _invoker = new CommandInvoker();
            
            _receiver.ValueChanged += OnReceiverValueChanged;

            DigitCommand = new RelayCommand(OnDigit);
            OperatorCommand = new RelayCommand(OnOperator);
            EqualsCommand = new RelayCommand(OnEquals);
            ClearCommand = new RelayCommand(OnClear);
            DotCommand = new RelayCommand(OnDot);
            NegateCommand = new RelayCommand(OnNegate);
            PercentCommand = new RelayCommand(OnPercent);
            UndoCommand = new RelayCommand(OnUndo, _ => _invoker.CanUndo());
            RedoCommand = new RelayCommand(OnRedo, _ => _invoker.CanRedo());
        }

        private void OnReceiverValueChanged(object? sender, double value)
        {
            Display = value.ToString();
        }

        private void AppendDigit(string digit)
        {
            if (_newInput)
            {
                Display = digit;
                _newInput = false;
            }
            else
            {
                Display += digit;
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
            // Получаем текущее значение с дисплея
            double currentValue = double.Parse(Display.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
            
            // КЛЮЧЕВОЕ ИСПРАВЛЕНИЕ: устанавливаем значение в receiver
            _receiver.SetValue(currentValue);
            
            _firstOperand = currentValue;
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

        private void CalculateAndExecuteCommand()
        {
            double secondOperand = double.Parse(Display.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
            
            var command = new CalculatorCommand(_receiver, _operator, secondOperand);
            _invoker.ExecuteCommand(command);
            
            Display = _receiver.GetCurrentValue().ToString();
            
            _operator = "";
            _newInput = true;
            
            (UndoCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (RedoCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void DoEquals()
        {
            if (_operator == "") return;
            
            ExprDisplay = $"{_firstOperand} {_operator} {Display} =";
            CalculateAndExecuteCommand();
        }

        private void Clear()
        {
            _receiver.Reset();
            Display = "0";
            ExprDisplay = "";
            _firstOperand = 0;
            _operator = "";
            _newInput = true;
        }

        private void Negate()
        {
            if (double.TryParse(Display.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture, out double val))
            {
                Display = (-val).ToString();
                _receiver.SetValue(-val);
            }
        }

        private void Percent()
        {
            if (double.TryParse(Display.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture, out double val))
            {
                Display = (val / 100).ToString();
                _receiver.SetValue(val / 100);
            }
        }

        private void OnUndo(object? parameter)
        {
            ExprDisplay = "";
            _invoker.Undo();
            Display = _receiver.GetCurrentValue().ToString();
            (UndoCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (RedoCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void OnRedo(object? parameter)
        {
            ExprDisplay = "";
            _invoker.Redo();
            Display = _receiver.GetCurrentValue().ToString();
            (UndoCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (RedoCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

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

        private void OnEquals(object? parameter) => DoEquals();
        private void OnDot(object? parameter) => AppendDot();
        private void OnClear(object? parameter) => Clear();
        private void OnNegate(object? parameter) => Negate();
        private void OnPercent(object? parameter) => Percent();
    }
}