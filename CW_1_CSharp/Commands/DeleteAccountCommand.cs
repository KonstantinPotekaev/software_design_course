using CW_1_CSharp.Facades;
using System;

namespace CW_1_CSharp.Commands
{
    public class DeleteAccountCommand : ICommand
    {
        private readonly BankAccountFacade _facade;
        private readonly int _accountId;

        public DeleteAccountCommand(BankAccountFacade facade, int accountId)
        {
            _facade = facade;
            _accountId = accountId;
        }

        public void Execute()
        {
            _facade.DeleteAccount(_accountId);
            Console.WriteLine("Счёт удалён.");
        }
    }
}