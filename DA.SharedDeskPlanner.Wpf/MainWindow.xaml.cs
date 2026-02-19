using DA.SharedDeskPlanner.Wpf.MVVM;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DA.SharedDeskPlanner.Wpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : RibbonWindow
	{
		public MainWindow()
		{
			InitializeComponent();
			MainWindowViewModel vm = (MainWindowViewModel)DataContext;
		}

		private void rbnMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Ribbon ribbon = (Ribbon)sender;
			if (ribbon != null)
			{
				grdMain.Children.Clear();
				Control? child = null;
				switch (ribbon.SelectedIndex)
				{
					case 0:
						child = new Controls.StartControl();
						break;
					case 1:
						child = new Controls.StammdatenControl();
						break;
					case 2:
						child = new Controls.UsersControl();
						break;
					case 3:
						child = new Controls.RoomsControl();
						break;
					case 4:
						child = new Controls.DesksControl();
						break;
					case 5:
						child = new Controls.BookingsControl();
						break;
					default:
						throw new ApplicationException($"unrecognized ribbon tab index: {ribbon.SelectedIndex}");
				}
				grdMain.Children.Add(child);
			}
			else
				throw new NullReferenceException(nameof(ribbon));
        }
    }
}