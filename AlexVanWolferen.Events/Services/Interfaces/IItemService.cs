using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlexVanWolferen.Events.Services.Interfaces
{
    public interface IItemService
    {
        Item GetItem(Guid id);

        Item GetItem(ID id);

        Item GetItem(string path);

    }
}