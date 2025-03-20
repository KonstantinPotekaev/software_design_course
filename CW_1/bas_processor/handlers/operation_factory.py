from bas_processor.handlers.operation import IncomeOperationHandler, ExpenseOperationHandler, OperationHandler
from utils.bas_utils.transaction import TransactionType


def get_operation_handler(op_type: TransactionType) -> OperationHandler:
    if op_type == TransactionType.income:
        return IncomeOperationHandler()
    elif op_type == TransactionType.expense:
        return ExpenseOperationHandler()
    else:
        raise ValueError("Unsupported operation type")
