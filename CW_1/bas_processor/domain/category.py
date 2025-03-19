from pydantic import BaseModel

from bas_processor.common.struct.transaction import TransactionType


class Category(BaseModel):
    name: str
    type: TransactionType
