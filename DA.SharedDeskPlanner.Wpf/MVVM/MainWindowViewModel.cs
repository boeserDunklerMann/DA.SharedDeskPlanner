using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Text;

namespace DA.SharedDeskPlanner.Wpf.MVVM
{
	internal class MainWindowViewModel:BaseViewModel
	{
		public MainWindowViewModel():base()
		{
			// TODO DA: set connstring here
		}

		#region Commands
		public DelegateCommand SaveChanges => new DelegateCommand(SaveChangesAsync);
		#endregion

		#region priv. Command methods
		private async void SaveChangesAsync()
		{

		}
		#endregion
	}
}
