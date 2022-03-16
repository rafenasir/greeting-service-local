param appName string
param sqlAdminUser string = 'rafe'
param sqlAdminPassword string
param location string = resourceGroup().location

// storage accounts must be between 3 and 24 characters in length and use numbers and lower-case letters only
var storageAccountName = '${substring(appName,0,10)}${uniqueString(resourceGroup().id)}'
var logginStorageAccountName = '${substring(appName,0,7)}log${uniqueString(resourceGroup().id)}' 
var hostingPlanName = '${appName}${uniqueString(resourceGroup().id)}'
var appInsightsName = '${appName}${uniqueString(resourceGroup().id)}'
var functionAppName = '${appName}'
var sqlServerName = '${appName}sqlserver'
var sqlDbName = '${appName}sqldb'
var serviceBusName = 'rafe-sb-dev'
var keyVaultName = 'rafe-kv-dev'
var asurgentTenantId = '9583541d-47a0-4deb-9e14-541050ac8bc1'


resource storageAccount 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: storageAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
    tier: 'Standard'
  }
}
resource loggingStorageAccount 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  name: logginStorageAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
    tier: 'Standard'
  }
}
resource appInsights 'Microsoft.Insights/components@2020-02-02-preview' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: { 
    Application_Type: 'web'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
  tags: {
    // circular dependency means we can't reference functionApp directly  /subscriptions/<subscriptionId>/resourceGroups/<rg-name>/providers/Microsoft.Web/sites/<appName>"
     'hidden-link:/subscriptions/${subscription().id}/resourceGroups/${resourceGroup().name}/providers/Microsoft.Web/sites/${functionAppName}': 'Resource'
  }
}

resource hostingPlan 'Microsoft.Web/serverfarms@2020-10-01' = {
  name: hostingPlanName
  location: location
  sku: {
    name: 'Y1' 
    tier: 'Dynamic'
  }
}

resource functionApp 'Microsoft.Web/sites@2020-06-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  properties: {
    httpsOnly: true
    serverFarmId: hostingPlan.id
    clientAffinityEnabled: true
    siteConfig: {
      appSettings: [
        {
          'name': 'APPINSIGHTS_INSTRUMENTATIONKEY'
          'value': appInsights.properties.InstrumentationKey
        }
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storageAccount.id, storageAccount.apiVersion).keys[0].value}'
        }
        {
          name: 'LoggingStorageAccount'
          value: '@Microsoft.KeyVault(SecretUri=https://rafe-kv-dev.vault.azure.net/secrets/LoggingStorageAccount/)'
        }
        {
          'name': 'FUNCTIONS_EXTENSION_VERSION'
          'value': '~4'
        }
        {
          'name': 'FUNCTIONS_WORKER_RUNTIME'
          'value': 'dotnet'
        }
        {
          'name': 'WEBSITE_RUN_FROM_PACKAGE'
          'value': '1'
        }
        {
          'name': 'Rafe'
          'value': 'summer2022'
        }
        {
          'name': 'FileRepositoryFilePath'
          'value': '/home/site/wwwroot/greeting.json'
        }
        {
          'name': 'GreetingDbConnectionString'
          'value': '@Microsoft.KeyVault(SecretUri=https://rafe-kv-dev.vault.azure.net/secrets/GreetingDbConnectionString/)'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storageAccount.id, storageAccount.apiVersion).keys[0].value}'
        }
        {
        name: 'ServiceBusConnectionString'
        value: '@Microsoft.KeyVault(SecretUri=https://rafe-kv-dev.vault.azure.net/secrets/ServiceBusConnectionString/)'
        }
        {
          name: 'KeyVaultUri'
          value: 'https:////rafe-kv-dev.vault.azure.net/'
        }
      ]
    }
  }
  
}

resource sqlServer 'Microsoft.Sql/servers@2019-06-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: sqlAdminUser
    administratorLoginPassword: sqlAdminPassword
    version: '12.0'
  }
  
  resource allowAllWindowsAzureIps 'firewallRules@2021-05-01-preview' = {
    name: 'AllowAllWindowsAzureIps'
    properties: {
      endIpAddress: '0.0.0.0'
      startIpAddress: '0.0.0.0'
    }
  }

  resource sqlDb 'databases@2019-06-01-preview' = {
    name: sqlDbName
    location: location
    sku: {
      name: 'Basic'
      tier: 'Basic'
      capacity: 5
    }
  } 
}

resource serviceBusNamespace 'Microsoft.ServiceBus/namespaces@2021-06-01-preview' = {
  name: serviceBusName
  location: location
  sku: {
    name: 'Standard'
  }
  resource mainTopic 'topics@2021-06-01-preview' = {
    name: 'main'

    resource greetingCreateSubscription 'subscriptions@2021-06-01-preview' = {
      name: 'greeting_create'

      resource subscriptionRule 'rules@2021-06-01-preview' = {
        name: 'subject'
        properties: {
          correlationFilter: {
            label: 'NewGreeting' 
          }
          filterType: 'CorrelationFilter'
        }
      }
    }
    resource greetingUpdateSubscription 'subscriptions@2021-06-01-preview' = {
      name: 'greeting_put'

      resource subscriptionRule 'rules@2021-06-01-preview' = {
        name: 'subject'
        properties: {
          correlationFilter: {
            label: 'UpdateGreeting' 
          }
          filterType: 'CorrelationFilter'
        }
      }
    }
    resource invoiceComputeSubscription 'subscriptions@2021-06-01-preview' = {
      name: 'greeting_compute_billing'

      resource subscriptionRule 'rules@2021-06-01-preview' = {
        name: 'subject'
        properties: {
          correlationFilter: {
            label: 'NewGreeting' 
          }
          filterType: 'CorrelationFilter'
        }
      }
    }
    resource userCreateSubscription 'subscriptions@2021-06-01-preview' = {
      name: 'user_create'

      resource subscriptionRule 'rules@2021-06-01-preview' = {
        name: 'subject'
        properties: {
          correlationFilter: {
            label: 'NewUser' 
          }
          filterType: 'CorrelationFilter'
        }
      }
    }
    resource userUpdateSubscription 'subscriptions@2021-06-01-preview' = {
      name: 'user_put'

      resource subscriptionRule 'rules@2021-06-01-preview' = {
        name: 'subject'
        properties: {
          correlationFilter: {
            label: 'UpdateUser' 
          }
          filterType: 'CorrelationFilter'
        }
      }
    }
    resource userVerificationSubscription 'subscriptions@2021-06-01-preview' = {
      name: 'user_approval'

      resource subscriptionRule 'rules@2021-06-01-preview' = {
        name: 'subject'
        properties: {
          correlationFilter: {
            label: 'NewUser' 
          }
          filterType: 'CorrelationFilter'
        }
      }
    }
  }
}

resource keyVault 'Microsoft.KeyVault/vaults@2019-09-01' = {
  name: keyVaultName
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: asurgentTenantId
    accessPolicies:[
      {
        permissions:{
          secrets:[
            'get'
            'list'
          ]
        }
        objectId: functionApp.identity.principalId
        tenantId: asurgentTenantId
      }
    ]
  }
}
