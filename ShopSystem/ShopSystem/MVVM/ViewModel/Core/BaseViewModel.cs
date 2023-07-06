using System;
using System.ComponentModel;
using System.Windows.Input;

namespace ShopSystem.MVVM.ViewModel.Core
{
    class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action RequestClose;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Commands

        public ICommand CloseCommand { get; }

        public void ExecuteClose(object parameter)
        {
            if (RequestClose != null)
            {
                RequestClose();
            }
        }

        #endregion

        public BaseViewModel()
        {
            CloseCommand = new BaseCommand(ExecuteClose);
        }
    }
}
