
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

        [Test, Category("Integrated Test")]
        public void ComplicatedTalk()
        {
            var msg = new SlackMessage();
            
            msg.Title("Testing *title*")
                .FallBack("this is fallbacktext")
                .UsingLeftBarColor(Color.Orange)
                .WithMessageText("okayhhiuo *jim* ok - _italic?_ <mailto:bob@example.com|Bob>")
                .AsAuthor("Louis")
                .AsUser("Some User")
                .Field("Coverage Result", "10%\n20%\n30%", true)
                .Field("Package Name", "godaddy.domain.cicd.trigger", true)
                .SetTextAsMarkDownField()
                ;
            
            sut.Send(msg);

            
        }
    }
}
