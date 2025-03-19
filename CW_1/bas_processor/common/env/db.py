import os

DATABASE_URL = os.getenv("DATABASE_URL", "postgresql+asyncpg://admin:admin@localhost:5432/db")
