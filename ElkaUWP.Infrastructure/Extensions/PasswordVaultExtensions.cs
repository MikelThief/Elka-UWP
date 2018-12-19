using System;
using Windows.Security.Credentials;

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

            var credentials = vault.FindAllByResource(resource: credential.Resource);

            foreach (var credentialItem in credentials)
            {
                vault.Remove(credential: credentialItem);
            }

            vault.Add(credential);
        }

        public static PasswordCredential GetUniversitySystemCredential(this Windows.Security.Credentials.PasswordVault vault, string systemResourceName)
        {
            var credentials = vault.FindAllByResource(resource: systemResourceName);

            return credentials[0];
        }
    }
}
