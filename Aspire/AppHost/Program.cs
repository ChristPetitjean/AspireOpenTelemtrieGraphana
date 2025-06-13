using AppHost.OTel;

var builder = DistributedApplication.CreateBuilder(args);

var prometheus = builder.AddContainer("prometheus", "prom/prometheus", "v3.2.1")
                        .WithBindMount("../prometheus", "/etc/prometheus", isReadOnly: true)
                        .WithArgs("--web.enable-otlp-receiver", "--config.file=/etc/prometheus/prometheus.yml")
                        .WithHttpEndpoint(targetPort: 9090, name: "http");

var grafana = builder.AddContainer("grafana", "grafana/grafana")
                     .WaitFor(prometheus)
                     .WithBindMount("../grafana/config",     "/etc/grafana",                isReadOnly: true)
                     .WithBindMount("../grafana/dashboards", "/var/lib/grafana/dashboards", isReadOnly: true)
                     .WithEnvironment("PROMETHEUS_ENDPOINT", prometheus.GetEndpoint("http"))
                     .WithHttpEndpoint(targetPort: 3000, name: "http");

builder.AddOpenTelemetryCollector("otelcollector", "../otelcollector/config.yaml")
       .WaitFor(prometheus)
       .WithEnvironment("PROMETHEUS_ENDPOINT", $"{prometheus.GetEndpoint("http")}/api/v1/otlp");

var rabbitUsername = builder.AddParameter("username", secret: true, value: "rabbit");
var rabbitPassword = builder.AddParameter("password", secret: true, value: "rabbit");
var rabbit = builder.AddRabbitMQ("rabbit", rabbitUsername, rabbitPassword)
                    .WithManagementPlugin();

builder.AddProject<Projects.Wolverine_Rabbit>("application")
       .WithEnvironment("GRAFANA_URL", grafana.GetEndpoint("http"))
       .WithExternalHttpEndpoints()
       .WithReference(rabbit)
       .WaitFor(rabbit);

builder.Build().Run();


