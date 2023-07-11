using ShopSystem.MVVM.Model;
using ShopSystem.MVVM.ViewModel.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ShopSystem.MVVM.ViewModel
{
    internal class RaportTabVM : BaseViewModel
    {
        #region Private Fields

        ShopSystemModel mainModel;

        private string _selectedAccount = string.Empty;
        private List<Receipt> receipts;
        private List<SimpleProduct> products;
        private decimal _price = decimal.Zero;
        private int _count = 0;

        #endregion
        #region Data Binding

        public ReadOnlyCollection<string> Accounts
        {
            get
            {
                List<string> list = mainModel.AccountsName;
                if (list.Count == 0) { list.Add("DEFAULT"); }
                return list.AsReadOnly();
            }
        }

        public string SelectedAccount
        {
            get { return _selectedAccount; }
            set
            {
                _selectedAccount = value;
                products = new List<SimpleProduct>();
                LoadRaport(value);
                OnPropertyChanged(nameof(Products));
                OnPropertyChanged(nameof(SelectedAccount));
            }
        }

        public string Price
        {
            get { return $"Total: {_price}$"; }
        }

        public string ReceiptCount
        {
            get { return $"Receipts: {_count}"; }
        }

        public ReadOnlyCollection<SimpleProduct> Products
        {
            get { return products.AsReadOnly(); }
        }

        #endregion
        #region Commands

        public ICommand Refresh { get; }
        public ICommand Save { get; }

        private void ExecuteRefresh(object parameter)
        {
            products = new List<SimpleProduct>();
            LoadRaport(SelectedAccount);
            OnPropertyChanged(nameof(Products));
        }
        private void ExecuteSave(object parameter)
        {
            if (SelectedAccount == string.Empty) return;

            string text = string.Empty;
            text += $"Account Name: {SelectedAccount}\n";
            text += $"{Price}\n";
            text += $"{ReceiptCount}\n";

            int count = 0;
            foreach (SimpleProduct element in products)
            {
                text += $"Product {count++}: {element.Name} | {element.Quantity} | {element.Vat} | {element.Brutto} | {element.FullBrutto}\n";
            }

            File.WriteAllText($"{SelectedAccount}Raport.txt", text);
            MessageBox.Show("Plik został utworzony.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion
        #region Functions

        private void LoadRaport(string accountName)
        {
            receipts = mainModel.Receipts.Where(r => r.AccountName.Equals(accountName)).ToList();
            if (receipts == null) return;

            _price = receipts.Sum(r => r.Price);
            _count = receipts.Count();

            LoadProducts();

            OnPropertyChanged(nameof(Price));
            OnPropertyChanged(nameof(ReceiptCount));
        }

        private void LoadProducts()
        {
            List<SimpleProduct> productList = new List<SimpleProduct>();
            foreach (Receipt receipt in receipts)
            foreach (SimpleProduct element in receipt.ProductList)
                {
                    SimpleProduct? result = products.FirstOrDefault(p => CheckProducts(p, element));
                    if (result == null) { products.Add(element); }
                    else { result.AddQuantity(element.Quantity); }
                }

            OnPropertyChanged(nameof(Products));
        }

        private bool CheckProducts(SimpleProduct el1, SimpleProduct el2)
        {
            if (!el1.Name.Equals(el2.Name)) return false;
            if (!el1.Brutto.Equals(el2.Brutto)) return false;
            return true;
        }

        #endregion

        public RaportTabVM()
        {
            mainModel = ShopSystemModel.GetInstance();
            products = new List<SimpleProduct>();

            Refresh = new BaseCommand(ExecuteRefresh);
            Save = new BaseCommand(ExecuteSave);
        }
    }
}
