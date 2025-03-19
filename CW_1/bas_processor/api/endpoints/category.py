import uuid

from fastapi import APIRouter, Depends
from typing import List

from bas_processor.schemas.category import CategoryCreate, CategoryRead, CategoryUpdate
from bas_processor.services.category_service import CategoryService

router = APIRouter()


@router.post("/", response_model=CategoryRead)
async def create_category(data: CategoryCreate,
                          service: CategoryService = Depends()):
    return await service.create_category(data)


@router.get("/", response_model=List[CategoryRead])
async def get_all_categories(service: CategoryService = Depends()):
    return await service.get_all_categories()


@router.get("/{category_id}", response_model=CategoryRead)
async def get_category_by_id(category_id: uuid.UUID, service: CategoryService = Depends()):
    return await service.get_category_by_id(category_id)


@router.put("/{category_id}", response_model=CategoryRead)
async def update_category(category_id: uuid.UUID,
                          data: CategoryUpdate,
                          service: CategoryService = Depends()):
    return await service.update_category(category_id, data)


@router.delete("/{category_id}")
async def delete_category(category_id: uuid.UUID, service: CategoryService = Depends()):
    await service.delete_category(category_id)
    return {"message": "Category deleted"}
