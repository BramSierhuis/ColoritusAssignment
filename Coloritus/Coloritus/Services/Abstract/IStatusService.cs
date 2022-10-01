using System.Threading.Tasks;
using Coloritus.Models.Enums;

namespace Coloritus.Services.Abstract;

public interface IStatusService
{
    Task<Status> GetStatusAsync(string id);
}