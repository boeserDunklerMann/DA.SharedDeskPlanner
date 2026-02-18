using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DA.SharedDeskPlanner.Wpf.MVVM
{
	/// <ChangeLog>
	/// <Create Datum="18.02.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal class ObservableObject : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
		protected void RaisePropChanged(string propertyName = "") =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}