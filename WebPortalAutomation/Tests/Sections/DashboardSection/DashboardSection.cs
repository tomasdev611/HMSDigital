using OpenQA.Selenium;

using System;
using System.Threading;
using System.Collections.Generic; // Dictionary type defined here

namespace WebPortalAutomation
{
    public class DashboardSection : DashboardPage
    {
        public DashboardSection(IWebDriver webdriver) : base(webdriver: webdriver) {}

        // Complete after new UI is finished
        public string ObtainLatestOrderIdInDashboard(
            string orderType = null,
            string orderStatus = null,
            string orderHospice = null,
            string orderSearchFilter = null
        )
        {
            CheckValidParams(orderType, orderStatus, orderHospice, orderSearchFilter);

            Click("Filter", parameterizedCategory: "Top Page Buttons");

            if(orderType is not null)
            {

            }

            if(orderStatus is not null)
            {

            }

            if(orderHospice is not null)
            {
                SearchAndClick(
                    searcherElement: "Filter By Hospice", input: "Only Love Hospice, LLC",
                    clickElement: "Only Love Hospice, LLC", parameterizedCategoryClick: "Hospices"
                );
            }

            if(orderSearchFilter is not null)
            {

            }

            return "";
        }

        public void CheckOrderCreated()
        {
            WaitElementLoad("Iteractable Section", waitElementTimeout: 60);
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }

        private void CheckValidParams(
            string orderType,
            string orderStatus,
            string orderHospice,
            string orderSearchFilter
        )
        {
            if(
                orderType is null && orderStatus is null && 
                orderHospice is null && orderSearchFilter is null
            )
            {
                throw new Exception(
                    "Error, all params to obtain last order id in dashboard are null"
                );
            }
        }
    }

}
