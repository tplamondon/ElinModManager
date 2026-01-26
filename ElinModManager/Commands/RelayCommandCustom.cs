using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ElinModManager.Commands
{
    // This might not even be needed as we have community mvvm
    public class RelayCommandCustom : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private Action<object> _Execute {  get; set; }
        private Predicate<object> _CanExecutePredicate { get; set; }

        public RelayCommandCustom(Action<object> executeMethod, Predicate<object> CanExecuteMethod)
        {
            _Execute = executeMethod;
            _CanExecutePredicate = CanExecuteMethod;
        }

        public bool CanExecute(object? parameter)
        {
            return _CanExecutePredicate(parameter);
        }
        public void Execute(object? parameter)
        {
            _Execute(parameter);
        }
    }
}
