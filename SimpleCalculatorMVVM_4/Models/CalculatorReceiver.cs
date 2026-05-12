using System;

namespace SimpleCalculatorMVVM_4.Models
{
    /// <summary>
    /// Конкретный компонент паттерна Decorator.
    /// Содержит базовую логику вычислений без каких-либо дополнительных обязанностей.
    /// Все расширения (валидация, логирование) добавляются декораторами снаружи.
    /// </summary>
    public class CalculatorReceiver : ICalculatorReceiver
    {
        private double _currentValue = 0;

        public event EventHandler<double>? ValueChanged;

        public double GetCurrentValue() => _currentValue;

        public void SetValue(double value)
        {
            _currentValue = value;
            OnValueChanged(value);
        }

        public void ExecuteOperation(string operatorSymbol, double operand)
        {
            double result = operatorSymbol switch
            {
                "+" => _currentValue + operand,
                "-" => _currentValue - operand,
                "*" => _currentValue * operand,
                "/" when operand != 0 => _currentValue / operand,
                "/" => throw new InvalidOperationException("Деление на ноль"),
                _ => operand
            };

            _currentValue = result;
            OnValueChanged(result);
        }

        public void Reset()
        {
            _currentValue = 0;
            OnValueChanged(0);
        }

        protected virtual void OnValueChanged(double value)
        {
            ValueChanged?.Invoke(this, value);
        }
    }
}
