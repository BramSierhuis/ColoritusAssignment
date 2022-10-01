param appName string = uniqueString(az.resourceGroup().id)
param location string = az.resourceGroup().location

var storageAccountName  = uniqueString(appName)
var appServicePlanName = toLower('AppServicePlan-${appName}')
var functionAppName = toLower('fapp-${appName}')

resource appServicePlan 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: appServicePlanName
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

resource appService 'Microsoft.Web/sites@2020-06-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  properties: {
    serverFarmId: appServicePlan.id
  }
}

resource storageaccount 'Microsoft.Storage/storageAccounts@2021-02-01' = {
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
      name: 'initialuploadqueue'
    }

    resource primaryeditqueue 'queues' = {
      name: 'primaryeditqueue'
    }
  }
  
  //Containers
  resource blobService 'blobServices' = {
    name: 'default'

    resource initialcontainer 'containers' = {
      name: 'initialcontainer'
    }

    resource primaryeditcontainer 'containers' = {
      name: 'primaryeditcontainer'
    }

    resource finalcontainer 'containers' = {
      name: 'finalcontainer'
    }
  }
}
