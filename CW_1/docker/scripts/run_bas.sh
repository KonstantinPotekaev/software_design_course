#!/bin/bash

set -e

echo "Запуск backend (bank_account_service)..."
docker run -d -p 8000:8000 --name bank_account_service bank_account_service
echo "Backend запущен!"
