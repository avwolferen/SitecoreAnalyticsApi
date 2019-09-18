namespace AlexVanWolferen.Events.Extensions
{
    using Sitecore.Data.Items;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public static class ItemExtensions
    {
        public static bool IsWildcard(this Item item)
        {
            return item != null && item.Name == "*";
        }
    }
}