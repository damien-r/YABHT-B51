using YABHTService;
using YABHTService.Configurations;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<YABHTWorker>();
builder.Services.AddSingleton<ConfigurationsManager>();

var host = builder.Build();
host.Run();
