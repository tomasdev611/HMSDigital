using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using System;
using System.Threading; // Thread sleep defined here
using System.Collections.Generic; // Dictionary type defined here

namespace WebPortalAutomation
{
    public partial class BasePage
    {
        // in seconds, time the page will load in an infinite scroll
        protected const double InfiniteScrollLoadTimeout = 3.5;
        protected const int maxAmountOfRetriesPagination = 15;
        protected const int sleepPagination = 1;

        public Dictionary<string, object> FindAllIterables(
            string element,
            string parameterizedCategory = null,
            IWebElement relativeTo = null,
            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {
            Logger.Info($"Finding all iterables of --{element}--");

            // Obtain locators
            Dictionary<string, By> locators = GetIterableLocatorFromJson(
                element,
                parameterizedCategory,
                checkRelative: (relativeTo is not null)
            );
            By rootLocator = locators["root"];
            By relativeRootLocator = locators["relative"];
            
            // Get root element
            IWebElement root = FindSingleWithRetries(
                rootLocator,
                relativeTo: relativeTo,
                waiter: ElementIsVisible(),
                retries: retries,
                sleep: sleep,
                waitElementTimeout: waitElementTimeout
            );

            // Find all iterable elements relative to root
            List<IWebElement> iterables = FindManyWithRetries(
                relativeRootLocator,
                relativeTo: root,
                waitElementTimeout: waitElementTimeout
            );

            Logger.Info($"All iterables of --{element}-- founded");

            return new Dictionary<string, object>(){
                { "root", (object)root },
                { "elements" , (object)iterables }
            };
        }

        public Dictionary<string, object> IterateAndCallFunction(
            string element,
            Func<IWebElement, object> mapMethod,
            string parameterizedCategory = null,
            IWebElement relativeTo = null,
            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {
            Logger.Info($"Iterating --{element}-- and calling a method");

            Dictionary<string, object> results = FindAllIterables(
                element,
                parameterizedCategory,
                relativeTo,
                retries,
                sleep,
                waitElementTimeout
            );

            // Iterate through the elements and apply the map method to them
            List<Tuple<IWebElement, object>> elementsExecuted = new List<Tuple<IWebElement, object>>(){};
            
            foreach(IWebElement iterElem in (List<IWebElement>)(results["elements"]))
            {
                elementsExecuted.Add(
                    new Tuple<IWebElement, object>(iterElem, mapMethod(iterElem))
                );
            }

            results["elements"] = (object)elementsExecuted;
            Logger.Info($"Finish iterating --{element}-- and calling a method");

            return results;
        }

        public IWebElement InfiniteScrollSearch(
            string iterableToScroll,
            Func<IWebElement, bool> elementSearcher,
            string parameterizedCategory = null,
            double infiniteSearchLoadTimeout = InfiniteScrollLoadTimeout,
            IWebElement relativeTo = null,
            bool raiseIfNotFound = false,
            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {
            Logger.Info("Iterating through infinity scroll table");

            IJavaScriptExecutor jsdriver = (IJavaScriptExecutor)webdriver;
            IWebElement lastFinalElement = null;
            IWebElement newFinalElement = null;

            // When the last element is repeated, is the end of the scroll
            while(true)
            {              
                try
                {
                    Logger.Info("Obtaining new elements");

                    // Load new elements
                    // THIS SLEEP MUST --> BE HERE <-- THE FIRST TIME THE TABLE IS LOADED
                    // THE ELEMENTS ARE ALSO LOADING AND IF THIS SLEEP IS NOT HERE,
                    // IT WOULD TAKE THE UNLOAD ROWS
                    Thread.Sleep(TimeSpan.FromSeconds(infiniteSearchLoadTimeout));

                    lastFinalElement = newFinalElement;

                    Dictionary<string, object> iterables = FindAllIterables(
                        iterableToScroll,
                        parameterizedCategory,
                        relativeTo,
                        retries,
                        sleep,
                        waitElementTimeout
                    );

                    List<IWebElement> iterableElements = (List<IWebElement>)(iterables["elements"]);

                    int numbElements = iterableElements.Count;                    
                    Logger.Info($"Amount of elements obtained: {numbElements}");
                    if(numbElements == 0) { break; } 

                    // Put the last element as new final
                    newFinalElement = iterableElements[numbElements - 1];
                    // Check if the last element is repeated and stop the loop
                    if(lastFinalElement is not null && lastFinalElement.Equals(newFinalElement))
                    {
                        Logger.Info(
                            "The last element of this iteration is the same that in the previous so the table has no more items."
                        );
                        break;
                    }

                    // Look for the element wanted in the ones loaded
                    foreach(IWebElement iterElem in iterableElements)
                    {
                        if(elementSearcher(iterElem))
                        {
                            Logger.Info($"Stop iterating, element founded: {iterElem}");
                            return iterElem;
                        }
                        else if(SetRowIsLoading(iterElem))
                        {
                            // If element is not loaded, is setted as newFinal so in the next
                            // iteration of the while, it loads
                            Logger.Info("Loading row");
                            newFinalElement = iterElem;
                            break;
                        }
                    }
                    
                    // Scroll Down
                    Logger.Info("Element not found, scroll and go to next iteration");
                    jsdriver.ExecuteScript("arguments[0].scrollIntoView(true);", newFinalElement);
                }
                catch(Exception e)
                {
                    Logger.Info($"Exception raised in an infinity search: {e.Message}\nStack: {e.StackTrace}");
                    break;
                }
            }

            // Not founded
            if(raiseIfNotFound)
            {
                throw new Exception("Stop iterating: element not found in table.");
            }
            else
            {
                Logger.Info("Stop iterating: element not found in table.");
                return null;  
            }
        }

        public IWebElement PaginationSearch(
            string iterableOfElements,
            Func<IWebElement, bool> elementSearcher,
            string nextPageElementClickable,
            string currentPageWaitable,

            string iterableParameterizedCategory = null,
            IWebElement iterableRelativeTo = null,
            bool raiseIfNotFound = false,

            string nextPageParameterizedCategory = null,
            string nextPageTypeClick = "normal",
            bool nextPageScrollDown = true,
            IWebElement nextPageRelativeTo = null,

            string currentPageParameterizedCategory = null,
            IWebElement currentPageRelativeTo = null,
            
            bool checkPageNumber = false,

            int retries = NumberOfRetries,
            int sleep = SleepThread,
            int waitElementTimeout = WaitLoadElementTimeout
        )
        {
            Logger.Info("Starting pagination search");
            int actualPage = 0;

            try
            {
                do
                {
                    actualPage++;

                    Dictionary<string, object> iterables = FindAllIterables(
                        iterableOfElements,
                        iterableParameterizedCategory,
                        iterableRelativeTo,
                        retries,
                        sleep,
                        waitElementTimeout
                    );
                    List<IWebElement> iterableElements = (List<IWebElement>)(iterables["elements"]);

                    int numbElements = iterableElements.Count;                    
                    Logger.Info($"Amount of elements obtained: {numbElements}");
                    if(numbElements == 0) { break; } 

                    // Look for the element wanted in the page
                    foreach(IWebElement iterElem in iterableElements)
                    {
                        if(elementSearcher(iterElem))
                        {
                            Logger.Info($"Element matched in pagination: {iterElem}");
                            return iterElem;
                        }
                        else
                        {
                            Logger.Info("Element not matched");
                        }
                    }  
                }
                while(
                    CanGoToTheNextPage(
                        nextPageElementClickable,
                        nextPageParameterizedCategory,
                        nextPageTypeClick,
                        nextPageScrollDown,
                        nextPageRelativeTo,
                        currentPageWaitable,
                        currentPageParameterizedCategory,
                        currentPageRelativeTo,
                        actualPage,
                        checkPageNumber,
                        retries,
                        sleep,
                        waitElementTimeout
                    )
                );
            }
            catch(Exception e)
            {
                Logger.Info($"Exception raised in pagination search: {e.Message}\nStack: {e.StackTrace}");
            }

            // Not founded
            if(raiseIfNotFound)
            {
                throw new Exception("Element not found in pagination.");
            }
            else
            {
                Logger.Info("Element not found in pagination.");
                return null;  
            }
        }

        protected bool SetRowIsLoading(IWebElement element)
        {
            Logger.Info($"Checking if --{element}-- is loading");

            try
            {
                // Only try 1 attemp and wait 2 seconds finding the element
                // the row is already loaded, the waiting to load is made before, if is not
                // loaded now, then probably not gonna load until the page be scrolled again
                IWebElement loadBox = WaitElementLoad(
                    "Loading Table Rows",
                    relativeTo: element,
                    retries: 1,
                    waitElementTimeout: 2
                );

                bool isLoading = AttributeValue(loadBox, "class") == "loading-text";
                string isLoadingString = isLoading ? "Yes" : "No";
                Logger.Info($"The element is loading? {isLoadingString}");
                
                return isLoading;
            }
            catch(Exception e)
            {
                Logger.Info($"Exception checking if it was not a loader: : {e.Message}\nStack: {e.StackTrace}");
                return false;
            }  
        }
        
        private bool CanGoToTheNextPage(
            string nextPageElementClickable,
            string nextPageParameterizedCategory,
            string nextPageTypeClick,
            bool nextPageScrollDown,
            IWebElement nextPageRelativeTo,
            
            string currentPageWaitable,
            string currentPageParameterizedCategory,
            IWebElement currentPageRelativeTo,
            
            int actualPage,
            bool checkPageNumber,
            int retries,
            int sleep,
            int waitElementTimeout
        )
        {
            try
            {
                Logger.Info("Going to the next page in pagination");

                int actualPageFromWeb = int.Parse(
                    LoadContentFromElement(
                        currentPageWaitable,
                        "waitables",
                        parameterizedCategory: currentPageParameterizedCategory,
                        relativeTo: currentPageRelativeTo,
                        retries: retries,
                        sleep: sleep,
                        waitElementTimeout: waitElementTimeout
                    )
                );

                if(actualPageFromWeb != actualPage & checkPageNumber)
                {
                    Logger.Error(
                        $"Navigation through pages failed: the expected page ({actualPage}) is diferent to the current one ({actualPageFromWeb})"
                    );
                    return false;
                }

                Click(
                    nextPageElementClickable,
                    nextPageParameterizedCategory,
                    nextPageTypeClick,
                    nextPageScrollDown,
                    nextPageRelativeTo,
                    retries,
                    sleep,
                    waitElementTimeout
                );

                WaitUntilNextPageInPaginationIsLoaded(
                    currentPageWaitable,
                    currentPageParameterizedCategory,
                    currentPageRelativeTo,
                    retries,
                    sleep,
                    waitElementTimeout,
                    actualPageFromWeb
                );

                Logger.Info("Allready went to next page in pagination");
                return true;
            }
            catch(Exception e)
            {
                Logger.Info($"Exception going to the next page: : {e.Message}\nStack: {e.StackTrace}");
                return false;
            }
        }

        private void WaitUntilNextPageInPaginationIsLoaded(
            string currentPageWaitable,
            string currentPageParameterizedCategory,
            IWebElement currentPageRelativeTo,
            int retries,
            int sleep,
            int waitElementTimeout,
            int actualPage
        )
        {
            int nextPageActualNumber = actualPage + 1;

            for(int retry = 0; retry < maxAmountOfRetriesPagination; retry++)
            {
                int actualPageFromWeb = int.Parse(
                    LoadContentFromElement(
                        currentPageWaitable,
                        "waitables",
                        parameterizedCategory: currentPageParameterizedCategory,
                        relativeTo: currentPageRelativeTo,
                        retries: retries,
                        sleep: sleep,
                        waitElementTimeout: waitElementTimeout
                    )
                );

                if(actualPageFromWeb == nextPageActualNumber) { return; }

                Thread.Sleep(TimeSpan.FromSeconds(sleepPagination));
            }

            throw new Exception("Timeout waiting next page in pagination");
        }
    }
}
