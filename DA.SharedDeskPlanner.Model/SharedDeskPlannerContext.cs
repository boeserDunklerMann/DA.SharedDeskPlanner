using Microsoft.EntityFrameworkCore;

namespace DA.SharedDeskPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="18.02.2026" Entwickler="DA" />
	/// <Change Datum="18.02.2026" Entwickler="DA">User and Booking added</Change>
	/// </ChangeLog>
	public class SharedDeskPlannerContext(string connectionString) : DbContext
	{
		public DbSet<InventoryItem> InventoryItems { get; set; }
		public DbSet<Desk> Desks { get; set; }
		public DbSet<Room> Rooms { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Booking> Bookings { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// https://stackoverflow.com/questions/74060289/mysqlconnection-open-system-invalidcastexception-object-cannot-be-cast-from-d
			// MariaDB 11+ doesnt work because of nullable PKs?
			optionsBuilder.UseMySQL(connectionString);  // CaptainTrips works with MariaDB 10
														//this.SavingChanges += OnSavingChanges;
														//this.ChangeTracker.StateChanged += OnStateChanged;
		}
		private void OnStateChanged(object? sender, Microsoft.EntityFrameworkCore.ChangeTracking.EntityStateChangedEventArgs e)
		{
			// TODO AD: https://learn.microsoft.com/de-de/ef/core/logging-events-diagnostics/events
		}

		private void OnSavingChanges(object? sender, SavingChangesEventArgs e)
		{
			// TODO AD: https://learn.microsoft.com/de-de/ef/core/logging-events-diagnostics/events
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Room>(entity =>
			{
				entity.HasKey(r => r.ID);
				entity.Property(r => r.Name).IsRequired();
			});
			modelBuilder.Entity<Desk>(entity =>
			{
				entity.HasKey(d => d.ID);
				entity.Property(d => d.Name).IsRequired();
				entity.HasOne(d => d.Room).WithMany(r => r.Desks);
				entity.HasMany(d => d.Inventory).WithOne(ii => ii.Desk);
				entity.HasMany(d => d.Bookings).WithOne(b => b.Desk);
			});
			modelBuilder.Entity<InventoryItem>(entity =>
			{
				entity.HasKey(ii => ii.ID);
				entity.Property(ii => ii.Name).IsRequired();
				//entity.HasOne(ii => ii.Desk).WithMany(d => d.Inventory);
			});
			modelBuilder.Entity<User>(ent =>
			{
				ent.HasKey(u => u.ID);
				ent.Property(u => u.Name).IsRequired();
				ent.HasMany(u => u.Bookings).WithOne(b => b.User);
			});
			modelBuilder.Entity<Booking>(ent =>
			{
				ent.HasKey(b => b.ID);
				ent.Property(b => b.Name).IsRequired();
			});
		}
	}
}