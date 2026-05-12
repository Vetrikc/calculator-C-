namespace SimpleCalculatorMVVM.Models
{
    public class CalculatorModel
    {
        private double _firstOperand = 0;
        private double _secondOperand = 0;
        private string _operator = "";
        private double _result = 0;

        public double FirstOperand
        {
            get => _firstOperand;
            set => _firstOperand = value;
        }

        public double SecondOperand
        {
            get => _secondOperand;
            set => _secondOperand = value;
        }

        public string CurrentOperator
        {
            get => _operator;
            set => _operator = value;
        }

        public double Result
        {
            get => _result;
            set => _result = value;
        }

        public double Calculate(double firstOperand, double secondOperand, string operatorSymbol)
        {
            return operatorSymbol switch
            {
                "+" => firstOperand + secondOperand,
                "-" => firstOperand - secondOperand,
                "*" => firstOperand * secondOperand,
                "/" => secondOperand != 0 ? firstOperand / secondOperand : throw new ArgumentException("Деление на ноль"),
                _ => throw new ArgumentException($"Неизвестный оператор: {operatorSymbol}")
            };
        }

        public void Reset()
        {
            _firstOperand = 0;
            _secondOperand = 0;
            _operator = "";
            _result = 0;
        }
    }
}
