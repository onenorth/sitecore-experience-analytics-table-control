using OneNorth.ExperienceAnalyticsTableControl.Api;
using System;

namespace OneNorth.ExperienceAnalyticsTableControl.DataSources
{
    public class SampleParameterDataSource
    {
        public ExperienceAnalyticsTableControlResponse Get(DateTime dateFrom, DateTime dateTo, string siteName, Guid id)
        {
            var reportData = new ExperienceAnalyticsTableControlData<dynamic>();

            var random = new Random();

            var count = 10;
            for (var i = 0; i < count; i++)
            {
                var item = new
                {
                    index = i,
                    id = Guid.NewGuid().ToString(),
                    datefrom = dateFrom.ToShortDateString(),
                    dateto = dateTo.ToShortDateString(),
                    sitename = siteName,
                    random = random.Next(0, 1000),
                    parameter = id.ToString()
                };

                reportData.AddItem(item);
            }
            var content = new ExperienceAnalyticsTableControlResponse()
            {
                Data = reportData,
                TotalRecordCount = count
            };

            return content;
        }
    }
}
