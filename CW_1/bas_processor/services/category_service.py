import uuid

from fastapi import Depends, HTTPException
from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession

from bas_processor.db.session import get_session
from bas_processor.db.models import CategoryModel
from bas_processor.schemas.category import CategoryCreate, CategoryUpdate


class CategoryService:
    def __init__(self, session: AsyncSession = Depends(get_session)):
        self.session = session

    async def create_category(self, data: CategoryCreate):
        new_category = CategoryModel(**data.model_dump())
        self.session.add(new_category)
        await self.session.commit()
        return new_category

    async def get_all_categories(self):
        result = await self.session.execute(select(CategoryModel))
        return result.scalars().all()

    async def get_category_by_id(self, category_id: uuid.UUID):
        result = await self.session.execute(
            select(CategoryModel).where(CategoryModel.id == category_id)
        )
        category = result.scalar_one_or_none()
        if not category:
            raise HTTPException(status_code=404, detail="Category not found")
        return category

    async def update_category(self, category_id: uuid.UUID, data: CategoryUpdate):
        category = await self.get_category_by_id(category_id)
        update_data = data.dict(exclude_unset=True)
        for field, value in update_data.items():
            setattr(category, field, value)
        self.session.add(category)
        await self.session.commit()
        return category

    async def delete_category(self, category_id: uuid.UUID):
        category = await self.get_category_by_id(category_id)
        await self.session.delete(category)
        await self.session.commit()
