using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

using System;
using System.Collections.Generic; // Dictionary type defined here

namespace WebPortalAutomation
{
    public partial class BasePage
    {

        private By GetGenericLocatorFromJson(
            string element,
            string type,
            string option,
            string category,
            bool checkRelative = false 
        )
        {
            By locator;

            if(option is not null)
            {
                switch(type)
                {
                    case("selectables"):
                        locator = GetSelectableLocatorFromJson(
                            element,
                            option,
                            random: false,
                            checkRelative: checkRelative
                        )["option"];

                        break;
                    default:
                        throw new Exception("option parameter is valid only with type 'selectables'");
                }
            }
            else
            {
                locator = GetSimpleLocatorFromJson(element, type, category, checkRelative: checkRelative);
            }

            return locator;
        }

        // Check if the element is in the json of the current page, and if not, check if
        // is in the generic page. If is not anywhere, raise an error.
        private By GetSimpleLocatorFromJson(
            string element,
            string type,
            string category,
            bool checkRelative = false
        )
        {
            By locator = SimpleLocator(element, type, category, this.elements, checkRelative) ?? 
                            SimpleLocator(element, type, category, this.genericElements, checkRelative);

            string catError = $"Category: {((category is null) ? "no category defined." : category)}";

            return (By)SanitizeLocator((object)locator, element, type, catError);
        }

        private By SimpleLocator(
            string element,
            string type,
            string category,
            Dictionary<string, object> webElementJson,
            bool checkRelative
        )
        {
            if(!webElementJson.ContainsKey(type)) { return null; }
            Dictionary<string, object> elementGroup = (Dictionary<string, object>)webElementJson[type];
            
            if(category is not null)
            {
                if(!elementGroup.ContainsKey("parameterized")) { return null; }
                Dictionary<string, object> parameterized = (Dictionary<string, object>)elementGroup["parameterized"];
                
                if(!parameterized.ContainsKey(category)) { return null; }
                Dictionary<string, object> categoryJson = (Dictionary<string, object>)parameterized[category];

                if(checkRelative &&
                    !(categoryJson.ContainsKey("shouldBeRelative") && (bool)categoryJson["shouldBeRelative"]))
                {
                    throw new Exception($"Element {element} should be used only as a relative");
                }

                string placeholder = (string)categoryJson["placeholder"];
                string xpath = (string)categoryJson["xpath"];
                return GenerateLocator("xpath", xpath.Replace(placeholder, element));
            }

            // Check if the element is defined, else try to elaborate a parameterized xpath
            if(elementGroup.ContainsKey("uniques"))
            {   
                Dictionary<string, object> uniques = (Dictionary<string, object>)elementGroup["uniques"];

                if(uniques.ContainsKey(element))
                {
                    Dictionary<string, object> elementJson = (Dictionary<string, object>)uniques[element];
            
                    if(checkRelative &&
                        !(elementJson.ContainsKey("shouldBeRelative") && (bool)elementJson["shouldBeRelative"]))
                    {
                        throw new Exception($"Element {element} should be used only as a relative");
                    }

                    string findBy = (string)elementJson["findBy"];
                    string finder = (string)elementJson[findBy];

                    return GenerateLocator(findBy, finder);
                }
            }
            
            // Check if the element is defined in the parameterized xpaths
            if(elementGroup.ContainsKey("mapped"))
            {
                return GetLocatorFromMapedXPath(elementGroup, element, checkRelative);   
            }

            return null;           
        }

        // Check if the element is in the json of the current page, and if not, check if
        // is in the generic page. If is not anywhere, raise an error.
        private Dictionary<string, By> GetSelectableLocatorFromJson(
            string element,
            string option,
            bool random,
            bool checkRelative = false
        )
        {
            Dictionary<string, By> locators = SelectableLocator(element, option, random, this.elements, checkRelative) ?? 
                                                SelectableLocator(element, option, random, this.genericElements, checkRelative);

            return (Dictionary<string, By>)SanitizeLocator((object)locators, element, "selectables");
        }

        private Dictionary<string, By> SelectableLocator(
            string element,
            string option,
            bool random,
            Dictionary<string, object> webElementJson,
            bool checkRelative
        )
        {
            Dictionary<string, object> results = LocatorAndDictOfSelectablesAndIterables(element, "selectables", webElementJson, checkRelative);
            if(results is null) { return null; }

            Dictionary<string, object> elementJson = (Dictionary<string, object>)results["elementJson"];
            Dictionary<string, object> optionJson = (Dictionary<string, object>)elementJson["options"];
            List<object> optionValues = (List<object>)optionJson["values"];

            if(random) 
            {
                option = (string)(RandomUtils.randomListElement(optionValues));
            }
            else if(!optionValues.Contains(option))
            {
                Logger.Warning($"Option {option} is not in the 'values' key.");  
            }
            
            string replace = (string)optionJson["placeholder"];
            string finderOption = ((string)optionJson["xpath"]).Replace(replace, option);

            return new Dictionary<string, By>(){
                { "menu", (By)results["locator"] },
                { "option", GenerateLocator("xpath", finderOption) }
            };
        }

