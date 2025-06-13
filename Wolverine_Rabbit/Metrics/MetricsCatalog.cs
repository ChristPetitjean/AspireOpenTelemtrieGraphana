using System.Diagnostics.Metrics;

namespace Wolverine_Rabbit.Metrics
{
    public class MetricsCatalog
    {
        private static Meter _appMeter = new Meter("Wolverine_Rabbit", "1.0.0");

        public static Counter<int> CountGreetings { get; } = _appMeter.CreateCounter<int>("greetings.count", description: "Counts the number of greetings");
    }
}
