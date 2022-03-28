using System;
using System.Diagnostics;

namespace MobileApp
{
    public static class PagesUtils
    {
        public static void PageNavigationFollowUp(string page)
        {
#if DEBUG
            Debug.WriteLine($"Current View: {page}");
#endif
        }
    }
}