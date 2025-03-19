from fastapi import APIRouter, Depends
from typing import List
from app.schemas.bank_account import BankAccountCreate, BankAccountRead, BankAccountUpdate
from app.services.bank_account_service import BankAccountService

router = APIRouter()


@router.post("/", response_model=BankAccountRead)
async def create_bank_account(
        data: BankAccountCreate,
        service: BankAccountService = Depends()
):
    return await service.create_bank_account(data)


@router.get("/", response_model=List[BankAccountRead])
async def get_all_accounts(service: BankAccountService = Depends()):
    return await service.get_all_bank_accounts()


@router.get("/{account_id}", response_model=BankAccountRead)
async def get_account_by_id(account_id: int, service: BankAccountService = Depends()):
    return await service.get_bank_account_by_id(account_id)


@router.put("/{account_id}", response_model=BankAccountRead)
async def update_account(
        account_id: int,
        data: BankAccountUpdate,
        service: BankAccountService = Depends()
):
    return await service.update_bank_account(account_id, data)


@router.delete("/{account_id}")
async def delete_account(account_id: int, service: BankAccountService = Depends()):
    await service.delete_bank_account(account_id)
    return {"message": "Bank account deleted"}
