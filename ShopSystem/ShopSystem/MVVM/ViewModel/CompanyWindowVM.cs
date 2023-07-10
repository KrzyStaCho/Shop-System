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
    internal class CompanyWindowVM : BaseViewModel
    {
        #region Private Fields

        ShopSystemModel mainModel;

        private string _name = string.Empty;
        private string _nip = string.Empty;
        private string _error = string.Empty;

        private static readonly string EmptyMessage = "cannot be empty";
        private static readonly string NotNumberIntMessage = "isn't a number. Use only number.";

        #endregion
        #region Data Binding

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Nip
        {
            get { return _nip; }
            set
            {
                _nip = value;
                OnPropertyChanged(nameof(Nip));
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
            if (!CheckString("Name", Name)) return;
            if (!CheckParseInt("NIP", Nip)) return;
            mainModel.SetCompanyData(new Company(Name, Nip));
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

        public CompanyWindowVM()
        {
            mainModel = ShopSystemModel.GetInstance();

            Submit = new BaseCommand(ExecuteSubmit);
        }
    }
}
