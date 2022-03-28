using System;
using OpenQA.Selenium;

namespace WebPortalAutomation
{
    public class PatientPage : BasePage
    {
        PatientInfoModalSection patientInfoModalSection;

        public PatientPage(IWebDriver webdriver) : 
            base(webdriver: webdriver, pathURL: "/patients")
        {
            this.elements = Utils.LoadJson(jsonFileName: "PatientPageElements.json");
            this.patientInfoModalSection = new PatientInfoModalSection(webdriver);
        }

        protected bool PatientInTableMatches(
            IWebElement rowPatient,
            string status = null,
            string name = null,
            string dob = null,
            string createdOn = null,
            string lastOrder= null,
            string hospiceLocation = null,
            string patientID = null
        )
        {
            Logger.Info($"Checking if patient --{rowPatient}-- matches");
            // Check correct usage of parameters
            if(
                status is null && name is null && dob is null && createdOn is null &&
                lastOrder is null && hospiceLocation is null && patientID is null
            )
            {
                throw new Exception("At least one attribute of the row must be passed");
            }

            try
            {
                // Discard wrong patients
                bool matched = 
                    (status is null || ObtainSubElem(rowPatient, "Patient Table Status") == status) &&
                    (name is null || ObtainSubElem(rowPatient, "Patient Table Name") == name) &&
                    (dob is null || ObtainSubElem(rowPatient, "Patient Table DOB") == dob) &&
                    (createdOn is null || ObtainSubElem(rowPatient, "Patient Table Created On") == createdOn) &&
                    (lastOrder is null || ObtainSubElem(rowPatient, "Patient Table Last Order") == lastOrder) &&
                    (hospiceLocation is null || ObtainSubElem(rowPatient, "Patient Table Hospice Location") == hospiceLocation);
            
                if(matched && patientID is not null)
                {
                    Click(rowPatient);
                    string obtainedID = patientInfoModalSection.ObtainPatientIDAndClose();
                    matched &= (obtainedID == patientID);
                }

                Logger.Info($"The result of check if matches is {matched}");
                return matched;
            }
            catch(Exception exc)
            {
                Logger.Error(
                    $"The attribute checking failed so it's assume that is not the correct patient. Error: {exc.Message}\n{exc.StackTrace}"
                );
                return false;
            }
        }

        private string ObtainSubElem(IWebElement row, string subElement)
        {
            // Only try 1 time to find the element, and wait 2 seconds max in that attemp
            // the iwebelement is already loaded
            return LoadContentFromElement(
                subElement,
                "waitables",
                relativeTo: row,
                retries: 1,
                waitElementTimeout: 2
            );
        }
    }
}
