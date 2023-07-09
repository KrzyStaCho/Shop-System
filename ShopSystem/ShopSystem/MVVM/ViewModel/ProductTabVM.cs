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
    class ProductTabVM : BaseViewModel
    {
        #region PrivateFields

        private ShopSystemModel mainModel;

        #endregion
        #region DataBinding

        public ReadOnlyCollection<Product> Products
        {
            get { return mainModel.Products; }
        }

        #endregion
        #region Commands

        public ICommand ReloadList { get; }
        public ICommand AddProduct { get; }
        public ICommand EditProduct { get; }
        public ICommand DeleteProduct { get; }

        private void ExecuteReloadList(object parameter)
        {
            mainModel.LoadDataFromFile();
            OnPropertyChanged(nameof(Products));
        }

        private void ExecuteAddProduct(object parameter)
        {
            var newWindow = new ProductWindowView();
            var viewModel = new ProductWindowVM();
            viewModel.RequestClose += () => { newWindow.Close(); };
            newWindow.DataContext = viewModel;
            newWindow.ShowDialog();

            if (viewModel.Status)
            {
                mainModel.AddProduct(viewModel.editProduct);
                OnPropertyChanged(nameof(Products));
            }
        }

        private void ExecuteEditProduct(object parameter)
        {
            var newWindow = new ProductWindowView();
            var editProduct = parameter as Product;
            var viewModel = new ProductWindowVM(editProduct);
            viewModel.RequestClose += () => { newWindow.Close(); };
            newWindow.DataContext = viewModel;
            newWindow.ShowDialog();

            if (viewModel.Status)
            {
                mainModel.EditProduct(viewModel.editProduct);
                OnPropertyChanged(nameof(Products));
            }
        }

        private void ExecuteDeleteProduct(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to delete product?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) { return; }

            Product product = parameter as Product;
            mainModel.DeleteProduct(product);
            OnPropertyChanged(nameof(Products));
        }

        #endregion
        #region Functions

        #endregion

        public ProductTabVM()
        {
            mainModel = ShopSystemModel.GetInstance();

            ReloadList = new BaseCommand(ExecuteReloadList);
            AddProduct = new BaseCommand(ExecuteAddProduct);
            EditProduct = new BaseCommand(ExecuteEditProduct);
            DeleteProduct = new BaseCommand(ExecuteDeleteProduct);
        }
    }
}
