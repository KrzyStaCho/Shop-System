using ShopSystem.MVVM.Repository;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ShopSystem.MVVM.Model
{
    class ShopSystemModel
    {
        #region Fields

        private static ShopSystemModel? instance;
        private AppRepo appRepo;
        private List<Product> products;

        public ReadOnlyCollection<Product> Products { get { return products.AsReadOnly(); } }

        #endregion
        #region Functions

        public static ShopSystemModel GetInstance()
        {
            if (instance == null) { instance = new ShopSystemModel(); }
            return instance;
        }

        private void PostLoadedWork()
        {
            Product.ResetLastId();
            products.ForEach(p => { p.AssignNewId(); p.CalculateNewBrutto(); });
        }

        public void LoadProductsFromFile()
        {
            List<Product>? loadedList = appRepo.GetProducts();
            if (loadedList == null) products = new List<Product>();
            else products = loadedList;

            PostLoadedWork();
        }

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

        private ShopSystemModel()
        {
            appRepo = new AppRepo();
            LoadProductsFromFile();
        }
    }
}
