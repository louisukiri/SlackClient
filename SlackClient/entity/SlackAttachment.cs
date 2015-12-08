using System;
using System.Collections.Generic;
using System.Drawing;

namespace Slack.Client.entity
{
    [Flags]
    public enum SlackTextFields
    {
        Text = 1
        ,Pretext=2
        ,Fields=4
    }
    public class SlackAttachment
    {
        private readonly List<SlackField> _fields = new List<SlackField>(); 
        public string Fallback { get; set; }
        public Color Color = Color.Transparent;
        public string Pretext { get; set; }
        public SlackAuthor Author { get; set; }
        public LinkedElement Title { get; set; }
        public Uri ImageUri { get; set; }
        public Uri ThumbUri { get; set; }
        public string Text { get; set; }
        public IReadOnlyList<SlackField> Fields {
            get { return _fields; }
        }
        public SlackTextFields MarkdownFields { get; set; }
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
