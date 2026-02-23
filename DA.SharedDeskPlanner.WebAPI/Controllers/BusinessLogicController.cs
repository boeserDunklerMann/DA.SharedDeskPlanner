using DA.SharedDeskPlanner.Model;
using DA.SharedDeskPlanner.WebAPI.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DA.SharedDeskPlanner.WebAPI.Controllers
{
	/// <ChangeLog>
	/// <Create Datum="23.02.2026" Entwickler="DA" />
	/// </ChangeLog>
	[Route("bl/[controller]")]
	[ApiController]
	public class BusinessLogicController(ILogger<BusinessLogicController> logger, IConfiguration cfg) : ControllerBase, IDisposable
	{
		private SharedDeskPlannerContext context = new SharedDeskPlannerContext(cfg["ConnectionStrings:da_sdp_db"]!);
		[HttpGet]
		[Route("GetBookings")]
		public async Task<IEnumerable<Booking>> GetBookingsAsync(DateTime begin, DateTime end)
		{
			logger.LogInformation(nameof(GetBookingsAsync));
			NullReferenceException.ThrowIfNull(context, nameof(context));
			return await context.Bookings
				.Include(b=>b.User)
				.Include(b=>b.Desk)
				.Where(b => !b.Deleted && b.BookingStart >= begin && b.BookingEnd <= end).ToListAsync();
		}
		[HttpGet]
		[Route("GetBookingsPerUser/{userID}")]
		public async Task<IEnumerable<Booking>> GetBookingsPerUserAsync(int userID)
		{
			logger.LogInformation(nameof(GetBookingsPerUserAsync));
			NullReferenceException.ThrowIfNull(context, nameof(context));
			return await context.Bookings
				.Include(b => b.User)
				.Where(b => !b.Deleted && b.User.ID == userID).ToListAsync();
		}

		public void Dispose()
		{
			if (context != null)
			{
				context.Dispose();
				context = null!;
			}
		}
	}
}