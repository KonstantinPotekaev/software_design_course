import uuid

from pydantic import BaseModel
from typing import Optional


class BankAccountBase(BaseModel):
    name: str
    balance: float = 0.0


class BankAccountCreate(BankAccountBase):
    pass


class BankAccountUpdate(BaseModel):
    name: Optional[str]
    balance: Optional[float]


class BankAccountRead(BankAccountBase):
    id: uuid.UUID

    class Config:
        from_attributes = True
