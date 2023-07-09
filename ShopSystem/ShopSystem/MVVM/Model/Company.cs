using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSystem.MVVM.Model
{
    internal class Company
    {
        public string Name { get; set; }
        public string NIP { get; set; }

        public Company(string name, string nip)
        {
            Name = name;
            NIP = nip;
        }
    }
}
