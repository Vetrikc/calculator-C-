using System;

namespace SimpleCalculatorMVVM_4.Models
{
    /// <summary>
    /// Компонентный интерфейс паттерна Decorator.
    /// Определяет контракт для конкретного компонента (CalculatorReceiver)
    /// и для всех декораторов, что позволяет подменять их прозрачно.
    /// </summary>
    public interface ICalculatorReceiver
    {
        /// <summary>Событие, уведомляющее подписчиков об изменении текущего значения.</summary>
        event EventHandler<double>? ValueChanged;

        /// <summary>Возвращает текущее накопленное значение.</summary>
        double GetCurrentValue();

        /// <summary>Устанавливает значение напрямую (например, при вводе операнда).</summary>
        void SetValue(double value);

        /// <summary>Выполняет арифметическую операцию над текущим значением.</summary>
        void ExecuteOperation(string operatorSymbol, double operand);

        /// <summary>Сбрасывает состояние к нулю.</summary>
        void Reset();
    }
}
