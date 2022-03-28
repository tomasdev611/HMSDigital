using System;

using OpenQA.Selenium;

namespace WebPortalAutomation
{
    public class PatientSection : PatientPage
    {
        public PatientSection(IWebDriver webdriver) : base(webdriver: webdriver) {}

        public void FindPatientAndGoToCreateOrder(
            string patientID,
            string patientName,
            string patientCreateOn
        )
        {
            Click("Filter", parameterizedCategory: "Top Page Buttons");
            
            SearchAndClick(
                searcherElement: "Filter By Hospice", input: "Only Love Hospice, LLC",
                clickElement: "Only Love Hospice, LLC", parameterizedCategoryClick: "Hospices"
            );

            SendInput("Search Bar", input: patientID);

            // Iterate through table searching for the correct patient
            IWebElement patient = InfiniteScrollSearch(
                raiseIfNotFound: true,
                iterableToScroll: "Patient Table",
                elementSearcher: patientRow => 
                    PatientInTableMatches(
                        patientRow,
                        name: patientName,
                        createdOn: patientCreateOn,
                        patientID: patientID
                    )
            );

            Click("Create Order To Patient", relativeTo: patient);
        }
    }
}
