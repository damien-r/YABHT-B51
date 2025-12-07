using YABHTService;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<YABHTWorker>();

var host = builder.Build();
host.Run();
