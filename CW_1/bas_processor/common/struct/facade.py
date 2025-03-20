from bas_processor.schemas.category import CategoryCreate
from bas_processor.services.category_service import CategoryService

from utils.bas_utils.transaction import TransactionType


class CategoryFacade:
    def __init__(self):
        self._service = CategoryService()

    async def create_category(self, name: str, type: TransactionType):
        return await self._service.create_category(CategoryCreate(name=name, type=type))

    async def list_categories(self):
        return await self._service.get_all_categories()
