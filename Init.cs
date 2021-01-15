using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hpstoretest
{
    class Init
    {
        #region Fields
        protected IWebDriver driver;
        protected string url;
        
        #endregion

        [OneTimeSetUp]
        public void setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("start-maximized");
            driver = new ChromeDriver(options);
            url = "https://store.hp.com";
        }

        [Test]
        [Order(1)]
        public void init()
        {
            driver.Url = url;
            Thread.Sleep(2000);
            PolicyHandler.closeCookieModal(driver);
        }
    }
}