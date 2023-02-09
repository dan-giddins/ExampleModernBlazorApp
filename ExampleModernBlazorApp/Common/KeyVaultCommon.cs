using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace ExampleModernBlazorApp.Common
{
	public class KeyVaultCommon
	{
		public static string GetSecret(
			string secretName,
			SecretClient? secretClient = null) =>
			(secretClient ?? GetSecretClient()).GetSecret(secretName).Value.Value;

		public static SecretClient GetSecretClient() =>
			// AZURE_CLIENT_ID, AZURE_CLIENT_SECRET, and AZURE_TENANT_ID
			new(
				vaultUri: new Uri(
					Environment.GetEnvironmentVariable("AZURE_VAULT_URL")
					?? throw new ArgumentNullException()),
				credential: new DefaultAzureCredential());
	}
}
