import asyncio
import sys
import logging
from logging import getLogger

import httpx

# Настройка логирования
logging.basicConfig(
    level="INFO",
    stream=sys.stdout,
    format='%(asctime)s [%(levelname)s] %(message)s'
)
logger = getLogger(__name__)

API_BASE_URL = "http://localhost:8080/api"


async def test_bank_accounts():
    async with httpx.AsyncClient() as client:
        # Создание банковского счета
        bank_payload = {"name": "Основной счет", "balance": 1000.0}
        response = await client.post(f"{API_BASE_URL}/bank_accounts/", json=bank_payload, timeout=1e9)
        logger.info("Create Bank Account: %s %s", response.status_code, response.json())
        bank_account = response.json()

        # Получение всех банковских счетов
        response = await client.get(f"{API_BASE_URL}/bank_accounts/", timeout=1e9)
        logger.info("Get All Bank Accounts: %s %s", response.status_code, response.json())

        # Получение банковского счета по ID
        account_id = bank_account.get("id")
        response = await client.get(f"{API_BASE_URL}/bank_accounts/{account_id}", timeout=1e9)
        logger.info("Get Bank Account by ID: %s %s", response.status_code, response.json())

        update_payload = {"name": "Обновленный счет", "balance": 2000.0}
        response = await client.put(f"{API_BASE_URL}/bank_accounts/{account_id}", json=update_payload, timeout=1e9)
        logger.info("Update Bank Account: %s %s", response.status_code, response.json())


async def test_categories():
    async with httpx.AsyncClient() as client:
        # Создание категории
        category_payload = {"type": "income", "name": "Зарплата"}
        response = await client.post(f"{API_BASE_URL}/categories/", json=category_payload, timeout=1e9)
        logger.info("Create Category: %s %s", response.status_code, response.json())
        category = response.json()

        # Получение всех категорий
        response = await client.get(f"{API_BASE_URL}/categories/", timeout=1e9)
        logger.info("Get All Categories: %s %s", response.status_code, response.json())

        # Получение категории по ID
        category_id = category.get("id")
        response = await client.get(f"{API_BASE_URL}/categories/{category_id}", timeout=1e9)
        logger.info("Get Category by ID: %s %s", response.status_code, response.json())


async def test_operations():
    async with httpx.AsyncClient() as client:
        # Создание банковского счета для операции
        bank_payload = {"name": "Операционный счет", "balance": 5000.0}
        bank_resp = await client.post(f"{API_BASE_URL}/bank_accounts/", json=bank_payload, timeout=1e9)
        bank_account = bank_resp.json()

        # Создание категории для операции
        category_payload = {"type": "expense", "name": "Кафе"}
        category_resp = await client.post(f"{API_BASE_URL}/categories/", json=category_payload, timeout=1e9)
        category = category_resp.json()

        # Создание операции (расход)
        operation_payload = {
            "type": "expense",
            "amount": 50.0,
            "description": "Обед в кафе",
            "bank_account_id": bank_account.get("id"),
            "category_id": category.get("id")
        }
        response = await client.post(f"{API_BASE_URL}/operations/", json=operation_payload, timeout=1e9)
        logger.info("Create Operation: %s %s", response.status_code, response.json())
        operation = response.json()

        # Получение всех операций
        response = await client.get(f"{API_BASE_URL}/operations/", timeout=1e9)
        logger.info("Get All Operations: %s %s", response.status_code, response.json(), )

        # Получение операции по ID
        operation_id = operation.get("id")
        response = await client.get(f"{API_BASE_URL}/operations/{operation_id}", timeout=1e9)
        logger.info("Get Operation by ID: %s %s", response.status_code, response.json())


async def main():
    logger.info("Тестирование эндпоинтов Bank Accounts:")
    await test_bank_accounts()
    logger.info("Тестирование эндпоинтов Categories:")
    await test_categories()
    logger.info("Тестирование эндпоинтов Operations:")
    await test_operations()


if __name__ == "__main__":
    asyncio.run(main())
