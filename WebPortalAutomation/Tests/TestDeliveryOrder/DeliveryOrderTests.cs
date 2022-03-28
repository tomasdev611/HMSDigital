using System; // Tuple
using System.Collections.Generic; // Dictionary type defined here
using NUnit.Framework;

using OpenQA.Selenium; // IWebElement

namespace WebPortalAutomation
{
    [TestFixture, Timeout(Timeout.Default)]
    [Parallelizable(scope: ParallelScope.All)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public class DeliveryOrderTests : BaseTest
    {
        // Patient Data
        private string patientID;
        private string patientName;
        private string patientCreatedOn;

        // Items Data
        private string itemToAddCpapBipapTubingName;
        private int itemToAddCpapBipapTubingAmount;
        private string itemToAddCpapBipapTubingSKU;
        private string itemToAddCpapBipapTubingId;
        private string itemToAddCpapBipapTubingCategory;

        // Sections
        private LoginSection loginSection;
        private MenuSection menuSection;
        private PatientSection patientSection;
        private CreateNewOrderModalSection cerateNewOrderModalSection;
        private OrderingSection orderingSection;
        private DeliveryDetailsSection deliveryDetailsSection;
        private ReviewYourOrderSection reviewYourOrderSection;
        private DashboardSection dashboardSection;

        [SetUp]
        public void SetUpDeliveryOrderCreationTest()
        {
            // Correct Data to use/assert
            this.patientID = "5d88f6db-2c46-4a60-90a7-0c7a7d10046b";
            this.patientName = "Johanna Testing Automation";
            this.patientCreatedOn = "Jul 01, 2021, 11:07 AM";
            
            this.itemToAddCpapBipapTubingName = "6 FT CPAP/BIPAP TUBING";
            this.itemToAddCpapBipapTubingAmount = 5;
            this.itemToAddCpapBipapTubingSKU = "1CB-0004";
            this.itemToAddCpapBipapTubingId = "25";
            this.itemToAddCpapBipapTubingCategory = "Respiratory";

            // Sections
            this.loginSection = new LoginSection(webdriver);
            this.menuSection = new MenuSection(webdriver);
            this.patientSection = new PatientSection(webdriver);
            this.cerateNewOrderModalSection = new CreateNewOrderModalSection(webdriver);
            this.orderingSection = new OrderingSection(webdriver);
            this.deliveryDetailsSection = new DeliveryDetailsSection(webdriver);
            this.reviewYourOrderSection = new ReviewYourOrderSection(webdriver);
            this.dashboardSection = new DashboardSection(webdriver);
        }

        [Test]
        public void VerifyDeliveryOrderCreation()
        {
            // Login
            loginSection.LoginToWebPortal();
            
            // Go to Patient Page
            menuSection.GoToPatientPage();

            // Search correct patient in the table and click "create order"
            patientSection.FindPatientAndGoToCreateOrder(
                this.patientID,
                this.patientName,
                this.patientCreatedOn
            );

            // Select "Delivery" and next
            cerateNewOrderModalSection.SelectOrderToCreate(type: "Delivery");
            
            // Create order and check amount of elements added
            AsserterItemsAddedInOrder asserterItemsAdded = orderingSection.SetItemsInOrder(
                new List<ItemToAdd>() {
                    new ItemToAdd(
                        this.itemToAddCpapBipapTubingName,
                        this.itemToAddCpapBipapTubingAmount,
                        this.itemToAddCpapBipapTubingSKU,
                        this.itemToAddCpapBipapTubingId
                    )
                }
            );

            // Check that correct items were added to the order
            ItemAdded itemTubing = asserterItemsAdded.GetItem(this.itemToAddCpapBipapTubingName);
            Assert.AreEqual(1, asserterItemsAdded.NumberOfItemsAdded());
            Assert.AreEqual(this.itemToAddCpapBipapTubingAmount, itemTubing.amount);
            Assert.AreEqual(this.itemToAddCpapBipapTubingCategory, itemTubing.category);

            // After assert, go to next page
            orderingSection.ExitIFrameAndGoToDeliveryDetails();

            // Set Delivery Details of the order
            deliveryDetailsSection.HighPriority2hsHospiceUser();
            
            // Get order created and assert all the fields possible
            AsserterOrderCreated orderDataToAssert = reviewYourOrderSection.GetOrderElementsToAssert();
            
            Assert.AreEqual(1, orderDataToAssert.ItemsTable.NumberOfRows());
            AssertTrimEquals("6 FT CPAP/BIPAP TUBING", orderDataToAssert.ItemsTable[0]["Items ordered"]);
            AssertTrimEquals("1CB-0004", orderDataToAssert.ItemsTable[0]["SKU"]);
            AssertTrimEquals("5", orderDataToAssert.ItemsTable[0]["Quantity"]);
            AssertTrimEquals("$12.50", orderDataToAssert.ItemsTable[0]["Price"]);
            AssertTrimEquals("", orderDataToAssert.ItemsTable[0]["Approval Required"]);
            AssertTrimEquals("", orderDataToAssert.ItemsTable[0]["Approver"]);

            AssertTrimEquals(this.patientName, orderDataToAssert.PatientName);
            AssertTrimEquals("Paul Shea", orderDataToAssert.Nurse);
            AssertTrimEquals("2 Hrs", orderDataToAssert.DeliveryTiming);
            AssertTrimEquals("Automation", orderDataToAssert.OrderNotes);
            
            // And then place the order
            reviewYourOrderSection.GoToPlaceOrder();

            // Finally check the data of the order created
            dashboardSection.CheckOrderCreated();
        }

        [Test]
        [Ignore("Creating new features for other test")]
        public void VerifyFulfilmentDeliveryOrder()
        {

        }
    }
}
