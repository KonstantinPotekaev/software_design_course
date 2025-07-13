# MINI_HW_1

## Описание проекта

**MINI_HW_1** — это информационная система для управления зоопарком, реализованная по принципам Domain-Driven Design (
DDD) и Clean Architecture. Проект состоит из нескольких слоёв:

- **Domain (ядро):** Содержит модели предметной области:
    - **Animal:** Абстрактная сущность с общими свойствами (вид, кличка, дата рождения, пол, любимая еда, статус
      здоровья).  
      Наследники: *Rabbit* (травоядное, наследник класса *Herbo* с дополнительным свойством *Kindness*), *Tiger*,
      *Wolf*, *Monkey*.
    - **Enclosure:** Модель вольера, характеризующаяся типом (например, для хищников, травоядных, птиц, аквариум и
      т.д.), размером, вместимостью, списком животных, находящихся внутри, и методом уборки.
    - **FeedingSchedule:** Расписание кормления для животных, хранит данные о животном, времени кормления, типе пищи и
      состоянии выполнения.
    - **Доменные события:** *AnimalMovedEvent* и *FeedingTimeEvent* фиксируют перемещение животных между вольерами и
      наступление времени кормления.
    - **Thing:** Базовый класс для инвентарных объектов (например, Table, Computer).

- **Application (бизнес-логика):** Содержит сервисы, реализующие ключевые сценарии использования:
    - **AnimalTransferService:** Отвечает за перемещение животных между вольерами с проверками (например, на вместимость
      вольера).
    - **FeedingOrganizationService:** Управляет планированием и обработкой кормлений, генерирует и обрабатывает доменные
      события.
    - **ZooStatisticsService:** Собирает сводную статистику по зоопарку: общее число животных, распределение по видам,
      заполненность вольеров и др.

- **Infrastructure (внешние взаимодействия):** Реализует in‑memory хранилище для животных, вольеров и расписаний
  кормления через паттерн *Repository*. Интерфейсы репозитория (IAnimalRepository, IEnclosureRepository,
  IFeedingScheduleRepository) определены в Domain, а их реализации находятся в Infrastructure.

- **Presentation (слой представления):** Предоставляет два варианта работы:
    - **Консольное приложение:** Менеджер меню (MenuManager) обеспечивает взаимодействие с пользователем через консоль.
    - **Web API (REST‑контроллеры):** Реализованы контроллеры для работы с животными, вольерами и расписаниями
      кормления. Контроллеры используют бизнес‑сервисы из Application. REST‑API документируется с помощью Swagger, что
      позволяет тестировать API через веб‑интерфейс.

## Принципы SOLID и архитектурные решения (DDD / Clean Architecture)

В проекте реализованы следующие принципы SOLID:

1. **Single Responsibility Principle (SRP):**  
   Каждый класс отвечает за свою область ответственности:
    - **Zoo** — управление списками животных и вещей.
    - **MenuManager** — взаимодействие с пользователем через консоль.
    - **VeterinaryClinic** — логика проверки здоровья животных.
    - **Фабрики** (RabbitCreator, TigerCreator и т.д.) — создание конкретных типов животных.
    - **InputHelper** — универсальный ввод данных.

2. **Open/Closed Principle (OCP):**  
   Для расширения ассортимента животных или вещей достаточно создать новый класс, унаследованный от базового (Animal,
   Thing) без изменения существующего кода.

3. **Liskov Substitution Principle (LSP):**  
   Наследники базовых классов (Animal, Thing) могут подставляться без нарушения работы приложения.

4. **Interface Segregation Principle (ISP):**  
   Интерфейсы определены максимально узко (например, IAlive содержит только данные о потреблении еды; IInventory —
   данные об инвентарном номере и названии).

5. **Dependency Inversion Principle (DIP):**  
   Зависимости между слоями осуществляются через абстракции (например, Zoo зависит от интерфейса IVeterinaryClinic, а не
   от конкретной реализации VeterinaryClinic).

Архитектура проекта соответствует концепции Clean Architecture:

