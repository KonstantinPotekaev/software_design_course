# MINI_HW_1

## Описание проекта

Проект **MINI_HW_1** — это консольное приложение для управления зоопарком. В проекте учитываются:
1. Животные разного типа (травоядные и хищники), которые потребляют разное количество еды.
2. Вещи (инвентарь), которые состоят на балансе в зоопарке.
3. Ветеринарная клиника, проверяющая здоровье животных перед добавлением их в зоопарк.

Приложение демонстрирует принципы **SOLID**, а также использование **Dependency Injection** (DI) через `Microsoft.Extensions.DependencyInjection`.


---

## Принципы SOLID

### 1. Single Responsibility Principle (SRP)
**Каждый класс имеет единственную зону ответственности.**
- **Zoo** отвечает за хранение и управление списком животных и вещей.
- **MenuManager** отвечает за взаимодействие с пользователем (чтение команд, вывод информации).
- **VeterinaryClinic** реализует логику проверки здоровья животных.
- **Фабрики** (RabbitCreator, TigerCreator, и т.д.) занимаются **только** созданием конкретного типа животного.
- **InputHelper** отвечает **только** за универсальный ввод данных (с проверками, отменой и т.п.).

### 2. Open/Closed Principle (OCP)
**Классы открыты для расширения, но закрыты для модификации.**
- Чтобы добавить нового хищника или травоядное животное, достаточно создать новый класс, унаследованный от Predator или Herbo, без изменения существующих классов.
- То же самое с новыми вещами: достаточно унаследовать от базового класса Thing.
- Фабрики (IAnimalCreator, IThingCreator) позволяют легко расширять ассортимент животных и вещей, не изменяя существующую логику.

### 3. Liskov Substitution Principle (LSP)
**Объекты в программе могут быть заменены экземплярами их подтипов без изменения корректности работы.**
- Наследники `Animal` (Rabbit, Tiger, Wolf, Monkey) корректно подставляются в коллекцию животных.
- Наследники `Thing` (Table, Computer) корректно подставляются в коллекцию инвентаря.

### 4. Interface Segregation Principle (ISP)
**Много интерфейсов, специально спроектированных под клиента, лучше, чем один интерфейс общего назначения.**
- **IAlive** содержит только информацию о потреблении еды.
- **IInventory** содержит только информацию об инвентарном номере и названии.
- **IVeterinaryClinic** содержит единственный метод `CheckAnimal`.
- Благодаря этому классы реализуют **только нужные** им интерфейсы.

### 5. Dependency Inversion Principle (DIP)
**Модули верхнего уровня не должны зависеть от модулей нижнего уровня. Оба должны зависеть от абстракций.**
- Класс **Zoo** зависит от интерфейса **IVeterinaryClinic**, а не от конкретной реализации `VeterinaryClinic`.
- Фабрики создают животных, а в клиентском коде (MenuManager) я вызываю методы интерфейса `IAnimalCreator`.

---

## Dependency Injection (DI)

В **Program.cs** я использую `Microsoft.Extensions.DependencyInjection`:

```csharp
var services = new ServiceCollection();

// Регистрация интерфейсов и их реализация
services.AddSingleton<IVeterinaryClinic, VeterinaryClinic>();
services.AddSingleton<Zoo>();
services.AddSingleton<MenuManager>();

// Регистрация фабрик для животных
services.AddTransient<IAnimalCreator, RabbitCreator>();
services.AddTransient<IAnimalCreator, TigerCreator>();
services.AddTransient<IAnimalCreator, WolfCreator>();
services.AddTransient<IAnimalCreator, MonkeyCreator>();

// Регистрация фабрик для вещей
services.AddTransient<IThingCreator, TableCreator>();
services.AddTransient<IThingCreator, ComputerCreator>();

var serviceProvider = services.BuildServiceProvider();
var menuManager = serviceProvider.GetRequiredService<MenuManager>();
menuManager.Run();
