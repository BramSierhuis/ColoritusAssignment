using System.Threading.Tasks;

namespace Coloritus.Services.Abstract;

public interface IColorApiClient
{
    public Task<string> GetColorName(string color);
}