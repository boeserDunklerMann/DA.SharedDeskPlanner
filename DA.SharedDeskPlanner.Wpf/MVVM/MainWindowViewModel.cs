using DA.SharedDeskPlanner.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Text;

namespace DA.SharedDeskPlanner.Wpf.MVVM
{
	internal class MainWindowViewModel:BaseViewModel
	{
		public MainWindowViewModel():base()
		{
			// TODO DA: set connstring here
			_users = [];
			//LoadListsAsync();
		}

		#region Bound props
		private string? _connString = "";
		public string? ConnString
		{
			get => _connString;
			set
			{
				_connString = value;
				RaisePropChanged(nameof(ConnString));
			}
		}

		private ObservableCollection<User> _users;
		public ObservableCollection<User> Users
		{
			get => _users;
			set
			{
				_users = value;
				RaisePropChanged(nameof(Users));
			}
		}
		#endregion

		#region Commands
		public DelegateCommand SaveChanges => new DelegateCommand(CmdSaveChangesAsync);
		public DelegateCommand ChangeConnString => new DelegateCommand(CmdChangeConnStringAsync);
		#endregion

		#region priv. (Command) methods
		private async void CmdSaveChangesAsync()
		{
			if (_context != null)
			{
				await _context.SaveChangesAsync();
			}
		}
		private async void CmdChangeConnStringAsync()
		{
			if (_context != null)
			{
				_context.Database.SetConnectionString(_connString);
			}
			else
				_context=new SharedDeskPlannerContext(_connString!);
			LoadLists();
		}
		private void LoadLists()
		{
			if (!string.IsNullOrEmpty(_context.Database.GetConnectionString()))
			{
				var user = _context.Users.Include(nameof(User.Bookings)).ToList();
				_users.Clear();
				user.ForEach(u => _users.Add(u));
				//...
				RaisePropChanged(nameof(Users));
			}
		}
		#endregion
	}
}
