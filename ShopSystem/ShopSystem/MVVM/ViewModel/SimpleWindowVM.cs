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
    internal class SimpleWindowVM : BaseViewModel
    {
        #region Private Fields

        private ShopSystemModel mainModel;
        public SimpleProduct currentProduct;
        public bool Status;

        private string _name = string.Empty;
        private string _quantity = string.Empty;
        private string _error = string.Empty;

        private static readonly string EmptyMessage = "cannot be empty";
        private static readonly string NotNumberIntMessage = "isn't a number. Use only number.";

        #endregion
        #region Data Binding

        public List<string> Name
        {
            get { return mainModel.Products.Select(p => p.Name).ToList(); }
        }

        public string SelectedName
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(SelectedName));
            }
        }

        public string Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged(nameof(Error));
            }
        }

        #endregion
        #region Commands

        public ICommand Submit { get; }

        private void ExecuteSubmit(object parameter)
        {
            if (!CheckString("Name", SelectedName)) return;
            if (!CheckParseInt("Quantity", Quantity)) return;
            Error = string.Empty;

            int quantity = int.Parse(Quantity);
            var product = mainModel.Products.FirstOrDefault(p => p.Name.Equals(SelectedName));

            currentProduct = new SimpleProduct(product.Name, quantity, product.Vat, product.Brutto);
            Status = true;
            CloseCommand.Execute(this);
        }

        #endregion
        #region Functions

        private bool CheckString(string prefix, string text)
        {
            if (IsEmpty(text))
            {
                Error = (prefix + " " + EmptyMessage);
                return false;
            }
            return true;
        }

        private bool CheckParseInt(string prefix, string text)
        {
            int parse;
            if (IsEmpty(text))
            {
                Error = (prefix + " " + EmptyMessage);
                return false;
            }
            if (!int.TryParse(text, out parse))
            {
                Error = (prefix + " " + NotNumberIntMessage);
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

        public SimpleWindowVM()
        {
            mainModel = ShopSystemModel.GetInstance();
            Status = false;

            Submit = new BaseCommand(ExecuteSubmit);
        }
    }
}
