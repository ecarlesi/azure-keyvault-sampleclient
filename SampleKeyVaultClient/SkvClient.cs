using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace SampleKeyVaultClient
{
    class SkvClient
    {
        private string clientSecret = ConfigurationManager.AppSettings.Get("KeyVaultClientSecret");
        private string clientId = ConfigurationManager.AppSettings.Get("KeyVaultClientId");
        private string baseSecretUri = ConfigurationManager.AppSettings.Get("KeyVaultUrl");

        private KeyVaultClient keyVault = null;

        public SkvClient()
        {
            this.keyVault = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(GetToken));
        }

        public async Task<string> GetToken(string authority, string resource, string scope)
        {
            AuthenticationContext authenticationContext = new AuthenticationContext(authority);
            ClientCredential clientCredential = new ClientCredential(this.clientId, this.clientSecret);
            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenAsync(resource, clientCredential);

            if (authenticationResult == null)
            {
                throw new InvalidOperationException("authenticationResult is null");
            }

            return authenticationResult.AccessToken;
        }

        public string Get(string key)
        {
            return Task.Run(() => keyVault.GetSecretAsync(String.Format("{0}/secrets/{1}", this.baseSecretUri, key))).Result.Value;
        }

        public SecretBundle Set(string key, string value)
        {
            Task<SecretBundle> tsb = keyVault.SetSecretAsync(this.baseSecretUri, key, value);

            return tsb.Result;
        }

        public SecretBundle Delete(string key)
        {
            Task<DeletedSecretBundle> tdsb = keyVault.DeleteSecretAsync(this.baseSecretUri, key);

            return tdsb.Result;
        }
    }
}
