using System;
using NUnit.Framework;
using Xamarin.UITest;

namespace MobileApp.Test
{
    public abstract class BasePage
    {
        protected IApp app => AppManager.App;
        protected bool OniOS => AppManager.Platform == Platform.iOS;

        protected abstract PlatformQuery Trait { get; }

        protected BasePage()
        {
            AssertOnPage(TimeSpan.FromSeconds(30));
            app.Screenshot("On " + this.GetType().Name);
        }

        protected void AssertOnPage(TimeSpan? timeout = default(TimeSpan?))
        {
            var message = "Unable to verify on page: " + this.GetType().Name;

            if (timeout == null)
            {
                Assert.IsNotEmpty(app.Query(Trait.Current), message);
            }
            else
            {
                Assert.DoesNotThrow(() => app.WaitForElement(Trait.Current, timeout: timeout), message);
            }
        }

        protected void WaitForPageToLeave(TimeSpan? timeout = default(TimeSpan?))
        {
            timeout = timeout ?? TimeSpan.FromSeconds(5);
            var message = "Unable to verify *not* on page: " + this.GetType().Name;

            Assert.DoesNotThrow(() => app.WaitForNoElement(Trait.Current, timeout: timeout), message);
        }
    }
}