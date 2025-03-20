from typing import Callable, Dict, List

class EventBus:
    def __init__(self):
        self.handlers: Dict[str, List[Callable]] = {}

    def subscribe(self, event_type: str, handler: Callable):
        if event_type not in self.handlers:
            self.handlers[event_type] = []
        self.handlers[event_type].append(handler)

    async def publish(self, event_type: str, event):
        handlers = self.handlers.get(event_type, [])
        for handler in handlers:
            await handler(event)


