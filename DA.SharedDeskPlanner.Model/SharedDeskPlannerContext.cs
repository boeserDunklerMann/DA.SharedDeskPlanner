using Microsoft.EntityFrameworkCore;

namespace DA.SharedDeskPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="24.04.2025" Entwickler="DA" />
	/// </ChangeLog>
	public class SharedDeskPlannerContext(string connectionString) : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// https://stackoverflow.com/questions/74060289/mysqlconnection-open-system-invalidcastexception-object-cannot-be-cast-from-d
			// MariaDB 11+ doesnt work because of nullable PKs?
			optionsBuilder.UseMySQL(connectionString);	// CaptainTrips works with MariaDB 10
			//this.SavingChanges += OnSavingChanges;
			//this.ChangeTracker.StateChanged += OnStateChanged;
		}
	}
}