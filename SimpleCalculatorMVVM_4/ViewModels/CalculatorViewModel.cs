using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SimpleCalculatorMVVM_4.Commands;
using SimpleCalculatorMVVM_4.Models;
using SimpleCalculatorMVVM_4.Services;

namespace SimpleCalculatorMVVM_4.ViewModels
{
    /// <summary>
    /// ViewModel калькулятора.
    /// Собирает цепочку декораторов:
    ///   CalculatorReceiver  (базовый компонент)
    ///       ↑ ValidationDecorator   (конкретный декоратор №1 — валидация)
    ///           ↑ LoggingDecorator  (конкретный декоратор №2 — логирование)
    ///
    /// ViewModel работает только через интерфейс ICalculatorReceiver,
    /// что делает её независимой от конкретной реализации или набора декораторов.
    /// </summary>
    public class CalculatorViewModel : ViewModelBase
    {
        private readonly ConfigurationService _configService;
        private readonly AudioService _audioService;

        // ── Паттерн Decorator: цепочка декораторов ──────────────────────────
        private readonly ICalculatorReceiver _receiver;          // внешний конец цепочки
        private readonly ValidationDecorator _validationLayer;   // ссылка для подписки на ошибки
        private readonly LoggingDecorator _loggingLayer;      // ссылка для подписки на журнал

        // ── Паттерн Command ─────────────────────────────────────────────────
        private readonly CommandInvoker _invoker;

        // ── Состояние ────────────────────────────────────────────────────────
        private string _display = "0";
        private string _exprDisplay = "";
        private double _firstOperand;
        private string _operator = "";
        private bool _newInput = true;

        // ── Свойства дисплея ─────────────────────────────────────────────────
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

        /// <summary>Журнал операций, отображаемый в ListBox через привязку данных.</summary>
        public ObservableCollection<string> OperationLog { get; } = new();

        // ── Команды ──────────────────────────────────────────────────────────
        public ICommand DigitCommand { get; }
        public ICommand OperatorCommand { get; }
        public ICommand EqualsCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand DotCommand { get; }
        public ICommand NegateCommand { get; }
        public ICommand PercentCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }
        public CalculatorViewModel() : this(new ConfigurationService())
        { 
        }
        public CalculatorViewModel(ConfigurationService configService)
        {
            _configService = configService;
            _audioService = new AudioService();

            _configService.ConfigChanged += OnConfigChanged;

            _invoker = new CommandInvoker();

            // ── Сборка цепочки декораторов ──────────────────────────────────
            var baseReceiver = new CalculatorReceiver();          // конкретный компонент
            _validationLayer = new ValidationDecorator(baseReceiver);   // декоратор 1
            _loggingLayer = new LoggingDecorator(_validationLayer);  // декоратор 2
            _receiver = _loggingLayer;                     // работаем через интерфейс

            // Подписки: ViewModel реагирует на события декораторов
            _receiver.ValueChanged += OnReceiverValueChanged;
            _validationLayer.ErrorOccurred += OnValidationError;
            _loggingLayer.LogAdded += OnLogAdded;

            // Инициализация команд
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

        // ── Обработчики событий декораторов ─────────────────────────────────

        private void OnReceiverValueChanged(object? sender, double value)
        {
            Display = FormatNumber(value);
        }

        private void OnValidationError(object? sender, string error)
        {
            PlayErrorSound();
            Display = error;
            ExprDisplay = "";
            _operator = "";
            _newInput = true;
        }

        private void OnLogAdded(object? sender, string entry)
        {
            // Добавляем в ObservableCollection — ListView обновится автоматически
            OperationLog.Add(entry);

            // Ограничиваем размер журнала (UI-удобство)
            if (OperationLog.Count > 100)
                OperationLog.RemoveAt(0);
        }

        // ── Вспомогательные методы ввода ─────────────────────────────────────

        private static string FormatNumber(double v)
        {
            if (double.IsNaN(v) || double.IsInfinity(v)) return "Ошибка";
            // До 10 значимых цифр без лишних нулей
            return v.ToString("G10", System.Globalization.CultureInfo.InvariantCulture);
        }

        private void AppendDigit(string digit)
        {
            if (_newInput) { Display = digit; _newInput = false; }
            else { Display += digit; }
        }

        private void AppendDot()
        {
            if (_newInput) { Display = "0."; _newInput = false; return; }
            if (!Display.Contains('.')) Display += '.';
        }

        private void SetOperator(string op)
        {
            double current = ParseDisplay();
            _receiver.SetValue(current);
            _firstOperand = current;
            _operator = op;
            _newInput = true;

            string sym = op switch { "+" => "+", "-" => "−", "*" => "×", "/" => "÷", _ => op };
            ExprDisplay = $"{FormatNumber(_firstOperand)} {sym}";
        }

        private void CalculateAndExecuteCommand()
        {
            double second = ParseDisplay();
            var command = new CalculatorCommand(_receiver, _operator, second);
            _invoker.ExecuteCommand(command);

            Display = FormatNumber(_receiver.GetCurrentValue());
            _operator = "";
            _newInput = true;

            (UndoCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (RedoCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void DoEquals()
        {
            if (_operator == "") return;
            string sym = _operator switch { "+" => "+", "-" => "−", "*" => "×", "/" => "÷", _ => _operator };
            ExprDisplay = $"{FormatNumber(_firstOperand)} {sym} {Display} =";
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
            double val = ParseDisplay();
            Display = FormatNumber(-val);
            _receiver.SetValue(-val);
        }

        private void Percent()
        {
            double val = ParseDisplay();
            Display = FormatNumber(val / 100);
            _receiver.SetValue(val / 100);
        }

        private double ParseDisplay() =>
            double.TryParse(Display.Replace(',', '.'),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out double v) ? v : 0;

        // ── Обработчики команд ────────────────────────────────────────────────

        private void OnDigit(object? p)
        {
            PlayButtonSound(); 
            if (p is string d) AppendDigit(d);
        }

        private void OnOperator(object? p)
        {
            PlayButtonSound(); 
            if (p is string op) SetOperator(op);
        }

        private void OnEquals(object? p)
        {
            PlayOperationSound(); 
            DoEquals();
        }

        private void OnDot(object? p)
        {
            PlayButtonSound();
            AppendDot();
        }

        private void OnClear(object? p)
        {
            PlayButtonSound();
            Clear();
        }

        private void OnNegate(object? p)
        {
            PlayButtonSound(); 
            Negate();
        }

        private void OnPercent(object? p)
        {
            PlayButtonSound(); 
            Percent();
        }

        private void OnUndo(object? p)
        {
            PlayButtonSound();
            ExprDisplay = "";
            _invoker.Undo();
            Display = FormatNumber(_receiver.GetCurrentValue());
            (UndoCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (RedoCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void OnRedo(object? p)
        {
            PlayButtonSound(); 
            ExprDisplay = "";
            _invoker.Redo();
            Display = FormatNumber(_receiver.GetCurrentValue());
            (UndoCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (RedoCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }


        private void PlayButtonSound()
        {
            if (_configService.Config.SoundEnabled)
            {
                _audioService?.PlayButtonClick();
            }
        }

        private void PlayOperationSound()
        {
            _audioService?.PlayOperation();
        }

        private void PlayErrorSound()
        {
            _audioService?.PlayError();
        }

        private void OnConfigChanged(object? sender, AppConfig config)
        {
            // Обновляем состояние звука
            // Применяем другие настройки при необходимости
        }
    }
}
