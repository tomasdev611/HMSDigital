using System;

using OpenQA.Selenium;

namespace WebPortalAutomation
{
    public class PatientInfoModalSection : PatientInfoModalPage
    {
        public PatientInfoModalSection(IWebDriver webdriver) : base(webdriver: webdriver) {}
    
        public string ObtainPatientIDAndClose()
        {   
            string patientID = LoadContentFromElement("Patient ID", type: "waitables");
            Click("Close Info Modal");
            return patientID;
        }
    }
}
