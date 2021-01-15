using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hpstoretest
{
    class Login : Init
    {
        private string email = "dusnen@gmail.com";

        [Test]
        [Order(2)]
        public void login() {
            IWebElement dropDown = driver.FindElements(By.ClassName("downDisclosure"))[0];
            dropDown.Click();
            IWebElement signInButton = driver.FindElement(By.XPath("//*[@id='utilityNav']/div/nav/section/ul/li[6]/ul/li[1]/a"));
            signInButton.Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            IWebElement emailInput = driver.FindElement(By.Id("username"));
            emailInput.SendKeys(email);
            IWebElement nextButton = driver.FindElement(By.Id("next_button"));
            nextButton.Click();
            Thread.Sleep(2000);
            
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                string failedLoginText = driver.FindElements(By.ClassName("vn-form-field__error-message"))[0].Text;
                if (failedLoginText.Contains("not found"))
                {
                    Assert.Pass("Success. Can not login with wrong credentials");
                    
                }
                else
                {
                    Assert.Fail("Wrong message has been shown");
                    
                }
            
           
            
            
        }
        
    }
}
