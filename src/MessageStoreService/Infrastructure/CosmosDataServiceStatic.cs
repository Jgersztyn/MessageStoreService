using MessageStoreService.Domain;
using Microsoft.Azure.Cosmos;

namespace MessageStoreService.Infrastructure;

public static class CosmosDataServiceStatic
{
    public static async Task GetMaintenanceRecordsVoid()
    {
        var connectionString = "AccountEndpoint=https://dev-climavision-cosmos.documents.azure.com:443/;AccountKey=Dzpad0rrKVoni9wsBBpPZeFjMDYRLE390cm37jLTKNRsurZhzP6xVCv6iwSjcpqUZTpeT0iPbH7jZwF176wZVQ==";
        var databaseName = "radar-maintenance";
        var containerName = "MaintenanceIncidents";

        var cosmosClient = new CosmosClient(connectionString);
        var database = cosmosClient.GetDatabase(databaseName);
        var container = database.GetContainer(containerName);

        QueryDefinition queryDefinition = new QueryDefinition("select * from MaintenanceIncidents t where t.siteId = @siteId")
            .WithParameter("@siteId", "v026");
        using (FeedIterator<MaintenanceIncidentDTO> feedIterator = container.GetItemQueryIterator<MaintenanceIncidentDTO>(
            queryDefinition,
            null,
            new QueryRequestOptions() { PartitionKey = new PartitionKey("id") }))
        {
            while (feedIterator.HasMoreResults)
            {
                try
                {
                    foreach (var item in await feedIterator.ReadNextAsync())
                    {
                        Console.WriteLine(item.Id);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }

    public static async Task QueryAttemptTwo()
    {
        string EndpointUrl = "https://dev-climavision-cosmos.documents.azure.com:443/";
        string PrimaryKey = "Dzpad0rrKVoni9wsBBpPZeFjMDYRLE390cm37jLTKNRsurZhzP6xVCv6iwSjcpqUZTpeT0iPbH7jZwF176wZVQ==";
        string DatabaseName = "radar-maintenance";
        string ContainerName = "MaintenanceIncidents";

        using (var client = new CosmosClient(EndpointUrl, PrimaryKey))
        {
            var container = client.GetContainer(DatabaseName, ContainerName);

            // Define query text to get all items
            var queryText = "SELECT * FROM MaintenanceIncidents";

            var queryDefinition = new QueryDefinition(queryText);

            // Querying the container for all records
            await QueryContainer(container, queryDefinition);
        }
    }

    public static async Task QueryAttemptTwoWithParamsExclamation()
    {
        string EndpointUrl = "https://dev-climavision-cosmos.documents.azure.com:443/";
        string PrimaryKey = "Dzpad0rrKVoni9wsBBpPZeFjMDYRLE390cm37jLTKNRsurZhzP6xVCv6iwSjcpqUZTpeT0iPbH7jZwF176wZVQ==";
        string DatabaseName = "radar-maintenance";
        string ContainerName = "MaintenanceIncidents";

        // for the query of the contianer lets goooo
        string siteId = "v026";

        using (var client = new CosmosClient(EndpointUrl, PrimaryKey))
        {
            var container = client.GetContainer(DatabaseName, ContainerName);

            // Define query text to get all items
            var queryText = "SELECT * FROM MaintenanceIncident WHERE MaintenanceIncident.siteId = @siteId";

            var queryDefinition = new QueryDefinition(queryText).WithParameter("@siteId", siteId);

            // Querying the container for all records
            await QueryContainer(container, queryDefinition);
        }
    }

    static async Task QueryContainer(Container container, QueryDefinition queryDefinition)
    {
        try
        {
            var queryResultSetIterator = container.GetItemQueryIterator<MaintenanceIncidentAsJson>(queryDefinition); // instead of 'dynamic', use MaintenanceIncidentDTO

        while (queryResultSetIterator.HasMoreResults)
        {
            var currentResultSet = await queryResultSetIterator.ReadNextAsync();

            foreach (var item in currentResultSet)
            {
                //MaintenanceIncidentDTO newItem = (MaintenanceIncidentDTO) item;

                Console.WriteLine(item);
            }
        }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
