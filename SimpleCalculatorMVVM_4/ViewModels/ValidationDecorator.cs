using System;

namespace SimpleCalculatorMVVM_4.Models
{
    /// <summary>
    /// Конкретный декоратор №1 — Валидация.
    /// Перехватывает вызовы ExecuteOperation и проверяет:
    ///   • деление на ноль;
    ///   • бесконечный или NaN-результат.
    /// При ошибке генерирует событие ErrorOccurred и НЕ делегирует вызов вниз,
    /// тем самым защищая базовый CalculatorReceiver от некорректного состояния.
    /// </summary>
    public class ValidationDecorator : CalculatorReceiverDecorator
    {
        /// <summary>Событие, возникающее при обнаружении ошибки валидации.</summary>
        public event EventHandler<string>? ErrorOccurred;

        /// <summary>Текст последней ошибки (null, если ошибок не было).</summary>
        public string? LastError { get; private set; }

        public ValidationDecorator(ICalculatorReceiver receiver) : base(receiver) { }

        public override void ExecuteOperation(string operatorSymbol, double operand)
        {
            LastError = null;

            // Проверка деления на ноль ещё до вызова базового компонента
            if (operatorSymbol == "/" && Math.Abs(operand) < double.Epsilon)
            {
                RaiseError("Ошибка: деление на ноль");
                return; // прерываем цепочку
            }

            // Делегируем вниз по цепочке
            base.ExecuteOperation(operatorSymbol, operand);

            // Проверяем результат на Infinity / NaN
            double result = GetCurrentValue();
            if (double.IsInfinity(result) || double.IsNaN(result))
            {
                RaiseError("Ошибка: результат вне допустимого диапазона");
                base.Reset(); // возвращаем корректное состояние
            }
        }

        private void RaiseError(string message)
        {
            LastError = message;
            ErrorOccurred?.Invoke(this, message);
        }
    }
}
