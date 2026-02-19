using DA.SharedDeskPlanner.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DA.SharedDeskPlanner.Wpf.MVVM
{
	/// <ChangeLog>
	/// <Create Datum="19.02.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal class BookingsViewModel : BaseViewModel
	{
		public BookingsViewModel() : base()
		{
			_bookings = [];
			_newBooking = BaseModel.Create<Booking>();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. DA: await not possible in ctor
			LoadListAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		}

		#region Bound props
		private readonly ObservableCollection<Booking> _bookings;
		public ObservableCollection<Booking> Bookings => _bookings;
		private Booking? _selectedBooking;
		public Booking? SelectedBooking
		{
			get => _selectedBooking;
			set
			{
				_selectedBooking = value;
				RaisePropChanged(nameof(SelectedBooking));
			}
		}
		private readonly Booking _newBooking;
		public Booking NewBooking => _newBooking;
		#endregion

		#region Commands
		public DelegateCommand DeleteBooking => new DelegateCommand(CmdDeleteBooking);
		#endregion

		#region private (command) methods
		private async void CmdDeleteBooking()
		{
			_selectedBooking!.Deleted = true;
			await _context.SaveChangesAsync();
			await LoadListAsync();
		}

		private async Task LoadListAsync()
		{
			var bookings = await _context.Bookings
				.Include(nameof(Booking.User))
				.Include(nameof(Booking.Desk))
				.Where(b => !b.Deleted).ToListAsync();
			_bookings.Clear();
			bookings.ForEach(_bookings.Add);
			RaisePropChanged(nameof(Bookings));
		}
		#endregion
	}
}