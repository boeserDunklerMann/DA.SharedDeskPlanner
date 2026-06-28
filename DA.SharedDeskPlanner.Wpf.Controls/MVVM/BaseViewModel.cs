using DA.SharedDeskPlanner.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.SharedDeskPlanner.Wpf.MVVM
{
	/// <ChangeLog>
	/// <Create Datum="18.02.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal class BaseViewModel : ObservableObject, IDisposable
	{
		protected Model.SharedDeskPlannerContext _context;

		public BaseViewModel()
		{
			_context = ContextSingletonFactory.Instance;
		}

		public void SetConfiguration(IConfiguration cfg)
		{
			_context.SetConfiguration(cfg);
		}

		public void Dispose()
		{
			if (_context != null)
			{
				_context.Dispose();
				_context = null!;
			}
		}
	}
}