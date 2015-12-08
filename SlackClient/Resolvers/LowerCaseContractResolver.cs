using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Slack.Client.Converters;
using Slack.Client.entity;

namespace Slack.Client.Resolvers
{
    public class LowerCaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            if (property.PropertyName.ToLower() == "message")
            {
                //the message field in SlackMessage needs a text field, not a message
                property.PropertyName = "text";
                return property;
            }
            if (!typeof (IReadOnlyList<SlackAttachment>).IsAssignableFrom(property.PropertyType)) return property;
            property.ItemConverter = new SlackItemConverter();
            return property;
        }
    }
}
