using MessageStoreService;
using Hangfire;
using Hangfire.PostgreSql;
using MinimalAzureServiceBus.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres");
        var config = new Config();

        configuration.Bind("Config", config);

        services.AddSingleton(config);

        //services.AddHangfire(config => config
        //    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        //    .UseSimpleAssemblyNameTypeSerializer()
        //    .UseRecommendedSerializerSettings()
        //    .UsePostgreSqlStorage(options =>
        //    {
        //        options.UseNpgsqlConnection(connectionString);
        //    }));

        services.AddScoped<IBuildPacketService, BuildPacketService>();

        //services.AddHangfireServer();

        services.RegisterAzureServiceBusWorker(config.AzureServiceBusConnectionString, "message-store-service-queue")
            .ProcessQueue(config.AzureServiceBusQueueName, Functions.HandleEnqueueJob);

        return services;
    }
}

public interface IBuildPacketService
{
    Task BuildPacket(DateTime from, DateTime to, List<string> radarList);
}

public class BuildPacketService : IBuildPacketService
{
    public Task BuildPacket(DateTime from, DateTime to, List<string> radarList)
    {
        Console.WriteLine("Building data packet");

        return Task.CompletedTask;
    }
}

//{
//    "JobType": "CreatePacket",
//    "Parameters": {
//        "radarList": "t001,t002,t003",
//        "from": "2024-03-26T12:00:00",
//        "to": "2024-03-28T18:00:00"
//    }
//}

public static class Functions
{
    public static async Task HandleEnqueueJob(EnqueueJob job)
    {
        var jobType = job.JobType;
        var parameters = job.Parameters;

        if (jobType == "CreatePacket")
        {
            var from = DateTime.Parse(parameters["from"]);
            var to = DateTime.Parse(parameters["to"]);
            var radarList = parameters["radarList"].Split(',').ToList();

            BackgroundJob.Enqueue<IBuildPacketService>(service => service.BuildPacket(from, to, radarList));
        }
    }
}

public class EnqueueJob
{
    public string JobType { get; set; } = ""; // Prob should be an enum

    public Dictionary<string, string> Parameters { get; set; } = new();
}