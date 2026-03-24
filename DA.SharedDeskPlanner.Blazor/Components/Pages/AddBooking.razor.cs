using DA.SharedDeskPlanner.WebAPI.Client;
using Microsoft.AspNetCore.Components.Forms;

namespace DA.SharedDeskPlanner.Blazor.Components.Pages
{
	/// <ChangeLog>
	/// <Create Datum="24.03.2026" Entwickler="DA" />
	/// </ChangeLog>
	public partial class AddBooking : IDisposable
	{
		public Model.Booking NewBooking { get; set; } = new();
		protected override async Task OnInitializedAsync()
		{
			editContext = new EditContext(NewBooking);
			await base.OnInitializedAsync();
		}

		public void Dispose()
		{
			if (this.apiClient != null)
			{
				apiClient = null;
			}
		}
	}
}
