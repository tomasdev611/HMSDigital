using MobileApp.Test.POMs;
using NUnit.Framework;
using Xamarin.UITest;

namespace MobileApp.Test
{
    public class Tests : BaseTestFixture
    {

        public Tests(Platform platform) : base(platform)
        {
        }

        [Test]
        public void ValidLogin()
        {
            var result = new LoginScreen()
                .EnterCredentials("mockdriver@example.com", "P@ssword~1")
                .ConfirmLogIn();
            Assert.IsNotNull(result);
        }
    }
}
