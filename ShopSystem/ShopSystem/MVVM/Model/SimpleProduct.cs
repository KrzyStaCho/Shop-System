﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ShopSystem.MVVM.Model
{
    class SimpleProduct
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Vat { get; set; }
        public decimal Brutto { get; set; }
        public decimal FullBrutto { get; set; }

        public SimpleProduct(string name, int quantity,  int vat, decimal brutto)
        {
            Name = name;
            Quantity = quantity;
            Vat = vat;
            Brutto = brutto;
            FullBrutto = GetFullBrutto(brutto, quantity);
        }

        public static decimal GetFullBrutto(decimal brutto, int quantity)
        {
            return (brutto * quantity);
        }

        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
            FullBrutto = GetFullBrutto(Brutto, Quantity);
        }
    }
}
