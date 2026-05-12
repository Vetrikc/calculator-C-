using System;
using SimpleCalculatorMVVM_4.Models;

namespace SimpleCalculatorMVVM_4.Commands
{
    /// <summary>
    /// Команда арифметической операции (паттерн Command).
    /// Принимает ICalculatorReceiver — благодаря этому команда работает
    /// с любой цепочкой декораторов, не зная о конкретных реализациях.
    /// </summary>
    public class CalculatorCommand : ICommandPattern
    {
        private readonly ICalculatorReceiver _receiver;   // ← интерфейс, не конкретный класс
        private readonly string _operator;
        private readonly double _operand;
        private double _previousValue;
        private bool _isPending;

        public CalculatorCommand(ICalculatorReceiver receiver, string operatorSymbol, double operand)
        {
            _receiver = receiver;
            _operator = operatorSymbol;
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
            if (_operator == "/" && Math.Abs(_operand) < 0.0001)
                return false;
            return true;
        }
    }
}
