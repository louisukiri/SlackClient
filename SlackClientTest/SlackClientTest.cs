using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Slack.Client;

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
