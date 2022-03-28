using System.Threading.Tasks;
using MobileApp.Exceptions;
using MobileApp.Interface;
using MobileApp.Methods;
using MobileApp.Models;
using Refit;
using System;

namespace MobileApp.Service
{
    public class DriverService
    {
        private IDriverApi _driverApi;

        private readonly CurrentLocation _currentLocation;

        public DriverService()
        {
            _driverApi = RestService.For<IDriverApi>(HMSHttpClientFactory.GetCoreHttpClient());
            _currentLocation = new CurrentLocation();
        }

        public async Task<Driver> GetDriverUserDetailsAsync()
        {
            try
            {
                var response = await _driverApi.GetDriverDetailsAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                return response.Content;
            }
            catch
            {
                throw;
            }
        }

        public async Task<DispatchResponse> GetDriversDispatchInstructions()
        {
            try
            {
                var response = await _driverApi.GetDispatchInstructionsAsync();
                if (response.IsSuccessStatusCode)
                {
                    return response.Content;
                }
                return null;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> SendDriverLocation()
        {
            try
            {
                var currentLocation = await _currentLocation.GetCurrentLocationAsync();

                var geoLocation = new GeoLocation()
                {
                    Latitude = (decimal)currentLocation.Latitude,
                    Longitude = (decimal)currentLocation.Longitude
                };

                var response = await _driverApi.SendDriverLocationAsync(geoLocation);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                throw;
            }
        }
    }
}
