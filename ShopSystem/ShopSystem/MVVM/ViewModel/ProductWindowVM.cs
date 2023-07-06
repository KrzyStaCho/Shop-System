using ShopSystem.MVVM.Model;
using ShopSystem.MVVM.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShopSystem.MVVM.ViewModel
{
    class ProductWindowVM : BaseViewModel
    {
        #region PrivateFields

        public Product? editProduct;
        public bool Status;

        private string _idBox = string.Empty;
        private string _nameBox = string.Empty;
        private string _quantityBox = string.Empty;
        private string _nettoBox = string.Empty;
        private string _vatBox = string.Empty;
        private string _bruttoBox = string.Empty;
        private string _errorBox = string.Empty;

        private static readonly string EmptyMessage = "cannot be empty";
        private static readonly string NotNumberDecimalMessage = "isn't a number. Use only number and one ','.";
        private static readonly string NotNumberIntMessage = "isn't a number. Use only number.";

        #endregion
        #region DataBinding

        public string IdBox
        {
            get { return _idBox; }
            set
            {
                _idBox = value;
                OnPropertyChanged(nameof(IdBox));
            }
        }

        public string NameBox
        {
            get { return _nameBox; }
            set
            {
                _nameBox = value;
                OnPropertyChanged(nameof(NameBox));
            }
        }
        public string QuantityBox
        {
            get { return _quantityBox; }
            set
            {
                _quantityBox = value;
                OnPropertyChanged(nameof(QuantityBox));
            }
        }
        public string NettoBox
        {
            get { return _nettoBox; }
            set
            {
                _nettoBox = value;
                OnPropertyChanged(nameof(NettoBox));
            }
        }
        public string VatBox
        {
            get { return _vatBox; }
            set
            {
                _vatBox = value;
                OnPropertyChanged(nameof(VatBox));
            }
        }
        public string BruttoBox
        {
            get { return _bruttoBox; }
            set
            {
                _bruttoBox = value;
                OnPropertyChanged(nameof(BruttoBox));
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

        #endregion
        #region Command

        public ICommand GetBrutto { get; }
        public ICommand Submit { get; }

        private void ExecuteGetBrutto(object parameter)
        {
            if (!CheckParseDecimal("Netto ", NettoBox)) return;
            if (!CheckParseInt("Vat ", VatBox)) return;

            decimal netto = decimal.Parse(NettoBox);
            int vat = int.Parse(VatBox);

            BruttoBox = Product.CalculateBrutto(netto, vat).ToString();
        }

        private void ExecuteSubmit(object parameter)
        {
            if (!CheckString("Name", NameBox)) return;
            if (!CheckParseInt("Quantity", QuantityBox)) return;
            if (!CheckParseDecimal("Netto ", NettoBox)) return;
            if (!CheckParseInt("Vat ", VatBox)) return;

            int quantity = int.Parse(QuantityBox);
            decimal netto = decimal.Parse(NettoBox);
            int vat = int.Parse(VatBox);

            if (editProduct == null)
                editProduct = new Product(NameBox, quantity, netto, vat);
            else editProduct.ChangeData(NameBox, quantity, netto, vat);
            Status = true;
            CloseCommand.Execute(this);
        }

        #endregion
        #region Functions

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
            IdBox = editProduct.Id.ToString();
            NameBox = editProduct.Name.ToString();
            QuantityBox = editProduct.Quantity.ToString();
            NettoBox = editProduct.Netto.ToString();
            VatBox = editProduct.Vat.ToString();
            BruttoBox = editProduct.Brutto.ToString();
        }

        #endregion

        public ProductWindowVM(Product product = null)
        {
            editProduct = product;
            if (editProduct != null)
            {
                LoadProductPropertiesToFields();
            }
            Status = false;

            GetBrutto = new BaseCommand(ExecuteGetBrutto);
            Submit = new BaseCommand(ExecuteSubmit);
        }
    }
}
