using System;
using Microsoft.Extensions.DependencyInjection;
using MINI_HW_1.Domain;
using MINI_HW_1.Services;
using MINI_HW_1.UI;
using MINI_HW_1.Interfaces;
using MINI_HW_1.AnimalCreators;
using MINI_HW_1.ThingCreators;

namespace MINI_HW_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            var menuManager = serviceProvider.GetRequiredService<MenuManager>();
            menuManager.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
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
        }
    }
}