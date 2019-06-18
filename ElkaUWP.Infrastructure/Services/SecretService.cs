using System;
using System.Linq;
using Windows.Security.Credentials;

namespace ElkaUWP.Infrastructure.Services
{
    public class SecretService
    {
        private PasswordVault _vault;

        public SecretService()
        {
            _vault = new PasswordVault();
        }
        public bool IsSecretPresent(string container, string key) => _vault.RetrieveAll().Any(x => x.Resource == container && x.UserName == key);

        public void CreateOrUpdateSecret(string container, string key, string secret)
        {
            if (_vault.RetrieveAll().Any(x => x.Resource == container && x.UserName == key))
            {
                var credential = _vault.Retrieve(resource: container, userName: key);
                credential.RetrievePassword();
                credential.Password = secret;
                _vault.Add(credential: credential);
            }
            else
            {
                var credential = new PasswordCredential(resource: container, userName: key, password: secret);
                _vault.Add(credential: credential);
            }
        }
        public void CreateOrUpdateSecret(PasswordCredential providedCredential)
        {
            if (_vault.RetrieveAll().Any(x => x.Resource == providedCredential.Resource && x.UserName == providedCredential.UserName))
            {
                var credential = _vault.Retrieve(resource: providedCredential.Resource, userName: providedCredential.UserName);
                providedCredential.RetrievePassword();
                credential.Password = providedCredential.Password;
                _vault.Add(credential: credential);
            }
            else
            {
                _vault.Add(credential: providedCredential);
            }
        }

        public void RemoveSecret(string container, string key)
        {
            if (_vault.RetrieveAll().Any(x => x.Resource == container && x.UserName == key))
            {
                var credential = _vault.Retrieve(resource: container, userName: key);
                _vault.Remove(credential: credential);
            }
        }

        public PasswordCredential GetSecret(string container, string key)
        {
            if (_vault.RetrieveAll().Any(x => x.Resource == container && x.UserName == key))
            {
                return _vault.Retrieve(resource: container, userName: key);
            }

            throw new ArgumentException(message: "Credential not found for pair: [" + container + ", " + key + "]");
        }

        public PasswordCredential GetSecret(string container)
        {
            if (_vault.RetrieveAll().Any(x => x.Resource == container))
            {
                var retrievedContainer = _vault.RetrieveAll().Single(x => x.Resource == container);

                return _vault.Retrieve(resource: container, userName: retrievedContainer.UserName);
            }

            throw new ArgumentException(message: "Credential not found for pair: [" + container + "]");
        }

        public bool IsContainerPresent(string container) =>
            _vault.RetrieveAll().Any(x => x.Resource == container);
    }

}
