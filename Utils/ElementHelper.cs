using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company.Project.App.AutomatedTests.Utils
{
    public class ElementHelper
    {
		private RemoteWebDriver _driver;
		private Dictionary<string, string> _elements;
		public ElementHelper(RemoteWebDriver driver)
		{
			_driver = driver;
			_elements = ConfigProvider.GetFromSection<Dictionary<string, string>>("elements");
		}

		private KeyValuePair<string,string> getLocatorData(string constantName)
		{
			var elementData = _elements[constantName].Split(':');
			return new KeyValuePair<string, string>(elementData[0], string.Join(":", elementData.Skip(1)));
		}

		public IWebElement GetVisibleElement(string constantName, int timeOut = 60)
		{
			var data = getLocatorData(constantName);
			return GetVisibleElement(data.Key, data.Value, timeOut);
		}


		public IWebElement GetVisibleElement(string locaterType, string locaterValue, int timeOut = 60)
		{	
			var waiter = new WebDriverWait(_driver, TimeSpan.FromSeconds(60));

			switch (locaterType.ToLower())
			{
				case "id":
					return waiter.Until(ExpectedConditions.ElementToBeClickable(By.Id(locaterValue)));

				case "css":
					return waiter.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(locaterValue)));

				case "xpath":
					return waiter.Until(ExpectedConditions.ElementToBeClickable(By.XPath(locaterValue)));

				case "name":
					return waiter.Until(ExpectedConditions.ElementToBeClickable(By.Name(locaterValue)));
			}

			throw new Exception("locater type was not found");
		}

		public IWebElement FindElement(string constantName)
		{
			var data = getLocatorData(constantName);
			return FindElement(data.Key, data.Value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="locatorType"></param>
		/// <param name="locatorValue"></param>
		/// <returns></returns>
		public IWebElement FindElement(string locatorType, string locatorValue)	
		{
			switch (locatorType.ToLower())
			{
				case "id":
					return _driver.FindElement(By.Id(locatorValue));

				case "css":
					return _driver.FindElement(By.CssSelector(locatorValue));

				case "xpath":
					return _driver.FindElement(By.XPath(locatorValue));

				case "name":
					return _driver.FindElement(By.Name(locatorValue));
			}

			throw new Exception("locater type was not found");
		}


		public IEnumerable<IWebElement> FindElements(string constantName)
		{
			var data = getLocatorData(constantName);
			return FindElements(data.Key, data.Value);
		}

		// todo - ask Oleksii to explain this
		public IEnumerable<IWebElement> FindElements(string locaterType, string locaterValue)
		{
			switch (locaterType.ToLower())
			{
				case "id":
					return _driver.FindElements(By.Id(locaterValue));

				case "xpath":
					return _driver.FindElements(By.XPath(locaterValue));

				case "css":
					return  _driver.FindElements(By.CssSelector(locaterValue));

				case "name":
					return _driver.FindElements(By.Name(locaterValue));

			}

			throw new Exception("locater type was not found");
		}
	}
}
