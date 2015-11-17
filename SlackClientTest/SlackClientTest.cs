using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Slack.Client;
using Slack.Client.entity;

namespace SlackClientTest
{
    [TestFixture]
    public class SlackClientTest
    {
        [TestFixture]
        public class ObjectRequires
        {
            [Test]
            public void WebHookUrl()
            {
                var sut = new SlackClient(Helpers.Random);
                Assert.IsNotNull(sut.WebHookUrl);
            }
        }

        public class GettingJson
        {
            [Test]
            public void ReturnsTextKey()
            {
                var sut = new SlackClient(Helpers.Random);
                var jsonString = sut.GetJson(Helpers.Random);
                var jobject = JObject.Parse(jsonString);
                Assert.IsNotNull(jobject.SelectToken("text"));
            }
        }
        public class GettingJsonFromSlackMessage
        {
            private SlackClient sut;
            private SlackMessage _message;
            private SlackAttachment _attachment;
            private string _fallback=Helpers.Random;
            private string _pretext;
            private Color _color;
            private string _authorName;
            private Uri _authorLink;
            private Uri _authorIcon;
            private string _titleName;
            private Uri _titleLink;
            private string _text;
            private string _fieldTitle;
            private string _fieldValue;
            private bool _fieldShort;
            private string _username;
            private Uri _thumbUrl;
            private Uri _imageUrl;
            private string _json;
            private Dictionary<string, string> _JsonPathAndValues;
                
            [SetUp]
            public void Setup()
            {
                sut = new SlackClient(Helpers.RandomUri.OriginalString);
                _JsonPathAndValues = new Dictionary<string, string>
                {
                    {"attachments[0].fallback",Helpers.Random}
                    ,{"attachments[0].color",Color.Gold.ToHex()}
                    ,{"attachments[0].pretext",Helpers.Random}
                    ,{"attachments[0].text",Helpers.Random}
                    ,{"attachments[0].author_name",Helpers.Random}
                    ,{"attachments[0].author_icon",Helpers.RandomUri.OriginalString}
                    ,{"attachments[0].author_link",Helpers.RandomUri.OriginalString}
                    ,{"attachments[0].title",Helpers.Random}
                    ,{"attachments[0].title_link",Helpers.RandomUri.OriginalString}
                    ,{"attachments[0].fields[0].title",Helpers.Random}
                    ,{"attachments[0].fields[0].value",Helpers.Random}
                    ,{"attachments[0].fields[0].short","True"}
                    ,{"attachments[0].image_url",Helpers.RandomUri.OriginalString}
                    ,{"attachments[0].thumb_url",Helpers.RandomUri.OriginalString}
                    ,{"username",Helpers.Random}
                };
                _fallback = _JsonPathAndValues["attachments[0].fallback"];
                _pretext = _JsonPathAndValues["attachments[0].pretext"];
                _color =  _JsonPathAndValues["attachments[0].color"].ToColor();
                _authorIcon = new Uri(_JsonPathAndValues["attachments[0].author_icon"]);
                _authorName = _JsonPathAndValues["attachments[0].author_name"];
                _authorLink = new Uri(_JsonPathAndValues["attachments[0].author_link"]); ;
                _titleName = _JsonPathAndValues["attachments[0].title"];
                _titleLink = new Uri(_JsonPathAndValues["attachments[0].title_link"]);
                _text = _JsonPathAndValues["attachments[0].text"];
                _fieldTitle = _JsonPathAndValues["attachments[0].fields[0].title"];
                _fieldValue = _JsonPathAndValues["attachments[0].fields[0].value"];
                _fieldShort = bool.Parse(_JsonPathAndValues["attachments[0].fields[0].short"]);
                _thumbUrl = new Uri(_JsonPathAndValues["attachments[0].thumb_url"]);
                _imageUrl = new Uri(_JsonPathAndValues["attachments[0].image_url"]);
                _username = _JsonPathAndValues["username"];
                _attachment = new SlackAttachment
                {
                    Fallback = _fallback
                    ,Author = new SlackAuthor { Icon = _authorIcon, Link = _authorLink, Name = _authorName}
                    ,Color = _color
                    ,Pretext = _pretext
                    ,Title = new LinkedElement {Link = _titleLink, Name = _titleName}
                    ,Text = _text
                    ,ImageUri = _imageUrl
                    ,ThumbUri = _thumbUrl
                    
                };
                _attachment.AddField(_fieldTitle, _fieldValue, _fieldShort);
                _message = new SlackMessage
                {
                    Username = _username
                };
                _message.SetMessage(_text);
                _message.Attach(_attachment);

                _json = sut.GetJson(_message);
            }

            [TestCase("attachments[0].fallback")]
            [TestCase("attachments[0].color")]
            [TestCase("attachments[0].pretext")]
            [TestCase("attachments[0].text")]
            [TestCase("username")]
            [TestCase("attachments[0].title")]
            [TestCase("attachments[0].title_link")]
            [TestCase("attachments[0].author_icon")]
            [TestCase("attachments[0].author_link")]
            [TestCase("attachments[0].author_name")]
            [TestCase("attachments[0].image_url")]
            [TestCase("attachments[0].thumb_url")]
            [TestCase("attachments[0].fields[0].title")]
            [TestCase("attachments[0].fields[0].value")]
            [TestCase("attachments[0].fields[0].short")]
            public void ReturnsJsonPath(string path)
            {
                var jObject = JObject.Parse(_json);
                var val = jObject.SelectToken(path, true);

                Assert.IsNotNull(val, message:"\"" + path + "\" does not exist in serialized JSON");
                Assert.AreEqual(_JsonPathAndValues[path], val.ToString());
            }
        }
        public class SendingMessage
        {
            [Test]
            public void SendsJsonBodyToWebHookUrl()
            {
                var sutMock = new Mock<SlackClient>(Helpers.Random) 
                {CallBase = true};
                string json = Helpers.RandomJson;
                sutMock.Setup(z => z.GetJson(It.IsAny<string>())).Returns(json);
                sutMock.Setup(z => z.Say(It.IsAny<string>()));

                sutMock.Object.Send(Helpers.Random);

                sutMock.Verify(z=> z.Say(json));

            }
        }
    }
}