        // Check if the element is in the json of the current page, and if not, check if
        // is in the generic page. If is not anywhere, raise an error.
        private Dictionary<string, By> GetIterableLocatorFromJson(
            string element,
            string parameterizedCategory,
            bool checkRelative = false
        )
        {
            Dictionary<string, By> locators = IterableLocator(element, this.elements, checkRelative) ?? 
                                                IterableLocator(element, this.genericElements, checkRelative);

            return (Dictionary<string, By>)SanitizeLocator((object)locators, element, "iterables");
        }

        private Dictionary<string, By> IterableLocator(
            string element,
            Dictionary<string, object> webElementJson,
            bool checkRelative
        )
        {
            Dictionary<string, object> results = LocatorAndDictOfSelectablesAndIterables(element, "iterables", webElementJson, checkRelative);
            if(results is null) { return null; }

            Dictionary<string, object> elementJson = (Dictionary<string, object>)results["elementJson"];
            
            Dictionary<string, object> relatives = (Dictionary<string, object>)elementJson["relatives"];
            string relativeFindBy = (string)relatives["findBy"];
            string relativeFinder = (string)relatives[relativeFindBy];

            return new Dictionary<string, By>(){
                { "root", (By)results["locator"] },
                { "relative", GenerateLocator(relativeFindBy, relativeFinder) }
            };
        }

        private Dictionary<string, object> LocatorAndDictOfSelectablesAndIterables(
            string element,
            string type,
            Dictionary<string, object> webElementJson,
            bool checkRelative
        )
        {
            if(!webElementJson.ContainsKey(type)) { return null; }
            Dictionary<string, object> elementGroup = (Dictionary<string, object>)webElementJson[type];
            
            // Currently only uniques iterables/selectables allowed, not sure if another type is useful to add here
            Dictionary<string, object> uniquesWebElements = (Dictionary<string, object>)elementGroup["uniques"];

            if(!uniquesWebElements.ContainsKey(element)) { return null; }
            Dictionary<string, object> elementJson = (Dictionary<string, object>)uniquesWebElements[element];
            
            if(checkRelative &&
                !(elementJson.ContainsKey("shouldBeRelative") && (bool)elementJson["shouldBeRelative"]))
            {
                throw new Exception($"Element {element} should be used only as a relative");
            }

            string findBy = (string)elementJson["findBy"];
            string finder = (string)elementJson[findBy];

            return new Dictionary<string, object>(){
                { "locator", GenerateLocator(findBy, finder) },
                { "elementJson", elementJson }
            };
        }

        private object SanitizeLocator(
            object locator,
            string element,
            string type,
            string extraError = ""
        )
        {
            if(locator is null)
            {
                throw new Exception(
                    $"The element {element} of type {type} was not found in the page json " +
                    $"nor the generic json. Check the name passed. {extraError}"
                );
            }

            return locator;
        }

        private By GetLocatorFromMapedXPath(Dictionary<string, object> elementGroup, string element, bool checkRelative = false)
        {
            // Iterate through diferent xpaths mapped
            List<object> mapped = (List<object>)elementGroup["mapped"];
            foreach(object mappedXPath in mapped)
            {
                Dictionary<string, object> mapCasted = (Dictionary<string, object>)mappedXPath;
                string xpath = (string)mapCasted["xpath"];

                // Get the replacements of the specific element
                Dictionary<string, object> maps = (Dictionary<string, object>)mapCasted["map"];
                
                if(!maps.ContainsKey(element)) { continue; }

                if(checkRelative &&
                    !(mapCasted.ContainsKey("shouldBeRelative") && (bool)mapCasted["shouldBeRelative"]))
                {
                    throw new Exception($"Element {element} should be used only as a relative");
                }

                Dictionary<string, object> elementReplaces = (Dictionary<string, object>)maps[element];
                
                // Replace the placeholder "replace.Key" in the xpath for the correct value "replace.Value"
                foreach(KeyValuePair<string, object> replace in elementReplaces)
                {
                    xpath = xpath.Replace(replace.Key, (string)replace.Value);
                }

                return GenerateLocator("xpath", xpath);
            }

            return null;
        }

        private By GenerateLocator(string findBy, string finder)
        {
            switch(findBy.ToLower())
            {
                case "id": return By.Id(finder);
                case "xpath": return By.XPath(finder);
                case "name": return By.Name(finder);
                case "class name": return By.ClassName(finder);
                case "css selector": return By.CssSelector(finder);
                case "link text": return By.LinkText(finder);
                case "partial link text": return By.PartialLinkText(finder);
                case "tag name": return By.TagName(finder);
                default: throw new Exception("unknown findby type");
            }
        }

    }
}
