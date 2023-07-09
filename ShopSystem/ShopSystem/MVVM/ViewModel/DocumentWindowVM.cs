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
    class DocumentWindowVM : BaseViewModel
    {
        #region Private Fields

        public Document? currentDocument;
        public bool Status;

        private string _city = string.Empty;
        private string _date = string.Empty;
        
        private string _sellerName = string.Empty;
        private string _sellerNip = string.Empty;
        private string _buyerName = string.Empty;
        private string _buyerNip = string.Empty;

        private List<SimpleProduct> products;
        private decimal _price = decimal.Zero;
        private string _errorBox = string.Empty;

        private static readonly string EmptyMessage = "cannot be empty";
        private static readonly string NotNumberDecimalMessage = "isn't a number. Use only number and one ','.";
        private static readonly string NotNumberIntMessage = "isn't a number. Use only number.";
        private static readonly string NotDataMessage = "isn't a valid date. Use format YYYY-MM-DD.";

        #endregion
        #region DataBinding

        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        public string Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public string SellerName
        {
            get { return _sellerName; }
            set
            {
                _sellerName = value;
                OnPropertyChanged(nameof(SellerName));
            }
        }

        public string SellerNip
        {
            get { return _sellerNip; }
            set
            {
                _sellerNip = value;
                OnPropertyChanged(nameof(SellerNip));
            }
        }

        public string BuyerName
        {
            get { return _buyerName; }
            set
            {
                _buyerName = value;
                OnPropertyChanged(nameof(BuyerName));
            }
        }

        public string BuyerNip
        {
            get { return _buyerNip; }
            set
            {
                _buyerNip = value;
                OnPropertyChanged(nameof(BuyerNip));
            }
        }

        public ReadOnlyCollection<SimpleProduct> Products
        {
            get { return products.AsReadOnly(); }
        }

        public decimal Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public string ErrorBox
        {
            get { return _errorBox; }
            set
            {
                _errorBox = value;
                OnPropertyChanged(nameof(ErrorBox));
            }
        }

        public bool IsEnabled
        {
            get { return CanExecute(null); }
        }

        #endregion
        #region Commands

        public ICommand AddProduct { get; }
        public ICommand DeleteProduct { get; }
        public ICommand Submit { get; }

        public void ExecuteAddProduct(object parameter)
        {
            var newWindow = new SimpleProductWindow();
            var viewModel = new SimpleProductWindowVM();
            viewModel.RequestClose += () => { newWindow.Close(); };
            newWindow.DataContext = viewModel;
            newWindow.ShowDialog();

            if (viewModel.Status)
            {
                Price += (viewModel.SProduct.Brutto * viewModel.SProduct.Quantity);

                products.Add(viewModel.SProduct);
                OnPropertyChanged(nameof(Products));
            }
        }
        public void ExecuteDeleteProduct(object parameter)
        {
            var product = (SimpleProduct)parameter;

            Price -= (product.Brutto * product.Quantity);

            products.Remove(product);
            OnPropertyChanged(nameof(Products));
        }
        public void ExecuteSubmit(object parameter)
        {
            if (currentDocument == null)
            {
                if (!CheckString("City", City)) return;
                if (!CheckDateTime("Date", Date)) return;
                if (!CheckString("Seller Name", SellerName)) return;
                if (!CheckParseInt("Seller NIP", SellerNip)) return;
                if (!CheckString("Buyer Name", BuyerName)) return;
                if (!CheckParseInt("Buyer NIP", BuyerNip)) return;
                if (products.Count == 0)
                {
                    ErrorBox = $"Products {EmptyMessage}";
                    return;
                }

                DateTime dateTime = DateTime.Parse(Date);

                currentDocument = new Document(City, dateTime, new Company(SellerName, SellerNip), new Company(BuyerName, BuyerNip), products);
                Status = true;
            }
            CloseCommand.Execute(this);
        }

        private bool CanExecute(object parameter)
        {
            return (currentDocument == null);
        }

        #endregion
        #region Functions

        private bool CheckDateTime(string prefix, string text)
        {
            DateTime parse;
            if (IsEmpty(text))
            {
                ErrorBox = (prefix + " " + EmptyMessage);
                return false;
            }
            if (!DateTime.TryParse(text, out parse))
            {
                ErrorBox = (prefix + " " + NotDataMessage);
                return false;
            }
            return true;
        }

        private bool CheckString(string prefix, string text)
        {
            if (IsEmpty(text))
            {
                ErrorBox = (prefix + " " + EmptyMessage);
                return false;
            }
            return true;
        }

        private bool CheckParseInt(string prefix, string text)
        {
            int parse;
            if (IsEmpty(text))
            {
                ErrorBox = (prefix + " " + EmptyMessage);
                return false;
            }
            if (!int.TryParse(text, out parse))
            {
                ErrorBox = (prefix + " " + NotNumberIntMessage);
                return false;
            }
            return true;
        }

        private bool CheckParseDecimal(string prefix, string text)
        {
            decimal parse;
            if (IsEmpty(text))
            {
                ErrorBox = (prefix + " " + EmptyMessage);
                return false;
            }
            if (!decimal.TryParse(text, out parse))
            {
                ErrorBox = (prefix + " " + NotNumberDecimalMessage);
                return false;
            }
            return true;
        }

        private bool IsEmpty(string text)
        {
            if (string.IsNullOrEmpty(text)) return true;
            return false;
        }

        private void LoadProductPropertiesToFields()
        {
            City = currentDocument.City.ToString();
            Date = currentDocument.Date.ToString();
            SellerName = currentDocument.Seller.Name.ToString();
            SellerNip = currentDocument.Seller.NIP.ToString();
            BuyerName = currentDocument.Buyer.Name.ToString();
            BuyerNip = currentDocument.Buyer.NIP.ToString();
            products = currentDocument.Products;
            currentDocument.GetFullPrice();
            Price = currentDocument.Price;
            OnPropertyChanged(nameof(Products));
        }

        #endregion

        public DocumentWindowVM(Document? document = null)
        {
            currentDocument = document;
            if (currentDocument != null)
            {
                LoadProductPropertiesToFields();
            }
            else products = new List<SimpleProduct>();
            Status = false;

            AddProduct = new BaseCommand(ExecuteAddProduct, CanExecute);
            DeleteProduct = new BaseCommand(ExecuteDeleteProduct, CanExecute);
            Submit = new BaseCommand(ExecuteSubmit);
        }
    }
}
