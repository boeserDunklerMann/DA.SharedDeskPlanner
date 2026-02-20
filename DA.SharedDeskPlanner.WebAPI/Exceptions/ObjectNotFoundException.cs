using ZstdSharp;

namespace DA.SharedDeskPlanner.WebAPI.Exceptions
{
	/// <ChangeLog>
	/// <Create Datum="20.02.2026" Entwickler="DA" />
	/// </ChangeLog>
	public class ObjectNotFoundException(string? entityName, int? id) : Exception
	{
		public override string ToString()
		{
			return $"Entity {entityName} with ID {id} not found.";
		}
		public static void ThrowIfNull(object? obj, string? entityName, int? id)
		{
			if (obj == null) throw new ObjectNotFoundException(entityName, id);
		}
	}
}
