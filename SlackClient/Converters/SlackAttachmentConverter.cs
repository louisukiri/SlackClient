using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Slack.Client.entity;
using Slack.Client.Resolvers;

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
            var objectToken = JToken.FromObject(value,serializer);
            var attachment = value as SlackAttachment;

            if (attachment == null)
                return;
            var o = (JObject) objectToken;

            var authorProp = o.Children<JProperty>().FirstOrDefault(z => z.Name == "author");
            if (authorProp != null)
            {
                authorProp.AddAfterSelf(new JProperty("author_name", attachment.Author.Name));
                authorProp.AddAfterSelf(new JProperty("author_link", attachment.Author.Link));
                authorProp.AddAfterSelf(new JProperty("author_icon", attachment.Author.Icon));
                authorProp.Remove();
            }
            var titleProp = o.Children<JProperty>().FirstOrDefault(z => z.Name == "title");
            if (titleProp != null && attachment.Title != null)
            {
                
                titleProp.Value = attachment.Title.Name;
                if(attachment.Title.Link != null)
                    titleProp.AddAfterSelf(new JProperty("title_link", attachment.Title.Link.OriginalString));
            }
            var imageUriProp = o.Children<JProperty>().FirstOrDefault(z => z.Name == "imageuri");
            if (imageUriProp != null && attachment.ImageUri != null)
            {
                imageUriProp.Replace("image_url", attachment.ImageUri.OriginalString);
            }
            var thumbUriProp = o.Children<JProperty>().FirstOrDefault(z => z.Name == "thumburi");
            if (thumbUriProp != null && attachment.ThumbUri != null)
            {
                thumbUriProp.Replace("thumb_url", attachment.ThumbUri.OriginalString);
            }

            var colorProp = o.Children<JProperty>().FirstOrDefault(z => z.Name == "color");

            if (colorProp != null)
            {
                colorProp.Replace("color", attachment.Color.ToHex());
            }

            serializer.Serialize(writer, o);
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
    }
}
