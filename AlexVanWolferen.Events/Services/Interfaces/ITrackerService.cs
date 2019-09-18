using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace AlexVanWolferen.Events.Services.Interfaces
{
    public interface ITrackerService
    {
        bool IsActive { get; }

        void TrackPageEvent(ID pageEventItemId);

        void TrackPageEvent(string pageEventName);

        void TrackPageEvent(ID pageEventItemId, string data, IDictionary<string, object> customValues = null);

        void TrackPageEvent(string pageEventName, string data, IDictionary<string, object> customValues = null);

        void ProcessTracking(Item item);

        void RegisterGoal(ID goalId);

        void RegisterGoal(Item goal);

        void RegisterSearchTerm(string term);

        Dictionary<string, Guid> GetGoals(List<string> goalNames);
    }
}