using System.Threading.Tasks;
using Coloritus.Models.Enums;
using Coloritus.Repositories.Abstract;
using Coloritus.Services.Abstract;

namespace Coloritus.Services;

public class StatusService : IStatusService
{
    private readonly ITableStorageRepository _repository;

    public StatusService(ITableStorageRepository repository)
    {
        _repository = repository;
    }

    public async Task<Status> GetStatusAsync(string id)
    {
        var entity = await _repository.GetEntityAsync("images", id);
        return entity.Status;
    }
}