using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Text;

namespace Company.Project.App.AutomatedTests.Utils.WebDriverBuilder
{
    public static class WebDriverBuilder
    {
		private static RemoteWebDriver GetChromeDriver(DriverOption option)
		{
			var chromeOptions = new ChromeOptions();
			chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
			chromeOptions.AddArgument("no-sandbox");
			chromeOptions.AddArguments("start-maximized");
			
			var driver = new ChromeDriver(option.DriverPath, chromeOptions);

			driver.Navigate().GoToUrl(option.StartUrl);
		
			driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(option.TimeOutPageLoad);
			driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(option.TimeOutImplicitWait);

			return driver;
		}

		private static RemoteWebDriver GetIEDriver (DriverOption option)
		{
			var driver = new InternetExplorerDriver(option.DriverPath);
			driver.Navigate().GoToUrl(option.StartUrl);

			driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(option.TimeOutPageLoad);
			driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(option.TimeOutImplicitWait);

			return driver;
		}

		public static RemoteWebDriver BuildDriver(DriverOption option)
		{
			switch (option.Browser)
			{
				case Browsers.Chrome:
					return GetChromeDriver(option);

				case Browsers.IE:
					return GetIEDriver(option);

				default:
					throw new Exception("Driver is not supported");
			}
		}



	}
}
