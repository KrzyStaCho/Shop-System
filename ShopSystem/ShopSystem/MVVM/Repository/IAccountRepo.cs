using ShopSystem.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSystem.MVVM.Repository
{
    internal interface IAccountRepo
    {
        List<Account>? GetAccounts();
        void SetAccounts(List<Account> accounts);
    }
}
