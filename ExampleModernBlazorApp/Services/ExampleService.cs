using ExampleModernBlazorApp.Common;
using ExampleModernBlazorApp.Dtos;
using Microsoft.Azure.Cosmos;

namespace ExampleModernBlazorApp.Services
{
	public class ExampleService
	{
		private readonly Container Container;
		private string UserGuid = string.Empty;

		//public ExampleService() =>
		//	Container = CosmosCommon.GetContainer<ExampleDto>();

		public void SetUserGuid(string userGuid) =>
			UserGuid = userGuid;

		public IList<ExampleDto> GetAll() =>
			//return CosmosCommon.GetDataQueryable<ExampleDto>(Container)
			//	.Where(x => x.UserGuid == UserGuid)
			//	.ToList();
			new List<ExampleDto>
			{
				new ExampleDto("title1", "body1", "0"),
				new ExampleDto("title2", "body2", "0"),
				new ExampleDto("title3", "body3", "0"),
			};

		public async Task Delete(ExampleDto thing) =>
			await CosmosCommon.RemoveItem<ExampleDto>(
				thing.Id.ToString(),
				Container);

		public async Task<ExampleDto> Patch(ExampleDto thing) =>
			await CosmosCommon.PatchItem<ExampleDto>(
				thing.Id.ToString(),
				new List<PatchOperation>()
				{
					PatchOperation.Replace("/Title", thing.Title),
					PatchOperation.Replace("/Body", thing.Body)
				},
				Container);

		public async Task<ExampleDto> Add(string title, string body)
		{
			var thing = new ExampleDto(title, body, UserGuid);
			await CosmosCommon.CreateItem(thing, Container);
			return thing;
		}
	}
}
