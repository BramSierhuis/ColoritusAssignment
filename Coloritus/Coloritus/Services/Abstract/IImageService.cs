using System;
using System.Threading.Tasks;

namespace Coloritus.Services.Abstract;

public interface IImageService
{
    Task<string> UploadInitialImage(byte[] image, string fileName);
    Task GetColorsAndEdit(string id);
    Task GetAndAddTexts(string id);
    Task<Uri> GetImageUri(string id);
}