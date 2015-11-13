using System.Collections.Generic;

namespace Slack.Client.entity
{
    public class SlackMessage
    {
        public string Username { get; set; }
        public IList<SlackAttachment> Attachments { get; set; }
    }

    public class SlackAttachment
    {
        public string Fallback { get; set; }
        public string Color { get; set; }
        public string Pretext { get; set; }
        public SlackAuthor Author { get; set; }
        public LinkedElement Title { get; set; }
        public string Text { get; set; }
        public IList<SlackField> Fields { get; set; } 
    }

    public class LinkedElement
    {
        public string Name { get; set; }
        public string Link { get; set; }
    }
    public class SlackAuthor: LinkedElement
    {
        public string Icon { get; set; }
    }

    public class SlackField
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public bool Short { get; set; }
    }
}
