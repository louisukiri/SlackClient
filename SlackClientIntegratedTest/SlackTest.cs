
using System;
using System.Drawing;
using NUnit.Framework;
using Slack.Client;
using Slack.Client.entity;

namespace SlackClientIntegratedTest
{
    [TestFixture]
    public class SlackTest
    {
        private SlackClient sut;
        [SetUp]
        public void Setup()
        {
            sut = new SlackClient("https://hooks.slack.com/services/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");   
        }
        [Test, Ignore, Category("Integrated Test")]
        public void Talks()
        {
            sut.Send("testing send " + Guid.NewGuid());
        }

        [Test, Category("Integrated Test"),Ignore]
        public void ComplicatedTalk()
        {
            var msg = new SlackMessage();
            msg.Title("Testing *title*")
                .FallBack("this is fallbacktext")
                .Color(Color.Indigo)
                .SetMessage("okay jim ok")
                .As("Louis")
                ;
            
            sut.Send(msg);

            
        }
    }
}
