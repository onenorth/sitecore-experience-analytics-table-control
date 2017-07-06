using Sitecore.Pipelines;
using System.Web.Http;
using System.Web.Routing;

namespace OneNorth.ExperienceAnalyticsTableControl.Api
{
    // https://kb.sitecore.net/articles/700677
    public class ExperienceAnalyticsTableControlWebApiInitializer
    {
        public void Process(PipelineArgs args)
        {
            GlobalConfiguration.Configure(Configure);
        }

        protected void Configure(HttpConfiguration configuration)
        {
            var routes = configuration.Routes;
            routes.MapHttpRoute("ExperienceAnalyticsTableControl", "sitecore/api/experienceanalyticstablecontrol/{datasource}/{sitename}", (object)new
            {
                controller = "ExperienceAnalyticsTableControl",
                action = "Get"
            });
        }
    }
}