namespace MessageStoreService;

public class Config
{
    public string AzureServiceBusConnectionString { get; set; } = string.Empty;
    public string AzureServiceBusQueueName { get; set; } = string.Empty;
    public string CosmosConnectionString { get; set; } = string.Empty;
    public string CosmosDatabaseName { get; set; } = string.Empty;
}