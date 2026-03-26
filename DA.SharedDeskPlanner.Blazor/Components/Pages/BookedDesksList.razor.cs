using DA.SharedDeskPlanner.WebAPI.Client.Models;

namespace DA.SharedDeskPlanner.Blazor.Components.Pages
{
	/// <ChangeLog>
	/// <Create Datum="04.03.2026" Entwickler="DA" />
	/// </ChangeLog>
	public partial class BookedDesksList : IDisposable
	{
		public List<Desk>? BookedDesks { get; private set; } = [];
		public IQueryable<Booking>? TodaysBookings { get; private set; }
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
					var allBookings = await apiClient.Api.Booking.GetAsync();
					var allDesks = await apiClient.Api.Desk.GetAsync();
					TodaysBookings = allBookings!.Where(b => b.BookingStart <= DateTime.UtcNow && b.BookingEnd >= DateTime.UtcNow).AsQueryable();
					if (TodaysBookings == null)
						throw new NullReferenceException(nameof(TodaysBookings));

					BookedDesks!.AddRange(allDesks?.Where(d => TodaysBookings.Any(b => b.DeskId == d.Id)) ?? Array.Empty<Desk>());
				}
				finally
				{
					Loading = false;
				}
			}
		}

		private string? GetUsernameForBookedDesk(int deskID)
			=> Users?.FirstOrDefault(u => u.Id == TodaysBookings?.FirstOrDefault(b => b.DeskId == deskID)?.UserId)?.Name ?? "";

		private string? GetDesknameFromBooking(Booking booking) => BookedDesks!.FirstOrDefault(d => d.Id == booking.DeskId)?.Name ?? "";

		private string? GetRoomnameFromBooking(Booking booking) => Rooms!.FirstOrDefault(r => r.Desks!.Any(d => d.Id == booking.DeskId))?.Name ?? "";

	}
}