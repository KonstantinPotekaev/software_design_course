import uuid
from fastapi import HTTPException, Depends
from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession

from bas_processor.db.session import get_session
from bas_processor.db.models import OperationModel, BankAccountModel, CategoryModel
from bas_processor.schemas.operation import OperationCreate, OperationUpdate
from bas_processor.domain.bank_account import BankAccount
from bas_processor.handlers.operation_factory import get_operation_handler
from utils.bas_utils.transaction import TransactionType

class OperationService:
    def __init__(self, session: AsyncSession = Depends(get_session)):
        self.session = session

    async def create_operation(self, data: OperationCreate) -> OperationModel:
        op_data = data.model_dump()

        # Извлекаем банковский счёт по bank_account_id
        result = await self.session.execute(
            select(BankAccountModel).where(BankAccountModel.id == op_data["bank_account_id"])
        )
        bank_account = result.scalar_one_or_none()
        if not bank_account:
            raise HTTPException(status_code=404, detail="Bank account not found")

        # Извлекаем категорию для определения типа операции
        if not op_data.get("category_id"):
            raise HTTPException(status_code=400, detail="Category must be provided")
        cat_result = await self.session.execute(
            select(CategoryModel).where(CategoryModel.id == op_data["category_id"])
        )
        category = cat_result.scalar_one_or_none()
        if not category:
            raise HTTPException(status_code=404, detail="Category not found")

        # Определяем тип операции на основе категории
        category_type = TransactionType(category.type)

        # Преобразуем ORM-счёт в доменную модель
        domain_account = BankAccount(name=bank_account.name, balance=bank_account.balance)

        # Получаем нужный обработчик (стратегию) по типу операции
        handler = get_operation_handler(category_type)
        try:
            handler.apply(domain_account, op_data["amount"])
        except ValueError as e:
            raise HTTPException(status_code=400, detail=str(e))

        # Обновляем баланс банковского счёта в ORM-объекте
        bank_account.balance = domain_account.balance

        # Создаём новую операцию (тип операции определяется по категории)
        new_op = OperationModel(**op_data)

        # Сохраняем операцию и обновлённый банковский счёт
        self.session.add(new_op)
        self.session.add(bank_account)
        await self.session.commit()
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
