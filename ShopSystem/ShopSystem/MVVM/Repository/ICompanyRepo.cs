using ShopSystem.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSystem.MVVM.Repository
{
    internal interface ICompanyRepo
    {
        Company? GetCompany();
        void SetCompany(Company company);
    }
}
