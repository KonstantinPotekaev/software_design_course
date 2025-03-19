import uuid
from sqlalchemy import Column, String, Float, ForeignKey, DateTime
from sqlalchemy.dialects.postgresql import UUID
from sqlalchemy.orm import declarative_base, relationship
from sqlalchemy.sql import func

from bas_processor.common.const.db import (
    BANK_ACCOUNTS_TABLE, CATEGORIES_TABLE, OPERATIONS_TABLE,
    FK_BANK_ACCOUNT, FK_CATEGORY,
    BANK_ACCOUNT_MODEL, CATEGORY_MODEL, OPERATION_MODEL,
    REL_OPERATION_BANK_ACCOUNT, REL_BANK_ACCOUNT_OPERATIONS,
    REL_OPERATION_CATEGORY, REL_CATEGORY_OPERATIONS,
    DEFAULT_BALANCE,
)

Base = declarative_base()


class BankAccountModel(Base):
    __tablename__ = BANK_ACCOUNTS_TABLE

    id = Column(UUID(as_uuid=True), primary_key=True, index=True, default=uuid.uuid4)
    name = Column(String, nullable=False)
    balance = Column(Float, default=DEFAULT_BALANCE)

    operations = relationship(OPERATION_MODEL, back_populates=REL_OPERATION_BANK_ACCOUNT)


class CategoryModel(Base):
    __tablename__ = CATEGORIES_TABLE

    id = Column(UUID(as_uuid=True), primary_key=True, index=True, default=uuid.uuid4)
    type = Column(String, nullable=False)
    name = Column(String, nullable=False)

    operations = relationship(OPERATION_MODEL, back_populates=REL_OPERATION_CATEGORY)


class OperationModel(Base):
    __tablename__ = OPERATIONS_TABLE

    id = Column(UUID(as_uuid=True), primary_key=True, index=True, default=uuid.uuid4)
    type = Column(String, nullable=False)
    bank_account_id = Column(UUID(as_uuid=True), ForeignKey(FK_BANK_ACCOUNT), nullable=False)
    amount = Column(Float, nullable=False)
    date = Column(DateTime(timezone=True), server_default=func.now())
    description = Column(String, nullable=True)
    category_id = Column(UUID(as_uuid=True), ForeignKey(FK_CATEGORY), nullable=True)

    bank_account = relationship(BANK_ACCOUNT_MODEL, back_populates=REL_BANK_ACCOUNT_OPERATIONS)
    category = relationship(CATEGORY_MODEL, back_populates=REL_CATEGORY_OPERATIONS)
