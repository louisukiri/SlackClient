using System;
using System.Collections.Generic;
using System.Drawing;

namespace Slack.Client.entity
{
    public class SlackAttachment
    {
        private readonly List<SlackField> _fields = new List<SlackField>(); 
        public string Fallback { get; set; }
        public Color Color { get; set; }
        public string Pretext { get; set; }
        public SlackAuthor Author { get; set; }
        public LinkedElement Title { get; set; }
        public Uri ImageUri { get; set; }
        public Uri ThumbUri { get; set; }
        public string Text { get; set; }
        public IReadOnlyList<SlackField> Fields {
            get { return _fields; }
        }

        public void AddField(string title, string value, bool Short)
        {
            AddField(new SlackField
            {
                Title = title
                ,Value = value
                ,Short = Short
            });
        }
        public void AddField(SlackField field)
        {
            _fields.Add(field);
        }
    }
}
