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
    class HpCart :Init
    {

        private char currency = '$';
        [Test]
        [Order(2)]
        public void addingProduct()
        {
            driver.Navigate().GoToUrl("https://store.hp.com/us/en/slp/weekly-deals/monitors");
            IWebElement[] products = driver.FindElement(By.XPath("//*[@id='root']/div/div[3]/div[5]/div/div/div[2]/div/div[2]/div")).FindElements(By.ClassName("cache-add-container")).ToArray();
            IWebElement product1 = products[0];
            IWebElement product2 = products[1];

            product1.FindElements(By.TagName("button"))[1].Click();
            Thread.Sleep(2000);
            product2.FindElements(By.TagName("button"))[1].Click();
            Thread.Sleep(3000);
            driver.Navigate().GoToUrl("https://store.hp.com/us/en/AjaxOrderItemDisplayView");

        }
        [Test]
        [Order(3)]
        public void checkTotalIsOK()
        {
            
            IWebElement[] products = driver.FindElements(By.ClassName("productrow")).ToArray();
            //Indexes 0,1 and 2 are reserved for static data
            IWebElement product1 = products[3];
            IWebElement product2 = products[4];

           string priceOne =  product1.FindElements(By.ClassName("product-price-tab"))[0].Text;
           string priceTwo = product2.FindElements(By.ClassName("product-price-tab"))[0].Text;
           string subTotal = driver.FindElement(By.Id("cartSubtotal")).Text;
            double price1 = removeCurrency(priceOne);
            double price2 = removeCurrency(priceTwo);
            double totalPrice = removeCurrency(subTotal);
           
            if(price1 + price2 == totalPrice)
            {
                Assert.Pass("Success. Sum is OK");
            }
            else
            {
                Assert.Fail("Error. Sum is not OK");
            }

        }

        public double removeCurrency(string value)
        {
            string[] parts = value.Split(currency);
            value = parts[1];
            return Double.Parse(value);
        }
    }
}
