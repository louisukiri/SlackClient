
using System;
using NUnit.Framework;
using Slack.Client;

namespace SlackClientIntegratedTest
{
    [TestFixture]
    public class SlackTest
    {
        [Test, Ignore]
        public void Talks()
        {
            SlackClient sut = new SlackClient("https://hooks.slack.com/services/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");
            sut.Send("testing send " + Guid.NewGuid());
        }
    }
}
