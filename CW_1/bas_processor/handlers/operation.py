from abc import ABC, abstractmethod
from bas_processor.domain.bank_account import BankAccount


class OperationHandler(ABC):
    @abstractmethod
    def apply(self, account: BankAccount, amount: float):
        pass


class IncomeOperationHandler(OperationHandler):
    def apply(self, account: BankAccount, amount: float):
        account.deposit(amount)


class ExpenseOperationHandler(OperationHandler):
    def apply(self, account: BankAccount, amount: float):
        account.withdraw(amount)
