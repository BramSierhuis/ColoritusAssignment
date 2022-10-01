using System;
using System.Threading.Tasks;
using Coloritus.Helpers;
using Coloritus.Models.Entities;
using Coloritus.Models.Enums;
using Coloritus.Repositories.Abstract;
using Coloritus.Services.Abstract;

namespace Coloritus.Services;

public class ImageService : IImageService
{
    private readonly IImageRepository _imageRepository;
    private readonly IColorApiClient _colorApiClient;
    private readonly IRelatedTextClient _relatedTextClient;
    private readonly ITableStorageRepository _tableStorageRepository;
    private readonly InitialUploadQueueClient _initialUploadQueueClient;
    private readonly PrimaryEditQueueClient _primaryEditQueueClient;

    public ImageService(IColorApiClient colorApiClient, IRelatedTextClient relatedTextClient, IImageRepository imageRepository, ITableStorageRepository tableStorageRepository, InitialUploadQueueClient initialUploadQueueClient, PrimaryEditQueueClient primaryEditQueueClient)
    {
        _colorApiClient = colorApiClient;
        _relatedTextClient = relatedTextClient;
        _imageRepository = imageRepository;
        _tableStorageRepository = tableStorageRepository;
        _initialUploadQueueClient = initialUploadQueueClient;
        _primaryEditQueueClient = primaryEditQueueClient;
    }

    public async Task<string> UploadInitialImage(byte[] image, string fileName)
    {
        var id = Guid.NewGuid().ToString();
        
        var entity = new ImageRecord()
        {
            RowKey = id,
            PartitionKey = "images",
            FileName = fileName,
            Status = Status.Unprocessed,
        };
        
        await _imageRepository.AddAsync("initialcontainer", image, id + fileName);
        await _tableStorageRepository.UpsertEntityAsync(entity);
        await _initialUploadQueueClient.SendMessageAsync(id);
        
        return id;
    }

    public async Task GetColorsAndEdit(string id)
    {
        var imageEntity = await _tableStorageRepository.GetEntityAsync("images", id);
        var image = await _imageRepository.GetAsync("initialcontainer", id + imageEntity.FileName);

        var (editedImage, colors) = ImageHelper.EditImage(image);
        
        var rnd = new Random();
        var index = rnd.Next(colors.Length);

        imageEntity.Status = Status.Edited;
        imageEntity.PrimaryColor = colors[index];

        await _imageRepository.AddAsync("primaryeditcontainer", editedImage, id+imageEntity.FileName);
        await _tableStorageRepository.UpsertEntityAsync(imageEntity);

        await _primaryEditQueueClient.SendMessageAsync(id);
    }

    public async Task GetAndAddTexts(string id)
    {
        var imageEntity = await _tableStorageRepository.GetEntityAsync("images", id);
        var image = await _imageRepository.GetAsync("primaryeditcontainer", id + imageEntity.FileName);
        
        var colorName = await _colorApiClient.GetColorName(imageEntity.PrimaryColor);
        var relatedText = await _relatedTextClient.GetRelatedText(colorName);
        
        //Add text to image
        var textedImage = ImageHelper.AddTextToImage(image, (relatedText, (10, 10), 30, "000000"));

        imageEntity.Status = Status.Finished;
        
        //Store new image
        await _imageRepository.AddAsync("finalcontainer", textedImage, id + imageEntity.FileName);
        await _tableStorageRepository.UpsertEntityAsync(imageEntity);
    }

    public async Task<Uri> GetImageUri(string id)
    {
        var imageEntity = await _tableStorageRepository.GetEntityAsync("images", id);

        return _imageRepository.GetUri("finalcontainer", id + imageEntity.FileName);
    }
}