using CW_1_CSharp.Facades;
using System;

namespace CW_1_CSharp.Commands
{
    public class CreateOperationCommand : ICommand
    {
        private readonly OperationFacade _facade;
        private readonly string _type; // "income"/"expense"
        private readonly int _accountId;
        private readonly double _amount;
        private readonly string _dateStr; // "yyyy-MM-dd"
        private readonly int _categoryId;
        private readonly string _description;

        public CreateOperationCommand(OperationFacade facade,
            int accountId, double amount,
            string dateStr, int categoryId,
            string description)
        {
            _facade = facade;
            _accountId = accountId;
            _amount = amount;
            _dateStr = dateStr;
            _categoryId = categoryId;
            _description = description;
        }

        public void Execute()
        {
            _facade.CreateOperation(_accountId, _amount, _dateStr, _categoryId, _description);
            Console.WriteLine("Операция успешно создана.");
        }
    }
}