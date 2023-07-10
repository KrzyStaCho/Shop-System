using ShopSystem.MVVM.Model;
using ShopSystem.MVVM.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ShopSystem.MVVM.ViewModel
{
    internal class LogInTabVM : BaseViewModel
    {
        #region Private Fields

        private ShopSystemModel mainModel;
        private MainWindowVM mainVM;

        private string _username = string.Empty;
        private string _pin = string.Empty;
        private string _error = string.Empty;

        #endregion
        #region Data Binding

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string PIN
        {
            get { return _pin; }
            set
            {
                _pin = value;
                OnPropertyChanged(nameof(PIN));
            }
        }

        public string ErrorBox
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged(nameof(ErrorBox));
            }
        }

        #endregion
        #region Commands

        public ICommand LogIn { get; }
        public ICommand Clear { get; }

        private void ExecuteLogIn(object parameter)
        {
            Account? account = mainModel.Accounts.FirstOrDefault(a => CheckAccount(a));
            if (account == null)
            {
                ErrorBox = "Incorrect username or password";
                return;
            }
            mainModel.SetAccount(account);
            mainVM.CorrectLogIn.Execute(account);
        }

        private void ExecuteClear(object parameter)
        {
            Username = string.Empty;
            PIN = string.Empty;
        }

        #endregion
        #region Functions

        private bool CheckAccount(Account account)
        {
            return (account.Name.Equals(Username) && account.PIN.Equals(PIN));
        }

        #endregion

        public LogInTabVM(MainWindowVM mainWindowVM)
        {
            mainVM = mainWindowVM;
            mainModel = ShopSystemModel.GetInstance();

            LogIn = new BaseCommand(ExecuteLogIn);
            Clear = new BaseCommand(ExecuteClear);
        }
    }
}
