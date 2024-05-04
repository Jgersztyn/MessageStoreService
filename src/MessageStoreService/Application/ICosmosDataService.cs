using MessageStoreService.Domain;

namespace MessageStoreService.Application;

public interface ICosmosDataService
{
    public Task<List<MaintenanceIncidentDTO>> GetMaintenanceRecords();

    public Task GetMaintenanceRecordsVoid();
}
