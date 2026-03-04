using DA.SharedDeskPlanner.WebAPI.Client;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace DA.SharedDeskPlanner.WebAPI.Client.Cons.Test
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("WebAPI gestartet?");
			Console.ReadKey();
			var authProvider = new AnonymousAuthenticationProvider();
			var adapter = new HttpClientRequestAdapter(authProvider);
			var client = new ApiClient(adapter);
			var bookings = client.Api.Booking.GetAsync().Result;
			var desks = client.Api.Desk.GetAsync().Result;
		}
	}
}
