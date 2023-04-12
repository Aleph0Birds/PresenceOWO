using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PresenceOWO.ViewModels
{
    public class VMCommand : ICommand
    {
        private static Regex urlRegEx = new Regex(@"((https?|\w+)://)?(www\.|\w+\.)?\w+\.\w+.*");
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
            try
            { 
                return parameter==null || canExecuteAction(parameter);
            } 
            catch (NullReferenceException e)
            {
                MessageBox.Show(e.StackTrace, $"{nameof(canExecuteAction)} is null");
                return false;
            }
        }

        public void Execute(object parameter)
        {
            executeAction(parameter);
        }

        public static bool NotNullAs<T>(object param) where T : class => !IsNullAs<T>(param);

        public static bool IsNullAs<T>(object param) where T : class => (param as T) == null;

        public static bool NotNull(object param) => !IsNull(param);

        public static bool IsNull(object param) => param == null;

        public static bool IsUrlLike(object param)
        {
            if(IsNullAs<string>(param)) return false;
            string link = param as string;
            if (string.IsNullOrEmpty(link)) return false; 
            return urlRegEx.IsMatch(link);
        }
    }
}
