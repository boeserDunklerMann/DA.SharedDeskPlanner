using DA.SharedDeskPlanner.Model;
using DA.SharedDeskPlanner.WebAPI.Exceptions;
using DA.SharedDeskPlanner.WebAPI.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DA.SharedDeskPlanner.WebAPI.Controllers
{
	/// <ChangeLog>
	/// <Create Datum="20.02.2026" Entwickler="DA" />
	/// </ChangeLog>
	[Route("api/[controller]")]
	[ApiController]
	public class UserController(ILogger<UserController>logger, IConfiguration cfg) : ControllerBase, IDisposable
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

		#region User FUCK
		[HttpPost]
		public async Task<IActionResult> CreateUserAsync(User? user)
		{
			logger.LogInformation(nameof(CreateUserAsync));
			NullReferenceException.ThrowIfNull(context, nameof(context));
			ArgumentNullException.ThrowIfNull(user, nameof(user));
			user.ID = 0;
			await context.Users.AddAsync(user);
			await context.SaveChangesAsync();
			return Ok();
		}
		[HttpGet]
		public async Task<IEnumerable<User>> GetUsersAsync()
		{
			logger.LogInformation(nameof(GetUsersAsync));
			NullReferenceException.ThrowIfNull(context, nameof(context));
			return await context.Users.Where(u=>!u.Deleted).ToListAsync();

		}
		[HttpGet]
		[Route("{userID}")]
		public async Task<User> GetUserAsync(int userID)
		{
			logger.LogInformation(nameof(GetUserAsync));
			NullReferenceException.ThrowIfNull(context, nameof(context));
			User? retval = await context.Users.FirstOrDefaultAsync(u => !u.Deleted && u.ID == userID);
			ObjectNotFoundException.ThrowIfNull(retval, nameof(User), userID);
			return retval!;
		}
		[HttpPut]
		public async Task<IActionResult> UpdateUserAsync(User? user)
		{
			logger.LogInformation(nameof(UpdateUserAsync));
			NullReferenceException.ThrowIfNull(context, nameof(context));
			ArgumentNullException.ThrowIfNull(user, nameof(user));
			User? userFromDB = await context.Users.FirstOrDefaultAsync(u => !u.Deleted && u.ID == user.ID);
			ObjectNotFoundException.ThrowIfNull(userFromDB, nameof(User), user.ID);
			userFromDB!.FirstName = user.FirstName;
			userFromDB.LastName = user.LastName;
			userFromDB.Bookings = user.Bookings;
			userFromDB.Name = user.Name;
			await context.SaveChangesAsync();
			return Ok(user);
		}
		[HttpDelete]
		[Route("{userID}")]
		public async Task<IActionResult> DeleteUserAsync(int userID)
		{
			logger.LogInformation(nameof(DeleteUserAsync));
			NullReferenceException.ThrowIfNull(context, nameof(context));
			User? userFromDB = await context.Users.FirstOrDefaultAsync(u => !u.Deleted && u.ID == userID);
			ObjectNotFoundException.ThrowIfNull(userFromDB, nameof(User), userID);
			userFromDB.Deleted = true;
			await context.SaveChangesAsync();
			return Ok();
		}
		#endregion
	}
}
