using System.Threading.Tasks;
using Coloritus.Models.Entities;

namespace Coloritus.Repositories.Abstract;

public interface ITableStorageRepository
{
    Task<ImageRecord> GetEntityAsync(string partitionKey, string id);
    Task<ImageRecord> UpsertEntityAsync(ImageRecord imageEntity);
}