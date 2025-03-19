import uuid

from pydantic import BaseModel, confloat
from typing import Optional
from datetime import datetime

from bas_processor.common.struct.transaction import TransactionType


class OperationBase(BaseModel):
    type: TransactionType
    amount: confloat(ge=0)
    description: Optional[str] = None
    bank_account_id: uuid.UUID
    category_id: Optional[uuid.UUID] = None


class OperationCreate(OperationBase):
    pass


class OperationUpdate(BaseModel):
    type: Optional[str]
    amount: Optional[confloat(ge=0)]
    description: Optional[str]
    bank_account_id: uuid.UUID
    category_id: Optional[uuid.UUID]


class OperationRead(OperationBase):
    id: uuid.UUID
    date: datetime

    class Config:
        from_attributes = True
