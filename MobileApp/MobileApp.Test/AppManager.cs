using System;
using Xamarin.UITest;

namespace MobileApp.Test
{
    static class AppManager
    {
        private static IApp app;
        public static IApp App
        {
            get
            {
                if (app == null)
                    throw new NullReferenceException("'AppManager.App' not set");
                return app;
            }
        }

        private static Platform platform;
        public static Platform Platform
        {
            get
            {
                return platform;
            }

            set
            {
                platform = value;
            }
        }

        public static void StartApp()
        {
            if (Platform == Platform.Android)
            {
                app = ConfigureApp
                    .Android
                    .StartApp();
            }

            if (Platform == Platform.iOS)
            {
                app = ConfigureApp
                   .iOS
                   .StartApp();
            }
        }
    }
}