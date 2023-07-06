﻿using System;
using System.Windows.Input;

namespace ShopSystem.MVVM.ViewModel.Core
{
    class BaseCommand : ICommand
    {
        private readonly Action<object> _command;
        private readonly Func<bool> _canExecute;

        public BaseCommand(Action<object> command, Func<bool> canExecute = null)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            _canExecute = canExecute;
            _command = command;
        }

        public void Execute(object parameter) {
            _command(parameter);
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null) return true;
            return _canExecute();
        }

        public event EventHandler CanExecuteChanged;
    }
}
