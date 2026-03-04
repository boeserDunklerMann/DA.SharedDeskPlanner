
namespace DA.SharedDeskPlanner.WebAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi(options =>
			{
				options.AddSchemaTransformer((schema, context, cancellationToken) =>
				{
					// Wir prüfen, ob der zugrundeliegende C#-Typ ein int oder int? ist
					if (context.JsonTypeInfo.Type == typeof(int) || context.JsonTypeInfo.Type == typeof(int?))
					{
						// Bei Schemas sind die Properties meist beschreibbar
						schema.Type = Microsoft.OpenApi.JsonSchemaType.Integer;
						schema.Format = "int32";

						// Falls Kiota wegen "Nullable" verwirrt ist, erzwingen wir hier Eindeutigkeit
						// (Je nach Version kann schema.Nullable direkt gesetzt werden)
						//if (context.JsonTypeInfo.Type == typeof(int))
						//{
						//	schema.Nullable = false;
						//}
					}
					return Task.CompletedTask;
				});
			}); 
			builder.Configuration.AddJsonFile("appsettings.local.json", false);  // there are some secrets which will not be committed to git

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
