using System;

namespace SimpleCalculatorMVVM_4.Commands
{
    public interface ICommandPattern
    {
        void Execute();
        void Undo();
        bool CanExecute();
    }
}