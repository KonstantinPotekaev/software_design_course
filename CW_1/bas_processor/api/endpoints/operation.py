import uuid

from fastapi import APIRouter, Depends
from typing import List

from bas_processor.schemas.operation import OperationCreate, OperationRead, OperationUpdate
from bas_processor.services.operation_service import OperationService

router = APIRouter()


@router.post("/", response_model=OperationRead)
async def create_operation(data: OperationCreate,
                           service: OperationService = Depends()):
    return await service.create_operation(data)


@router.get("/", response_model=List[OperationRead])
async def get_all_operations(service: OperationService = Depends()):
    return await service.get_all_operations()


@router.get("/{operation_id}", response_model=OperationRead)
async def get_operation_by_id(operation_id: uuid.UUID, service: OperationService = Depends()):
    return await service.get_operation_by_id(operation_id)


@router.put("/{operation_id}", response_model=OperationRead)
async def update_operation(operation_id: uuid.UUID,
                           data: OperationUpdate,
                           service: OperationService = Depends()):
    return await service.update_operation(operation_id, data)


@router.delete("/{operation_id}")
async def delete_operation(operation_id: uuid.UUID, service: OperationService = Depends()):
    await service.delete_operation(operation_id)
    return {"message": "Operation deleted"}
