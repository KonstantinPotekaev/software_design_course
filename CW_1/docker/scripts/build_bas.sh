#!/bin/bash

set -e

echo "Сборка backend (bank_account_service)..."
docker build -t my_extrbank_account_serviceactor -f docker/dockerfiles/Dockerfile.bas .
echo "Образ bank_account_service готов!"
