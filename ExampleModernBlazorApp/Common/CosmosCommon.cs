using Azure.Security.KeyVault.Secrets;
using ExampleModernBlazorApp.Dtos;
using Microsoft.Azure.Cosmos;

namespace ExampleModernBlazorApp.Common
{
	public class CosmosCommon
	{
		private static readonly Dictionary<Type, string> ContainerIds = new()
		{
			{ typeof(ExampleDto), "ExampleContainer" }
		};

		public static IOrderedQueryable<T> GetDataQueryable<T>(Container? container = null) =>
			(container ?? GetContainer<T>()).GetItemLinqQueryable<T>(true);

		public static (string, string) GetSecrets()
		{
			// Connect to Cosmos DB
			var secretClient = KeyVaultCommon.GetSecretClient();
			// Return CosmosDB secrets
			return (
				KeyVaultCommon.GetSecret("EndpointUri", secretClient),
				KeyVaultCommon.GetSecret("PrimaryKey", secretClient));
		}

		public static CosmosClient GetCosmosClient()
		{
			// Get Cosmos DB Secrets
			(var endpointUri, var primaryKey) = GetSecrets();
			// Connect to Azure CosmosDb Client
			return new CosmosClient(
				endpointUri,
				primaryKey,
				new CosmosClientOptions() { ConnectionMode = ConnectionMode.Gateway });
		}

		public static Container GetContainer<T>(SecretClient? secretClient = null) =>
			// Connect to container
			GetCosmosClient().GetContainer(
				KeyVaultCommon.GetSecret(
					"DatabaseId",
					secretClient ?? KeyVaultCommon.GetSecretClient()),
				ContainerIds[typeof(T)]);

		public static async Task CreateItems<T>(IEnumerable<T> collection, Container? container = null)
		{
			container ??= GetContainer<T>();
			foreach (var item in collection)
			{
				_ = await container.CreateItemAsync(item);
			}
		}

		public static async Task CreateItem<T>(T item, Container? container = null) =>
			await (container ?? GetContainer<T>()).CreateItemAsync(item);

		public static async Task<T> PatchItem<T>(
			string id,
			IReadOnlyList<PatchOperation> patchOperations,
			Container? container = null) =>
			(await (container ?? GetContainer<T>()).PatchItemAsync<T>(
				id,
				new PartitionKey(id),
				patchOperations)).Resource;

		public static async Task RemoveItem<T>(
			string id,
			Container? container = null) =>
			await (container ?? GetContainer<T>()).DeleteItemAsync<T>(
				id,
				new PartitionKey(id));
	}
}
