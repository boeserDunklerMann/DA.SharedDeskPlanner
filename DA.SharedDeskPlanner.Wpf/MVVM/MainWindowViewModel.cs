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
			_newUser = BaseModel.Create<User>();
			LoadListsAsync();
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

		private readonly ObservableCollection<User> _users;
		public ObservableCollection<User> Users
		{
			get => _users;
		}

		private User _newUser;
		public User NewUser
		{
			get => _newUser;
		}
		#endregion

		#region Commands
		public DelegateCommand SaveChanges => new DelegateCommand(CmdSaveChangesAsync);
		public DelegateCommand CreateUser => new DelegateCommand(CmdCreateUser);
		#endregion

		#region priv. (Command) methods
		private async void CmdCreateUser()
		{
			_newUser.ChangeDate = _newUser.CreationDate = DateTime.UtcNow;
			await _context.Users.AddAsync(_newUser);
			await _context.SaveChangesAsync();
		}

		private async void CmdSaveChangesAsync()
		{
			if (_context != null)
			{
				await _context.SaveChangesAsync();
			}
		}
		private async Task LoadListsAsync()
		{
			if (!string.IsNullOrEmpty(_context.Database.GetConnectionString()))
			{
				var user = await _context.Users.Include(nameof(User.Bookings)).ToListAsync();
				_users.Clear();
				user.ForEach(u => _users.Add(u));
				//...
				RaisePropChanged(nameof(Users));
			}
		}
		#endregion
	}
}
