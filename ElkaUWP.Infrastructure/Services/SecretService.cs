using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (_vault.RetrieveAll().Any(predicate: x => x.Resource == container && x.UserName == key))
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
            if (_vault.RetrieveAll().Any(predicate: x => x.Resource == providedCredential.Resource && x.UserName == providedCredential.UserName))
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
            if (_vault.RetrieveAll().Any(predicate: x => x.Resource == container && x.UserName == key))
            {
                var credential = _vault.Retrieve(resource: container, userName: key);
                _vault.Remove(credential: credential);
            }
        }

        public PasswordCredential GetCredential(string container, string key)
        {
            if (_vault.RetrieveAll().Any(predicate: x => x.Resource == container && x.UserName == key))
            {
                var credential = _vault.Retrieve(resource: container, userName: key);
                return credential;
            }
            else
                throw new ArgumentException(message: "Credential not found for pair: [" + container + ", " + key + "]");
        }

        public bool IsContainerPresent(string container) =>
            _vault.RetrieveAll().Any(predicate: x => x.Resource == container);
    }

}
