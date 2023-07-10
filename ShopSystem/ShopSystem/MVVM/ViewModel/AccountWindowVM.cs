using ShopSystem.MVVM.Model;
using ShopSystem.MVVM.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShopSystem.MVVM.ViewModel
{
    internal class AccountWindowVM : BaseViewModel
    {
        #region Private Fields

        public Account? currentAccount;
        public bool Status;

        private string _name = string.Empty;
        private string _pin = string.Empty;
        private AccountType _type = AccountType.Standart;
        private string _error = string.Empty;

        private static readonly string EmptyMessage = "cannot be empty";
        private static readonly string NotNumberIntMessage = "isn't a number. Use only number, length must be a 4";
        private static readonly string NotLengthMessage = "must have only 4 digits";

        #endregion
        #region Data Binding

        public string NameBox
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(NameBox));
            }
        }

        public string PinBox
        {
            get { return _pin; }
            set
            {
                _pin = value;
                OnPropertyChanged(nameof(PinBox));
            }
        }

        public List<AccountType> TypeBox
        {
            get
            {
                var list = Enum.GetValues(typeof(AccountType)).Cast<AccountType>().ToList();
                list.Remove(AccountType.None);
                return list;
            }
        }

        public AccountType SelectedTypeBox
        {
            get { return _type; }
            set { _type = value; }
        }

        public string ErrorBox
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged(nameof(ErrorBox));
            }
        }

        #endregion
        #region Commands

        public ICommand Submit { get; }

        public void ExecuteSubmit(object parameter)
        {
            if (!CheckString("Name", NameBox)) return;
            if (!CheckParseInt("PIN ", PinBox)) return;
            if (PinBox.Length != 4)
            {
                ErrorBox = $"PIN {NotLengthMessage}";
                return;
            }
            ErrorBox = string.Empty;

            if (currentAccount == null)
                currentAccount = new Account(NameBox, PinBox, SelectedTypeBox);
            else currentAccount.ChangeData(NameBox, PinBox, SelectedTypeBox);
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

        private bool IsEmpty(string text)
        {
            if (string.IsNullOrEmpty(text)) return true;
            return false;
        }

        private void LoadAccountPropertiesToField()
        {
            NameBox = currentAccount.Name.ToString();
            PinBox = currentAccount.PIN.ToString();
            SelectedTypeBox = currentAccount.Type;
        }

        #endregion

        public AccountWindowVM(Account? account = null)
        {
            currentAccount = account;
            if (currentAccount != null)
            {
                LoadAccountPropertiesToField();
            }
            Status = false;
            Submit = new BaseCommand(ExecuteSubmit);
        }
    }
}
