using System.IO;
using System.Threading.Tasks;

namespace Coloritus.Extensions;

public static class StreamExtensions
{
    public static async Task<byte[]> GetBytes(this Stream input)
    {
        using var ms = new MemoryStream();
        await input.CopyToAsync(ms);
        return ms.ToArray();
    }
}