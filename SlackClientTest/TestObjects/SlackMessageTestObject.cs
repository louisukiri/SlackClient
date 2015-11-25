using System.Drawing;
using NUnit.Framework;
using Slack.Client.entity;

namespace SlackClientTest.TestObjects
{
    public class SlackMessageTestObject
    {
        public SlackMessage Sut = new SlackMessage();
        public readonly SlackAttachment DefaultAttachment = new SlackAttachment();
        public SlackMessage Result;
        //008000
        public readonly Color Color = Color.Green;
        public void AddDefaultAttachment()
        {
            Sut.Attach(DefaultAttachment);
        }

        internal void ResetAttachments()
        {
            Sut = new SlackMessage();
        }
    }
}
