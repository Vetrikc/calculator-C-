using System;

namespace SimpleCalculatorMVVM_4.Models
{
    /// <summary>
    /// Абстрактный декоратор паттерна Decorator.
    /// Хранит ссылку на оборачиваемый ICalculatorReceiver и делегирует
    /// все вызовы ему по умолчанию. Конкретные декораторы переопределяют
    /// нужные методы, добавляя поведение до/после делегирования.
    /// </summary>
    public abstract class CalculatorReceiverDecorator : ICalculatorReceiver
    {
        /// <summary>Ссылка на оборачиваемый компонент (или другой декоратор).</summary>
        protected readonly ICalculatorReceiver _wrapped;

        // Прокси события: подписки перенаправляются во внутренний компонент.
        public event EventHandler<double>? ValueChanged
        {
            add    => _wrapped.ValueChanged += value;
            remove => _wrapped.ValueChanged -= value;
        }

        protected CalculatorReceiverDecorator(ICalculatorReceiver receiver)
        {
            _wrapped = receiver ?? throw new ArgumentNullException(nameof(receiver));
        }

        public virtual double GetCurrentValue()                              => _wrapped.GetCurrentValue();
        public virtual void   SetValue(double value)                        => _wrapped.SetValue(value);
        public virtual void   ExecuteOperation(string operatorSymbol,
                                               double operand)              => _wrapped.ExecuteOperation(operatorSymbol, operand);
        public virtual void   Reset()                                       => _wrapped.Reset();
    }
}
