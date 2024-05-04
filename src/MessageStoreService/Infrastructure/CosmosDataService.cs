using MessageStoreService.Application;
using MessageStoreService.Domain;
using Microsoft.Azure.Cosmos;

namespace MessageStoreService.Infrastructure
{
    public class CosmosDataService : ICosmosDataService
    {
        // want to inject the Cosmos client
        private readonly CosmosClient _cosmosClient;

        public CosmosDataService(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
            // here we will inject the Cosmos client
        }

        public async Task<List<MaintenanceIncidentDTO>> GetMaintenanceRecords()
        {
            //string DatabaseName = "radar-maintenance";
            string containerName = "MaintenanceIncidents";
            string siteId = "v026";

            var temp = _cosmosClient.ClientOptions;
            var databaseName = temp.ApplicationName; // does this get what we need?

            var container = _cosmosClient.GetContainer(databaseName, containerName);

            // Define query text to get all items
            var queryText = "SELECT * FROM MaintenanceIncident WHERE MaintenanceIncident.siteId = @siteId";

            var queryDefinition = new QueryDefinition(queryText).WithParameter("@siteId", siteId);

            // Querying the container for all records
            await QueryContainer(container, queryDefinition);

            return new List<MaintenanceIncidentDTO>(); // be sure to return the correct type here
        }

        public async Task GetMaintenanceRecordsVoid()
        {
            var connectionString = "";
            var databaseName = "";
            var containerName = "";

            var cosmosClient = new CosmosClient(connectionString);
            var database = cosmosClient.GetDatabase(databaseName);
            var container = database.GetContainer(containerName);

            using (FeedIterator<MaintenanceIncidentDTO> feedIterator = container.GetItemQueryIterator<MaintenanceIncidentDTO>(
                "Select * from MaintenanceIncident m where m.siteId == 'v026'",
                null,
                new QueryRequestOptions() { PartitionKey = new PartitionKey("Id") }))
            {
                while (feedIterator.HasMoreResults)
                {
                    foreach (var item in await feedIterator.ReadNextAsync())
                    {
                        Console.WriteLine(item.Id);
                    }
                }
            }
        }

        Task ICosmosDataService.GetMaintenanceRecordsVoid()
        {
            throw new NotImplementedException();
        }



        // testing out
        // some different
        // functionality; this works in the static
        // class created

        public async Task QueryAttemptTwo()
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

        public async Task QueryAttemptTwoWithParamsExclamation()
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

        public async Task QueryContainer(Container container, QueryDefinition queryDefinition)
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

        // end the different
        // functionality area
    }

    public class TestRecordDTO
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
