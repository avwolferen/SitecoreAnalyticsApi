using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlexVanWolferen.Events.Models
{
    [Serializable]
    public class EventRequestModel
    {
        public string Data { get; set; }

        public IDictionary<string, object> CustomValues { get; set; }
    }
}