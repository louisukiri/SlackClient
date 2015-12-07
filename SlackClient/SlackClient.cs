using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Slack.Client.Converters;
using Slack.Client.entity;
using Slack.Client.Resolvers;

namespace Slack.Client
{
    public class SlackClient
    {
        public string WebHookUrl { get; private set; }
        public SlackClient(string webbHookUrl)
        {
            WebHookUrl = webbHookUrl;
        }

        public virtual string GetJson(string content)
        {
            return string.Format("{{\"text\":\"{0}\"}}",content);
        }

        public void Send(string content)
        {
            string text = GetJson(content);
            Say(text);
        }

        public void Send(SlackMessage message)
        {
            string text = GetJson(message);
            Say(text);
        }
        public string GetJson(SlackMessage message)
        {
            var settings = new JsonSerializerSettings {ContractResolver = new LowerCaseContractResolver()
                , NullValueHandling = NullValueHandling.Ignore
            };
            //settings.Converters.Add(new LinkedItemConverter());

            return JsonConvert.SerializeObject(message, settings);
        }
        public virtual void Say(string jsonText)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(jsonText);
                var response = client.PostAsync(WebHookUrl, content).Result;
            }
        }

    }
}
