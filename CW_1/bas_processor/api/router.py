from fastapi import APIRouter
from bas_processor.api.endpoints.bank_account import router as bank_account_router
from bas_processor.api.endpoints.category import router as category_router
from bas_processor.api.endpoints.operation import router as operation_router
#from bas_processor.api.endpoints.import_export import router as import_export_router

router = APIRouter()
router.include_router(bank_account_router, prefix="/bank_accounts", tags=["Bank Accounts"])
router.include_router(category_router, prefix="/categories", tags=["Categories"])
router.include_router(operation_router, prefix="/operations", tags=["Operations"])
#router.include_router(import_export_router, prefix="/data", tags=["Import/Export"])
