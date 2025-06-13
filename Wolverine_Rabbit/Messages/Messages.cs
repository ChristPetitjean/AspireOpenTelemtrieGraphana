using Wolverine.Attributes;

[MessageIdentity("Ping")]
public class PingMessage
{
    public int Number { get; set; }
}

[MessageIdentity("Pong")]
public class PongMessage
{
    public int Number { get; set; }
}