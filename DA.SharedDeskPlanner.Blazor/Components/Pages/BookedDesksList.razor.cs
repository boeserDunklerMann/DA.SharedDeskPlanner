using DA.SharedDeskPlanner.WebAPI.Client;
using DA.SharedDeskPlanner.WebAPI.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace DA.SharedDeskPlanner.Blazor.Components.Pages
{
	/// <ChangeLog>
	/// <Create Datum="04.03.2026" Entwickler="DA" />
	/// </ChangeLog>
	public partial class BookedDesksList : ComponentBase, IDisposable
	{
		private WebAPI.Client.ApiClient? apiClient;
		public bool Loading { get; private set; } = false;
		public List<Desk>? BookedDesks { get; private set; }
		public List<Booking>? TodaysBookings { get; private set; }
		public List<Room>? Rooms { get; private set; }
		public void Dispose()
		{
		}
		protected override async Task OnInitializedAsync()
		{
			if (apiClient == null)
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
					var allBookings = await apiClient.Api.Booking.GetAsync();
					Rooms = await apiClient.Api.Room.GetAsync();
					TodaysBookings = allBookings!.Where(b => b.BookingStart <= DateTime.UtcNow && b.BookingEnd >= DateTime.UtcNow).ToList();
					if (TodaysBookings == null)
						throw new NullReferenceException(nameof(TodaysBookings));

					BookedDesks = TodaysBookings
						.Select(b => b.Desk).ToList()!;
				}
				finally
				{
					Loading = false;
				}
			}
		}
	}
}