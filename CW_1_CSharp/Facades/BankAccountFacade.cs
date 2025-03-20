using System;
using System.Collections.Generic;
using CW_1_CSharp.Domain;
using CW_1_CSharp.Factories;
using CW_1_CSharp.Proxies;

namespace CW_1_CSharp.Facades
{
    public class BankAccountFacade
    {
        private readonly DataProxy _proxy;

        public BankAccountFacade(DataProxy proxy)
        {
            _proxy = proxy;
        }

        public void CreateAccount(string name, double balance)
        {
            var acc = DomainFactory.CreateBankAccount(name, balance);
            _proxy.AddBankAccount(acc);
        }

        public void UpdateAccountName(int accountId, string newName)
        {
            var acc = _proxy.GetBankAccount(accountId);
            if (acc == null)
            {
                throw new Exception("Счёт не найден.");
            }
            acc.Name = newName;
            _proxy.UpdateBankAccount(acc);
        }

        public void DeleteAccount(int accountId)
        {
            _proxy.DeleteBankAccount(accountId);
        }

        public List<BankAccount> ListAccounts()
        {
            return _proxy.GetAllBankAccounts();
        }
    }
}