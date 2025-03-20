from pydantic import BaseModel, confloat
from typing import Optional

from utils.bas_utils.transaction import TransactionType


class Operation(BaseModel):
    type: TransactionType
    amount: confloat(ge=0)
    description: Optional[str] = None
