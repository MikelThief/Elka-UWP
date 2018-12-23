using System;
using System.Collections.Generic;
using Windows.Security.Credentials;
using Windows.Storage;

namespace ElkaUWP.Infrastructure.Extensions
{
    public static class PasswordVaultExtensions
    {
        /// <summary>
        /// Adds a university's system credential to the system vault. Removes all existing credentials of this type.
        /// </summary>
        /// <param name="vault">System credentials vault</param>
        /// <param name="credential">The credential to be stored</param>
        /// <param name="universitySystemToken">Unique name of the university's system</param>
        /// <remarks>Ultimately, there should be only one credential of this type.</remarks>
        public static void AddUniversitySystemCredential(this Windows.Security.Credentials.PasswordVault vault, PasswordCredential credential)
        {
            if (credential == null)
                throw new ArgumentNullException(paramName: nameof(credential));

            var settings = ApplicationData.Current.RoamingSettings;

            settings.SaveString("oauth_token", value: credential.UserName);

            IReadOnlyList <PasswordCredential> credentials = null;

            try
            {
                credentials = vault.FindAllByResource(resource: credential.Resource);
            }
            catch (Exception)
            {
                // let it silently die
                // i.e. no associated records were found
            }

            if (credentials != null)
            {
                foreach (var credentialItem in credentials)
                {
                    vault.Remove(credential: credentialItem);
                }
            }

            vault.Add(credential: credential);
        }

        public static PasswordCredential GetUniversitySystemCredential(this PasswordVault vault, string systemResourceName)
        {
            IReadOnlyList<PasswordCredential> credentials = null;

            try
            {
                credentials = vault.FindAllByResource(resource: systemResourceName);
            }
            catch (Exception)
            {
                throw;
            }

            return vault.Retrieve(resource: systemResourceName, userName: credentials[0].UserName);
        }
    }
}
