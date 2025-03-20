import uuid
from dataclasses import dataclass


@dataclass
class OperationCreatedEvent:
    operation_id: uuid.UUID
