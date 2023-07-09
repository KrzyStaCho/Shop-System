using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSystem.MVVM.Model
{
    internal class Document
    {
        private static int LastAssignID = -1;

        public int Id { get; private set; }
        public string City { get; set; }
        public DateTime Date { get; private set; }
        public Company Seller { get; set; }
        public Company Buyer { get; set; }
        public List<SimpleProduct> Products { get; set; }
        public decimal Price { get; private set; }

        public Document(string city, DateTime date, Company seller, Company buyer, List<SimpleProduct> products)
        {
            AssignNewId();
            City = city;
            Date = date;
            Seller = seller;
            Buyer = buyer;
            Products = products;
            GetFullPrice();
        }

        public void ChangeData(string city, DateTime date, Company seller, Company buyer, List<SimpleProduct> products)
        {
            City = city;
            Date = date;
            Seller = seller;
            Buyer = buyer;
            Products = products;
            GetFullPrice();
        }

        public void AssignNewId()
        {
            Id = ++LastAssignID;
        }

        public void GetFullPrice()
        {
            Price = Products.Sum(e => (e.Brutto * e.Quantity));
        }

        public static void ResetLastId()
        {
            LastAssignID = -1;
        }
    }
}
