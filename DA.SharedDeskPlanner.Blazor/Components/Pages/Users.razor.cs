using DA.SharedDeskPlanner.WebAPI.Client.Api.User;
using DA.SharedDeskPlanner.WebAPI.Client.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace DA.SharedDeskPlanner.Blazor.Components.Pages
{
	/// <ChangeLog>
	/// <Create Datum="04.03.2026" Entwickler="DA" />
	/// <Change Datum="15.03.2026" Entwickler="DA">User creation</Change>
	/// <Change Datum="24.03.2026" Entwickler="DA">inherited from PageBase</Change>
	/// <Change Datum="25.03.2026" Entwickler="DA">DelUserAsync added</Change>
	/// </ChangeLog>
	public partial class Users : IDisposable
	{
		/// <summary>
		/// Usermodel for creating a new one
		/// </summary>
		public Model.User NewUser { get; set; } = Model.BaseModel.Create<Model.User>();
		public IQueryable<User>? UserList { get; private set; }
		protected override async Task OnInitializedAsync()
		{
			editContext = new EditContext(NewUser);
			await base.OnInitializedAsync();

			if (!Loading)
			{
				try
				{
					Loading = true;
					await LoadUsersAsync();
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

		private async Task DelUserAsync(User? user)
		{
			if (!Loading && user != null && user.Id != null)
			{
				try
				{
					Loading = true;
					await apiClient!.Api.User[user.Id.Value].DeleteAsync();
					await LoadUsersAsync();
				}
				finally
				{
					Loading = false;
				}
				navMgr.NavigateTo(nameof(Users), true);
			}
		}

		private async Task LoadUsersAsync()
		{
			UserList = (await apiClient!.Api.User.GetAsync())!.AsQueryable();
		}
		public void Dispose()
		{
		}
	}
}