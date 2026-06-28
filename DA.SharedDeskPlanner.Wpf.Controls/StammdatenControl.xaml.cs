using DA.Wpf.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DA.SharedDeskPlanner.Wpf.Controls
{
	/// <ChangeLog>
	/// <Create Datum="08.05.2026" Entwickler="DA" />
	/// </ChangeLog>
	/// <summary>
	/// Interaction logic for StammdatenControl.xaml
	/// </summary>
	public partial class StammdatenControl : UserControl, IPlugIn
	{
		public StammdatenControl()
		{
			InitializeComponent();
		}

		public void OnInit(DbContext ctx)
		{
			throw new NotImplementedException();
		}

		public void OnStart()
		{
			throw new NotImplementedException();
		}

		public void OnStop()
		{
			throw new NotImplementedException();
		}
	}
}