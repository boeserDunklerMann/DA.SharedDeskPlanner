namespace DA.SharedDeskPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="18.02.2026" Entwickler="DA" />
	/// </ChangeLog>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable. AD: Darum kümmert sich EFCore
	public class User : BaseModel
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }

		#region Overrides
		public override bool Equals(object? obj)
		{
			if (obj==null || !(obj is User)) return false;
			return ID == ((User)obj).ID;
		}
		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}
		#endregion
		public ICollection<Booking> Bookings { get; set; }
	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable. AD: Darum kümmert sich EFCore
}
