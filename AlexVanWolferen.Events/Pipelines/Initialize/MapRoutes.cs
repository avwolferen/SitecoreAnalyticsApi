namespace AlexVanWolferen.Events.Pipelines.Initialize
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Sitecore.Pipelines;

    public class MapRoutes
    {
        public void Process(PipelineArgs args)
        {            
            RouteTable.Routes.MapRoute(
                "RegisterEvents",
                string.Format("{0}/events/{{action}}", Constants.API_BASEPATH),
                new { controller = "EventApi", action = "Post" });
        }
    }
}