- **Domain** не зависит ни от чего внешнего,
- **Application** и **Infrastructure** зависят только от Domain через интерфейсы,
- **Presentation** (Web API или консольный UI) использует внутренние слои через внедрение зависимостей (DI).

## Dependency Injection (DI)

В проекте используется DI с помощью `Microsoft.Extensions.DependencyInjection`. В файле `Program.cs` регистрируются все
зависимости:

- Для консольного режима — регистрация интерфейсов, фабрик, Zoo и MenuManager.
- Для веб‑API — регистрация контроллеров, репозиториев (in‑memory), бизнес‑сервисов и Swagger.

Пример регистрации (из файла Program.cs):

```csharp
// Режим веб‑API:
services.AddControllers()
        .AddApplicationPart(typeof(MINI_HW_1.Presentation.WebApi.Controllers.AnimalsController).Assembly);
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddSingleton<IAnimalRepository, InMemoryAnimalRepository>();
services.AddSingleton<IEnclosureRepository, InMemoryEnclosureRepository>();
services.AddSingleton<IFeedingScheduleRepository, InMemoryFeedingScheduleRepository>();
services.AddScoped<AnimalTransferService>();
services.AddScoped<FeedingOrganizationService>();
services.AddScoped<ZooStatisticsService>();

// Общие зависимости:
services.AddSingleton<IVeterinaryClinic, VeterinaryClinic>();
services.AddSingleton<Zoo>();
services.AddSingleton<MenuManager>();
services.AddTransient<IAnimalCreator, RabbitCreator>();
services.AddTransient<IAnimalCreator, TigerCreator>();
services.AddTransient<IAnimalCreator, WolfCreator>();
services.AddTransient<IAnimalCreator, MonkeyCreator>();
services.AddTransient<IThingCreator, TableCreator>();
services.AddTransient<IThingCreator, ComputerCreator>();
```

## Способы запуска

Приложение поддерживает два режима работы: консольный и веб‑сервис (REST API).

### 1. Консольное приложение (режим по умолчанию)

Запуск:

```bash
dotnet run
```

- Откроется консольное меню, через которое можно выполнять операции по управлению зоопарком (добавление животных, расчет
  общего расхода еды, вывод инвентарных объектов и т.д.).

### 2. Веб‑сервис (REST‑API):

Запуск

```bash
dotnet run -- --web
```

- Приложение запустится как веб‑сервер (Kestrel) на порту, например, http://localhost:5000

- Swagger‑UI будет доступен по адресу:
    ```bash
    http://localhost:5000/swagger
    ```
- Через Swagger‑UI вы можете протестировать следующие операции:

    - Контроллер для животных (AnimalsController): Добавление, удаление, получение информации, перемещение животных.

    - Контроллер для вольеров (EnclosuresController): Добавление, удаление, получение информации, уборка вольеров.

    - Контроллер для расписания кормлений (FeedingSchedulesController): Планирование кормления, изменение расписания,
      отметка выполнения, обработка наступивших кормлений.

    - Контроллер для статистики (StatisticsController): Получение сводной статистики по зоопарку (количество животных,
      распределение, заполненность вольеров).

#### Важно:

Для тестирования веб‑режима рекомендуется запускать приложение в режиме разработки. Установите переменную окружения
ASPNETCORE_ENVIRONMENT в значение "Development" перед запуском:

```bash 
export ASPNETCORE_ENVIRONMENT=Development
dotnet run -- --web
```

## Тестирование

В проекте присутствует отдельный набор юнит‑тестов (UTests_MINI_HW_2), покрывающий более 65% кода. Тесты разделены по
модулям:

- DomainTests: Проверка корректности работы доменных моделей (Animal, Enclosure, FeedingSchedule, Zoo).

- ApplicationTests: Тестирование бизнес‑сервисов (AnimalTransferService, FeedingOrganizationService, ZooStatisticsService).

- AnimalCreatorsTests, ThingCreatorsTests, UtilsTests: Тесты для фабрик и утилит.

Запустить тесты можно командой:
```bash
dotnet test
```
