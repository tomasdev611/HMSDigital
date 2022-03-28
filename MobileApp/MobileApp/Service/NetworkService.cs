using MobileApp.Pages.CommonPages;
using Xamarin.Essentials;

namespace MobileApp.Service
{
    public static class NetworkService
    {
        private static bool _isOnline = true;

        private static readonly object _sync = new object();

        public static void RegisterConnectivityEvent()
        {
            Connectivity.ConnectivityChanged += ConnectivityChanged;
        }

        private static void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            CheckConnectivity(e.NetworkAccess);
        }

        public static bool CheckConnectivity(NetworkAccess netWorkAccess)
        {
            lock (_sync)
            {
                if (netWorkAccess != NetworkAccess.Internet)
                {
                    if (_isOnline)
                    {
                        App.NavigationService.PushModalAsync<NoInternetAccess>();
                        _isOnline = false;
                    }
                }
                else
                {
                    if (!_isOnline)
                    {
                        App.NavigationService.PopModalAsync();
                        _isOnline = true;
                    }
                }
                return _isOnline;
            }
        }

        public static bool CheckConnectivity()
        {
            return CheckConnectivity(GetNetworkAccess());
        }

        public static NetworkAccess GetNetworkAccess()
        {
            return Connectivity.NetworkAccess;
        }
    }
}
