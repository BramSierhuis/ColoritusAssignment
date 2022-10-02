param location string = az.resourceGroup().location

param prefix string
param colorApiBaseAddress string = 'https://www.thecolorapi.com/'
param relatedTextApiBaseAddress string = 'https://api.datamuse.com/'
param initialuploadqueueName string = 'initialuploadqueue'
param primaryeditqueueName string = 'primaryeditqueue'
param initialcontainerName string = 'initialcontainer'
param finalcontainerName string = 'finalcontainer'
param primaryeditcontainerName string = 'primaryeditcontainer'
param imageTableName string = 'ImageRecord'

var storageAccountName = replace(toLower('${prefix}-SA-1'), '-', '')
var functionAppName = '${prefix}-FA-1'
var serverFarmName = '${prefix}-ASP-1'

resource serverFarm 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: serverFarmName
  location: location
  properties: {
    reserved: true
  }
  sku: {
    name: 'B1'
    tier: 'Basic'
    size: 'B1'
    family: 'B'
    capacity: 1
  }
  kind: 'app'
}

resource storageaccount 'Microsoft.Storage/storageAccounts@2022-05-01' = {
  name: storageAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }

  //Queues
  resource queueService 'queueServices' = {
    name: 'default'

    resource initialuploadqueue 'queues' = {
      name: initialuploadqueueName
    }

    resource primaryeditqueue 'queues' = {
      name: primaryeditqueueName
    }
  }
  
  //Containers
  resource blobService 'blobServices' = {
    name: 'default'

    resource initialcontainer 'containers' = {
      name: initialcontainerName
    }

    resource primaryeditcontainer 'containers' = {
      name: primaryeditcontainerName
    }

    resource finalcontainer 'containers' = {
      name: finalcontainerName
    }
  }

  resource tableService 'tableServices' = {
    name: 'default'

    resource imageTable 'tables' = {
      name: imageTableName
    }
  }
}

resource appService 'Microsoft.Web/sites@2022-03-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  properties: {
    serverFarmId: resourceId('Microsoft.Web/serverfarms', serverFarm.name)
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageaccount.name};AccountKey=${listKeys(resourceId('Microsoft.Storage/storageAccounts', storageaccount.name), '2022-05-01').keys[0].value};EndpointSuffix=core.windows.net'

        }
        {
          name: 'RelatedTextApiBaseAddress'
          value: colorApiBaseAddress
        }
        {
          name: 'ColorApiBaseAddress'
          value: relatedTextApiBaseAddress
        }
        {
          name: 'InitialUploadQueue'
          value: initialuploadqueueName
        }
        {
          name: 'PrimaryEditQueue'
          value: primaryeditqueueName
        }
        {
          name: 'InitialContainer'
          value: initialcontainerName
        }
        {
          name: 'FinalContainer'
          value: finalcontainerName
        }
        {
          name: 'PrimaryEditContainer'
          value: primaryeditcontainerName
        }
        {
          name: 'ImageTable'
          value: imageTableName
        }
      ]
    }
  }
}
