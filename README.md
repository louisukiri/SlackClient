# SlackClient

Send messages to Slack using Incoming webhooks.

Here's an example:

```C#
var msg = new SlackMessage();
  
msg.Title("Testing *title*")
.FallBack("this is fallbacktext")
.UsingLeftBarColor(Color.Orange)
.WithMessageText("okayhhiuo *jim* ok - _italic?_ <mailto:bob@example.com|Bob>")
.AsAuthor("Louis")
.AsUser("Some User")
.Field("Coverage Result", "10%\n20%\n30%", true)
.Field("Package Name", "some.text", true)
.SetTextAsMarkDownField();

var SlackClient = new SlackClient("<<HOOK_URL>>");
SlackClient.Send(msg);
```
