using System.Threading.Tasks;
using Coloritus.Services.Abstract;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Coloritus.Controllers;

public class QueueController
{
    private readonly IImageService _imageService;
    
    public QueueController(IImageService imageService)
    {
        _imageService = imageService;
    }
    
    [FunctionName("InitialUploadTrigger")]
    public async Task InitialUploadTrigger([QueueTrigger("initialuploadqueue", Connection = "AzureWebJobsStorage")] string id, ILogger log)
    {
        await _imageService.GetColorsAndEdit(id);
    }
    
    [FunctionName("ImageEditedTrigger")]
    public async Task ImageEditedTrigger([QueueTrigger("primaryeditqueue", Connection = "AzureWebJobsStorage")] string id, ILogger log)
    {
        await _imageService.GetAndAddTexts(id);
    }
}