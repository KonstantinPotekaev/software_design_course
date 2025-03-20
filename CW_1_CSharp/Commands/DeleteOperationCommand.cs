using CW_1_CSharp.Facades;
using System;

namespace CW_1_CSharp.Commands
{
    public class DeleteOperationCommand : ICommand
    {
        private readonly OperationFacade _facade;
        private readonly int _operationId;

        public DeleteOperationCommand(OperationFacade facade, int operationId)
        {
            _facade = facade;
            _operationId = operationId;
        }

        public void Execute()
        {
            _facade.DeleteOperation(_operationId);
            Console.WriteLine("Операция удалена.");
        }
    }
}