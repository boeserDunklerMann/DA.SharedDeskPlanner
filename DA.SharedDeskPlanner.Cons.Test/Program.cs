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
			
			// create desk with inventory
			var desk = BaseModel.Create<Desk>("White 'Maja' Desk");
			desk.Remarks = "Höhenverstellbar";
			var monitor = ctx.InventoryItems.FirstOrDefault(ii => ii.ID == 1);
			var power = ctx.InventoryItems.FirstOrDefault(ii => ii.ID == 2);
			var heater = ctx.InventoryItems.FirstOrDefault(ii => ii.ID == 3);
			var mouse = ctx.InventoryItems.FirstOrDefault(ii => ii.ID == 4);

			//desk.Inventory.Add(monitor!);
			//desk.Inventory.Add(power!);
			//desk.Inventory.Add(heater!);
			//desk.Inventory.Add(mouse!);
			// assign desk to invitems
			monitor!.Desk= desk;
			power!.Desk = desk;
			heater!.Desk = desk;
			mouse!.Desk = desk;

			ctx.Desks.Add(desk);
			//await ctx.SaveChangesAsync();

			var room = BaseModel.Create<Room>("Arbeitszimmer");
			desk.Room = room;
			//room.Desks.Add(desk);
			ctx.Rooms.Add(room);
			await ctx.SaveChangesAsync();
		}

		static async Task ReadDataAsync()
		{
			using SharedDeskPlannerContext ctx = new SharedDeskPlannerContext("Server=192.168.2.108;Database=SDP_dev;Uid=root;Pwd=");
			var inventory = ctx.InventoryItems.ToList();
			var rooms = await ctx.Rooms
				.Include(nameof(Room.Desks))
				.ToListAsync();
			// jetzt müssten auch die Desks und InvItems geladen worden sein
			var inventoryItems = await ctx.InventoryItems.ToListAsync();
			inventoryItems.ForEach(ii =>
			{
				Console.WriteLine(ii.Name);
			});
		}
	}
}
