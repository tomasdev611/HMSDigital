using System;
using System.IO; // File, Path defined here
using System.Linq;

using OpenQA.Selenium;

using System.Threading;
using System.Collections.Generic; // Dictionary type defined here

namespace WebPortalAutomation
{
    public partial class OrderingPage : BasePage
    {
        protected const int maxAmountOfRetries = 15;
        protected const int sleepIfNeededInRetries = 1;
        protected const int sleepAddToItemAddedInCart = 3;

        public OrderingPage(IWebDriver webdriver) : 
            base(webdriver: webdriver, pathURL: "/orders") 
        {
            this.elements = Utils.LoadJson(jsonFileName: "OrderingPageElements.json");
        }

        protected void AddItemsToTheCart(List<ItemToAdd> items)
        {
            Logger.Info("Adding items to the cart");
            List<ItemToAdd> sortedItems = items.OrderBy(item => item.name).ToList();

            for(int i = 0; i < sortedItems.Count; i++)
            {
                ItemToAdd itemToAdd = sortedItems[i];
                string nameToAdd = itemToAdd.name;
                int amountToAdd = itemToAdd.amount;

                (IWebElement, int) itemAndAmountLoaded = GetItemAndAmountLoadedInCart(nameToAdd);
                int amountLoadedInCart = itemAndAmountLoaded.Item2;

                if(amountLoadedInCart > 0)
                {
                    SendInput(
                        element: "Item added amount",
                        relativeTo: itemAndAmountLoaded.Item1,
                        clearBefore: true,
                        input: (amountLoadedInCart + amountToAdd).ToString()
                    );
                }
                else
                {
                    SearchAndAddNewItemToTheCart(itemToAdd);

                    CheckNewItemLoadedInCart(
                        nameToAdd: nameToAdd,
                        finalAmount: amountLoadedInCart + amountToAdd,
                        isNotLastItem: i < items.Count - 1
                    );
                }

                Logger.Info($"Element --{nameToAdd}-- succesfully loaded in the cart");            
            }
        }

        protected bool PaginationElementMatches(
            IWebElement element,
            string name,
            string SKU,
            string id
        )
        {
            try
            {
                return AttributeValue(element, "data-sku") == SKU &&
                       AttributeValue(element, "data-item-id") == id &&
                       LoadContentFromElement("Item to add name", "waitables", relativeTo: element) == name;
            }
            catch(Exception exc)
            {
                Logger.Error(
                    $"The checking failed so it's assume that is not the correct element. Error: {exc.Message}\n{exc.StackTrace}"
                );
                return false;
            }
        }

        protected void AddMoreAmountToItemInCartClickingButtons(
            IWebElement itemAlreadyLoaded,
            int amountAddedBefore,
            int amountToAdd,
            string name
        )
        {
            Logger.Info("Adding more to an item already loaded in the cart clicking buttons");

            string changeBy1Button = amountToAdd > 0 ? "+" : "-";
            int amountAddedClickingPlus1 = 0;
            int retry = 1;

            // Click the button many times as amountToAdd says
            while(amountAddedClickingPlus1 < amountToAdd & retry <= maxAmountOfRetries)
            {
                // Check if the timeout is reached
                CheckRetryTimeoutLoops(retry);

                // Click the button and wait some time so the amount is updated in the page
                itemAlreadyLoaded = ClickChange1InItemInCart(
                    itemAlreadyLoaded,
                    name,
                    changeBy1Button
                );
                Thread.Sleep(TimeSpan.FromSeconds(sleepAddToItemAddedInCart));

                // Check if amount is correctly updated and return an iwebelement
                // if the current one of the element has to be updated
                (bool, IWebElement) updateInCart = IsAmountUpdatedInCart(
                    itemAlreadyLoaded,
                    amountAddedBefore + amountAddedClickingPlus1 + 1,
                    name
                );
    
                // If it updated well, continue; else add 1 to the retries and later
                // check if the max amount of retries is reached
                if(updateInCart.Item1) { amountAddedClickingPlus1++; retry = 0; }
                else { retry++; }

                itemAlreadyLoaded = updateInCart.Item2;             
            }

            Logger.Info("Finish to add more to an item already loaded in the cart clicking buttons");
        }

        private (IWebElement, int) GetItemAndAmountLoadedInCart(string name)
        {
            Logger.Info($"Checking if {name} was already in the cart");

            // Check if the item was already loaded in the cart and get the amount
            IWebElement itemAlreadyLoaded = ReturnElementIfPresent(
                element: name,
                type: "waitables",
                parameterizedCategory: "Specific item added to cart",
                retries: 0,
                sleep: 0,
                waitElementTimeout: 5
            );

            int amount = 0;

            if(itemAlreadyLoaded is not null)
            {
                amount = int.Parse(LoadAttributeFromElement(
                    element: "Item added amount",
                    type: "writables",
                    relativeTo: itemAlreadyLoaded,
                    attribute: "value",
                    retries: 0,
                    sleep: 0,
                    waitElementTimeout: 5
                ));

                Logger.Info($"{name} was already in the cart with {amount} item(s).");
            }
            else
            {
                Logger.Info($"{name} was not loaded yet in the cart");
            }

            return (itemAlreadyLoaded, amount);
        }

