using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using WorkerServiceWithQuartz.JobFactory;
using WorkerServiceWithQuartz.Jobs;
using WorkerServiceWithQuartz.Models;
using WorkerServiceWithQuartz.Schedular;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        #region service worker
        //services.AddHostedService<Worker>();
        //services.AddHostedService<MyService>();
        #endregion


        services.AddSingleton<IJobFactory, MyJobFactory>();
        services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

        #region Adding JobType
        services.AddSingleton<NotificationJob>();
        services.AddSingleton<LoggerJob>();
        #endregion

        #region Adding Jobs 
        List<JobMetadata> jobMetadatas = new List<JobMetadata>();
        jobMetadatas.Add(new JobMetadata(Guid.NewGuid(), typeof(NotificationJob), "Notify Job", "0/5 * * * * ?"));
        jobMetadatas.Add(new JobMetadata(Guid.NewGuid(), typeof(LoggerJob), "Log Job", "0/10 * * * * ?"));

        services.AddSingleton(jobMetadatas);
        #endregion

        services.AddHostedService<MySchedular>();
    })
    .Build();

await host.RunAsync();
