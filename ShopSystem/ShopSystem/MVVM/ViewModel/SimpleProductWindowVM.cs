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
    class SimpleProductWindowVM : BaseViewModel
    {
        #region Private Fields

        public SimpleProduct SProduct;
        private ShopSystemModel mainModel;
        public bool Status;

        private string _nameBox = string.Empty;
        private string _quantityBox = string.Empty;
        private string _vatBox = string.Empty;
        private string _bruttoBox = string.Empty;
        private string _errorBox = string.Empty;

        private static readonly string EmptyMessage = "cannot be empty";
        private static readonly string NotNumberDecimalMessage = "isn't a number. Use only number and one ','.";
        private static readonly string NotNumberIntMessage = "isn't a number. Use only number.";

        #endregion
        #region Data Binding

        public List<string> NameBox
        {
            get { return mainModel.Products.Select(p => p.Name).ToList(); }
        }
        public string SelectedNameBox
        {
            get { return _nameBox; }
            set { _nameBox = value; }
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
        #region Commands

        public ICommand Submit { get; }

        public void ExecuteSubmit(object parameter)
        {
            if (!CheckParseInt("Quantity", QuantityBox)) return;
            if (!CheckParseInt("Vat ", VatBox)) return;
            if (!CheckParseDecimal("Brutto ", BruttoBox)) return;
            ErrorBox = string.Empty;

            int quantity = int.Parse(QuantityBox);
            decimal brutto = decimal.Parse(BruttoBox);
            int vat = int.Parse(VatBox);

            SProduct = new SimpleProduct(SelectedNameBox, quantity, vat, brutto);
            Status = true;
            CloseCommand.Execute(this);
        }

        #endregion
        #region Functions

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

        #endregion

        public SimpleProductWindowVM()
        {
            mainModel = ShopSystemModel.GetInstance();
            Submit = new BaseCommand(ExecuteSubmit);
            Status = false;
        }
    }
}
