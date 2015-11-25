
using System;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Slack.Client.entity;
using Slack.Client.Resolvers;
using System.Collections.Generic;

namespace SlackClientTest
{
    [TestFixture]
    public class ContractResolverTest
    {
        private SlackMessage _slackMessage;
        private JsonSerializerSettings settings;
        private SlackAttachment _attachment;
        [SetUp]
        public void Setup()
        {
            _slackMessage = new SlackMessage();
            _attachment = new SlackAttachment();
            _slackMessage.Attach(_attachment);
            settings = new JsonSerializerSettings { ContractResolver = new LowerCaseContractResolver() };
        }
        [Test]
        public void ConvertsAllKeysAndValuesToLowerCase()
        {
            var message = Helpers.Random;
            _slackMessage.WithMessageText(message);

            var result = JsonConvert.SerializeObject(message, settings);

            var isAllLower = result.All(c => !Char.IsUpper(c));
            Assert.IsTrue(isAllLower);
        }
        [Test]
        public void ReturnsTitleFromFlattenedTitleProperty()
        {
            var title = Helpers.Random;
            var titleUri = Helpers.RandomUri;
            const string path = "attachments[0].title";
            _attachment.Title = new LinkedElement {Link = titleUri, Name = title};

            var val = Act(path);

            Assert.IsNotNull(val, message: "\"" + path + "\" does not exist in serialized JSON");
            Assert.AreEqual(title, val.ToString());
        }
        [Test]
        public void ReturnsTitleLinkFromFlattenedTitleProperty()
        {
            var title = Helpers.Random;
            var titleUri = Helpers.RandomUri;
            const string path = "attachments[0].title_link";
            _attachment.Title = new LinkedElement { Link = titleUri, Name = title };

            var val = Act(path);

            Assert.IsNotNull(val, message: "\"" + path + "\" does not exist in serialized JSON");
            Assert.AreEqual(titleUri.OriginalString, val.ToString());
        }
        [Test]
        public void DontReturnTitleLinkWhenTitleLinkPropertyIsNull()
        {
            var title = Helpers.Random;
            var titleUri = Helpers.RandomUri;
            const string path = "attachments[0].title_link";
            _attachment.Title = new LinkedElement { Link = null, Name = title };

            var val = Act(path,false);

            Assert.IsNull(val);
        }
        [Test]
        public void ReturnsAuthorIconFromFlattenedAuthorProperty()
        {
            const string path = "attachments[0].author_icon";
            var authorIcon = Helpers.RandomUri;
            _attachment.Author = new SlackAuthor {Icon = authorIcon};

            var val = Act(path);

            Assert.IsNotNull(val, message: "\"" + path + "\" does not exist in serialized JSON");
            Assert.AreEqual(authorIcon.OriginalString, val.ToString());
        }
        [Test]
        public void ReturnsAuthorLinkFromFlattenedAuthorProperty()
        {
            const string path = "attachments[0].author_link";
            var authorLink = Helpers.RandomUri;
            _attachment.Author = new SlackAuthor { Link = authorLink };

            var val = Act(path);

            Assert.IsNotNull(val, message: "\"" + path + "\" does not exist in serialized JSON");
            Assert.AreEqual(authorLink.OriginalString, val.ToString());
        }
        [Test]
        public void ReturnsAuthorNameFromFlattenedAuthorProperty()
        {
            const string path = "attachments[0].author_name";
            var authorName = Helpers.Random;
            _attachment.Author = new SlackAuthor { Name = authorName };

            var val = Act(path);

            Assert.IsNotNull(val, message: "\"" + path + "\" does not exist in serialized JSON");
            Assert.AreEqual(authorName, val.ToString());
        }
        [Test]
        public void ReturnsImageUrlFromFlattenedUriProperty()
        {
            const string path = "attachments[0].image_url";
            var randomUri = Helpers.RandomUri;
            _attachment.ImageUri = randomUri;

            var val = Act(path);

            Assert.IsNotNull(val, message: "\"" + path + "\" does not exist in serialized JSON");
            Assert.AreEqual(randomUri.OriginalString, val.ToString());
        }
        [Test]
        public void ReturnsThumbUrlFromFlattenedThumbProperty()
        {
            const string path = "attachments[0].image_url";
            var randomUri = Helpers.RandomUri;
            _attachment.ImageUri = randomUri;

            var val = Act(path);

            Assert.IsNotNull(val, message: "\"" + path + "\" does not exist in serialized JSON");
            Assert.AreEqual(randomUri.OriginalString, val.ToString());
        }
        [Test]
        public void ReturnsColorFromColorProperty()
        {
            const string path = "attachments[0].color";
            var colorName = Color.Goldenrod;
            _attachment.Color = colorName;

            var val = Act(path);

            Assert.IsNotNull(val, message: "\"" + path + "\" does not exist in serialized JSON");
            Assert.AreEqual(colorName.ToHex(), val.ToString());
        }
        [TestCase(SlackTextFields.Text, "text")]
        [TestCase(SlackTextFields.Pretext, "pretext")]
        [TestCase(SlackTextFields.Fields, "fields")]
        public void ReturnsMarkdwnArrayFor(SlackTextFields slackTextFields, string expected) {
            const string path = "attachments[0].mrkdwn_in";
            _attachment.MarkdownFields = slackTextFields;

            var val = Act(path,false);

            Assert.IsNotNull(val, message: "\"" + path + "\" does not exist in serialized JSON");
            var valArray = val.ToObject<List<string>>();
            Assert.IsTrue(valArray.Contains(expected));
        }
        [Test]
        public void RemovesMarkDownFields()
        {
            const string path = "attachments[0].markdownfields";
            _attachment.MarkdownFields = SlackTextFields.Pretext;

            var val = Act(path, false);

            Assert.IsNull(val, message: "\"" + path + "\" exists in serialized JSON");
        }
        private JToken Act(string path, bool errorWhenNoMatch=true)
        {
            var result = JsonConvert.SerializeObject(_slackMessage, settings);

            var jObject = JObject.Parse(result);
            var val = jObject.SelectToken(path, errorWhenNoMatch);
            return val;
        }
    }
}