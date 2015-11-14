using System;

namespace Slack.Client.entity
{
    public class LinkedElement
    {
        public string Name { get; set; }
        public Uri Link { get; set; }
    }
    public class SlackAuthor : LinkedElement
    {
        public Uri Icon { get; set; }
    }
}
