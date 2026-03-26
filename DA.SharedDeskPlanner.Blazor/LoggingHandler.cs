namespace DA.SharedDeskPlanner.Blazor
{
	public class LoggingHandler : DelegatingHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			Console.WriteLine($"Request URI: {request.RequestUri}");
			if (request.Content != null)
			{
				var content = await request.Content.ReadAsStringAsync();
				Console.WriteLine($"Body: {content}");
			}
			return await base.SendAsync(request, cancellationToken);
		}
	}
}
