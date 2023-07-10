using ShopSystem.MVVM.Model;
using ShopSystem.MVVM.View;
using ShopSystem.MVVM.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ShopSystem.MVVM.ViewModel
{
    internal class AccountTabVM : BaseViewModel
    {
        #region Private Fields

        private ShopSystemModel mainModel;

        #endregion
        #region Data Binding

        public ReadOnlyCollection<Account> Accounts
        {
            get { return mainModel.Accounts; }
        }

        #endregion
        #region Commands

        public ICommand Refresh { get; }
        public ICommand AddAccount { get; }
        public ICommand EditAccount { get; }
        public ICommand DeleteAccount { get; }

        private void ExecuteRefresh(object parameter)
        {
            mainModel.LoadDataFromFile();
            OnPropertyChanged(nameof(Accounts));
        }

        private void ExecuteAddAccount(object parameter)
        {
            var newWindow = new AccountWindowView();
            var viewModel = new AccountWindowVM();
            viewModel.RequestClose += () => { newWindow.Close(); };
            newWindow.DataContext = viewModel;
            newWindow.ShowDialog();

            if (viewModel.Status)
            {
                mainModel.AddAccount(viewModel.currentAccount);
                OnPropertyChanged(nameof(Accounts));
            }
        }

        private void ExecuteEditAccount(object parameter)
        {
            var newWindow = new AccountWindowView();
            var editAccount = parameter as Account;
            var viewModel = new AccountWindowVM(editAccount);
            viewModel.RequestClose += () => { newWindow.Close(); };
            newWindow.DataContext = viewModel;
            newWindow.ShowDialog();

            if (viewModel.Status)
            {
                mainModel.EditAccount(viewModel.currentAccount);
                OnPropertyChanged(nameof(Accounts));
            }
        }

        private void ExecuteDeleteAccount(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to delete account?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) { return; }

            var account = parameter as Account;
            mainModel.DeleteAccount(account);
            OnPropertyChanged(nameof(Accounts));
        }

        #endregion
        #region Functions

        #endregion

        public AccountTabVM()
        {
            mainModel = ShopSystemModel.GetInstance();

            Refresh = new BaseCommand(ExecuteRefresh);
            AddAccount = new BaseCommand(ExecuteAddAccount);
            EditAccount = new BaseCommand(ExecuteEditAccount);
            DeleteAccount = new BaseCommand(ExecuteDeleteAccount);
        }
    }
}
