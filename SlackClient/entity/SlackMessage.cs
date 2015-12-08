using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace Slack.Client.entity
{
    public class SlackMessage
    {
        private readonly IList<SlackAttachment> _attachments;
        public string Username { get; private set; }
        public IReadOnlyList<SlackAttachment> Attachments {
            get { return _attachments.ToList(); }
        }

        public string Message { get; private set; }

        public SlackMessage()
        {
            _attachments = new List<SlackAttachment>();
        }
        public SlackMessage WithMessageText(string message)
        {
            Message = message;
            if (Attachments.Any())
            {
                Attachments.Last().Text = message;
            }
            return this;
        }

        public SlackMessage Attach(SlackAttachment attachment)
        {
            _attachments.Add(attachment);
            return this;
        }

        private SlackAttachment LastAttachment
        {
            get
            {
                if (!_attachments.Any())
                {
                    Attach(new SlackAttachment());
                }
                return _attachments.Last();
            }
        }
        public SlackMessage UsingLeftBarColor(Color color)
        {
            LastAttachment.Color = color;
            return this;
        }
        public SlackMessage AsUser(string UserName)
        {
            this.Username = UserName;
            return this;
        }
        public SlackMessage AsAuthor(string authorName, string authorIconUri="", string authorLinkUri="")
        {
            Uri authorLink;
            Uri authorIcon;
            try
            {
                authorLink = new Uri(authorLinkUri);
            }
            catch (Exception)
            {
                authorLink = null;
            }
            try
            {
                authorIcon = new Uri(authorIconUri);
            }
            catch (Exception)
            {
                authorIcon = null;
            }
            return As(new SlackAuthor{
                Name = authorName
                ,Icon = authorIcon
                ,Link = authorLink
            });
        }
        public SlackMessage As(SlackAuthor author)
        {
            LastAttachment.Author = author;
            return this;
        }

        public SlackAuthor Author {
            get
            {
                return !_attachments.Any() ? null : LastAttachment.Author;
            }
        }

        public SlackMessage Title(string title)
        {
            return Title(title,(Uri)null);
        }
        public SlackMessage Title(string title, Uri link)
        {
            LastAttachment.Title = new LinkedElement {Link = link, Name = title};
            return this;
        }
        public SlackMessage Title(string title, string link)
        {
            Uri linkUri;
            try
            {
                linkUri = new Uri(link);
            }
            catch (UriFormatException)
            {
                linkUri = null;
            }
            return Title(title, linkUri);
        }

        public SlackMessage Field(string title, string value, bool Short)
        {
            LastAttachment.AddField(title, value, Short);
            return this;
        }

        public SlackMessage FallBack(string fallbackText)
        {
            LastAttachment.Fallback = fallbackText;
            return this;
        }

        public SlackMessage Pretext(string text)
        {
            LastAttachment.Pretext = text;
            return this;
        }

        public SlackMessage SetTextAsMarkDownField()
        {
            LastAttachment.MarkdownFields |= SlackTextFields.Text;

            return this;
        }

        public SlackMessage SetPretextAsMarkDown()
        {
            LastAttachment.MarkdownFields |= SlackTextFields.Pretext;

            return this;
        }

        public SlackMessage SetFieldsAsMarkDown()
        {
            LastAttachment.MarkdownFields |= SlackTextFields.Fields;

            return this;
        }

        public SlackMessage AddData(DataTable Table)
        {
            foreach(DataColumn column in Table.Columns)
            {
                string val = string.Empty;
                foreach(DataRow row in Table.Rows)
                {
                    val += row[column].ToString()+"\n";
                }
                val = val.TrimEnd('\n');
                Field(column.ColumnName, val, true);
            }
            return this;
        }
    }
    public static class ColorExtension
    {
        public static string ToHex(this Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static Color ToColor(this string Hex)
        {
            return ColorTranslator.FromHtml(Hex);
        }
    }
}
