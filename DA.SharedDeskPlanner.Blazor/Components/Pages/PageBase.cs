using DA.SharedDeskPlanner.WebAPI.Client;
using DA.SharedDeskPlanner.WebAPI.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace DA.SharedDeskPlanner.Blazor.Components.Pages
{
	/// <ChangeLog>
	/// <Create Datum="24.03.2026" Entwickler="DA" />
	/// </ChangeLog>
	public class PageBase : ComponentBase
	{
		protected ApiClient? apiClient;
		protected EditContext? editContext;
		protected List<Room>? Rooms { get; set; } = [];
		protected List<User>? Users { get; set; } = [];

		protected bool Loading { get; set; } = false;

		protected override async Task OnInitializedAsync()
		{
			if (apiClient==null)
			{
				var authProvider = new AnonymousAuthenticationProvider();
				var httpClient = new HttpClient(new LoggingHandler { InnerHandler = new HttpClientHandler() });
				var adapter = new HttpClientRequestAdapter(authProvider, httpClient: httpClient);
				apiClient = new ApiClient(adapter);
			}
			await Task.CompletedTask;
		}
	}
}