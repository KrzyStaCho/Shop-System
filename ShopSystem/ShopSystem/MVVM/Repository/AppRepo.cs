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
    class AppRepo : IProductRepo, IDocumentRepo
    {
        private static readonly string productFileName = "Products.json";
        private static readonly string documentFileName = "Documents.json";
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
    }
}
