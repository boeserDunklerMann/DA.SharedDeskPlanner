using DA.SharedDeskPlanner.Model;
using DA.SharedDeskPlanner.WebAPI.Exceptions;
using DA.SharedDeskPlanner.WebAPI.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DA.SharedDeskPlanner.WebAPI.Controllers
{
	/// <ChangeLog>
	/// <Create Datum="20.02.2026" Entwickler="DA" />
	/// <Change Datum="28.06.2026" Entwickler="DA">property Desk in InventoryItem removed</Change>
	/// </ChangeLog>
	[Route("api/[controller]")]
	[ApiController]
	public class InventoryItemController : ControllerBase, IDisposable
	{
		private readonly ILogger<InventoryItemController> _logger;
		private SharedDeskPlannerContext context;

		public InventoryItemController(ILogger<InventoryItemController> logger, IConfiguration configuration)
		{
			_logger = logger;
			context = new SharedDeskPlannerContext(configuration["ConnectionStrings:da_sdp_db"]!);

		}
		public void Dispose()
		{
			if (context != null)
			{
				context.Dispose();
				context = null!;
			}
		}

		#region InventoryItem FUCK
		[HttpPost]
		public async Task<IActionResult> CreateInventoryItem(InventoryItem item)
		{
			NullReferenceException.ThrowIfNull(context, nameof(context));
			item.ID = 0; // will be assigned by DB
		//	item.Desk = null;   // Desk can be assigned in Update
			await context.InventoryItems.AddAsync(item);
			await context.SaveChangesAsync();
			return Ok(item);
		}
		[HttpGet]
		public async Task<IEnumerable<InventoryItem>> GetInventoryItemsAsync()
		{
			NullReferenceException.ThrowIfNull(context, nameof(context));
			return await context.InventoryItems
				//.Include(nameof(InventoryItem.Desk))
				.Where(ii => !ii.Deleted)
				.ToListAsync();
		}
		[HttpGet]
		[Route("{inventoryItemID}")]
		public async Task<InventoryItem> GetInventoryItemAsync(int inventoryItemID)
		{
			NullReferenceException.ThrowIfNull(context, nameof(context));
			InventoryItem? retval = await context.InventoryItems
				//.Include(nameof(InventoryItem.Desk))
				.FirstOrDefaultAsync(ii => !ii.Deleted && ii.ID == inventoryItemID);
			ObjectNotFoundException.ThrowIfNull(retval, nameof(InventoryItem), inventoryItemID);
			return retval!;
		}
		[HttpPut]
		public async Task<IActionResult> UpdateInventoryItemAsync(InventoryItem item)
		{
			NullReferenceException.ThrowIfNull(context, nameof(context));
			InventoryItem? itemFromDB = await context.InventoryItems.FirstOrDefaultAsync(i => i.ID == item.ID && !i.Deleted);
			ObjectNotFoundException.ThrowIfNull(itemFromDB, nameof(InventoryItem), item.ID);
			itemFromDB!.Name = item.Name;
			//itemFromDB.Desk= item.Desk;
			await context.SaveChangesAsync();
			return Ok(item);
		}
		[HttpDelete]
		[Route("{inventoryItemID}")]
		public async Task<IActionResult> DeleteInventoryItemAsync(int inventoryItemID)
		{
			NullReferenceException.ThrowIfNull(context, nameof(context));
			InventoryItem? itemFromDB = await context.InventoryItems.FirstOrDefaultAsync(i => i.ID == inventoryItemID && !i.Deleted);
			ObjectNotFoundException.ThrowIfNull(itemFromDB, nameof(InventoryItem), inventoryItemID);
			itemFromDB!.Deleted = true;
			await context.SaveChangesAsync();
			return Ok();
		}
		#endregion
	}
}