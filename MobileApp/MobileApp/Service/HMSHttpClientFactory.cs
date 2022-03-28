using MobileApp.Assets;
using System;
using System.Net.Http;

namespace MobileApp.Service
{
    public static class HMSHttpClientFactory
    {
        private static HttpClient coreHttpClient;

        public static HttpClient GetCoreHttpClient()
        {
            if (coreHttpClient == null)
            {
                coreHttpClient = new HttpClient(AuthenticatedHttpClientHandler.GetAuthenticatedClientHandler());
                coreHttpClient.BaseAddress = new Uri(AppConfiguration.BaseUrl);
            }
            return coreHttpClient;
        }

        private static HttpClient patientHttpClient;

        public static HttpClient GetPatientHttpClient()
        {
            if (patientHttpClient == null)
            {
                patientHttpClient = new HttpClient(AuthenticatedHttpClientHandler.GetAuthenticatedClientHandler());
                patientHttpClient.BaseAddress = new Uri(AppConfiguration.PatientUrl);
            }
            return patientHttpClient;
        }

        private static HttpClient notificationHttpClient;

        public static HttpClient GetNotificationHttpClient()
        {
            if (notificationHttpClient == null)
            {
                notificationHttpClient = new HttpClient(AuthenticatedHttpClientHandler.GetAuthenticatedClientHandler());
                notificationHttpClient.BaseAddress = new Uri(AppConfiguration.NotificationUrl);
            }
            return notificationHttpClient;
        }
    }
}
