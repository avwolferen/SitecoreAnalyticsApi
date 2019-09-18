using AlexVanWolferen.Events.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Sitecore.Analytics;
using Sitecore.Analytics.Data;
using Sitecore.Analytics.Tracking;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Exceptions;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Definitions.Goals;
using AlexVanWolferen.Events.Extensions;
using System.Globalization;

namespace AlexVanWolferen.Events.Services
{
    public class TrackerService : ITrackerService
    {
        private readonly ILoggingService logging;
        private readonly IItemService items;
        private readonly IDefinitionManager<IGoalDefinition> goalsDefinitionManager;

        public TrackerService(IItemService items, ILoggingService logging)
        {
            Assert.ArgumentNotNull(logging, nameof(logging));
            Assert.ArgumentNotNull(items, nameof(items));

            this.items = items;
            this.logging = logging;
            this.goalsDefinitionManager = (GoalDefinitionManager)ServiceLocator.ServiceProvider.GetDefinitionManagerFactory().GetDefinitionManager<IGoalDefinition>();
        }

        public bool IsActive => Tracker.Current != null && Tracker.Current.IsActive;

        public virtual void TrackPageEvent(ID pageEventItemId)
        {
            this.TrackPageEvent(pageEventItemId, string.Empty, null);
        }

        public virtual void TrackPageEvent(string pageEventName)
        {
            this.TrackPageEvent(pageEventName, string.Empty, null);
        }

        public void TrackPageEvent(ID pageEventItemId, string data, IDictionary<string, object> customValues = null)
        {
            Assert.ArgumentNotNull(pageEventItemId, nameof(pageEventItemId));
            var pageEventItem = Tracker.MarketingDefinitions.PageEvents.FirstOrDefault(p => p.Id == pageEventItemId.Guid);
            Assert.IsNotNull(pageEventItem, $"Cannot find page event: {pageEventItemId}");

            this.TrackPageEvent(pageEventItem.Name, data, customValues);
        }

        public void TrackPageEvent(string pageEventName, string data, IDictionary<string, object> customValues = null)
        {
            Assert.ArgumentNotNull(pageEventName, nameof(pageEventName));

            if (!this.IsActive)
            {
                return;
            }

            var pageEventItem = Tracker.MarketingDefinitions.PageEvents.FirstOrDefault(p => p.Name.Equals(pageEventName, StringComparison.InvariantCultureIgnoreCase));
            if (pageEventItem != null)
            {
                IPageContext pageContext = Tracker.Current.CurrentPage;
                if (pageContext.Url.Path.StartsWith($"/{Events.Constants.API_BASEPATH}/"))
                {
                    pageContext = this.GetApiPageContext();
                }

                var ped = new PageEventData(pageEventItem.Name, pageEventItem.Id);

                if (!string.IsNullOrWhiteSpace(data))
                {
                    ped.Data = data;
                }

                if (customValues != null)
                {
                    customValues.Keys.Where(k => !ped.CustomValues.ContainsKey(k)).ToList().ForEach(key =>
                    {
                        ped.CustomValues.Add(key, customValues[key]);
                    });
                }

                if (pageContext != null)
                {
                    pageContext.Register(ped);
                }
            }
        }

        public void ProcessTracking(Item item)
        {
            if (item == null)
            {
                return;
            }

            try
            {
                TrackingFieldProcessor.Process(item);
            }
            catch (ItemNotFoundException ex)
            {
                this.logging.Error(this.GetType(), $"Could not track item '{item.ID}'", ex);
            }
        }

        public void RegisterGoal(ID goalId)
        {
            var goal = this.items.GetItem(goalId);

            if (goal == null)
            {
                this.logging.Warning(this.GetType(), $"Registering invalid goalid {goalId}.");
                return;
            }

            this.RegisterGoal(goal);
        }

        public void RegisterSearchTerm(string term)
        {
            if (this.IsActive)
            {
                var searchPageEventId = new Guid("0C179613-2073-41AB-992E-027D03D523BF");

                IPageContext page = Tracker.Current.CurrentPage;
                if (page.Url.Path.StartsWith($"/{Events.Constants.API_BASEPATH}/"))
                {
                    page = this.GetApiPageContext();
                }

                if (page == null)
                {
                    this.logging.Warning(this.GetType(), $"Register search term - Cannot resolve page context");
                    return;
                }

                var pageEvent = Tracker.MarketingDefinitions.PageEvents.FirstOrDefault(p => p.Name == "Search"); //.Id == searchPageEventId);
                var pageEventData = new PageEventData(pageEvent.Name, pageEvent.Id);
                if (page != null)
                {
                    pageEventData.ItemId = page.Item.Id;
                    pageEventData.Data = term;
                    pageEventData.DataKey = term;
                    pageEventData.Text = term;
                    page.Register(pageEventData);
                }
            }
        }

        /// <summary>
        /// Registers the goal.
        /// </summary>
        /// <param name="goal">The goal.</param>
        public void RegisterGoal(Item goal)
        {
            var goalDefinition = Tracker.MarketingDefinitions.Goals.FirstOrDefault(g => g.Id == goal.ID.Guid);
            if (goalDefinition == null)
            {
                return;
            }

            IPageContext pageContext = Tracker.Current.CurrentPage;
            if (pageContext.Url.Path.StartsWith(($"/{Events.Constants.API_BASEPATH}/")))
            {
                pageContext = this.GetApiPageContext();
            }

            if (pageContext == null)
            {
                this.logging.Warning(this.GetType(), $"Register goal - Cannot resolve page context for goal {goal.ID.ToString()}'");
                return;
            }

            pageContext.RegisterGoal(goalDefinition);

            this.logging.Debug(this.GetType(), $"Goal '{goalDefinition.Name} triggered with {goalDefinition.EngagementValuePoints} points'");
        }

        public Dictionary<string, Guid> GetGoals(List<string> goalNames)
        {
            Dictionary<string, Guid> goals = new Dictionary<string, Guid>();

            var allGoals = this.goalsDefinitionManager.GetAll(new CultureInfo("en"));

            goalNames.ForEach(key =>
            {
                var goal = allGoals.FirstOrDefault(g => g.Data.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase));
                if (goal != null)
                {
                    goals.Add(key, goal.Data.Id);
                }
            });

            return goals;
        }

        private IPageContext GetApiPageContext()
        {
            var referrerPath = HttpContext.Current?.Request?.UrlReferrer?.AbsolutePath;

            return referrerPath.IsNullOrWhiteSpace()
                ? Tracker.Current.Interaction.PreviousPage
                : (IPageContext)Tracker.Current.Interaction.Pages.LastOrDefault(p => p.Url.Path.Equals(referrerPath, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}