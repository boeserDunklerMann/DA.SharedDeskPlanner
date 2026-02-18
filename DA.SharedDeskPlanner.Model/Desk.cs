using System;
using System.Collections.Generic;
using System.Text;

namespace DA.SharedDeskPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="18.02.2026" Entwickler="DA" />
	/// </ChangeLog>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable. AD: Darum kümmert sich EFCore
	public class Desk : BaseModel
	{
		/// <summary>
		/// like "Höhenverstellbar"
		/// </summary>
		public string? Remarks { get; set; }

		public override bool Equals(object? obj)
		{
			if (obj == null || !(obj is Desk)) return false;
			return ID == ((Desk)obj).ID;
		}
		public override int GetHashCode()
		{
			return ID.GetHashCode();
		}

		public virtual ICollection<InventoryItem> Inventory { get; set; }
		public virtual Room Room { get; set; }
	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable. AD: Darum kümmert sich EFCore
}