using SM_1;

var factory = new FactoryAF([
    new Customer("Alice", null),
    new Customer("Bob", null),
    new Customer("Charlie", null)
]);

// Добавляем автомобили на производство
factory.ProduceCars(5);

// Показать начальное состояние склада и клиентов
Console.WriteLine("Before SaleCar():");
factory.ShowCars();
factory.ShowCustomers();

// Выполняем метод SaleCar()
factory.SaleCar();

// Показать конечное состояние после продажи автомобилей
Console.WriteLine("\nAfter SaleCar():");
factory.ShowCars();
factory.ShowCustomers();