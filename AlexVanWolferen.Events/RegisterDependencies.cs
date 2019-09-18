namespace AlexVanWolferen.Events
{
    using AlexVanWolferen.Events.Extensions;
    using AlexVanWolferen.Events.Services;
    using AlexVanWolferen.Events.Services.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;

    public class RegisterDependencies : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            if (serviceCollection.IsNullOrEmpty())
            {
                return;
            }

            // Services
            //serviceCollection.AddSingleton<ICacheService>(x => new CacheService());
            serviceCollection.AddTransient<IItemService>(x => new ItemService());
            serviceCollection.AddTransient<ILoggingService>(x => new LoggingService());

            // Service container
            serviceCollection.AddTransient<IServiceContainer>(x => new ServiceContainer());

            // Constructor Injectors
            serviceCollection.AddSingleton<ITrackerService>(x => new TrackerService(x.GetService<IItemService>(), x.GetService<ILoggingService>()));
        }
    }
}