using System.Collections;

namespace OneNorth.ExperienceAnalyticsTableControl.Api
{
    public interface IExperienceAnalyticsTableControlData
    {
        IEnumerable Content { get; }
        Localization Localization { get; set; }
    }
}