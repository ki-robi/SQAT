using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using CsvHelper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        IWebDriver driver = new ChromeDriver();
        ExtentReports extent = new ExtentReports();
        ExtentSparkReporter htmlreporter = new ExtentSparkReporter(@"C:\Users\ThinkPad\source\repos\SQAT_Project_1\ReportResult\report.html");

        extent.AttachReporter(htmlreporter);
        ExtentTest test = extent.CreateTest("Test Case", "Positive LogIn test");

        try
        {
            OpenUrl(driver, test, "https://parabank.parasoft.com/parabank/index.htm");

            using (var reader = new StreamReader(@"C:\Users\ThinkPad\source\repos\SQAT_Project_1\UserData\UserData.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<UserData>();
                foreach (var record in records)
                {
                    RegisterUser(driver, test, record.FirstName, record.LastName, record.Address, record.City, record.State, record.ZipCode, record.Phone, record.SSN, record.Username, record.Password);
                    Logout(driver, test);
                    PerformLogin(driver, test, record.Username, record.Password);
                    ValidateLogin(driver, test);
                }
            }
        }
        catch (Exception ex)
        {
            test.Log(Status.Fail, $"Test failed with exception: {ex.Message}");
        }
        finally
        {
            extent.Flush();
            driver.Quit();
        }
    }

    static void OpenUrl(IWebDriver driver, ExtentTest test, string url)
    {
        driver.Navigate().GoToUrl(url);
        Thread.Sleep(1000);
        test.Log(Status.Info, $"Open url: {url}");
        Console.WriteLine($"Open url: {url}");
        driver.Manage().Window.Maximize();
    }

    static void RegisterUser(IWebDriver driver, ExtentTest test, string firstName, string lastName, string address, string city, string state, string zipCode, string phone, string ssn, string username, string password)
    {
        driver.FindElement(By.LinkText("Register")).Click();
        Thread.Sleep(1000);

        driver.FindElement(By.Id("customer.firstName")).SendKeys(firstName);
        Console.WriteLine($"Type first name {firstName} into First Name field");
        test.Log(Status.Info, $"Type first name {firstName} into First Name field");

        driver.FindElement(By.Id("customer.lastName")).SendKeys(lastName);
        Console.WriteLine($"Type last name {lastName} into Last Name field");
        test.Log(Status.Info, $"Type last name {lastName} into Last Name field");

        driver.FindElement(By.Id("customer.address.street")).SendKeys(address);
        Console.WriteLine($"Type address {address} into Address field");
        test.Log(Status.Info, $"Type address {address} into Address field");

        driver.FindElement(By.Id("customer.address.city")).SendKeys(city);
        Console.WriteLine($"Type city {city} into City field");
        test.Log(Status.Info, $"Type city {city} into City field");

        driver.FindElement(By.Id("customer.address.state")).SendKeys(state);
        Console.WriteLine($"Type state {state} into State field");
        test.Log(Status.Info, $"Type state {state} into State field");

        driver.FindElement(By.Id("customer.address.zipCode")).SendKeys(zipCode);
        Console.WriteLine($"Type zip code {zipCode} into Zip Code field");
        test.Log(Status.Info, $"Type zip code {zipCode} into Zip Code field");

        driver.FindElement(By.Id("customer.phoneNumber")).SendKeys(phone);
        Console.WriteLine($"Type phone number {phone} into Phone Number field");
        test.Log(Status.Info, $"Type phone number {phone} into Phone Number field");

        driver.FindElement(By.Id("customer.ssn")).SendKeys(ssn);
        Console.WriteLine($"Type SSN {ssn} into SSN field");
        test.Log(Status.Info, $"Type SSN {ssn} into SSN field");

        driver.FindElement(By.Id("customer.username")).SendKeys(username);
        Console.WriteLine($"Type username {username} into Username field");
        test.Log(Status.Info, $"Type username {username} into Username field");

        driver.FindElement(By.Id("customer.password")).SendKeys(password);
        Console.WriteLine($"Type password {password} into Password field");
        test.Log(Status.Info, $"Type password {password} into Password field");

        driver.FindElement(By.Id("repeatedPassword")).SendKeys(password);
        Console.WriteLine($"Type password {password} into Confirm Password field");
        test.Log(Status.Info, $"Type password {password} into Confirm Password field");

        driver.FindElement(By.CssSelector("#customerForm > table > tbody > tr:nth-child(13) > td:nth-child(2) > input")).Click();
        test.Log(Status.Info, "User registered successfully");
        Console.WriteLine("User registered successfully");
        Thread.Sleep(1000);
    }

    static void Logout(IWebDriver driver, ExtentTest test)
    {
        driver.FindElement(By.CssSelector("#leftPanel > ul > li:nth-child(8) > a")).Click();
        test.Log(Status.Pass, "Logout successfully");
        Console.WriteLine("User logged out successfully");
        Thread.Sleep(1000);
    }

    static void PerformLogin(IWebDriver driver, ExtentTest test, string username, string password)
    {
        driver.FindElement(By.CssSelector("#loginPanel > form > div:nth-child(2) > input")).SendKeys(username);
        Console.WriteLine($"Type username {username} into Username field");
        test.Log(Status.Info, $"Type username {username} into Username field");

        driver.FindElement(By.CssSelector("#loginPanel > form > div:nth-child(4) > input")).SendKeys(password);
        Console.WriteLine($"Type password {password} into Password field");
        test.Log(Status.Info, $"Type password {password} into Password field");

        driver.FindElement(By.CssSelector("#loginPanel > form > div:nth-child(5) > input")).Click();
        test.Log(Status.Info, "Push Submit button");
        Console.WriteLine("Push Submit button");
        Thread.Sleep(1000);
    }

    static void ValidateLogin(IWebDriver driver, ExtentTest test)
    {
        try
        {
            driver.FindElement(By.CssSelector("#leftPanel > ul > li:nth-child(8) > a")).Click();
            test.Log(Status.Pass, "Login successfully");
        }
        catch (NoSuchElementException)
        {
            test.Log(Status.Fail, "Login failed");
        }
    }
}

public class UserData
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Phone { get; set; }
    public string SSN { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}