using DA.SharedDeskPlanner.Model;
using Microsoft.EntityFrameworkCore;

namespace DA.SharedDeskPlanner.Cons.Test
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine("Hello, World!");
			Console.WriteLine("DB will be dropped!!!");
			Console.WriteLine("press any key or hit Ctrl+C to cancel");
			Console.ReadKey();
			await CreateDataAsync();
			await ReadDataAsync();
		}

		static async Task CreateDataAsync()
		{
			using SharedDeskPlannerContext ctx = new SharedDeskPlannerContext("Server=192.168.2.108;Database=SDP_dev;Uid=root;Pwd=");
			await ctx.Database.EnsureDeletedAsync();	// delete DB
			await ctx.Database.EnsureCreatedAsync();	// create DB

			// create inv.items
			ctx.InventoryItems.AddRange([BaseModel.Create<InventoryItem>("24'' Dell Monitor"),
				BaseModel.Create<InventoryItem>("Dell Notebook Power Supply"),
				BaseModel.Create<InventoryItem>("Coffee Mug Heater"),
				BaseModel.Create<InventoryItem>("Black SharkForce cablemouse")
			]);
			await ctx.SaveChangesAsync();
		}

		static async Task ReadDataAsync()
		{
			using SharedDeskPlannerContext ctx = new SharedDeskPlannerContext("Server=192.168.2.108;Database=SDP_dev;Uid=root;Pwd=");
			var inventoryItems = await ctx.InventoryItems.ToListAsync();
			inventoryItems.ForEach(ii =>
			{
				Console.WriteLine(ii.Name);
			});
		}
	}
}
