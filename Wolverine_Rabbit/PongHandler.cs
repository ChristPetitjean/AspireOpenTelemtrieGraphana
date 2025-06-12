using Wolverine.Attributes;

namespace Wolverine_Rabbit;

// Simple message handler for the PongMessage responses
// The "Handler" suffix is important as a naming convention
// to let Wolverine know that it should build a message handling
// pipeline around public methods on this class
[WolverineHandler]
public class PongHandler
{
    // "Handle" is recognized by Wolverine as a message handling
    // method. Handler methods can be static or instance methods
    public void Handle(PongMessage message, ILogger<PongHandler> logger)
    {
        logger.LogInformation("[blue]Got pong #{MessageNumber}[/]", message.Number);
    }
    
    // "Handle" is recognized by Wolverine as a message handling
    // method. Handler methods can be static or instance methods
    public void Handle(PingMessage message, ILogger<PongHandler> logger)
    {
        logger.LogInformation("[blue]Got ping #{MessageNumber}[/]", message.Number);
    }
}