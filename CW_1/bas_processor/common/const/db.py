# Имена таблиц
BANK_ACCOUNTS_TABLE = "bank_accounts"
CATEGORIES_TABLE = "categories"
OPERATIONS_TABLE = "operations"

# ForeignKey targets
FK_BANK_ACCOUNT = f"{BANK_ACCOUNTS_TABLE}.id"
FK_CATEGORY     = f"{CATEGORIES_TABLE}.id"

# Модели (имена классов)
BANK_ACCOUNT_MODEL = "BankAccountModel"
CATEGORY_MODEL     = "CategoryModel"
OPERATION_MODEL    = "OperationModel"

# Названия связей (back_populates)
REL_BANK_ACCOUNT_OPERATIONS = "operations"
REL_OPERATION_BANK_ACCOUNT = "bank_account"
REL_CATEGORY_OPERATIONS     = "operations"
REL_OPERATION_CATEGORY      = "category"

# Значения по‑умолчанию
DEFAULT_BALANCE = 0.0
