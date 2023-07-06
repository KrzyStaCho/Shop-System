using ShopSystem.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSystem.MVVM.Repository
{
    interface IProductRepo
    {
        List<Product>? GetProducts();
        void SetProducts(List<Product> products);
    }
}
