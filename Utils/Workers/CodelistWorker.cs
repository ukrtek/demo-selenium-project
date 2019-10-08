using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Company.Project.App.AutomatedTests.Models;
using OpenQA.Selenium.Interactions;

namespace Company.Project.App.AutomatedTests.Utils.Workers
{
    public class CodelistWorker
    {
        private RemoteWebDriver _driver;
        private ElementHelper _elements;
        private Vocabulary _vocabulary;

        private CodeList _codelist;

        private WebDriverWait _waiter;

        private bool isAfterAdd;
        public bool GroupsOn;

        public int TotalCodes;

        public CodelistWorker(RemoteWebDriver driver)
        {
            _driver = driver;

            _elements = new ElementHelper(driver);
            _codelist = new CodeList();
            _waiter = new WebDriverWait(_driver,TimeSpan.FromSeconds(15));
        }
        
        public string getCodelistNameFromBreadcrumb()
        {
            return _elements.FindElement("breadcrumb").Text;
        }

        public void makeGlobalSearch(string criteria)
        {
            var searchInput = _elements.GetVisibleElement("globalSearchInput");
            searchInput.Click();
            searchInput.SendKeys(criteria);
            _elements.FindElement("globalSearchButton").Click();
        }
    
        //wishlist: pass column name to params so that no need to hardcode column name in the locator
        public int MakeColumnSearch(string criteria)
        {
            var searchInput = _elements.GetVisibleElement("sourceNameColumnSearchInput");
            searchInput.Click();
            searchInput.SendKeys(criteria);
            searchInput.SendKeys(Keys.Return);
            return GetTotalSearchResultsCount();
        }

        public void ClearSearchWhenAllResultsAdded()
                 {
                     _elements.FindElement("clear-search-results-button").Click();
                 }

        //to do - get number of selected codes and store it in a variable; after adding, return it
        public int selectAndAdd1Code() 
        {
            _elements.FindElement("rowInTable").Click();
            _elements.FindElement("addSelectedButton").Click();
            return 1;
        }

        // adds all results and return code count displayed in the codelist header after add
        public void addAllResults()
        {
            //find and click on 'Add all' button
            _elements.FindElement("look-up-table-action-button-add-all").Click();
            
            //clear search input after search was made
            isAfterAdd = true;
            
            TotalCodes = GetTotalCodeCount();
            
            ClearSearchWhenAllResultsAdded();
        }

        //returns number of codes in codelist (displayed in the header)
        public int GetTotalCodeCount()
        {
            if (isAfterAdd)
            {
                _waiter.Until(ExpectedConditions.ElementIsVisible(By.ClassName("notification-success")));
 
            }

            return ConvertStringFromElementToInt("codesCount"); 
        }
        
        //todo: this method works only for single-group scenario - tbu
        public int GetCodeCountForGroup()
        {
            return ConvertStringFromElementToInt("total-codes-count-for-group");
        }

        private int GetTotalSearchResultsCount()
        {
            return ConvertSubstringFromElementToInt("searchResultsCount", 15);
        }
        
        private int ConvertStringFromElementToInt(string element)
        {
            return Convert.ToInt32(_elements.FindElement(element).Text);
        }
        
        private int ConvertSubstringFromElementToInt(string element, int startPosition)
        {
            return Convert.ToInt32(_elements.FindElement(element).Text.Substring(startPosition));
        }
        

        private void SearchAndAddResults(string query)
        {
            
        }

        public void OpenSettingsModal()
        {
            _elements.FindElement("navigation-bar-setting-button").Click();
        }

        public void IsGroupEnabled()
        {
//            if (_elements.FindElement("groups-list-add-group-button").Displayed)
//            {
//                GroupsOn = true;
//            }
//            try
//            {
//                new WebDriverWait()
//            }
//            catch (NoSuchElementException ex)
//            {
//                Console.WriteLine(ex);
//                GroupsOn = false;
//            }

            //if element was found, GroupsOn = true
            // else GroupsOn = false

            try
            {
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;
                if (_elements.FindElement("groups-list-add-group-button").Displayed)
                {
                    GroupsOn = true;
                }
            }
            catch (Exception e)
            {
                GroupsOn = false;
            }
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(90);
        }

        public void SetGroupsStateAsRequired(bool requiredGroupsState)
        {
            if (GroupsOn != requiredGroupsState)
            {
                _elements.FindElement("groups-off-on-toggle").Click();
                _elements.FindElement("settings-modal-apply-button").Click();
                GroupsOn = requiredGroupsState;
            }
        }
        

        //todo !!!!!!!!!!!!! 
        public bool globalSearchVerifyResults (string criteria)
        {
            // var searchInput = _elements.GetVisibleElement("globalSearch");
            // searchInput.Click();
            // searchInput.SendKeys(criteria);
            return true;
        }
        
        //todo
        public void selectCodes(int number) 
        {

        }
    }
}