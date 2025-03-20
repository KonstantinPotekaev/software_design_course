FROM my_utils AS base

WORKDIR /app

COPY ../../bas_processor /app/bas_processor

COPY ../../bas_processor/requirements.txt /app
RUN pip install --no-cache-dir -r requirements.txt

ENV PYTHONPATH=/app

CMD ["python", "bas_processor/main.py"]
