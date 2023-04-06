using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PresenceOWO.ViewModels
{
    public class VMCommand : ICommand
    {
        private readonly Action<object> executeAction;
        private readonly Predicate<object> canExecuteAction;

        public VMCommand(Action<object> executeAction)
        {
            this.executeAction = executeAction;
            canExecuteAction = null;
        }

        public VMCommand(Action<object> executeAction, Predicate<object> canExecuteAction)
        {
            this.executeAction = executeAction;
            this.canExecuteAction = canExecuteAction;
        }



        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        public bool CanExecute(object parameter)
        {
            return parameter==null || canExecuteAction(parameter);
        }

        public void Execute(object parameter)
        {
            executeAction(parameter);
        }
    }
}
