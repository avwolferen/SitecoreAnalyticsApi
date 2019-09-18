namespace AlexVanWolferen.Events.Services
{
    using AlexVanWolferen.Events.Services.Interfaces;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using System;

    public class ItemService : IItemService
    {
        public Item GetItem(Guid id)
        {
            return this.GetItem(new ID(id));
        }

        public Item GetItem(ID id)
        {
            return Sitecore.Context.Database.GetItem(id);
        }

        public Item GetItem(string path)
        {
            return Sitecore.Context.Database.GetItem(path);
        }
    }
}