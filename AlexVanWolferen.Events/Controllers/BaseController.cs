namespace AlexVanWolferen.Events.Controllers
{
    using AlexVanWolferen.Events.Services.Interfaces;
    using Sitecore.DependencyInjection;
    using Sitecore.Diagnostics;
    using Sitecore.Mvc.Controllers;

    public class BaseController : SitecoreController
    {
        public BaseController(IServiceContainer services)
        {
            Assert.ArgumentNotNull(services, "services");

            this.Services = services;
        }

        public BaseController()
        {
            this.Services = this.Resolve<IServiceContainer>();
        }

        public IServiceContainer Services { get; private set; }

        protected T Resolve<T>()
            where T : class
        {
            return ServiceLocator.ServiceProvider.GetService(typeof(T)) as T;
        }
    }
}