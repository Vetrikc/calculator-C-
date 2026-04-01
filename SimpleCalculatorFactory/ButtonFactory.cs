namespace SimpleCalculatorFactory
{
    // =========================================================
    //  Базовый абстрактный класс кнопки
    // =========================================================
    public abstract class CalcButton
    {
        public abstract string Press();
    }

    // =========================================================
    //  Конкретные классы кнопок
    // =========================================================

    /// <summary>Цифровая кнопка (0–9)</summary>
    public class DigitButton : CalcButton
    {
        private readonly int _digit;
        public DigitButton(int digit) => _digit = digit;
        public override string Press() => _digit.ToString();
    }

    /// <summary>Операционная кнопка (+, -, *, /)</summary>
    public class OperatorButton : CalcButton
    {
        private readonly string _op;
        public OperatorButton(string op) => _op = op;
        public override string Press() => _op;
    }

    /// <summary>Кнопка «Равно»</summary>
    public class EqualsButton : CalcButton
    {
        public override string Press() => "=";
    }

    /// <summary>Кнопка «Очистить» (C)</summary>
    public class ClearButton : CalcButton
    {
        public override string Press() => "C";
    }

    /// <summary>Кнопка «Процент» (%)</summary>
    public class PercentButton : CalcButton
    {
        public override string Press() => "%";
    }

    /// <summary>Кнопка «Смена знака» (±)</summary>
    public class NegateButton : CalcButton
    {
        public override string Press() => "±";
    }

    /// <summary>Кнопка «Десятичная точка» (.)</summary>
    public class DotButton : CalcButton
    {
        public override string Press() => ".";
    }

    // =========================================================
    //  Factory Method
    //  Принимает строковый ключ и возвращает нужный объект кнопки.
    //  Чтобы добавить новый тип — достаточно дописать один case
    //  и создать новый класс выше. Остальной код не меняется.
    // =========================================================
    public static class ButtonFactory
    {
        public static CalcButton Create(string key)
        {
            if (int.TryParse(key, out int digit))
                return new DigitButton(digit);

            return key switch
            {
                "+" or "-" or "*" or "/" => new OperatorButton(key),
                "="  => new EqualsButton(),
                "C"  => new ClearButton(),
                "%"  => new PercentButton(),
                "±"  => new NegateButton(),
                "."  => new DotButton(),
                _    => throw new ArgumentException($"Неизвестная кнопка: {key}")
            };
        }
    }
}
