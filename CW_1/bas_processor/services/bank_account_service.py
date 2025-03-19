import uuid

from fastapi import Depends, HTTPException
from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession

from bas_processor.db.session import get_session
from bas_processor.db.models import BankAccountModel
from bas_processor.schemas.bank_account import BankAccountCreate, BankAccountUpdate


class BankAccountService:
    def __init__(self, session: AsyncSession = Depends(get_session)):
        self.session = session

    async def create_bank_account(self, data: BankAccountCreate):
        new_account = BankAccountModel(**data.model_dump())
        self.session.add(new_account)
        await self.session.commit()
        return new_account

    async def get_all_bank_accounts(self):
        result = await self.session.execute(select(BankAccountModel))
        return result.scalars().all()

    async def get_bank_account_by_id(self, account_id: uuid.UUID):
        result = await self.session.execute(
            select(BankAccountModel).where(BankAccountModel.id == account_id)
        )
        account = result.scalar_one_or_none()
        if not account:
            raise HTTPException(status_code=404, detail="Bank account not found")
        return account

    async def update_bank_account(self, account_id: uuid.UUID, data: BankAccountUpdate):
        account = await self.get_bank_account_by_id(account_id)
        update_data = data.model_dump(exclude_unset=True)
        for field, value in update_data.items():
            setattr(account, field, value)
        self.session.add(account)
        await self.session.commit()
        return account

    async def delete_bank_account(self, account_id: uuid.UUID):
        account = await self.get_bank_account_by_id(account_id)
        async with self.session.begin():
            await self.session.delete(account)
