using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShopSystem.MVVM.Model
{
    internal class Account
    {
        private static int LastAssignID = -1;

        public int Id { get; set; }
        public string Name { get; set; }
        public string PIN { get; set; }
        public AccountType Type { get; private set; }

        [JsonConstructor]
        public Account(string name, string pin, AccountType type)
        {
            AssignNewId();
            Name = name;
            PIN = pin;
            Type = type;
        }

        public Account(int id, string name, string pin, AccountType type)
        {
            Id = id;
            Name = name;
            PIN = pin;
            Type = type;
        }

        public void ChangeData(string name, string pin, AccountType type)
        {
            Name = name;
            PIN = pin;
            Type = type;
        }

        public void AssignNewId()
        {
            Id = ++LastAssignID;
        }

        public static void ResetLastId()
        {
            LastAssignID = -1;
        }
    }

    enum AccountType
    {
        None = 0, //None
        Standart, //Only Receipt
        Serwis, //All except creating new users
        Admin //All
    }
}
