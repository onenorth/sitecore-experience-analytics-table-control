using System.Collections;
using Sitecore.ExperienceAnalytics.Api.Response;

namespace OneNorth.ExperienceAnalyticsTableControl.Api
{
    public interface IExperienceAnalyticsTableControlData
    {
        IEnumerable Content { get; }
        Localization Localization { get; set; }
    }
}