using WorkerServiceWithQuartz;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
