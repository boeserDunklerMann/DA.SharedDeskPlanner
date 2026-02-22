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
				.Include(r => r.Desks)
				.Where(r => !r.Deleted).ToListAsync();
		}
		[HttpGet]
		[Route("{roomID}")]
		public async Task<Room> GetRoomAsync(int roomID)
		{
			logger.LogInformation(nameof(GetRoomAsync));
			NullReferenceException.ThrowIfNull(context, nameof(context));
			Room? roomFromDB = await context.Rooms.FirstOrDefaultAsync(r => r.Deleted && r.ID == roomID);
			ObjectNotFoundException.ThrowIfNull(roomFromDB, nameof(Room), roomID);
			return roomFromDB!;
		}
		[HttpPut]
		public async Task<IActionResult> UpdateRoomAsync(Room? room)
		{
			logger.LogInformation(nameof(UpdateRoomAsync));
			NullReferenceException.ThrowIfNull(context, nameof(context));
			ArgumentNullException.ThrowIfNull(room, nameof(room));
			Room? roomFromDB = await context.Rooms.FirstOrDefaultAsync(r => r.Deleted && r.ID == room.ID);
			ObjectNotFoundException.ThrowIfNull(roomFromDB, nameof(Room), room.ID);
			roomFromDB!.Name = room.Name;
			roomFromDB.Desks = room.Desks;
			await context.SaveChangesAsync();
			return Ok(room);
		}
		[HttpDelete]
		[Route("{roomID}")]
		public async Task<IActionResult> DeleteRoomAsync(int roomID)
		{
			logger.LogInformation(nameof(DeleteRoomAsync));
			NullReferenceException.ThrowIfNull(context, nameof(context));
			Room? roomFromDB = await context.Rooms.FirstOrDefaultAsync(r => r.Deleted && r.ID == roomID);
			ObjectNotFoundException.ThrowIfNull(roomFromDB, nameof(Room), roomID);
			roomFromDB!.Deleted = true;
			await context.SaveChangesAsync();
			return Ok();
		}
		#endregion
	}
}