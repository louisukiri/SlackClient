
using System;
using System.Drawing;
using NUnit.Framework;
using Slack.Client;
using Slack.Client.entity;
using System.Data;

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

        [Test, Category("Integrated Test"), Ignore]
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
        [Test, Category("Integrated Test"), Ignore]
        public void MoreTalking()
        {
            DataTable dtable = new DataTable();
            dtable.Columns.Add("Class");
            dtable.Columns.Add("Errors/Failures/Totals");

            dtable.Rows.Add("BusinessObjects.Domains.Tests-nunitTestResults", "0/0/64");
            dtable.Rows.Add("BusinessObjects.Domains.Tests-nunitTestResults", "0/6/6");

            sut = new SlackClient(new Uri("https://hooks.slack.com/services/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").ToString());
            var msg = new SlackMessage();
            //msg.Attach(new SlackAttachment());
            msg.AddData(dtable)
                //.UsingLeftBarColor(Color.White)
                ;

            sut.Send(msg);
        }
    }
}