        private void SearchAndAddNewItemToTheCart(ItemToAdd item)
        {
            Logger.Info("Search and adding new item to the cart");

            // Search and add more items to the cart
            IWebElement elementFound = PaginationSearch(
                iterableOfElements: "Order items pagination",
                nextPageElementClickable: "Next page in pagination",
                currentPageWaitable: "Current page in pagination",
                raiseIfNotFound: true,
                checkPageNumber: false,
                elementSearcher: element => 
                    PaginationElementMatches(element, item.name, item.SKU, item.id)
            );
            
            SendInput(
                "Amount to load",
                relativeTo: elementFound,
                input: item.amount.ToString(),
                clearBefore: true
            );

            Click("Add product to cart", relativeTo: elementFound);
        }

        private void GoBackToFirstPage()
        {
            Logger.Info("Returning to first page in pagination");

            int retry = 1;
            while(
                ElementIsPresent(
                    "Previous page in pagination", "clickables",
                    retries: 0, sleep: 0, waitElementTimeout: 5
                )
            )
            {
                GoToFirstPosiblePage();

                CheckRetryTimeoutLoops(retry);  
                retry++;
            }

            Logger.Info("Already returned to the first page in pagination");
        }

        private void GoToFirstPosiblePage()
        {
            // If an exception is raised better not to catch it 
            // so the execution finish here and we can fix the error
            string newPage;
            string oldPage = LoadContentFromElement(
                element: "Current page in pagination",
                type: "waitables"
            );
            Click("First page to click in pagination");

            // Wait until page changes
            for(int retry = 1; retry <= maxAmountOfRetries; retry++)
            {
                newPage = LoadContentFromElement(
                    element: "Current page in pagination",
                    type: "waitables"
                );
                if(newPage != oldPage) { break; }

                CheckRetryTimeoutLoops(retry);
                Thread.Sleep(TimeSpan.FromSeconds(sleepIfNeededInRetries));
            }
        }
                   
        private (bool, IWebElement) IsAmountUpdatedInCart(
            IWebElement element,
            int finalAmount,
            string name
        )
        {
            int amountInPage = -1;

            for(int retry = 1; retry <= maxAmountOfRetries; retry++)
            {
                CheckRetryTimeoutLoops(retry);

                try
                {
                    amountInPage = int.Parse(LoadAttributeFromElement(
                        element: "Item added amount",
                        type: "writables",
                        relativeTo: element,
                        attribute: "value",
                        retries: 0,
                        sleep: 0,
                        waitElementTimeout: 5
                    ));
                    break;
                }
                catch(Exception exc)
                {
                    element = ReloadItemAddedtoCartInException(name, exc);
                }
            }

            Logger.Info($"Current value is {amountInPage}");
            return (amountInPage == finalAmount, element);
        }

        private IWebElement ClickChange1InItemInCart(
            IWebElement itemAlreadyLoaded,
            string name,
            string plusOrMinus
        )
        {
            for(int retry = 1; retry <= maxAmountOfRetries; retry++)
            {
                CheckRetryTimeoutLoops(retry);

                try
                {
                    Click(
                        element: plusOrMinus,
                        parameterizedCategory: "Change by 1 already loaded",
                        relativeTo: itemAlreadyLoaded,
                        retries: 0,
                        sleep: 0,
                        waitElementTimeout: 3
                    );
                    break;
                }
                catch(Exception exc)
                {
                    itemAlreadyLoaded = ReloadItemAddedtoCartInException(name, exc);
                }
            }
            return itemAlreadyLoaded;
        }

        private void CheckNewItemLoadedInCart(
            string nameToAdd,
            int finalAmount,
            bool isNotLastItem
        )
        {
            // Check element already loaded in cart
            IWebElement itemAlreadyLoaded = WaitElementLoad(
                element: nameToAdd,
                parameterizedCategory: "Specific item added to cart",
                retries: 7,
                sleep: 1,
                waitElementTimeout: 14
            );
                    
            for(int retry = 1; retry <= maxAmountOfRetries; retry++)
            {
                if(IsAmountUpdatedInCart(itemAlreadyLoaded, finalAmount, nameToAdd).Item1)
                {
                    break;
                }

                CheckRetryTimeoutLoops(retry);
            }

            // Go back to first page unless it's the last item
            // if(isNotLastItem) { GoBackToFirstPage(); }
        }

        private IWebElement ReloadItemAddedtoCartInException(string name, Exception exc)
        {
            Logger.Error($"Exception waiting for element add to cart: {exc.Message}");
            Logger.Info("Apparently the html was updated, reloading element");                    
            
            IWebElement itemAlreadyLoaded = ReturnElementIfPresent(
                element: name,
                type: "waitables",
                parameterizedCategory: "Specific item added to cart"
            );                    
            
            Logger.Info("Element reloaded");
            return itemAlreadyLoaded;
        }

        private void CheckRetryTimeoutLoops(int retry)
        {            
            if(retry == maxAmountOfRetries)
            {
                throw new Exception("Max amount of retries reached");
            }
        }
    }
}
