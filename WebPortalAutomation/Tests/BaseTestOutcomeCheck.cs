using System;

using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace WebPortalAutomation
{
    public partial class BaseTest
    {
        private void CheckOutcome()
        {
            ResultState result = TestContext.CurrentContext.Result.Outcome;
            string outcome = AnalizeOutcome(result.Status, result.Label, result.Site);

            Logger.Info($"Test result: {outcome}");
        }

        private string AnalizeOutcome(Enum status, string label, Enum site)
        {
            switch(status)
            {
                case TestStatus.Passed:
                    return "Passed ✅";
                case TestStatus.Inconclusive:
                    return "Inconclusive 🟤";
                case TestStatus.Failed:
                    return $"Fail (reason: {AnalizeFailureOutcome(label, site)}) ❌";
                case TestStatus.Skipped:
                    return $"Skipped (reason: {AnalizeSkippedOutcome(label)}) 🟡";

                default:
                    return "Status not recognized 🟣";
            }
        }

        private string AnalizeFailureOutcome(string label, Enum site)
        {
            Screenshoter.TakeScreenshot(
                webdriver,
                prefix: this.browser,
                suffix: "TEST_FAILED",
                screenshotDescription: "Last page in test failed"
            );

            string failureSite = GetFailureSite(site);

            switch(label)
            {
                case null:
                case "":
                    return $"Asssert Failure in {failureSite}";
                case "Error":
                    return $"Unexpected Exception in {failureSite}";
                case "Invalid":
                    return $"Not Runnable in {failureSite}";
                case "Cancelled":
                    return $"Cancelled in {failureSite}";

                default:
                    return $"Failed ({label}) in {failureSite}";
            }
        }

        private string AnalizeSkippedOutcome(string label)
        {
            switch(label)
            {
                case "Ignored":
                    return "Test Ignored";
                case "Explicit":
                    return "Explicit Skip";
                case "":
                case null:
                    return "Unknown";

                default:
                    return label;
            }    
        }

        private string GetFailureSite(Enum site)
        {
            switch(site)
            {
                case FailureSite.Test:
                    return "Test";
                case FailureSite.SetUp:
                    return "SetUp";
                case FailureSite.TearDown:
                    return "TearDown";
                case FailureSite.Parent:
                    return "Parent";
                case FailureSite.Child:
                    return "Child";

                default:
                    return "Site not recognized";
            }
        }
    }
}
