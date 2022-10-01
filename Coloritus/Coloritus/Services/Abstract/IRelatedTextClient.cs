using System.Threading.Tasks;

namespace Coloritus.Services.Abstract;

public interface IRelatedTextClient
{
    Task<string> GetRelatedText(string input);
}