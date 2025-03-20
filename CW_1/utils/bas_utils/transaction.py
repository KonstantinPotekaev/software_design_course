from enum import Enum

from bas_processor.common.const.transaction import INCOME, EXPENSE


class TransactionType(str, Enum):
    income = INCOME
    expense = EXPENSE
