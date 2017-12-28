namespace OneNorth.ExperienceAnalyticsTableControl.Api
{
    public class Message
    {
        public const string Info = "INFO";

        public string MessageType
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public Message(string messageType, string text)
        {
            this.MessageType = messageType;
            this.Text = text;
        }

        public Message()
        {
            this.MessageType = "INFO";
            this.Text = string.Empty;
        }
    }
}