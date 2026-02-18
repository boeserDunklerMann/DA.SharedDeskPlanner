namespace DA.SharedDeskPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="18.02.2026" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// Items that belongs to a desk, like mouse, port-replicator, monitor, ...
	/// </summary>
	public class InventoryItem : BaseModel
	{
		public override bool Equals(object? obj)
		{
			if (obj == null || !(obj is InventoryItem)) return false;
			return ID == ((InventoryItem)obj).ID;
		}
		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}
	}
}