using System;
using System.Net;
using System.Threading.Tasks;
using Coloritus.Extensions;
using Coloritus.Models.Requests;
using Coloritus.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace Coloritus.Controllers;

public class ImageController
{
    private readonly IImageService _imageService;

    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [FunctionName("UploadImage")]
    [OpenApiOperation(operationId: "UploadAnImage", tags: new[] { "Images" })]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
    [OpenApiRequestBody(contentType: "multipart/form-data", bodyType: typeof(UploadImageRequest), Required = true, Description = "Image data")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "images")] HttpRequest req, ILogger log)
    {
        var file = req.Form.Files["File"];

        var id = await _imageService.UploadInitialImage(await file.GetBytes(), file.FileName);
        
        return new OkObjectResult(id);
    }
    
    [FunctionName("GetImage")]
    [OpenApiOperation(operationId: "GetImage", tags: new[] { "Images" })]
    [OpenApiParameter(name: "id", Type = typeof(Guid), Required = true)]
    public async Task<IActionResult> GetImage(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "images/{id}")] HttpRequest req, Guid id)
    {
        var entity = await _imageService.GetImageUri(id.ToString());
        
        return new OkObjectResult(entity.ToString());
    }
}