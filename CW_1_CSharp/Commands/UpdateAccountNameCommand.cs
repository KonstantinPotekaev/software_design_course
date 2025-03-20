using CW_1_CSharp.Facades;
using System;

namespace CW_1_CSharp.Commands
{
    public class UpdateAccountNameCommand : ICommand
    {
        private readonly BankAccountFacade _facade;
        private readonly int _accountId;
        private readonly string _newName;

        public UpdateAccountNameCommand(BankAccountFacade facade, int accountId, string newName)
        {
            _facade = facade;
            _accountId = accountId;
            _newName = newName;
        }

        public void Execute()
        {
            _facade.UpdateAccountName(_accountId, _newName);
            Console.WriteLine("Имя счёта обновлено.");
        }
    }
}