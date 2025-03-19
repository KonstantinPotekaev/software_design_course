import uuid

from fastapi import Depends, HTTPException
from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession

from bas_processor.db.session import get_session
from bas_processor.db.models import OperationModel
from bas_processor.schemas.operation import OperationCreate, OperationUpdate


class OperationService:
    def __init__(self, session: AsyncSession = Depends(get_session)):
        self.session = session

    async def create_operation(self, data: OperationCreate):
        new_op = OperationModel(**data.model_dump())
        async with self.session.begin():
            self.session.add(new_op)
        return new_op

    async def get_all_operations(self):
        result = await self.session.execute(select(OperationModel))
        return result.scalars().all()

    async def get_operation_by_id(self, operation_id: uuid.UUID):
        result = await self.session.execute(
            select(OperationModel).where(OperationModel.id == operation_id)
        )
        operation = result.scalar_one_or_none()
        if not operation:
            raise HTTPException(status_code=404, detail="Operation not found")
        return operation

    async def update_operation(self, operation_id: uuid.UUID, data: OperationUpdate):
        operation = await self.get_operation_by_id(operation_id)
        update_data = data.model_dump(exclude_unset=True)
        for field, value in update_data.items():
            setattr(operation, field, value)
        async with self.session.begin():
            self.session.add(operation)
        return operation

    async def delete_operation(self, operation_id: uuid.UUID):
        operation = await self.get_operation_by_id(operation_id)
        async with self.session.begin():
            await self.session.delete(operation)
