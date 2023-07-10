using ShopSystem.MVVM.Model;
using ShopSystem.MVVM.View;
using ShopSystem.MVVM.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShopSystem.MVVM.ViewModel
{
    internal class ReceiptTabVM : BaseViewModel
    {
        #region Private Fields

        ShopSystemModel mainModel;

        private decimal _price = decimal.Zero;
        private decimal _vatPrice = decimal.Zero;
        private List<SimpleProduct> _products;

        #endregion
        #region Data Binding

        public Company Seller
        {
            get { return mainModel.CompanyData; }
        }
        public string Price
        {
            get { return $"Total: {_price}$"; }
        }
        public string VatPrice
        {
            get { return $"Vat: {Math.Round(_vatPrice, 2)}$"; }
        }
        public ReadOnlyCollection<SimpleProduct> Products
        {
            get { return _products.AsReadOnly(); }
        }

        #endregion
        #region Commands

        public ICommand DeleteProduct { get; }
        public ICommand AddProduct { get; }
        public ICommand Submit { get; }
        
        private void ExecuteDeleteProduct(object parameter)
        {
            var product = (SimpleProduct)parameter;
            _price -= product.FullBrutto;
            _vatPrice -= Receipt.GetVatPrice(product);

            _products.Remove(product);
            OnPropertyChanged(nameof(Products));
            OnPropertyChanged(nameof(VatPrice));
            OnPropertyChanged(nameof(Price));
        }
        private void ExecuteAddProduct(object parameter)
        {
            var newWindow = new SimpleWindowView();
            var viewModel = new SimpleWindowVM();
            viewModel.RequestClose += () => { newWindow.Close(); };
            newWindow.DataContext = viewModel;
            newWindow.ShowDialog();

            if (viewModel.Status)
            {
                _price += viewModel.currentProduct.FullBrutto;
                _vatPrice += Receipt.GetVatPrice(viewModel.currentProduct);

                _products.Add(viewModel.currentProduct);
                OnPropertyChanged(nameof(Products));
                OnPropertyChanged(nameof(VatPrice));
                OnPropertyChanged(nameof(Price));
            }
        }
        private void ExecuteSubmit(object parameter)
        {
            if (_products.Count == 0) return;
            mainModel.AddReceipt(_products);
            _products = new List<SimpleProduct>();
            _price = 0;
            _vatPrice = 0;

            OnPropertyChanged(nameof(Products));
            OnPropertyChanged(nameof(VatPrice));
            OnPropertyChanged(nameof(Price));
        }

        #endregion
        #region Functions

        #endregion

        public ReceiptTabVM()
        {
            mainModel = ShopSystemModel.GetInstance();
            _products = new List<SimpleProduct>();

            DeleteProduct = new BaseCommand(ExecuteDeleteProduct);
            AddProduct = new BaseCommand(ExecuteAddProduct);
            Submit = new BaseCommand(ExecuteSubmit);
        }
    }
}
