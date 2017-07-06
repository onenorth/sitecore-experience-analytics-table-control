using Sitecore.ExperienceAnalytics.Api.Response;
using System.Collections.Generic;
using System.Linq;

namespace OneNorth.ExperienceAnalyticsTableControl.Api
{
    public class ExperienceAnalyticsTableControlResponse
    {
        private readonly IList<Message> _messages = new List<Message>();

        public IExperienceAnalyticsTableControlData Data { get; set; }

        public Message[] Messages
        {
            get
            {
                return Enumerable.ToArray(_messages);
            }
        }

        public int TotalRecordCount { get; set; }

        public void AddMessage(Message message)
        {
            _messages.Add(message);
        }
    }
}