using DA.SharedDeskPlanner.Model;
using DA.SharedDeskPlanner.WebAPI.Exceptions;
using DA.SharedDeskPlanner.WebAPI.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DA.SharedDeskPlanner.WebAPI.Controllers
{
	/// <ChangeLog>
	/// <Create Datum="20.02.2026" Entwickler="DA" />
	/// </ChangeLog>
	[Route("api/[controller]")]
	[ApiController]
	public class BookingController(ILogger<InventoryItemController> logger, IConfiguration configuration) : ControllerBase, IDisposable
	{
		private SharedDeskPlannerContext context = new SharedDeskPlannerContext(configuration["ConnectionStrings:da_sdp_db"]!);
		public void Dispose()
		{
			if (context != null)
			{
				context.Dispose();
				context = null!;
			}
		}

		#region Booking FUCK
		[HttpPost]
		public async Task<IActionResult> CreateBookingAsync(Booking? booking)
		{
			logger.LogInformation("Create booking");
			NullReferenceException.ThrowIfNull(context, nameof(context));
			ArgumentNullException.ThrowIfNull(booking, nameof(Booking));
			booking.ID = 0;
			await context.Bookings.AddAsync(booking);
			await context.SaveChangesAsync();
			return Ok(booking);
		}
		[HttpGet]
		public async Task<IEnumerable<Booking>> GetBookingsAsync()
		{
			logger.LogInformation("Load bookings");
			NullReferenceException.ThrowIfNull(context, nameof(context));
			return await context.Bookings
				.Include(b => b.Desk).Include(b => b.User)
				.Where(b => !b.Deleted).ToListAsync();
		}
		[HttpGet]
		[Route("{bookingID}")]
		public async Task<Booking>GetBookingAsync(int bookingID)
		{
			logger.LogInformation("Load booking");
			NullReferenceException.ThrowIfNull(context, nameof(context));
			Booking? retval = await context.Bookings.Include(b => b.Desk).Include(b => b.User)
				.FirstOrDefaultAsync(b => b.ID == bookingID && !b.Deleted);
			ObjectNotFoundException.ThrowIfNull(retval, nameof(Booking), bookingID);
			return retval!;
		}
		[HttpPut]
		public async Task<IActionResult> UpdateBookingAsync(Booking? booking)
		{
			logger.LogInformation(nameof(UpdateBookingAsync));
			NullReferenceException.ThrowIfNull(context, nameof(context));
			ArgumentNullException.ThrowIfNull(booking, nameof(Booking));
			Booking? bookingFromDB = await context.Bookings
				.FirstOrDefaultAsync(b => b.ID == booking.ID && !b.Deleted);
			ObjectNotFoundException.ThrowIfNull(bookingFromDB, nameof(Booking), booking.ID);
			bookingFromDB!.Name = booking.Name;
			bookingFromDB.BookingEnd = booking.BookingEnd;
			bookingFromDB.BookingStart = booking.BookingStart;
			bookingFromDB.User = booking.User;
			bookingFromDB.Desk = booking.Desk;
			await context.SaveChangesAsync();
			return Ok(booking);
		}
		[HttpDelete]
		[Route("{bookingID}")]
		public async Task<IActionResult> DeleteBookingAsync(int bookingID)
		{
			logger.LogInformation(nameof(DeleteBookingAsync));
			NullReferenceException.ThrowIfNull(context, nameof(context));
			Booking? bookingFromDB = await context.Bookings.FirstOrDefaultAsync(b => b.ID == bookingID && !b.Deleted);
			ObjectNotFoundException.ThrowIfNull(bookingFromDB, nameof(Booking), bookingID);
			bookingFromDB!.Deleted = true;
			await context.SaveChangesAsync();
			return Ok();
		}
		#endregion
	}
}