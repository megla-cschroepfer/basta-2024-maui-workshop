using DontLetMeExpire.Models;
using DontLetMeExpire.Services;
using DontLetMeExpire.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DontLetMeExpire.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private int _stockCount;
        private IItemService _itemService;
        private IStorageLocationService _storageLocationService;
        private INavigationService _navigationService;

        public int StockCount
        {
            get => _stockCount;
            set => SetProperty(ref _stockCount, value);
        }

        private int _expiringSoonCount;
        public int ExpiringSoonCount
        {
            get => _expiringSoonCount;
            set => SetProperty(ref _expiringSoonCount, value);
        }

        private int _expiresTodayCount;
        public int ExpiresTodayCount
        {
            get => _expiresTodayCount;
            set => SetProperty(ref _expiresTodayCount, value);
        }

        private int _expiredCount;


        public int ExpiredCount
        {
            get => _expiredCount;
            set => SetProperty(ref _expiredCount, value);
        }

        private IEnumerable<StorageLocationWithItemCount> _storageLocations = [];
        public IEnumerable<StorageLocationWithItemCount> StorageLocations { get => _storageLocations; set => SetProperty(ref _storageLocations, value); }
        public ICommand NavigateToAddItemCommand { get; }

        private async Task NavigateToAddItem()
        {
            await _navigationService.GoToAsync(nameof(ItemPage));
        }

        public ICommand NavigateToStockCommand { get; }
        private async Task NavigateToStock()
        {
            await NavigateToItemsPage("Stock");
        }

        public ICommand NavigateToExpiringSoonCommand { get; }
        private async Task NavigateToExpiringSoon()
        {
            await NavigateToItemsPage("ExpiringSoon");
        }

        public ICommand NavigateToExpiresTodayCommand { get; }
        private async Task NavigateToExpiresToday()
        {
            await NavigateToItemsPage("ExpiresToday");
        }

        public ICommand NavigateToExpiredCommand { get; }
        private async Task NavigateToExpired()
        {
            await NavigateToItemsPage("Expired");
        }
        public ICommand NavigateToLocationCommand { get; set; }
        private async Task NavigateToLocation(string locationId)
        {
            await NavigateToItemsPage("Location", locationId);
        }


        private async Task NavigateToItemsPage(string displayMode, string? locationId = null)
        {
            var navigationParams = new Dictionary<string, object>
            {
                { "DisplayMode", displayMode },
                { "LocationId", locationId }
            };
            await _navigationService.GoToAsync(nameof(ItemsPage), navigationParams);
        }

        public MainViewModel(IItemService itemService, IStorageLocationService storageLocationService, INavigationService navigationService)
        {
            _itemService = itemService;
            NavigateToAddItemCommand = new Command(async () => await NavigateToAddItem());
            NavigateToStockCommand = new Command(async () => await NavigateToStock());
            NavigateToExpiringSoonCommand = new Command(async () => await NavigateToExpiringSoon());
            NavigateToExpiresTodayCommand = new Command(async () => await NavigateToExpiresToday());
            NavigateToExpiredCommand = new Command(async () => await NavigateToExpired());
            NavigateToLocationCommand = new Command<string>(async (locationId) => await NavigateToLocation(locationId));

            _navigationService = navigationService;
            _storageLocationService = storageLocationService;
        }

        public async Task InitializeAsync()
        {
            var locations = await _storageLocationService.GetWithItemCountAsync();
            StorageLocations = locations;


            StockCount = (await _itemService.GetAsync()).Count();
            ExpiringSoonCount = (await _itemService.GetExpiresSoonAsync()).Count(); ;
            ExpiresTodayCount = (await _itemService.GetExpiresTodayAsync()).Count(); ;
            ExpiredCount = (await _itemService.GetExpiredAsync()).Count();
        }

    }
}
