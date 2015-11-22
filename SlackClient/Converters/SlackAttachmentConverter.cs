using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Slack.Client.entity;
using Slack.Client.Resolvers;
using System.Collections.Generic;

namespace Slack.Client.Converters
{
    public class SlackItemConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(SlackAttachment);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        } 
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var objectToken = JToken.FromObject(value, serializer);
            var attachment = value as SlackAttachment;

            if (attachment == null)
                return;
            var o = (JObject)objectToken;

            FlattenAuthorProperty(o, attachment);
            FlattenTitleProperty(o, attachment);

            List<string> markdownFields = FlattenMarkdownField(attachment);
            o.RemoveField("markdownfields");
            if (markdownFields.Any())
            {
                o.Add("mrkdwn_in", JToken.FromObject(markdownFields));
            }
            //imageuri, thumuri and color all need to be rewritten
            //their names need to match what slack expects in its payload
            if (attachment.ImageUri != null)
                o.Replace("imageuri", "image_url", attachment.ImageUri.OriginalString);
            if (attachment.ThumbUri != null)
                o.Replace("thumburi", "thumb_url", attachment.ThumbUri.OriginalString);
            o.Replace("color", "color", attachment.Color.ToHex());

            serializer.Serialize(writer, o);
        }
        private List<string> FlattenMarkdownField(SlackAttachment attachment)
        {
            List<string> markdownFields = new List<string>();
            if (attachment.MarkdownFields.HasFlag(SlackTextFields.Text))
            {
                markdownFields.Add("text");
            }
            if (attachment.MarkdownFields.HasFlag(SlackTextFields.Pretext))
            {
                markdownFields.Add("pretext");
            }
            if (attachment.MarkdownFields.HasFlag(SlackTextFields.Fields))
            {
                markdownFields.Add("fields");
            }
            return markdownFields; 
        }

        private static void FlattenTitleProperty(JObject o, SlackAttachment attachment)
        {
            var titleProp = o.Children<JProperty>().FirstOrDefault(z => z.Name == "title");
            if (titleProp != null && attachment.Title != null)
            {
                titleProp.Value = attachment.Title.Name;
                if (attachment.Title.Link != null)
                    titleProp.AddAfterSelf(new JProperty("title_link", attachment.Title.Link.OriginalString));
            }
        }

        private static void FlattenAuthorProperty(JObject o, SlackAttachment attachment)
        {
            var authorProp = o.Children<JProperty>().FirstOrDefault(z => z.Name == "author");
            if (authorProp != null && attachment.Author != null)
            {
                authorProp.AddAfterSelf(new JProperty("author_name", attachment.Author.Name));
                authorProp.AddAfterSelf(new JProperty("author_link", attachment.Author.Link));
                authorProp.AddAfterSelf(new JProperty("author_icon", attachment.Author.Icon));
                authorProp.Remove();
            }
        }
    }
    static class JConvertExtensions
    {
        public static void Replace(this JProperty oldProperty, string newPropertyName, string newPropertyValue)
        {
            if (String.Equals(oldProperty.Name.ToLower(), newPropertyName.ToLower()))
            {
                oldProperty.Value = newPropertyValue;
                return;
            }
            oldProperty.AddAfterSelf(new JProperty(newPropertyName, newPropertyValue));
            oldProperty.Remove();
        }
        public static void Replace(this JObject jsonObject, string currentPropertyName, string newPropertyName, string newPropertyValue)
        {
            var oldProperty = jsonObject.Children<JProperty>().FirstOrDefault(z => z.Name == currentPropertyName);
            oldProperty.Replace(newPropertyName, newPropertyValue);
        }
        public static void RemoveField(this JObject o, string fieldName)
        {
            var Property = o.Children<JProperty>().FirstOrDefault(z => z.Name == fieldName);
            if (Property != null)
            {
                Property.Remove();
            }
        }
    }
}
