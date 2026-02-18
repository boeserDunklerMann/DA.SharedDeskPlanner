using System.ComponentModel.DataAnnotations.Schema;

namespace DA.SharedDeskPlanner.Model
{
	/// <ChangeLog>
	/// <Create Datum="18.02.2026" Entwickler="DA" />
	/// </ChangeLog>
	public abstract class BaseModel
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ID { get; set; }
		public string Name { get; set; } = "";

		/// <summary>
		/// Änderungsdatum des Datensatzes
		/// </summary>
		public DateTime? ChangeDate { get; set; }

		/// <summary>
		/// Erstelldatum
		/// </summary>
		public DateTime? CreationDate { get; set; }
		public bool Deleted { get; set; }
		public override string ToString()
		{
			return	Name;
		}
		public static T Create<T>(string name="") where T: BaseModel, new()
		{
			return new T { Name = name, CreationDate = DateTime.UtcNow, ChangeDate = DateTime.UtcNow };
		}
	}
}