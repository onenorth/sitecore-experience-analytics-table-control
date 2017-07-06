using System.Collections;
using Sitecore.ExperienceAnalytics.Api.Response;
using System.Collections.Generic;

namespace OneNorth.ExperienceAnalyticsTableControl.Api
{
    public class ExperienceAnalyticsTableControlData<TItem> : IExperienceAnalyticsTableControlData
    {
        private readonly List<TItem> _content;

        public ExperienceAnalyticsTableControlData()
        {
            Localization = new Localization();
            _content = new List<TItem>();
        }

        public IEnumerable Content
        {
            get { return _content; }
        }

        public Localization Localization { get; set; }

        public void AddItem(TItem item)
        {
            _content.Add(item);
        }
    }
}