using DA.SharedDeskPlanner.WebAPI.Client;
using DA.SharedDeskPlanner.WebAPI.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace DA.SharedDeskPlanner.Blazor.Pages
{
	public partial class DeskList_Code : ComponentBase, IDisposable
	{
		private ApiClient? apiClient = null;
		public bool Loading { get; set; } = false;
		public List<Desk>? Desks { get; private set; }
		public List<Room>? Rooms { get; private set; }
		public List<Booking>? Bookings { get; private set; }
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
					Desks = (await apiClient.Api.Desk.GetAsync())!.ToList();
					Rooms = (await apiClient.Api.Room.GetAsync())!.ToList();
					Bookings = (await apiClient.Api.Booking.GetAsync())!.ToList();
				}
				finally
				{
					Loading = false;
				}
			}
		}

		public Room? GetRoomByDesk(Desk desk)
		{
			if (Rooms != null)
				return Rooms.First(r => r.Desks!.Any(d => d.Id == desk.Id));
			return null;
		}

		public bool IsDeskBookedToday(Desk desk)
		{
			return Bookings!.Any(b => b.DeskId == desk.Id && b.BookingStart < DateTime.UtcNow && b.BookingEnd > DateTime.UtcNow);
		}
	}
}