using Company.Project.App.AutomatedTests.Utils;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;
using System.Diagnostics;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Company.Project.App.AutomatedTests.Models;
using Company.Project.App.AutomatedTests.Utils.WebDriverBuilder;
using Company.Project.App.AutomatedTests.Utils.Workers;

namespace Company.Project.App.AutomatedTests
{
    [TestFixture]
    public class CodelistV2
    {
        private RemoteWebDriver _driver { get; set; }

        private string _codelistName { get; set; }

        private PlatformWorker _platformWorker;

        private CodelistWorker _codelistWorker;

        //values for column (source name, standard name columns) and global search in OMOP vocabulary
        private const String TEXTSEARCH_QUERY_1 = "sickle cell";
        private const String TEXTSEARCH_QUERY_2 = "iron deficiency anemia";

        [OneTimeSetUp]
        public void init()
        {
            //kill all Google Chrome processes - windows only
            /*foreach (var processToKill in new[] { "chromedriver", "chrome" })
                foreach (var process in System.Diagnostics.Process.GetProcessesByName(processToKill))
                {
                    try
                    {
                        var processDescription = FileVersionInfo.GetVersionInfo(process.MainModule.FileName);
                        if (processDescription.ProductName.Equals("Google Chrome") || process.ProcessName.Equals("chromedriver"))
                        {
                            process.Kill();
                        }
                    }
                    // correct???
                    catch (System.Exception ex)
                    {
                        Trace.WriteLine("access denied");
                    }
                }*/

            var driverOption = ConfigProvider.GetFromSection<DriverOption>("driverOption");
            var codelist = ConfigProvider.GetFromSection<CodeList>("codelist");
            _codelistName = codelist.Name;

            if (codelist.UseExistingCodelist)
            {
                driverOption.StartUrl = codelist.ExistingCodelistURL;
            }

            _driver = WebDriverBuilder.BuildDriver(driverOption);
            var user = ConfigProvider.GetFromSection<TestUser>("testUser");

            _codelistWorker = new CodelistWorker(_driver);
            _platformWorker = new PlatformWorker(_driver);
            _platformWorker.LoginToPlatform(user);

            if (!codelist.UseExistingCodelist)
            {
                _platformWorker.NavigateTo("App", "View Grouped Codelists");
                _platformWorker.OpenTile("Marias Automated Tests");

                _platformWorker.OpenCreateCodelistModal();
                _codelistName = _platformWorker.FillAndSubmitNewCodelistForm(codelist);
            }
        }

        [Test]
        public void createCodelist()
        {
            var codelistNameFromBreadcrumb = _codelistWorker.getCodelistNameFromBreadcrumb();
            Assert.AreEqual(_codelistName, codelistNameFromBreadcrumb,
            $"codelist name from breadcrumb {codelistNameFromBreadcrumb} does not match name of codelist created {_codelistName}");
        }

        [Test]
        public void addCodeToFlatCodelistFromGlobalSearchSelect()
        //
        {
                _codelistWorker.makeGlobalSearch(TEXTSEARCH_QUERY_1); 
                var codesCount = _codelistWorker.selectAndAdd1Code();
                var countHeader = _codelistWorker.GetTotalCodeCount();
                Assert.AreEqual(codesCount,countHeader);
        }

        [Test] 
        //pre-condition of this test is that the codelist is empty
        public void addAllCodesFromColumnSearch()
        {
            //make search in Source Name column and assign search results count to a variable
            int searchResultsCount = _codelistWorker.MakeColumnSearch(TEXTSEARCH_QUERY_2);
            
            //compare search results number with codes number displayed in the header after adding the results
            Assert.AreEqual(searchResultsCount, _codelistWorker.TotalCodes);
            
            //clear search
            _codelistWorker.ClearSearchWhenAllResultsAdded();
        }
        
        

        [Test]
        public void enableGroups()
        {
            //pre-condition 1: groups disabled - this is default by now, but we might need to check this condition
            //pre-condition 2: codes were added to codelist
            _codelistWorker.makeGlobalSearch(TEXTSEARCH_QUERY_1);
            _codelistWorker.addAllResults();

            //actions:
            
            //find out groups current state
            _codelistWorker.IsGroupEnabled();
            
            //open Codelist Settings modal
            _codelistWorker.OpenSettingsModal();
            
            //verify groups are disabled
            _codelistWorker.SetGroupsStateAsRequired(false);
            
            //enable groups
            _codelistWorker.SetGroupsStateAsRequired(true);
            
            //verify groups are on
            Assert.IsTrue(_codelistWorker.GroupsOn);
            
            //verify code count in group is the same as codelist total value
            Assert.AreEqual(_codelistWorker.GetCodeCountForGroup(), _codelistWorker.GetTotalCodeCount());
        }

        [Test]
        public void addGroup()
        {
            //pre-condition 1: groups enabled
            
            //actions:
            //click 'add group' button 
            //type in a name for the new group
            //hit [Enter] key
            //verify new group displayed in the group list
            //verify the group has the name entered by user

        }
        
        

        [Test]
        public void addToExistingGroupFromColumnSearch()
        {
            //pre-condition 1: groups enabled
            //pre-condition 2: add a new group (there should be an empty group in the codelist
            
            //actions:
            //switch to Browse Codes
            //clear searches if there are any uncleared 
            //make column search in any column 
            //save search results count
            //click add all
            //verify groups modal was opened
            //click on the new empty group in the list
            //click 'Add' button
            //verify codes count in the group is the same as search result count saved

        }

        [Test]
        public void addToNewGroupFromEqualsColumnSearch()
            //pre-condition 1: groups enabled
            //...
        
            //actions:
            //...

        {
            
        }
        
        [Test]
        public void deleteGroup()
        {

        }
        
        [Test]
        public void disableGroups()
        {
            
        }
        
        [Test]
        public void moveFromOneGroupToAnotherCodesFromSelection()
        {
            
        }

        [Test]
        public void deleteCodesFromGroupBySearch()
        {
        }

        [Test]
        public void RenameCodelist()
        {
        }


        [Test]
        public void RenameGroup()
        {
        }

        [Test]
        public void SwitchFromMyCodelistToBrowseCodes()
        {
            
        }

        [Test]
        public void ChangeTableColumnsSelection()
        {
            
        }

        [Test]
        public void sortTableByColumn ()
        {
            
        }


        [OneTimeTearDown]
        public void Cleanup()
        {
            _driver.Quit();
        }
    }
}