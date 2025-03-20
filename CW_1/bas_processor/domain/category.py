from pydantic import BaseModel

from utils.bas_utils.transaction import TransactionType


class Category(BaseModel):
    name: str
    type: TransactionType
