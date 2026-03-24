using DA.SharedDeskPlanner.WebAPI.Client;
using DA.SharedDeskPlanner.WebAPI.Client.Api.User;
using DA.SharedDeskPlanner.WebAPI.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace DA.SharedDeskPlanner.Blazor.Components.Pages
{
	/// <ChangeLog>
	/// <Create Datum="04.03.2026" Entwickler="DA" />
	/// <Change Datum="15.03.2026" Entwickler="DA">User creation</Change>
	/// <Change Datum="24.03.2026" Entwickler="DA">inherited from PageBase</Change>
	/// </ChangeLog>
	public partial class Users : IDisposable
	{
		/// <summary>
		/// Usermodel for creating a new one
		/// </summary>
		public Model.User NewUser { get; set; } = new();
		public List<User>? UserList { get; private set; }
		protected override async Task OnInitializedAsync()
		{
			editContext = new EditContext(NewUser);
			await base.OnInitializedAsync();

			if (!Loading)
			{
				try
				{
					Loading = true;
					UserList = await apiClient!.Api.User.GetAsync();
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