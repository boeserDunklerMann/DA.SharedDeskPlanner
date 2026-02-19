using DA.SharedDeskPlanner.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Text;

namespace DA.SharedDeskPlanner.Wpf.MVVM
{
	/// <ChangeLog>
	/// <Create Datum="18.02.2026" Entwickler="DA" />
	/// </ChangeLog>
	internal class MainWindowViewModel : BaseViewModel
	{
		public MainWindowViewModel() : base()
		{
			// TODO DA: set connstring here
			_users = [];
			_newUser = BaseModel.Create<User>();

			_inventory = [];
			_newInventoryItem = BaseModel.Create<InventoryItem>();

			_desks = [];
			_newDesk = BaseModel.Create<Desk>();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed, DA: cannot await inside ctor
			LoadListsAsync(ListToLoad.All);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
		}

		#region Bound props
		private readonly ObservableCollection<Desk> _desks;
		public ObservableCollection<Desk> Desktops => _desks;
		private Desk? _selectedDesk;
		public Desk? SelectedDesk
		{
			get => _selectedDesk;
			set
			{
				_selectedDesk = value;
				RaisePropChanged(nameof(SelectedDesk));
			}
		}
		private Desk _newDesk;
		public Desk NewDesk => _newDesk;

		private readonly ObservableCollection<InventoryItem> _inventory;
		public ObservableCollection<InventoryItem> Inventory => _inventory;
		private InventoryItem? _selectedInventoryItem;
		public InventoryItem? SelectedInventoryItem
		{
			get => _selectedInventoryItem;
			set
			{
				_selectedInventoryItem = value;
				RaisePropChanged(nameof(SelectedInventoryItem));
			}
		}
		private InventoryItem _newInventoryItem;
		public InventoryItem NewInventoryItem => _newInventoryItem;

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
		public User NewUser => _newUser;

		private User? _selectedUser;
		public User? SelectedUser
		{
			get => _selectedUser;
			set
			{
				_selectedUser = value;
				RaisePropChanged(nameof(SelectedUser));
			}
		}
		#endregion

		#region Commands
		public DelegateCommand SaveChanges => new DelegateCommand(CmdSaveChangesAsync);
		public DelegateCommand CreateUser => new DelegateCommand(CmdCreateUser);
		public DelegateCommand DeleteUser => new DelegateCommand(CmdDeleteUser);
		public DelegateCommand DeleteInventoryItem => new DelegateCommand(CmdDeleteInventoryItem);
		public DelegateCommand CreateInventoryItem => new DelegateCommand(CmdCreateInventoryItem);
		public DelegateCommand DeleteDesk => new DelegateCommand(CmdDeleteDesk);
		#endregion

		#region priv. (Command) methods
		private async void CmdDeleteDesk()
		{
			_selectedDesk!.Deleted = true;
			await _context.SaveChangesAsync();
			await LoadListsAsync(ListToLoad.Desks);
		}
		private async void CmdCreateInventoryItem()
		{
			await _context.InventoryItems.AddAsync(_newInventoryItem);
			await _context.SaveChangesAsync();
			await LoadListsAsync(ListToLoad.Inventory);
		}
		private async void CmdDeleteInventoryItem()
		{
			_selectedInventoryItem!.Deleted = true;
			await _context.SaveChangesAsync();
			await LoadListsAsync(ListToLoad.Inventory);
		}
		private async void CmdDeleteUser()
		{
			_selectedUser!.Deleted = true;

			await _context.SaveChangesAsync();
			await LoadListsAsync(ListToLoad.Users);
		}
		private async void CmdCreateUser()
		{
			await _context.Users.AddAsync(_newUser);
			await _context.SaveChangesAsync();
			await LoadListsAsync(ListToLoad.Users);
		}

		private async void CmdSaveChangesAsync()
		{
			if (_context != null)
			{
				await _context.SaveChangesAsync();
			}
		}
		enum ListToLoad
		{
			All, Users, Inventory, Desks
		}

		private async Task LoadListsAsync(ListToLoad toLoad)
		{
			if (!string.IsNullOrEmpty(_context.Database.GetConnectionString()))
			{
				switch (toLoad)
				{
					case ListToLoad.All:
						await LoadInventoryAsync();
						await LoadUsersAsync();
						await LoadDesksAsync();
						break;
					case ListToLoad.Inventory:
						await LoadInventoryAsync();
						break;
					case ListToLoad.Users:
						await LoadUsersAsync();
						break;
					case ListToLoad.Desks:
						await LoadDesksAsync();
						break;
					default:
						throw new ApplicationException($"unrecognized List to load: {toLoad.ToString()}");
				}
			}
		}
		private async Task LoadUsersAsync()
		{
			var user = await _context.Users.Include(nameof(User.Bookings)).Where(u => !u.Deleted).ToListAsync();
			_users.Clear();
			user.ForEach(_users.Add);
			RaisePropChanged(nameof(Users));
		}
		private async Task LoadInventoryAsync()
		{
			var invitems = await _context.InventoryItems.Where(ii => !ii.Deleted).ToListAsync();
			_inventory.Clear();
			invitems.ForEach(_inventory.Add);
			RaisePropChanged(nameof(Inventory));
		}
		private async Task LoadDesksAsync()
		{
			var desks = await _context.Desks.Include(nameof(Desk.Inventory))
				.Include(nameof(Desk.Room))
				.Include(nameof(Desk.Bookings))
				.Where(d => !d.Deleted).ToListAsync();

			_desks.Clear();
			desks.ForEach(_desks.Add);
			RaisePropChanged(nameof(Desktops));
		}
		#endregion
	}
}