using System;
using System.Threading.Tasks;

namespace Coloritus.Repositories.Abstract;

public interface IImageRepository
{
    Task AddAsync(string containerName, byte[] image, string fileName);
    Task<byte[]> GetAsync(string containerName, string fileName);
    Uri GetUri(string containerName, string fileName);
}