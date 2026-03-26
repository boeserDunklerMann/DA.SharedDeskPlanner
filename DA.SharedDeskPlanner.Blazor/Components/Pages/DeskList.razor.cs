using DA.SharedDeskPlanner.Blazor.Components.Pages;
using DA.SharedDeskPlanner.WebAPI.Client.Models;

namespace DA.SharedDeskPlanner.Blazor.Pages
{
	public partial class DeskList_Code : PageBase, IDisposable
	{
		/// <ChangeLog>
		/// <Create Datum="??.03.2026" Entwickler="DA" />
		/// <Change Datum="26.03.2026" Entwickler="DA">did some simplyfying</Change>
		/// </ChangeLog>
		public IQueryable<Desk>? Desks { get; private set; }
		public List<Booking>? Bookings { get; private set; }
		public void Dispose()
		{
		}

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			if (!Loading && apiClient != null)
			{
				try
				{
					Loading = true;
					Desks = (await apiClient.Api.Desk.GetAsync())!.AsQueryable();
					Bookings = await apiClient.Api.Booking.GetAsync();
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