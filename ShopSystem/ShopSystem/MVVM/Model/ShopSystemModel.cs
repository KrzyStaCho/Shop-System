using ShopSystem.MVVM.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ShopSystem.MVVM.Model
{
    class ShopSystemModel
    {
        #region Fields

        private static ShopSystemModel? instance;
        private AppRepo appRepo;
        private Account setAccount;
        private List<Product> products;
        private List<Document> documents;
        private List<Account> accounts;

        public ReadOnlyCollection<Product> Products { get { return products.AsReadOnly(); } }
        public ReadOnlyCollection<Document> Documents { get { return documents.AsReadOnly(); } }
        public ReadOnlyCollection<Account> Accounts { get { return accounts.AsReadOnly(); } }
        public AccountType SetAccountType { get { return setAccount.Type; } }

        #endregion
        #region Functions

        public static ShopSystemModel GetInstance()
        {
            if (instance == null) { instance = new ShopSystemModel(); }
            return instance;
        }

        private void PostLoadedWork()
        {
            //Product
            Product.ResetLastId();
            products.ForEach(p => { p.AssignNewId(); p.CalculateNewBrutto(); });

            //Documents
            Document.ResetLastId();
            documents.ForEach(d => { d.AssignNewId(); d.GetFullPrice(); });

            //Accounts
            Account.ResetLastId();
            accounts.ForEach(a => { a.AssignNewId(); });
        }

        public void LoadDataFromFile()
        {
            //Product
            List<Product>? loadedProducts = appRepo.GetProducts();
            if (loadedProducts == null) products = new List<Product>();
            else products = loadedProducts;

            //Document
            List<Document> loadedDocuments = appRepo.GetDocuments();
            if (loadedDocuments == null)  documents = new List<Document>();
            else documents = loadedDocuments;

            //Account
            List<Account> loadedAccounts = appRepo.GetAccounts();
            if (loadedAccounts == null) accounts = new List<Account>();
            else accounts = loadedAccounts;

            PostLoadedWork();
        }

        #region Product Function

        public void AddProduct(Product product)
        {
            products.Add(product);
            appRepo.SetProducts(products);
        }

        public void EditProduct(Product product)
        {
            products[product.Id] = product;
            appRepo.SetProducts(products);
        }

        public void DeleteProduct(Product product)
        {
            products.Remove(product);
            appRepo.SetProducts(products);
        }

        #endregion
        #region Document Function

        public void AddDocument(Document document)
        {
            documents.Add(document);
            DoDocument(document);
            appRepo.SetDocuments(documents);
        }

        public void EditDocument(Document document)
        {
            documents[document.Id] = document;
            appRepo.SetDocuments(documents);
        }

        public void DeleteDocument(Document document)
        {
            documents.Remove(document);
            appRepo.SetDocuments(documents);
        }

        public void DoDocument(Document document)
        {
            foreach (SimpleProduct element in document.Products)
            {
                var product = products.FirstOrDefault(p => p.Name == element.Name);
                if (product != null)
                {
                    product.AddQuantity(element.Quantity);
                }
            }
            appRepo.SetProducts(products);
        }

        #endregion
        #region Account Function

        public void AddAccount(Account account)
        {
            accounts.Add(account);
            appRepo.SetAccounts(accounts);
        }

        public void EditAccount(Account account)
        {
            try
            {
                accounts[account.Id] = account;
                appRepo.SetAccounts(accounts);
            } catch (Exception e)
            {
                MessageBox.Show("Cos");
            }
        }

        public void DeleteAccount(Account account)
        {
            accounts.Remove(account);
            appRepo.SetAccounts(accounts);
        }

        public void SetAccount(Account account)
        {
            setAccount = account;
        }

        #endregion

        #endregion

        private ShopSystemModel()
        {
            appRepo = new AppRepo();
            LoadDataFromFile();
        }
    }
}
