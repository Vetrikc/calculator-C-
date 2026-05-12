using SimpleCalculatorMVVM_4.Models;

namespace SimpleCalculatorMVVM_4.Commands
{
    public class CalculatorCommand : ICommandPattern
    {
        private readonly CalculatorReceiver _receiver;
        private readonly string _operator;
        private readonly double _operand;
        private double _previousValue;
        private bool _isPending;

        public CalculatorCommand(CalculatorReceiver receiver, string operator_symbol, double operand)
        {
            _receiver = receiver;
            _operator = operator_symbol;
            _operand = operand;
        }

        public void Execute()
        {
            _previousValue = _receiver.GetCurrentValue();
            _receiver.ExecuteOperation(_operator, _operand);
            _isPending = true;
        }

        public void Undo()
        {
            if (_isPending)
            {
                _receiver.SetValue(_previousValue);
                _isPending = false;
            }
        }

        public bool CanExecute()
        {
            // Проверка деления на ноль
            if (_operator == "/" && Math.Abs(_operand) < 0.0001)
                return false;
            return true;
        }
    }
}