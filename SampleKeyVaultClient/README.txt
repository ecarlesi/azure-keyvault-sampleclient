This program shows how to simply use Azure Key Vault and how to manage the Secret inside it.

This is sample code and should not be used in a production environment.

This program require Azure CLI: https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest

Use the following commands to create a Key Vault.

# Using this command you can see all the location available
az account list-locations

az login

# The following command a new resource group. If you already have one you can use it. 
# Choose your location
az group create -n "resource-group-name" -l "North Europe"

az provider register -n Microsoft.KeyVault

# Create the Key Vault. This call return the URL of the key vault.
az keyvault create --name "keyvault-name" --resource-group "resource-group-name" --location "North Europe"

# Add some secrets to the vault
az keyvault secret set --vault-name "keyvault-name" --name "secret-1" --value "test 1"
az keyvault secret set --vault-name "keyvault-name" --name "secret-2" --value "test 2"
az keyvault secret set --vault-name "keyvault-name" --name "secret-3" --value "test 3"

# List all the secrets in the specified vault
az keyvault secret list --vault-name "keyvault-name"

# Create an app. this call return the appId and secret to use in the app.config
az ad sp create-for-rbac -n "app-name" --skip-assignment

# Trust the key vault to be accessed with the app credentials
az keyvault set-policy --name "keyvault-name" --spn <use the appId previously created> --secret-permissions get list set
