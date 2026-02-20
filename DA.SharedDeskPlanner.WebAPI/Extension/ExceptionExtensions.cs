namespace DA.SharedDeskPlanner.WebAPI.Extension
{
	/// <ChangeLog>
	/// <Create Datum="20.02.2026" Entwickler="DA" />
	/// </ChangeLog>
	public static class ExceptionExtensions
	{
		extension(NullReferenceException ex)
		{
			public static void ThrowIfNull(object? value, string? msg)
			{
				if (value == null)
					throw new NullReferenceException(msg);
			}
		}
	}
}