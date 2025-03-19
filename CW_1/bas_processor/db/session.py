from sqlalchemy.ext.asyncio import create_async_engine, AsyncSession, async_sessionmaker

from bas_processor.common.env.db import DATABASE_URL

engine = create_async_engine(DATABASE_URL, echo=False)
AsyncSessionLocal = async_sessionmaker(bind=engine, expire_on_commit=False)


async def get_session() -> AsyncSession:
    async with AsyncSessionLocal() as session:
        yield session
