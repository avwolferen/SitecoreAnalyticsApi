namespace AlexVanWolferen.Events.Pipelines.InitializeTracker
{
    using AlexVanWolferen.Events.Extensions;
    using Sitecore;
    using Sitecore.Analytics.Pipelines.InitializeTracker;
    using Sitecore.Analytics.Tracking;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Web;
    using System;
    using System.Web;

    /// <summary>
    /// Custom create page pipeline for resolving wildcard pages
    /// </summary>
    /// <seealso cref="Sitecore.Analytics.Pipelines.InitializeTracker.InitializeTrackerProcessor" />
    public class CreatePage : InitializeTrackerProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreatePage"/> class.
        /// </summary>
        public CreatePage()
        {
        }

        /// <summary>
        /// Processes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public override void Process(InitializeTrackerArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            if (args.IsSessionEnd)
            {
                return;
            }

            HttpContextBase httpContext = args.HttpContext;
            if (httpContext == null)
            {
                args.AbortPipeline();
                return;
            }

            this.CreateAndInitializePage(args.Session.Interaction);
        }

        /// <summary>
        /// Creates the and initialize page.
        /// Modifictaion: If the page is a wildcard page, start the pipeline to resolve the correct item
        /// </summary>
        /// <param name="visit">The visit.</param>
        private void CreateAndInitializePage(CurrentInteraction visit)
        {
            IPageContext empty = visit.CreatePage();
            empty.SetUrl(WebUtil.GetRawUrl());

            DeviceItem device = Context.Device;

            if (device == null)
            {
                empty.SitecoreDevice.Id = Guid.Empty;
                empty.SitecoreDevice.Name = string.Empty;
            }
            else
            {
                empty.SitecoreDevice.Id = device.ID.Guid;
                empty.SitecoreDevice.Name = device.Name;
            }

            Item item = Context.Item;

            if (item.IsWildcard())
            {
                //// If you are actually using wildcard items then you can throw in your pipeline to resolve it. More information can be found online ;) Referencing an old blog from an old-coworker.
                ////var wildCardItem = ResolveWildcardItemPipeline.Run(item);

                ////if (wildCardItem != null)
                ////{
                ////    item = wildCardItem;
                ////}
            }

            if (item == null)
            {
                //// This is really important for every Sitecore platform, rule out your api's in your XDB data... Need to share this with the Sitecore Community @ Achmea
                if (empty.Url.Path.StartsWith($"/{Events.Constants.API_BASEPATH}/"))
                {
                    visit.CurrentPage.Cancel();
                }

                return;
            }

            empty.SetItemProperties(item.ID.Guid, item.Language.Name, item.Version.Number);
        }
    }
}