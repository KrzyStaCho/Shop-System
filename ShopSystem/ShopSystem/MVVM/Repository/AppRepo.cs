using ShopSystem.MVVM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShopSystem.MVVM.Repository
{
    class AppRepo : IProductRepo, IDocumentRepo, IAccountRepo
    {
        private static readonly string productFileName = "Products.json";
        private static readonly string documentFileName = "Documents.json";
        private static readonly string accountFileName = "Accounts.json";
        private static readonly string companyFileName = "CompanyData.json";
        private static readonly JsonSerializerOptions _options =
            new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = true };

        #region IProductRepo

        public List<Product>? GetProducts()
        {
            if (File.Exists(productFileName))
            {
                var jsonString = File.ReadAllText(productFileName);
                List<Product>? productList = JsonSerializer.Deserialize<List<Product>>(jsonString, _options);
                return productList;
            }
            else { return null; }
        }

        public void SetProducts(List<Product> products)
        {
            var jsonString = JsonSerializer.Serialize(products, _options);
            File.WriteAllText(productFileName, jsonString);
        }

        #endregion
        #region IDocumentRepo

        public List<Document>? GetDocuments()
        {
            if (File.Exists(documentFileName))
            {
                var jsonString = File.ReadAllText(documentFileName);
                List<Document>? documentList = JsonSerializer.Deserialize<List<Document>>(jsonString, _options);
                return documentList;
            }
            else { return null; }
        }

        public void SetDocuments(List<Document> documents)
        {
            var jsonString = JsonSerializer.Serialize(documents, _options);
            File.WriteAllText(documentFileName, jsonString);
        }

        #endregion
        #region IAccountRepo

        public List<Account>? GetAccounts()
        {
            if (File.Exists(accountFileName))
            {
                var jsonString = File.ReadAllText(accountFileName);
                List<Account>? accountList = JsonSerializer.Deserialize<List<Account>>(jsonString, _options);
                return accountList;
            }
            else { return null; }
        }

        public void SetAccounts(List<Account> accounts)
        {
            var jsonString = JsonSerializer.Serialize(accounts, _options);
            File.WriteAllText(accountFileName, jsonString);
        }

        #endregion
        #region ICompany

        public Company? GetCompany()
        {
            if (File.Exists(companyFileName))
            {
                var jsonString = File.ReadAllText(companyFileName);
                Company? company = JsonSerializer.Deserialize<Company>(jsonString, _options);
                return company;
            }
            else { return null; }
        }

        public void SetCompany(Company company)
        {
            var jsonString = JsonSerializer.Serialize(company, _options);
            File.WriteAllText(companyFileName, jsonString);
        }

        #endregion
    }
}
