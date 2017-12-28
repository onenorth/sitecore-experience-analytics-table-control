using System.Collections.Generic;

namespace OneNorth.ExperienceAnalyticsTableControl.Api
{
    public class ReportField
    {
        public string Field
        {
            get;
            private set;
        }

        public IDictionary<string, string> Translations
        {
            get;
            private set;
        }

        public ReportField(string field, IDictionary<string, string> translations)
        {
            this.Field = field;
            this.Translations = translations;
        }
    }
}