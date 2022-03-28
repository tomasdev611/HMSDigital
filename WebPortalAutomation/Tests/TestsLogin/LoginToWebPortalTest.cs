using System;
using NUnit.Framework;

namespace WebPortalAutomation
{
    [TestFixture, Timeout(Timeout.Default)]
    [Parallelizable(scope: ParallelScope.All)]
    [FixtureLifeCycleAttribute(LifeCycle.InstancePerTestCase)]
    public class LoginToWebPortalTest : BaseTest
    {
        private string adminAccountName;
        private LoginSection loginSection;
        private MenuSection menuSection;

        [SetUp]
        public void SetUpLoginToWebPortalTest()
        {            
            // Correct Data to use/assert
            this.adminAccountName = "Johanna Santos";

            // Sections
            this.loginSection = new LoginSection(webdriver);
            this.menuSection = new MenuSection(webdriver);
        }

        [Test]
        //[Ignore("Creating new features for other test")]
        public void ValidUserCanLoginToWebPortal()
        {
            // Login
            loginSection.LoginToWebPortal();
            
            // Get the data in the menu to assert
            AsserterAccount accountToAssert = menuSection.GetAccountName();
            AssertTrimEquals(this.adminAccountName, accountToAssert.Name);
        }
    }
}
