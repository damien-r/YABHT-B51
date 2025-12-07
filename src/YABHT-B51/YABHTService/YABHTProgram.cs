using YABHTService;
using YABHTService.Configurations;
using YABHTService.Repositories;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "YABHT-B51";
});

builder.Services.AddHostedService<YABHTWorker>();
builder.Services.AddSingleton<ConfigurationsManager>();
builder.Services.AddSingleton<RepositoriesManager>();

var host = builder.Build();
host.Run();
