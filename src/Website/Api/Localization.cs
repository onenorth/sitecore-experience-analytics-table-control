using System.Collections.Generic;
using System.Linq;

namespace OneNorth.ExperienceAnalyticsTableControl.Api
{
    public class Localization
    {
        private IList<ReportField> fieldsList;

        public ReportField[] Fields
        {
            get
            {
                return this.fieldsList.ToArray<ReportField>();
            }
            set
            {
                this.fieldsList = value;
            }
        }

        public Localization()
        {
            this.fieldsList = new List<ReportField>();
        }

        public void AddField(string fieldName, IDictionary<string, string> translations)
        {
            this.fieldsList.Add(new ReportField(fieldName, translations));
        }
    }
}