namespace AlexVanWolferen.Events.Services
{
    using AlexVanWolferen.Events.Services.Interfaces;
    using Sitecore.DependencyInjection;

    public class ServiceContainer : IServiceContainer
    {
        private ITrackerService tracker;

        public ITrackerService Tracker
        {
            get
            {
                return this.tracker ?? (this.tracker = this.Resolve<ITrackerService>());
            }

            set
            {
                this.tracker = value;
            }
        }

        private T Resolve<T>()
            where T : class
        {
            return ServiceLocator.ServiceProvider.GetService(typeof(T)) as T;
        }
    }
}