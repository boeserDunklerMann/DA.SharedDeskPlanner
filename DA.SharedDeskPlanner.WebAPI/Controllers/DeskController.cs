using DA.SharedDeskPlanner.Model;
using DA.SharedDeskPlanner.WebAPI.Exceptions;
using DA.SharedDeskPlanner.WebAPI.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DA.SharedDeskPlanner.WebAPI.Controllers
{
	/// <ChangeLog>
	/// <Create Datum="22.02.2026" Entwickler="DA" />
	/// </ChangeLog>
	[Route("api/[controller]")]
	[ApiController]
	public class DeskController(ILogger<DeskController> logger, IConfiguration cfg) : ControllerBase, IDisposable
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

		#region Desk FUCK
		[HttpPost]
		public async Task<IActionResult> CreateDeskAsync(Desk? desk)
		{
			logger.LogInformation(nameof(CreateDeskAsync));
			NullReferenceException.ThrowIfNull(context, nameof(context));
			ArgumentNullException.ThrowIfNull(desk, nameof(desk));
			desk.ID = 0;
			await context.Desks.AddAsync(desk);
			await context.SaveChangesAsync();
			return Ok();
		}
		[HttpGet]
		public async Task<IEnumerable<Desk>> GetDesksAsync()
		{
			logger.LogInformation(nameof(GetDesksAsync));
			NullReferenceException.ThrowIfNull(context, nameof(context));
			return await context.Desks
				.Include(d => d.Room)
				.Where(d => !d.Deleted).ToListAsync();
		}
		[HttpGet]
		[Route("{deskID}")]
		public async Task<Desk> GetDeskAsync(int deskID)
		{
			logger.LogInformation(nameof(GetDeskAsync));
			NullReferenceException.ThrowIfNull(context, nameof(context));
			Desk? deskFromDB = await context.Desks.Include(d => d.Room).FirstOrDefaultAsync(d => !d.Deleted && d.ID == deskID);
			ObjectNotFoundException.ThrowIfNull(deskFromDB, nameof(Desk), deskID);
			return deskFromDB!;
		}
		[HttpPut]
		public async Task<IActionResult> UpdateDeskAsync(Desk? desk)
		{
			logger.LogInformation(nameof(UpdateDeskAsync));
			NullReferenceException.ThrowIfNull(context, nameof(context));
			ArgumentNullException.ThrowIfNull(desk, nameof(desk));
			Desk? deskFromDB = await context.Desks.Include(d => d.Room).FirstOrDefaultAsync(d => !d.Deleted && d.ID == desk.ID);
			ObjectNotFoundException.ThrowIfNull(deskFromDB, nameof(Desk), desk.ID);
			deskFromDB!.Name = desk.Name;
			deskFromDB.Remarks = desk.Remarks;
			deskFromDB.Room = desk.Room;
			await context.SaveChangesAsync();
			return Ok();
		}
		[HttpDelete]
		[Route("{deskID}")]
		public async Task<IActionResult> DeleteDeskAsync(int deskID)
		{
			logger.LogInformation(nameof(DeleteDeskAsync));
			NullReferenceException.ThrowIfNull(context, nameof(context));
			Desk? deskFromDB = await context.Desks.Include(d => d.Room).FirstOrDefaultAsync(d => !d.Deleted && d.ID == deskID);
			ObjectNotFoundException.ThrowIfNull(deskFromDB, nameof(Desk), deskID);
			deskFromDB!.Deleted = true;
			await context.SaveChangesAsync();
			return Ok();
		}
		#endregion
	}
}
