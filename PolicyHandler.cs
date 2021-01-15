using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hpstoretest
{
    public static class PolicyHandler
    {
        public static void closeCookieModal(IWebDriver driver)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            IWebElement closeAccept = driver.FindElement(By.Id("onetrust-accept-btn-handler"));
            closeAccept.Click();
        }
    }
}
