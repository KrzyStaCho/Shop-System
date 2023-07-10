using System;

namespace ShopSystem.MVVM.Model
{
    class Product
    {
        private static int LastAssignID = -1;

        public int Id { get; private set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Netto { get; set; }
        public int Vat { get; set; }
        public decimal Brutto { get; private set; }

        public Product(string name, int quantity, decimal netto, int vat)
        {
            AssignNewId();
            Name = name;
            Quantity = quantity;
            Netto = netto;
            Vat = vat;
            CalculateNewBrutto();
        }

        public void AssignNewId()
        {
            Id = ++LastAssignID;
        }

        public void ChangeData(string name, int quantity, decimal netto, int vat)
        {
            Name = name;
            Quantity = quantity;
            Netto = netto;
            Vat = vat;
            CalculateNewBrutto();
        }

        public void CalculateNewBrutto()
        {
            Brutto = Math.Round((decimal)(decimal.ToDouble(Netto) * (1 + (Vat / 100.0))), 2);
        }

        public static decimal CalculateBrutto(decimal netto, int vat)
        {
            return Math.Round((decimal)(decimal.ToDouble(netto) * (1 + (vat / 100.0))), 2);
        }

        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
        }

        public void RemoveQuantity(int quantity)
        {
            Quantity -= quantity;
        }

        public static void ResetLastId()
        {
            LastAssignID = -1;
        }
    }
}
