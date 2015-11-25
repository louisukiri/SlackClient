using System;
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
                _result = _sut.WithMessageText(_message);
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

            [Test]
            public void SetsAttachmentTextWhenPossible()
            {
                var randomMessage = Helpers.Random;
                var attachment = new SlackAttachment{};
                var sut = new SlackMessage();
                sut.Attach(attachment);

                sut.WithMessageText(randomMessage);

                Assert.AreEqual(randomMessage, attachment.Text);
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
                SutTestObject.Result = SutTestObject.Sut.AsAuthor(AuthorName, AuthorIcon, AuthorLink);
            }
            [Test]
            public void CreatesAttachmentIfNoCurrentAttachment()
            {
                var sut = new SlackMessage();
                sut.AsAuthor(AuthorName, AuthorIcon, AuthorLink);
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
                sutTestObject.Sut.AsAuthor(Helpers.Random, AuthorIcon, Helpers.Random);

                Assert.IsNull(sutTestObject.Sut.Author.Link);
            }
            [Test]
            public void SetsAuthorIconToNullIfInvalidUri()
            {
                var sutTestObject = new SlackMessageTestObject();
                sutTestObject.Sut.AsAuthor(Helpers.Random, Helpers.Random, AuthorLink);

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
                    .UsingLeftBarColor(SutTestObject.Color);
            }
            [Test]
            public void CreatesAttachmentIfNoCurrentAttachment()
            {
                var sut = new SlackMessage();
                sut.UsingLeftBarColor(Color.GreenYellow);
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

                Assert.AreEqual(initial + 1, _sut.Attachments.Count);
            }
            [Test]
            public void CanBeChained()
            {
                Assert.IsNotNull(_result);
                Assert.IsInstanceOf<SlackMessage>(_result);
            }
        }
        [TestFixture]
        public class SetMarkdownField
        {
            public SlackMessageTestObject SutTestObject;
            [SetUp]
            public void SetUp()
            {
                SutTestObject = new SlackMessageTestObject();
                SutTestObject.AddDefaultAttachment();
            }
            [Test]
            public void OnTextField()
            {
                SutTestObject.Result = SutTestObject.Sut.SetTextAsMarkDownField();
                Assert.AreEqual(SlackTextFields.Text, SutTestObject.DefaultAttachment.MarkdownFields);
            }
            [Test]
            public void OnPreTextField()
            {
                SutTestObject.Result = SutTestObject.Sut.SetPretextAsMarkDown();
                Assert.AreEqual(SlackTextFields.Pretext, SutTestObject.DefaultAttachment.MarkdownFields);
            }
            [Test]
            public void OnFieldsField()
            {
                SutTestObject.Result = SutTestObject.Sut.SetFieldsAsMarkDown();
                Assert.AreEqual(SlackTextFields.Fields, SutTestObject.DefaultAttachment.MarkdownFields);
            }
            [Test]
            public void CanBeStacked()
            {
                SutTestObject.Result = SutTestObject.Sut
                    .SetFieldsAsMarkDown()
                    .SetPretextAsMarkDown();

                Assert.IsTrue(SutTestObject.DefaultAttachment.MarkdownFields.HasFlag(SlackTextFields.Fields));
                Assert.IsTrue(SutTestObject.DefaultAttachment.MarkdownFields.HasFlag(SlackTextFields.Pretext));
            }
            [Test]
            public void CanBeChained()
            {
                SutTestObject.Result = SutTestObject.Sut.SetTextAsMarkDownField();
                Assert.IsNotNull(SutTestObject.Result);
                Assert.IsInstanceOf<SlackMessage>(SutTestObject.Result);
            }
        }
        [TestFixture]
        public class Title
        {
            private string _title;
            private string _randomUri;
            private SlackMessageTestObject _sutObject;
            private SlackMessage _result;
            [SetUp]
            public void Setup()
            {
                _title = Helpers.Random;
                _randomUri = "http://" + Helpers.Random + ".com/";
                _sutObject = new SlackMessageTestObject();
                _sutObject.AddDefaultAttachment();

                _result = _sutObject.Sut.Title(_title, _randomUri);
            }
            [Test]
            public void AddsTitleToLastAttachment()
            {
                Assert.AreEqual(_title, _sutObject.DefaultAttachment.Title.Name);
                Assert.AreEqual(_randomUri, _sutObject.DefaultAttachment.Title.Link.OriginalString);
            }
            [Test]
            public void CreateAttachmentWhenNoneExists()
            {
                _sutObject.ResetAttachments();
                var oldCount = _sutObject.Sut.Attachments.Count;
                _sutObject.Sut.Title(_title, _randomUri);

                Assert.AreEqual(0, oldCount);
                Assert.AreEqual(1, _sutObject.Sut.Attachments.Count);
            }

            [Test]
            public void SetLinkToNullGivenInvalidUrl()
            {
                _sutObject.Sut.Title(Helpers.Random, Helpers.Random);
                Assert.IsNull(_sutObject.DefaultAttachment.Title.Link);
            }
            [Test]
            public void CanBeChained()
            {
                Assert.IsNotNull(_result);
                Assert.IsInstanceOf<SlackMessage>(_result);
            }
        }
        [TestFixture]
        public class Field
        {
            private SlackMessageTestObject _testObject;
            private SlackMessage _result;
            private string _title;
            private string _value;
            private bool _short;
            [SetUp]
            public void Setup()
            {
                _title = Helpers.Random;
                _value = Helpers.Random;
                _short = true;

                _testObject = new SlackMessageTestObject();
                _testObject.AddDefaultAttachment();
                _result = _testObject.Sut.Field(_title, _value, _short);
            }
            [Test]
            public void AddsFieldToLastAttachment()
            {
                Assert.IsNotNull(_testObject.DefaultAttachment.Fields);
            }
            [Test]
            public void CreateAttachmentIfNoneExists()
            {
                _testObject.ResetAttachments();
                var initialCount = _testObject.Sut.Attachments.Count;
                _result = _testObject.Sut.Field(_title, _value, _short);

                Assert.AreEqual(0, initialCount);
                Assert.AreEqual(1, _testObject.Sut.Attachments.Count);
            }
            [Test]
            public void CanBeChained()
            {
                Assert.IsNotNull(_result);
                Assert.IsInstanceOf<SlackMessage>(_result);
            }
        }
        [TestFixture]
        public class Fallback
        {
            private SlackMessageTestObject _testObject;
            private SlackMessage _result;
            private string _fallBack;
            [SetUp]
            public void Setup()
            {
                _testObject = new SlackMessageTestObject();
                _testObject.AddDefaultAttachment();
                _fallBack = Helpers.Random;
                _result = _testObject.Sut.FallBack(_fallBack);
            }
            [Test]
            public void SetsFallBackOnTheLastAttachment()
            {
                Assert.AreEqual(_fallBack, _testObject.DefaultAttachment.Fallback);
            }
            [Test]
            public void CreateNewAttahmentWhenAttachmentEmpty()
            {
                _testObject.ResetAttachments();
                int initial = _testObject.Sut.Attachments.Count;

                _result = _testObject.Sut.FallBack(_fallBack);

                Assert.AreEqual(0, initial);
                Assert.AreEqual(1, _testObject.Sut.Attachments.Count);
            }

            [Test]
            public void CanBeChained()
            {
                Assert.IsNotNull(_result);
                Assert.IsInstanceOf<SlackMessage>(_result);
            }
        }
        [TestFixture]
        public class PreText
        {
            private SlackMessageTestObject _testObject;
            private SlackMessage _result;
            private string _preText;
            [SetUp]
            public void Setup()
            {
                _testObject = new SlackMessageTestObject();
                _testObject.AddDefaultAttachment();
                _preText = Helpers.Random;
                _result = _testObject.Sut.Pretext(_preText);
            }
            [Test]
            public void SetsPretextOnTheLastAttachment()
            {
                Assert.AreEqual(_preText, _testObject.DefaultAttachment.Pretext);
            }
            [Test]
            public void CreateNewAttahmentWhenAttachmentEmpty()
            {
                _testObject.ResetAttachments();
                int initial = _testObject.Sut.Attachments.Count;

                _result = _testObject.Sut.Pretext(_preText);

                Assert.AreEqual(0, initial);
                Assert.AreEqual(1, _testObject.Sut.Attachments.Count);
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
