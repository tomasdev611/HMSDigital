using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MobileApp.Assets.Constants;
using MobileApp.Exceptions;
using MobileApp.Models;
using MobileApp.Pages.Screen;
using MobileApp.Pages.Screen.InventoryManagement;
using MobileApp.Pages.Screen.PurchaseOrder;
using MobileApp.Service;
using MobileApp.Utils;
using Xamarin.Forms;

namespace MobileApp.ViewModels
{
    public class HamburgerMenuMasterViewModel : BaseViewModel
    {
        private readonly UserService _userService;

        public HamburgerMenuMasterViewModel() : base()
        {
            _userService = new UserService();
            _menuItems = new ObservableCollection<HamburgerMenuItem>();
            LoadMenuItems();
            GetUserProfile();
        }

        private async void LoadMenuItems()
        {

            _menuItems.Add(new HamburgerMenuItem
            {
                Title = "Dashboard",
                ViewType = typeof(Dashboard)
            });

            if (await RolePermissionUtils.CheckPermission(PermissionName.Inventory, PermissionAccess.READ))
            {
                _menuItems.Add(new HamburgerMenuItem
                {
                    Title = "Inventory",
                    ViewType = typeof(CurrentInventory)
                });
            }

            // TODO: Verify permissions for Inventory Management
            _menuItems.Add(new HamburgerMenuItem
            {
                Title = "Purchase Orders",
                ViewType = typeof(POViewPage)
            });

            _menuItems.Add(new HamburgerMenuItem
            {
                Title = "Inventory Management",
                ViewType = typeof(InventoryTransferMainPage)
            });

            if (await RolePermissionUtils.CheckPermission(PermissionName.Inventory, PermissionAccess.UPDATE))
            {
                _menuItems.Add(new HamburgerMenuItem
                {
                    Title = "Load Inventory",
                    ViewType = typeof(InventoryLoad)
                });

                _menuItems.Add(new HamburgerMenuItem
                {
                    Title = "Physical Inventory",
                    ViewType = typeof(InventoryAuditView),
                });
            }

            _menuItems.Add(new HamburgerMenuItem
            {
                Title = "Profile",
                ViewType = typeof(ProfileScreen)
            });
        }

        private ObservableCollection<HamburgerMenuItem> _menuItems;

        public ObservableCollection<HamburgerMenuItem> MenuItems
        {
            get
            {
                return _menuItems;
            }
            set
            {
                _menuItems = value;
                OnPropertyChanged();
            }
        }

        private User _userDetails;

        public User UserDetails
        {
            get
            {
                return _userDetails;
            }
            set
            {
                _userDetails = value;
                OnPropertyChanged();
            }
        }

        private string _userName;

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        private ICommand _navigateToProfileCommand;

        public ICommand NavigateToProfileCommand
        {
            get
            {
                return _navigateToProfileCommand ?? (_navigateToProfileCommand = new Command(NavigateToProfile));
            }
        }

        private ICommand _menuItemSelectedCommand;

        public ICommand MenuItemSelectedCommand
        {
            get
            {
                return _menuItemSelectedCommand ?? (_menuItemSelectedCommand = new Command(MenuItemSelected));
            }
        }

        public async void NavigateToProfile()
        {
            await _navigationService.NavigateToAsync(typeof(ProfileScreen));
        }

        private void MenuItemSelected(object menuItemSelectedEventArgs)
        {
            var menuItem = ((menuItemSelectedEventArgs as ItemTappedEventArgs)?.Item as HamburgerMenuItem);

            if (menuItem != null)
            {
                _navigationService.ChangeDetailPageAsync(menuItem.ViewType);
            }
        }

        public async void GetUserProfile()
        {
            try
            {
                UserDetails = await CacheManager.GetUserDetails();
                if (UserDetails != null)
                {
                    UserName = $"{UserDetails.FirstName} {UserDetails.LastName}";
                }
            }
            catch
            {
                _toastMessageService.ShowToast("Something went wrong");
            }
        }
    }
}
