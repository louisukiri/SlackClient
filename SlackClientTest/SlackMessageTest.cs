using NUnit.Framework;
using Slack.Client.entity;
using System.Drawing;
using SlackClientTest.TestObjects;

namespace SlackClientTest
{
    [TestFixture]
    public class SlackMessageTest
    {
        [TestFixture]
        public class AuthorProperty
        {
            private readonly SlackMessageTestObject _sutTestObject = new SlackMessageTestObject();
            [Test]
            public void ReturnsLastAttachmentsAuthor()
            {
                _sutTestObject.DefaultAttachment.Author = new SlackAuthor();
                _sutTestObject.AddDefaultAttachment();

                Assert.IsNotNull(_sutTestObject.Sut.Author);
            }

            [Test]
            public void ReturnsNullWhenThereAreNoAttachments()
            {
                var sut = new SlackMessage();
                Assert.IsNull(sut.Author);
            }
        }
        [TestFixture]
        public class SettingMessage
        {
            readonly SlackMessage _sut = new SlackMessage();
            private string _message = string.Empty;
            private SlackMessage _result;
            [SetUp]
            public void Setup()
            {
                _message = Helpers.Random;
                _result = _sut.SetMessage(_message);
            }
            [Test]
            public void SetsMessage()
            {
                Assert.AreEqual(_message, _sut.Message);
            }            
            [Test]
            public void CanBeChained()
            {
                Assert.IsNotNull(_result);
                Assert.IsInstanceOf<SlackMessage>(_result);
            }
        }
        [TestFixture]
        public class As
        {
            public readonly SlackMessageTestObject SutTestObject = new SlackMessageTestObject();
            public string AuthorName = Helpers.Random;
            public string AuthorIcon = "http://"+ Helpers.Random+".com/";
            public string AuthorLink = "http://"+ Helpers.Random+".com/";
            [SetUp]
            public void SetUp()
            {
                SutTestObject.AddDefaultAttachment();
                SutTestObject.Result = SutTestObject.Sut.As(AuthorName, AuthorIcon, AuthorLink);
            }
            [Test]
            public void CreatesAttachmentIfNoCurrentAttachment()
            {
                var sut = new SlackMessage();
                sut.As(AuthorName, AuthorIcon, AuthorLink);
                Assert.AreEqual(1, sut.Attachments.Count);
            }
            [Test]
            public void SetsLastAttachmentsAuthor()
            {
                Assert.AreEqual(AuthorName, SutTestObject.DefaultAttachment.Author.Name);
                Assert.AreEqual(AuthorIcon, SutTestObject.DefaultAttachment.Author.Icon.OriginalString);
                Assert.AreEqual(AuthorLink, SutTestObject.DefaultAttachment.Author.Link.OriginalString);
            }

            [Test]
            public void SetsAuthorLinkToNullIfInvalidUrl()
            {
                var sutTestObject = new SlackMessageTestObject();
                sutTestObject.Sut.As(Helpers.Random, AuthorIcon, Helpers.Random);

                Assert.IsNull(sutTestObject.Sut.Author.Link);
            }
            [Test]
            public void SetsAuthorIconToNullIfInvalidUri()
            {
                var sutTestObject = new SlackMessageTestObject();
                sutTestObject.Sut.As(Helpers.Random, Helpers.Random, AuthorLink);

                Assert.IsNull(sutTestObject.Sut.Author.Icon);
            }
            [Test]
            public void CanBeChained()
            {
                Assert.IsNotNull(SutTestObject.Result);
                Assert.IsInstanceOf<SlackMessage>(SutTestObject.Result);
            }
        }
        [TestFixture]
        public class AddingColor
        {
            public readonly SlackMessageTestObject SutTestObject = new SlackMessageTestObject();
            [SetUp]
            public void SetUp()
            {
                SutTestObject.AddDefaultAttachment();
                SutTestObject.Result = SutTestObject.Sut
                    .Color(SutTestObject.Color);
            }
            [Test]
            public void CreatesAttachmentIfNoCurrentAttachment()
            {
                var sut = new SlackMessage();
                sut.Color(Color.GreenYellow);
                Assert.AreEqual(1, sut.Attachments.Count);
            }
            [Test]
            public void SetsColorForLastAttachment()
            {
                Assert.AreEqual(SutTestObject.Color, SutTestObject.DefaultAttachment.Color);
            }

            [Test]
            public void CanBeChained()
            {
                Assert.IsNotNull(SutTestObject.Result);
                Assert.IsInstanceOf<SlackMessage>(SutTestObject.Result);
            }
        }
        [TestFixture]
        public class AddingAttachment
        {
            readonly SlackMessage _sut = new SlackMessage();
            private SlackMessage _result;
            [Test]
            public void AddsToAttachments()
            {
                var attachment = new SlackAttachment();
                int initial = _sut.Attachments.Count;

                _result = _sut.Attach(attachment);

                Assert.AreEqual(initial+1, _sut.Attachments.Count);
            }
            [Test]
            public void CanBeChained()
            {
                Assert.IsNotNull(_result);
                Assert.IsInstanceOf<SlackMessage>(_result);
            }
        }
    }
}
