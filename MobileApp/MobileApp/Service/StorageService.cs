using System;
using Xamarin.Essentials;

namespace MobileApp.Service
{
    public class StorageService
    {
        public string GetFromStorage(string key, bool isSecret = false)
        {
            if (isSecret)
            {
                try
                {
#if DEBUG
                    return Preferences.Get(key, null);
#else
                    return SecureStorage.GetAsync(key).Result;
#endif
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                return Preferences.Get(key, null);
            }
        }

        public void AddToStorage(string key, string value, bool isSecret = false)
        {
            if (isSecret)
            {
                try
                {
#if DEBUG
                    Preferences.Set(key, value);
#else
                    SecureStorage.SetAsync(key, value);
#endif
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                Preferences.Set(key, value);
            }
        }

        public void RemoveFromStorage(string key, bool isSecret = false)
        {
            if (isSecret)
            {
#if DEBUG
                Preferences.Remove(key);
#else
                SecureStorage.Remove(key);
#endif

            }
            else
            {
                Preferences.Remove(key);
            }
        }

        public void ClearStorage()
        {
            Preferences.Clear();
            SecureStorage.RemoveAll();
        }
    }
}