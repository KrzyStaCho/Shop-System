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
    internal class DocumentTabVM : BaseViewModel
    {
        #region Private Fields

        private ShopSystemModel mainModel;

        #endregion
        #region Data Binding

        public ReadOnlyCollection<Document> Documents
        {
            get { return mainModel.Documents; }
        }

        #endregion
        #region Command

        public ICommand Refresh { get; }
        public ICommand AddDocument { get; }
        public ICommand ShowDocument { get; }
        public ICommand DeleteDocument { get; }

        private void ExecuteRefresh(object parameter)
        {
            mainModel.LoadDataFromFile();
            OnPropertyChanged(nameof(Documents));
        }
        private void ExecuteAddDocument(object parameter)
        {
            if (mainModel.CompanyData == null) return;

            var newWindow = new DocumentWindowView();
            var viewModel = new DocumentWindowVM(mainModel.CompanyData);
            viewModel.RequestClose += () => { newWindow.Close(); };
            newWindow.DataContext = viewModel;
            newWindow.ShowDialog();

            if (viewModel.Status)
            {
                mainModel.AddDocument(viewModel.currentDocument);
                OnPropertyChanged(nameof(Documents));
            }
        }
        private void ExecuteShowDocument(object parameter)
        {
            var document = parameter as Document;
            var newWindow = new DocumentWindowView();
            var viewModel = new DocumentWindowVM(mainModel.CompanyData, document);
            viewModel.RequestClose += () => { newWindow.Close(); };
            newWindow.DataContext = viewModel;
            newWindow.ShowDialog();
        }
        private void ExecuteDeleteDocument(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to delete document?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) { return; }

            var document = parameter as Document;
            mainModel.DeleteDocument(document);
            OnPropertyChanged(nameof(Documents));
        }

        #endregion
        #region Functions

        #endregion

        public DocumentTabVM()
        {
            mainModel = ShopSystemModel.GetInstance();

            Refresh = new BaseCommand(ExecuteRefresh);
            AddDocument = new BaseCommand(ExecuteAddDocument);
            ShowDocument = new BaseCommand(ExecuteShowDocument);
            DeleteDocument = new BaseCommand(ExecuteDeleteDocument);
        }
    }
}
