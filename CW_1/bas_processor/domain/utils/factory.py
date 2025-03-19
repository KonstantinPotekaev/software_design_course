from bas_processor.common.struct.transaction import TransactionType
from bas_processor.domain.bank_account import BankAccount
from bas_processor.domain.category import Category
from bas_processor.domain.operation import Operation


class DomainFactory:
    @staticmethod
    def create_bank_account(name: str, balance: float = 0.0) -> BankAccount:
        return BankAccount(name, balance)

    @staticmethod
    def create_category(name: str, type: TransactionType) -> Category:
        return Category(name=name, type=type)

    @staticmethod
    def create_operation(type: TransactionType, amount: float, description: str = None) -> Operation:
        return Operation(type=type, amount=amount, description=description)
