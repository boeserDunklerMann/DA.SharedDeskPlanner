using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Windows;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using DA.SharedDeskPlanner.Wpf.MVVM;
namespace DA.SharedDeskPlanner.Wpf
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public IServiceProvider? ServiceProvider { get; private set; }
		public IConfiguration? Configuration { get; private set; }

		protected override void OnStartup(StartupEventArgs e)
		{
			//base.OnStartup(e);
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.local.json", optional: false, reloadOnChange: true);
			Configuration = builder.Build();
			ServiceCollection serviceCollection = new();
			ConfigureServices(serviceCollection);

			ServiceProvider= serviceCollection.BuildServiceProvider();
			MainWindow mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
			((BaseViewModel)mainWindow.DataContext).SetConfiguration(Configuration);
			mainWindow.Show();
		}
		private void ConfigureServices(IServiceCollection services)
		{
			services.AddTransient(typeof(MainWindow));
		}
	}

}
