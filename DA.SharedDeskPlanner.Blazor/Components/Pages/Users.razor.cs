using DA.SharedDeskPlanner.WebAPI.Client;
using DA.SharedDeskPlanner.WebAPI.Client.Api.User;
using DA.SharedDeskPlanner.WebAPI.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace DA.SharedDeskPlanner.Blazor.Components.Pages
{
	/// <ChangeLog>
	/// <Create Datum="04.03.2026" Entwickler="DA" />
	/// <Change Datum="15.03.2026" Entwickler="DA">User creation</Change>
		/// </ChangeLog>
	public partial class Users : ComponentBase, IDisposable
	{
		private ApiClient? apiClient;
		public bool Loading { get; private set; } = false;
		/// <summary>
		/// Usermodel for creating a new one
		/// </summary>
		//[SupplyParameterFromForm]
		public Model.User NewUser { get; set; } = new();
		//public string? NewUserLastname { get; set; }
		//public string? NewUserFirstname { get; set; }
		//public string? NewUserName { get; set; }
		public List<User>? UserList { get; private set; }
		protected override async Task OnInitializedAsync()
		{
			if (apiClient == null)	// TODO DA: das hier in eine Basisklasse auslagern
			{
				var authProvider = new AnonymousAuthenticationProvider();
				var adapter = new HttpClientRequestAdapter(authProvider);
				apiClient = new ApiClient(adapter);
			}
			if (!Loading)
			{
				try
				{
					Loading = true;
					UserList = await apiClient.Api.User.GetAsync();
				}
				finally
				{
					Loading = false;
				}
			}
		}
		private async Task NewUserSubmittedAsync()
		{
			// Diese Methode wird NUR aufgerufen, wenn alle Felder valide sind.
			if (Loading)
				return;
			try
			{
				Loading = true;
				var postBody = new UserRequestBuilder.UserPostRequestBody() { User = new() };
				postBody.User.LastName = NewUser.LastName;
				postBody.User.FirstName = NewUser.FirstName;
				postBody.User.Name = NewUser.Name;
				await apiClient!.Api.User.PostAsync(postBody);
				navMgr.NavigateTo(nameof(Users), true);
			}
			finally
			{
				Loading = false;
			}
		}

		public void Dispose()
		{
		}
	}
}