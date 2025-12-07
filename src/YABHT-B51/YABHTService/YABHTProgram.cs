using YABHTService;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]


var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<YABHTWorker>();

var host = builder.Build();
host.Run();
