namespace Wolverine_Rabbit.MessageHandlers;

// Simple message handler for the PongMessage responses
// The "Handler" suffix is important as a naming convention
// to let Wolverine know that it should build a message handling
// pipeline around public methods on this class
public class PongHandler(ILogger<PongHandler> logger)
{
    // "Handle" is recognized by Wolverine as a message handling
    // method. Handler methods can be static or instance methods
    public void Handle(PongMessage message)
    {
        logger.LogInformation("[blue]Got pong #{MessageNumber}[/]", message.Number);
    }
    
    // "Handle" is recognized by Wolverine as a message handling
    // method. Handler methods can be static or instance methods
    public void Handle(PingMessage message)
    {
        logger.LogInformation("[blue]Got ping #{MessageNumber}[/]", message.Number);
    }
}