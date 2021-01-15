using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace hpstoretest
{
    class Discounts : Init
    {
        [Test]
        [TestCase("top-deals", 4)]
        [TestCase("laptops",4)]
        [TestCase("desktops", 4)]
        [TestCase("printers", 5)]
        [TestCase("monitors", 5)]
        [TestCase("accessories", 4)]
        [Order(2)]
        public void CheckDiscounts(string urlPart, int index)
        {
            driver.Navigate().GoToUrl("https://store.hp.com/us/en/slp/weekly-deals/"+urlPart);
            Thread.Sleep(2000);
            IWebElement[] products = driver.FindElement(By.XPath("//*[@id='root']/div/div[3]/div["+index+"]/div/div/div[2]/div/div[2]/div")).FindElements(By.ClassName("cache-add-container")).ToArray();
            List<string> productsWithError = new List<string>();
            for (int i=0; i<products.Length; i++)
            {
                bool isTherePromoText = FindElementIfExists(driver, By.ClassName("promo-text"), products[i]);
                if (isTherePromoText)
                {
                    string promoText = products[i].FindElements(By.ClassName("promo-text"))[0].Text;
                    //checking only absolute discounts
                    if (promoText.ToLower().Contains("save") && !promoText.Contains("%"))
                    {
                        char[] currency = products[i].FindElements(By.ClassName("currency-symbol"))[0].Text.ToCharArray();
                        string[] parts = promoText.Split(currency[0]);
                        string txtPrice = parts[parts.Length - 1];
                        double discountPrice = Double.Parse(txtPrice);

                        string txtSalePrice = products[i].FindElements(By.ClassName("sale-price"))[0].Text;
                        string txtListPrice = products[i].FindElements(By.ClassName("list-price"))[0].Text;
                        txtListPrice = removeExtras(txtListPrice, currency[0].ToString());
                        txtSalePrice = removeExtras(txtSalePrice, currency[0].ToString());
                        txtListPrice = removeExtras(txtListPrice, "Starting at ");
                        txtSalePrice = removeExtras(txtSalePrice, "Starting at ");
                        double salePrice = Double.Parse(txtSalePrice);
                        double listPrice = Double.Parse(txtListPrice);

                        if (Math.Round(listPrice - salePrice) != Math.Round(discountPrice))
                        {
                            string productName = products[i].FindElements(By.ClassName("product-details"))[0].FindElements(By.TagName("a"))[0].Text;
                            productsWithError.Add(productName + " price is incorrect because " + listPrice + "-" + salePrice + " is not equal to " + discountPrice + "\n"); //adding product name with bad price to list
                        }

                    }
                } 
            }

            if(productsWithError.Count == 0)
            {
                Assert.Pass("Success. Everything is calculated well");
                driver.Quit();
            } else
            {
                string fullProductList = "";
                for(int i=0; i<productsWithError.Count; i++)
                {
                    fullProductList += productsWithError[i];
                }
                File.WriteAllText(@"C:\productsWithErrors.txt", fullProductList);
                Assert.Fail("Error. There are " + productsWithError.Count + " products with bad prices");
                driver.Quit();
            }
        }

        public string removeExtras(string value, string toRemove)
        {
            return value.Replace(toRemove, "");
        }

        public bool FindElementIfExists(IWebDriver driver, By by, IWebElement parent = null)
        {
            dynamic elements;
            if (parent == null)
            {
                elements = driver.FindElements(by);
            }
            else
            {
                elements = parent.FindElements(by);
            }
            if (elements.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
