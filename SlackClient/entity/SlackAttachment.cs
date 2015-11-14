using System.Collections.Generic;
using System.Drawing;

namespace Slack.Client.entity
{
    public class SlackAttachment
    {
        public string Fallback { get; set; }
        public Color Color { get; set; }
        public string Pretext { get; set; }
        public SlackAuthor Author { get; set; }
        public LinkedElement Title { get; set; }
        public string Text { get; set; }
        public IList<SlackField> Fields { get; set; }
    }
}
