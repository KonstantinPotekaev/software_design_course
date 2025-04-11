using MINI_HW_2.AnimalCreators;
using MINI_HW_2.Application.Services;
using MINI_HW_2.Domain;
using MINI_HW_2.Infrastructure.Repositories;
using MINI_HW_2.Interfaces;
using MINI_HW_2.Presentation.ConsoleUI;
using MINI_HW_2.Presentation.WebApi.Controllers;
using MINI_HW_2.Services;
using MINI_HW_2.ThingCreators;


namespace MINI_HW_2
{
    class Program
    {
        static void Main(string[] args)
        {
            bool runWeb = args.Length > 0 && args.Contains("--web");

            var host = CreateHostBuilder(args, runWeb).Build();

            if (runWeb)
            {
                host.Run();
            }
            else
            {
                var menuManager = host.Services.GetRequiredService<MenuManager>();
                menuManager.Run();
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args, bool runWeb) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    if (runWeb)
                    {
                        // Регистрируем контроллеры, Swagger и веб-зависимости
                        services.AddControllers()
                            .AddApplicationPart(typeof(AnimalsController)
                                .Assembly);
                        services.AddEndpointsApiExplorer();
                        services.AddSwaggerGen();

                        // Регистрируем репозитории (in-memory)
                        services.AddSingleton<IAnimalRepository, InMemoryAnimalRepository>();
                        services.AddSingleton<IEnclosureRepository, InMemoryEnclosureRepository>();
                        services.AddSingleton<IFeedingScheduleRepository, InMemoryFeedingScheduleRepository>();

                        // Регистрируем бизнес-сервисы для веб-API
                        services.AddScoped<AnimalTransferService>();
                        services.AddScoped<FeedingOrganizationService>();
                        services.AddScoped<ZooStatisticsService>();
                    }

                    // Регистрация общих зависимостей для обоих режимов
                    services.AddSingleton<IVeterinaryClinic, VeterinaryClinic>();
                    services.AddSingleton<Zoo>();
                    services.AddSingleton<MenuManager>();

                    // Регистрация фабрик животных
                    services.AddTransient<IAnimalCreator, RabbitCreator>();
                    services.AddTransient<IAnimalCreator, TigerCreator>();
                    services.AddTransient<IAnimalCreator, WolfCreator>();
                    services.AddTransient<IAnimalCreator, MonkeyCreator>();

                    // Регистрация фабрик для вещей
                    services.AddTransient<IThingCreator, TableCreator>();
                    services.AddTransient<IThingCreator, ComputerCreator>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    if (runWeb)
                    {
                        webBuilder.Configure(app =>
                        {
                            var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                            if (env.IsDevelopment())
                            {
                                app.UseDeveloperExceptionPage();
                                app.UseSwagger();
                                app.UseSwaggerUI();
                            }

                            app.UseRouting();
                            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
                        });
                    }
                });
    }
}