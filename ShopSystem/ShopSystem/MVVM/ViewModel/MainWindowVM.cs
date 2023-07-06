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
            get { return "Ver 1.0"; }
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

        #endregion
        #region Commands

        public ICommand ShowProductTab { get; }
        public ICommand AddProduct { get; }

        private void ExecuteShowProductTab(object parameter)
        {
            if (CurrentChildView != null && CurrentChildView.GetType() == typeof(ProductTabView)) return;
            var view = new ProductTabView();
            view.DataContext = ProductTabViewModel;
            CurrentChildView = view;
        }

        private void ExecuteAddProduct(object parameter)
        {
            ProductTabViewModel.AddProduct.Execute(this);
        }

        #endregion
        #region Functions

        private BaseViewModel? FindInstanceVM(string instanceClassName)
        {
            var viewModel = baseViewModels.FirstOrDefault(e => e.GetType().Name == instanceClassName);
            return viewModel;
        }

        #endregion

        public MainWindowVM()
        {
            baseViewModels = new List<BaseViewModel>();
            mainModel = ShopSystemModel.GetInstance();
            AppStatus = true;

            ShowProductTab = new BaseCommand(ExecuteShowProductTab);
            AddProduct = new BaseCommand(ExecuteAddProduct);
            ShowProductTab.Execute(this);
        }
    }
}
