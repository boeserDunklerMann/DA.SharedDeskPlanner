using DA.SharedDeskPlanner.WebAPI.Client;
using DA.SharedDeskPlanner.WebAPI.Client.Api.Booking;
using DA.SharedDeskPlanner.WebAPI.Client.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace DA.SharedDeskPlanner.Blazor.Components.Pages
{
	/// <ChangeLog>
	/// <Create Datum="25.03.2026" Entwickler="DA" />
	/// </ChangeLog>
	public partial class AddBooking : IDisposable
	{
		public Booking NewBooking { get; set; } = new();
		public IQueryable<User>? UserList { get; private set; }
		public IQueryable<Room>? RoomsList { get; private set; }
		public int SelectedUserID { get; set; }
		public IQueryable<Booking>? UsersBookings { get; private set; }
		protected override async Task OnInitializedAsync()
		{
			editContext = new EditContext(NewBooking);
			await base.OnInitializedAsync();
			if (!Loading && apiClient != null)
			{
				try
				{
					Loading = true;
					UserList = (await apiClient.Api.User!.GetAsync())!.AsQueryable();
					RoomsList = (await apiClient.Api.Room!.GetAsync())!.AsQueryable();
					SelectedUserID = UserList.First().Id!.Value;
				}
				finally
				{
					Loading = false;
				}
			}
		}

		private async Task SelectUserAsync(int newUserID)
		{
			if (!Loading && apiClient != null)
			{
				SelectedUserID = newUserID;
				var usersBookings = (await apiClient.Api.Booking.GetAsync())!.Where(b => b.UserId == SelectedUserID);
				if (usersBookings != null)
					UsersBookings = usersBookings.AsQueryable();
				else
					throw new ApplicationException(nameof(usersBookings));
			}
		}

		private string GetRoomName(Booking b) => RoomsList?.Where(r => r.Desks!.Select(d => d.Id).Contains(b.DeskId))?.FirstOrDefault()?.Name ?? "unbekannt";

		private async Task OnNewBookingValidSubmittedAsync()
		{
			if (!Loading && apiClient != null && UserList != null)
			{
				try
				{
					Loading = true;
					var postBody = new BookingRequestBuilder.BookingPostRequestBody() { Booking = new() };
					//postBody.Booking.User = UserList.FirstOrDefault(u => u.Id == SelectedUserID);
					postBody.Booking.User = null;
					postBody.Booking.UserId = SelectedUserID;
					postBody.Booking.Name = NewBooking.Name;
					postBody.Booking.BookingStart = NewBooking.BookingStart;
					postBody.Booking.BookingEnd = NewBooking.BookingEnd;
					postBody.Booking.Desk = null;
					postBody.Booking.DeskId = 1;
					await apiClient.Api.Booking.PostAsync(postBody);
				}
				catch(Exception e)
				{
					Console.WriteLine(e.Message);
					if (e.InnerException != null)
						Console.WriteLine(e.InnerException.Message);
				}
				finally
				{
					Loading = false;
				}
			}
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
