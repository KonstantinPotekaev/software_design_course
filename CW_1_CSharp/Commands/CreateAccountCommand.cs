using CW_1_CSharp.Facades;
using System;

namespace CW_1_CSharp.Commands
{
    public class CreateAccountCommand : ICommand
    {
        private readonly BankAccountFacade _facade;
        private readonly string _name;
        private readonly double _balance;

        public CreateAccountCommand(BankAccountFacade facade, string name, double balance)
        {
            _facade = facade;
            _name = name;
            _balance = balance;
        }

        public void Execute()
        {
            _facade.CreateAccount(_name, _balance);
            Console.WriteLine("Счёт успешно создан!");
        }
    }
}