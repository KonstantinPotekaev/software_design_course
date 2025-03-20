import sys
from pathlib import Path

import streamlit as st
import requests

BASE_DIR = Path(__file__).absolute().parent.parent
sys.path.append(str(BASE_DIR))

from web.common.env.general import API_BASE_URL


### Функции для работы с банковскими счетами ###
def get_bank_accounts():
    response = requests.get(f"{API_BASE_URL}/bank_accounts/")
    if response.ok:
        return response.json()
    else:
        st.error("Ошибка получения списка счетов")
        return []


def create_bank_account(name: str, balance: float):
    payload = {"name": name, "balance": balance}
    response = requests.post(f"{API_BASE_URL}/bank_accounts/", json=payload)
    if response.ok:
        st.success("Счёт успешно создан")
        return response.json()
    else:
        st.error(f"Ошибка при создании счёта: {response.text}")
        return None


def delete_bank_account(account_id: str):
    response = requests.delete(f"{API_BASE_URL}/bank_accounts/{account_id}")
    if response.ok:
        st.success("Счёт успешно удалён")
    else:
        st.error(f"Ошибка при удалении счёта: {response.text}")


### Функции для работы с категориями ###
def get_categories():
    response = requests.get(f"{API_BASE_URL}/categories/")
    if response.ok:
        return response.json()
    else:
        st.error("Ошибка получения списка категорий")
        return []


def create_category(name: str, cat_type: str):
    payload = {"name": name, "type": cat_type}
    response = requests.post(f"{API_BASE_URL}/categories/", json=payload)
    if response.ok:
        st.success("Категория успешно создана")
        return response.json()
    else:
        st.error(f"Ошибка создания категории: {response.text}")
        return None


def delete_category(category_id: str):
    response = requests.delete(f"{API_BASE_URL}/categories/{category_id}")
    if response.ok:
        st.success("Категория успешно удалена")
    else:
        st.error(f"Ошибка удаления категории: {response.text}")


### Функции для работы с операциями ###
def get_expense_history():
    response = requests.get(f"{API_BASE_URL}/operations/")
    if response.ok:
        operations = response.json()
        # Фильтруем операции типа "expense"
        expenses = [op for op in operations if op.get("type") == "expense"]
        return expenses
    else:
        st.error("Ошибка получения операций")
        return []


def create_operation(amount: float, description: str, bank_account_id: str, category_id: str):
    payload = {
        "amount": amount,
        "description": description,
        "bank_account_id": bank_account_id,
        "category_id": category_id
    }
    response = requests.post(f"{API_BASE_URL}/operations/", json=payload)
    if response.ok:
        st.success("Операция успешно создана")
        return response.json()
    else:
        st.error(f"Ошибка создания операции: {response.text}")
        return None


def main():
    # Заголовок приложения
    st.title("HSE-Bank Online")

    # Меню навигации в боковой панели
    menu_option = st.sidebar.selectbox(
        "Навигация",
        ("Dashboard", "Управление счетами", "Управление категориями", "История расходов", "Добавить операцию")
    )

    if menu_option == "Dashboard":
        st.header("Панель управления")
        st.write("Добро пожаловать в HSE-Bank Online. Используйте боковое меню для навигации.")
        accounts = get_bank_accounts()
        if accounts:
            st.subheader("Сводка счетов")
            st.table(accounts)

    elif menu_option == "Управление счетами":
        st.header("Управление счетами")

        st.subheader("Создать новый счёт")
        account_name = st.text_input("Название счёта", value="Новый счёт")
        account_balance = st.number_input("Начальный баланс", value=0.0, step=10.0)
        if st.button("Создать счёт"):
            create_bank_account(account_name, account_balance)
            st.rerun()

        st.subheader("Существующие счета")
        accounts = get_bank_accounts()
        if accounts:
            for account in accounts:
                st.write(f"**ID:** {account.get('id')}")
                st.write(f"**Название:** {account.get('name')}, **Баланс:** {account.get('balance')}")
                if st.button(f"Удалить счёт {account.get('id')}", key=account.get('id')):
                    delete_bank_account(account.get("id"))
                    st.rerun()

    elif menu_option == "Управление категориями":
        st.header("Управление категориями")

        st.subheader("Создать новую категорию")
        category_name = st.text_input("Название категории", value="")
        category_type = st.selectbox("Тип категории", ["income", "expense"])
        if st.button("Создать категорию"):
            if category_name.strip() == "":
                st.error("Введите корректное название категории")
            else:
                create_category(category_name.strip(), category_type)
                st.rerun()

        st.subheader("Список категорий")
        categories = get_categories()
        if categories:
            st.table(categories)
            for cat in categories:
                if st.button(f"Удалить категорию '{cat.get('name')}'", key=f"cat_{cat.get('id')}"):
                    delete_category(cat.get("id"))
                    st.rerun()

    elif menu_option == "История расходов":
        st.header("История расходов")
        expenses = get_expense_history()
        if expenses:
            st.table(expenses)
        else:
            st.info("Операции расходов отсутствуют.")

    elif menu_option == "Добавить операцию":
        st.header("Добавить новую операцию")

        op_amount = st.number_input("Сумма", value=0.0, step=1.0)
        op_description = st.text_area("Описание", value="Введите описание операции")

        # Выбор счёта из списка
        accounts = get_bank_accounts()
        if accounts:
            account_options = {account.get("name"): account.get("id") for account in accounts}
            selected_account_name = st.selectbox("Выберите счёт", list(account_options.keys()))
            bank_account_id = account_options.get(selected_account_name)
        else:
            bank_account_id = None
            st.warning("Сначала создайте хотя бы один счёт.")

        # Выбор категории из списка (по названию)
        categories = get_categories()
        if categories:
            category_options = {cat.get("name"): cat.get("id") for cat in categories}
            selected_category_name = st.selectbox("Выберите категорию", list(category_options.keys()))
            category_id = category_options.get(selected_category_name)
        else:
            category_id = None
            st.warning("Сначала создайте хотя бы одну категорию.")

        if st.button("Создать операцию"):
            if bank_account_id and category_id:
                create_operation(op_amount, op_description, bank_account_id, category_id)
            else:
                st.error("Выберите корректный счёт и категорию.")


if __name__ == '__main__':
    main()
