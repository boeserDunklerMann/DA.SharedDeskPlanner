using DA.SharedDeskPlanner.Model;
using DA.SharedDeskPlanner.WebAPI.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DA.SharedDeskPlanner.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RoomController(ILogger<RoomController> logger, IConfiguration cfg) : ControllerBase, IDisposable
	{
		private SharedDeskPlannerContext context = new SharedDeskPlannerContext(cfg["ConnectionStrings:da_sdp_db"]!);
		public void Dispose()
		{
			if (context != null)
			{
				context.Dispose();
				context = null!;
			}
		}

		#region Room FUCK
		[HttpPost]
		public async Task<IActionResult> CreateRoomAsync(Room? room)
		{
			logger.LogInformation(nameof(CreateRoomAsync));
			ArgumentNullException.ThrowIfNull(room, nameof(room));
			NullReferenceException.ThrowIfNull(context, nameof(context));
			room.ID = 0;
			await context.Rooms.AddAsync(room);
			await context.SaveChangesAsync();
			return Ok();
		}
		[HttpGet]
		public async Task<IEnumerable<Room>> GetRoomsAsync()
		{
			logger.LogInformation(nameof(GetRoomsAsync));
			NullReferenceException.ThrowIfNull(context, nameof(context));
			return await context.Rooms
				.Include(r=>r.Desks)
				.Where(r => !r.Deleted).ToListAsync();
		}
		#endregion
	}
}
