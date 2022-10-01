using System;
using System.Net;
using System.Threading.Tasks;
using Coloritus.Models.Entities;
using Coloritus.Repositories.Abstract;
using Coloritus.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace Coloritus.Controllers;

public class StatusController
{
    private readonly IStatusService _statusService;

    public StatusController(IStatusService statusService)
    {
        _statusService = statusService;
    }

    [FunctionName("GetStatus")]
    [OpenApiOperation(operationId: "GetStatus", tags: new[] { "Statuses" })]
    [OpenApiParameter(name: "id", Type = typeof(Guid), Required = true)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(ImageRecord), Description = "User has been created")]
    public async Task<IActionResult> GetStatus(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "statuses/{id}")] HttpRequest req, Guid id)
    {
        var entity = await _statusService.GetStatusAsync(id.ToString());
        
        return new OkObjectResult(entity.ToString());
    }
}