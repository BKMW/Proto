using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerServiceWithQuartz
{
    public class MyService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"My service is running at *****:{DateTime.Now}");
                 await Task.Delay(15000, stoppingToken);
            }
        }
    }
}
