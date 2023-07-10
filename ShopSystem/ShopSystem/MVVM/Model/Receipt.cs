using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace ShopSystem.MVVM.Model
{
    internal class Receipt
    {
        private static int LastAssignID = -1;

        public int Id { get; set; }
        public Company SellerCompany { get; set; }
        public List<SimpleProduct> ProductList { get; set; }
        public string AccountName { get; set; }
        public decimal VatPrice { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

        public Receipt(Company company, List<SimpleProduct> products, string accountName)
        {
            AssignNewId();
            SellerCompany = company;
            ProductList = products;
            VatPrice = GetVatPrice(products);
            Price = GetPrice(products);
            Date = DateTime.Now;
            AccountName = accountName;
        }

        public void AssignNewId()
        {
            Id = ++LastAssignID;
        }

        public static decimal GetVatPrice(List<SimpleProduct> products)
        {
            return products.Sum(p => GetVatPrice(p));
        }

        public static decimal GetPrice(List<SimpleProduct> products)
        {
            return products.Sum(p => p.FullBrutto);
        }

        public static decimal GetVatPrice(SimpleProduct product)
        {
            double vatProcent = (product.Vat / 100.0);
            decimal netto = (decimal)(decimal.ToDouble(product.FullBrutto) / (1.0 + vatProcent));
            return (product.FullBrutto - netto);
        }
    }
}
