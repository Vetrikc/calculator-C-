using System;
using System.Collections.Generic;

namespace SimpleCalculatorMVVM_4.Models
{
    /// <summary>
    /// Конкретный декоратор №2 — Логирование.
    /// Оборачивает любой ICalculatorReceiver (в том числе другой декоратор)
    /// и записывает каждую операцию в журнал, не изменяя основную логику.
    /// Порядок в цепочке: LoggingDecorator → ValidationDecorator → CalculatorReceiver.
    /// </summary>
    public class LoggingDecorator : CalculatorReceiverDecorator
    {
        private readonly List<string> _log = new();

        /// <summary>Событие, возникающее при добавлении новой записи в журнал.</summary>
        public event EventHandler<string>? LogAdded;

        /// <summary>Доступ только для чтения к накопленному журналу операций.</summary>
        public IReadOnlyList<string> Log => _log;

        public LoggingDecorator(ICalculatorReceiver receiver) : base(receiver) { }

        public override void SetValue(double value)
        {
            Append($"SET  ▶  {value}");
            base.SetValue(value);
        }

        public override void ExecuteOperation(string operatorSymbol, double operand)
        {
            double before = GetCurrentValue();
            string symbol = operatorSymbol switch
            {
                "+" => "+", "-" => "−", "*" => "×", "/" => "÷", _ => operatorSymbol
            };
            Append($"OP   ▶  {before} {symbol} {operand}");

            base.ExecuteOperation(operatorSymbol, operand);

            Append($"RES  ▶  {GetCurrentValue()}");
        }

        public override void Reset()
        {
            Append("RESET ▶  0");
            base.Reset();
        }

        // ── вспомогательный метод ────────────────────────────────────────────
        private void Append(string message)
        {
            var entry = $"[{DateTime.Now:HH:mm:ss}]  {message}";
            _log.Add(entry);
            LogAdded?.Invoke(this, entry);
        }
    }
}
