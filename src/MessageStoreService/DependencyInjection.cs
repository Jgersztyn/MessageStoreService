using MessageStoreService;
using Hangfire;
using Hangfire.PostgreSql;
using MinimalAzureServiceBus.Core;
using MessageStoreService.Application;
using MessageStoreService.Infrastructure;
using Microsoft.Azure.Cosmos;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        //var connectionString = configuration.GetConnectionString("Postgres");
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

        // this is ONLY if Hangfire is being used
        services.AddScoped<IBuildPacketService, BuildPacketService>();

        //services.AddHangfireServer();

        //services.RegisterAzureServiceBusWorker(config.AzureServiceBusConnectionString, "message-store-service-queue")
        //    .ProcessQueue(config.AzureServiceBusQueueName, Functions.HandleEnqueueJob);

        services.AddScoped<ICosmosDataService, CosmosDataService>();

        //// Register CosmosClient with database name
        //services.AddSingleton<CosmosClient>(sp =>
        //{
        //    var cosmosConnectionString = config.CosmosConnectionString;
        //    return new CosmosClient(cosmosConnectionString);
        //});

        //// Also register the container name
        //services.Configure<CosmosDatabaseName>(config.CosmosDatabaseName);

        services.AddSingleton((provider) =>
        {
            var endpointUri = config.CosmosConnectionString;
            var databaseName = config.CosmosDatabaseName;

            var cosmosClientOptions = new CosmosClientOptions()
            {
                ApplicationName = databaseName
            };

            // Optionally add logging

            var cosmosClient = new CosmosClient(endpointUri, cosmosClientOptions);

            return cosmosClient;
        });

        return services;
    }
}

/// <summary>
/// Hangfire related interface
/// </summary>
public interface IBuildPacketService
{
    Task BuildPacket(DateTime from, DateTime to, List<string> radarList);
}

/// <summary>
/// Hnagire related implementation
/// </summary>
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

// Hangfire related
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

// Hangfire related
public class EnqueueJob
{
    public string JobType { get; set; } = ""; // Prob should be an enum

    public Dictionary<string, string> Parameters { get; set; } = new();
}