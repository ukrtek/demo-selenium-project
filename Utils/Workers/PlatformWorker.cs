using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Company.Project.App.AutomatedTests.Models;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Company.Project.App.AutomatedTests.Utils.Workers
{
    public class PlatformWorker
    {
        private RemoteWebDriver _driver;
        private ElementHelper _elements;

        private CodeList _codelist;

        public PlatformWorker(RemoteWebDriver driver)
        {
            _driver = driver;
            _elements = new ElementHelper(driver);
        }

        /// <summary>
        ///  Login user to the platform 
        /// </summary>
        /// <param name="user"></param>
        public void LoginToPlatform(TestUser user)
        {
            _elements.GetVisibleElement("logUserName").SendKeys(user.Login);
            _elements.GetVisibleElement("logUserPassword").SendKeys(user.Password);
            _elements.GetVisibleElement("logLoginButton").Click();

            // set up memory phrase letters
            for (int i = 1; i < 4; i++)
            {
                var firstCharNumber = Convert.ToInt32(_elements.FindElement("logCharacter" + i).Text.Substring(10, 1));
                var firstChar = user.Phrase.Substring(firstCharNumber - 1, 1);
                var select = new SelectElement(_elements.FindElement("logCharacterEdit" + i));
                select.SelectByValue(firstChar.ToUpper());
            }

            _elements.FindElement("logLoginButton2").Click();
        }

        //todo - replace hardcoded locators 
        public void OpenCreateCodelistModal()
        {
            //var vocabularyName = ConfigProvider.GetFromSection<CodeList>("codelist");
            _elements.GetVisibleElement("createButtonWorkspace").Click();
            _elements.GetVisibleElement("groupedCodelistItem").Click();
            //this.SelectVocabulary(string(vocabularyName));

        }

        public string FillAndSubmitNewCodelistForm(CodeList codeList)
        {
            _elements.FindElement("vocabularyDropdown").Click();
            var vocabulary = ConfigProvider.GetFromSection<Vocabulary>("vocabulary");
            _elements.FindElement(vocabulary.LocatorType, vocabulary.SelectorValue).Click();
            if (!vocabulary.DataType.Equals("OMOP"))
            {
                var select = new SelectElement(_elements.FindElement("codelistTypeDropdown"));
                            select.SelectByText(codeList.Type);
            }

            var random = new Random();
            var codelistName = $"TestCodelist {random.Next(1, 9999)}";
            _elements.FindElement("nameInput").SendKeys(codelistName);
            _elements.FindElement("notesInput").SendKeys(codeList.Notes);

            if (codeList.IsGroupsEnabled)
            {
                _elements.FindElement("groupsOnOffToggle").Click();
            }

            _elements.FindElement("locationPickerButton").Click();
            _elements.FindElement("locationPickerTreeItem").Click();
            //locationPickerButton.Click();
            _elements.FindElement("createButtonCreateCodelistModal").Click();

            return codelistName;
        }

        public void OpenTile(string tileName)
        {
            _elements.FindElements("cardTile").First(x => x.Text.Trim() == tileName).Click();
        }

        /// <summary>
        /// selects an entry and subentry from menu		
        /// </summary>
        /// <param name="title"></param>
        /// <param name="subtitle"></param>
        public void NavigateTo(string title, string subtitle)
        {
            var hamburger = _elements.GetVisibleElement("hamburgerIcon");
            if (hamburger.Displayed)
                hamburger.Click();

            try
            {
                _elements.FindElements("hamburgerTitle")
                    .FirstOrDefault(x => x.Text.Trim() == title).Click();

                _elements.FindElements("hamburgerSubtitle")
                    .FirstOrDefault(x => x.Displayed && x.Text.Trim() == subtitle).Click();
            }
            catch (Exception)
            {
                Trace.WriteLine("element was not found");
            }
        }
    }
}
