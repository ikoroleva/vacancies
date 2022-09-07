using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace selenium_test;

public class Tests
{
    protected IWebDriver driver;
    private const string AllDepartments = "All departments";
    private const string AllLanguages = "All languages";

    [SetUp]
    public void Setup()
    {
        new DriverManager().SetUpDriver(new ChromeConfig());
        driver = new ChromeDriver();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);
    }

    [Test]
    [TestCase("Research & Development", "English", 12)]
    [TestCase("Quality Assurance", "Russian", 3)]
    [TestCase("IT", "French", 0)]
    public void TestCareers(string department, string language, int jobsCount)
    {
        driver.Navigate().GoToUrl("https://cz.careers.veeam.com/vacancies");
        driver.Manage().Window.Maximize();

        // find select element for departments
        IWebElement allDepartmentsEl = driver.FindElement(By.XPath($"//button[text()='{AllDepartments}']"));
        allDepartmentsEl.Click();

        // find department
        IWebElement departmentEl = allDepartmentsEl.FindElement(By.XPath($"//a[text()='{department}']"));
        departmentEl.Click();

        // find select element for languages
        IWebElement allLanguagesEl = driver.FindElement(By.XPath($"//button[text()='{AllLanguages}']"));
        allLanguagesEl.Click();

        // find language
        IWebElement languageEl = allLanguagesEl.FindElement(By.XPath($"//label[text()='{language}']"));
        languageEl.Click();

        // close the list of languages
        allLanguagesEl.Click();

        // find all cards with jobs
        IReadOnlyList<IWebElement> jobs = driver.FindElements(By.CssSelector(".card.card-sm.card-no-hover"));

        Assert.AreEqual(jobs.Count, jobsCount);
    }

    [TearDown]
    public void TestTearDown()
    {
        driver.Quit();
    }
}