using ShopSystem.MVVM.Model;
using ShopSystem.MVVM.View;
using ShopSystem.MVVM.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ShopSystem.MVVM.ViewModel
{
    class MainWindowVM : BaseViewModel
    {
        #region PrivateFields

        private List<BaseViewModel> baseViewModels;
        private bool appStatus;
        private FrameworkElement currentChildView;
        private ShopSystemModel mainModel;

        #endregion
        #region DataBinding

        public string LogoName
        {
            get { return "Shop System"; }
        }

        public string Version
        {
            get { return "Ver 4.0"; }
        }

        public bool AppStatus
        {
            get { return appStatus; }
            set
            {
                appStatus = value;
                OnPropertyChanged(nameof(ColorStatus));
            }
        }

        public Brush ColorStatus
        {
            get
            {
                if (AppStatus) return new SolidColorBrush(Colors.LightGreen);
                else return new SolidColorBrush(Colors.Red);
            }
        }

        public bool IsEnabled
        {
            get
            {
                return ((int)mainModel.SetAccountType >= 1);
            }
        }

        public FrameworkElement CurrentChildView
        {
            get { return currentChildView; }
            set
            {
                currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        public ProductTabVM ProductTabViewModel
        {
            get
            {
                BaseViewModel? viewModel = FindInstanceVM(typeof(ProductTabVM).Name);
                if (viewModel == null)
                {
                    viewModel = new ProductTabVM();
                    baseViewModels.Add(viewModel);
                }
                return viewModel as ProductTabVM;
            }
        }

        public DocumentTabVM DocumentTabViewModel
        {
            get
            {
                BaseViewModel? viewModel = FindInstanceVM(typeof(DocumentTabVM).Name);
                if (viewModel == null)
                {
                    viewModel = new DocumentTabVM();
                    baseViewModels.Add(viewModel);
                }
                return viewModel as DocumentTabVM;
            }
        }

        public AccountTabVM AccountTabViewModel
        {
            get
            {
                BaseViewModel? viewModel = FindInstanceVM(typeof(AccountTabVM).Name);
                if (viewModel == null)
                {
                    viewModel = new AccountTabVM();
                    baseViewModels.Add(viewModel);
                }
                return viewModel as AccountTabVM;
            }
        }

        public LogInTabVM LogInTabViewModel
        {
            get
            {
                BaseViewModel? viewModel = FindInstanceVM(typeof(LogInTabVM).Name);
                if (viewModel == null)
                {
                    viewModel = new LogInTabVM(this);
                    baseViewModels.Add(viewModel);
                }
                (viewModel as LogInTabVM).Clear.Execute(1);
                return viewModel as LogInTabVM;
            }
        }

        public CompanyWindowVM CompanyWindowViewModel
        {
            get
            {
                BaseViewModel? viewModel = FindInstanceVM(typeof(CompanyWindowVM).Name);
                if (viewModel == null)
                {
                    viewModel = new CompanyWindowVM();
                    baseViewModels.Add(viewModel);
                }
                return viewModel as CompanyWindowVM;
            }
        }

        public ReceiptTabVM ReceiptTabViewModel
        {
            get
            {
                BaseViewModel? viewModel = FindInstanceVM(typeof(ReceiptTabVM).Name);
                if (viewModel == null)
                {
                    viewModel = new ReceiptTabVM();
                    baseViewModels.Add(viewModel);
                }
                return viewModel as ReceiptTabVM;
            }
        }

        #endregion
        #region Commands

        public ICommand CorrectLogIn { get; }
        public ICommand LogOut { get; }
        public ICommand ShowLogInTab { get; }
        public ICommand ShowProductTab { get; }
        public ICommand ShowDocumentTab { get; }
        public ICommand ShowReceiptTab { get; }
        public ICommand ShowAccountTab { get; }
        public ICommand AddProduct { get; }
        public ICommand AddDocument { get; }
        public ICommand AddAccount { get; }

        private void ExecuteCorrectLogIn(object parameter)
        {
            OnPropertyChanged(nameof(IsEnabled));
            CurrentChildView = null;
        }

        private void ExecuteLogOut(object parameter)
        {
            mainModel.SetAccount(new Account("NO ACCOUNT", "", AccountType.None));
            OnPropertyChanged(nameof(IsEnabled));
            ShowLogInTab.Execute(this);
        }

        private bool CanExecuteLogOut(object parameter)
        {
            return (mainModel.Accounts.Count != 0);
        }

        private void ExecuteShowLogInTab(object parameter)
        {
            if (CurrentChildView != null && CurrentChildView is LogInTabView) return;
            var view = new LogInTabView();
            view.DataContext = LogInTabViewModel;
            CurrentChildView = view;
        }

        private void ExecuteShowProductTab(object parameter)
        {
            if (!CanExecuteSerwisUser(1)) return;

            if (CurrentChildView != null && CurrentChildView is ProductTabView) return;
            var view = new ProductTabView();
            view.DataContext = ProductTabViewModel;
            CurrentChildView = view;
        }

        private void ExecuteShowDocumentTab(object parameter)
        {
            if (!CanExecuteSerwisUser(1)) return;

            if (CurrentChildView != null && CurrentChildView is DocumentTabView) return;
            var view = new DocumentTabView();
            view.DataContext = DocumentTabViewModel;
            CurrentChildView = view;
        }

        private void ExecuteShowReceiptTab(object parameter)
        {
            if (!CanExecuteStandartUser(1)) return;
            if (mainModel.CompanyData == null) return;

            if (CurrentChildView != null && CurrentChildView is ReceiptTabView) return;
            var view = new ReceiptTabView();
            view.DataContext = ReceiptTabViewModel;
            CurrentChildView = view;
        }

        private void ExecuteShowAccountTab(object parameter)
        {
            if (!CanExecuteAdminUser(1)) return;

            if (CurrentChildView != null && CurrentChildView is AccountTabVM) return;
            var view = new AccountTabView();
            view.DataContext = AccountTabViewModel;
            CurrentChildView = view;
        }

        private void ExecuteAddProduct(object parameter)
        {
            if (!CanExecuteSerwisUser(1)) return;

            ProductTabViewModel.AddProduct.Execute(this);
        }

        private void ExecuteAddDocument(object parameter)
        {
            if (!CanExecuteSerwisUser(1)) return;

            DocumentTabViewModel.AddDocument.Execute(this);
        }

        private void ExecuteAddAccount(object parameter)
        {
            if (!CanExecuteAdminUser(1)) return;

            AccountTabViewModel.AddAccount.Execute(this);
        }

        private bool CanExecuteStandartUser(object parameter)
        {
            return ((int)mainModel.SetAccountType >= (int)AccountType.Standart);
        }
        private bool CanExecuteSerwisUser(object parameter)
        {
            return ((int)mainModel.SetAccountType >= (int)AccountType.Serwis);
        }
        private bool CanExecuteAdminUser(object parameter)
        {
            return ((int)mainModel.SetAccountType >= (int)AccountType.Admin);
        }

        #endregion
        #region Functions

        private BaseViewModel? FindInstanceVM(string instanceClassName)
        {
            var viewModel = baseViewModels.FirstOrDefault(e => e.GetType().Name == instanceClassName);
            return viewModel;
        }

        private void CheckCompanyData()
        {
            if (mainModel.CompanyData == null)
            {
                var newWindow = new CompanyWindowView();
                var viewModel = new CompanyWindowVM();
                viewModel.RequestClose += () => { newWindow.Close(); };
                newWindow.DataContext = viewModel;
                newWindow.ShowDialog();
            }
        }

        #endregion

        public MainWindowVM()
        {
            baseViewModels = new List<BaseViewModel>();
            mainModel = ShopSystemModel.GetInstance();
            AppStatus = true;

            CorrectLogIn = new BaseCommand(ExecuteCorrectLogIn);
            LogOut = new BaseCommand(ExecuteLogOut, CanExecuteLogOut);
            ShowLogInTab = new BaseCommand(ExecuteShowLogInTab);
            ShowProductTab = new BaseCommand(ExecuteShowProductTab);
            ShowDocumentTab = new BaseCommand(ExecuteShowDocumentTab);
            ShowReceiptTab = new BaseCommand(ExecuteShowReceiptTab);
            ShowAccountTab = new BaseCommand(ExecuteShowAccountTab);
            AddProduct = new BaseCommand(ExecuteAddProduct);
            AddDocument = new BaseCommand(ExecuteAddDocument);
            AddAccount = new BaseCommand(ExecuteAddAccount);
            
            if (mainModel.Accounts.Count == 0)
            {
                mainModel.SetAccount(new Account(-1, "DEFAULT", "", AccountType.Admin));
            }
            else
            {
                mainModel.SetAccount(new Account(-1, "NO ACCOUNT", "", AccountType.None));
                ShowLogInTab.Execute(this);
            }

            CheckCompanyData();
        }
    }
}
