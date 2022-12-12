using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

//added using statements 
using Newtonsoft.Json;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Data.SQLite;
using HtmlAgilityPack;

namespace ProjectService
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        //Declare  an application path.
        static string appPath = @"C:\ITS462\ProjectService\ProjectService\";
        //Set sqllite connection
        static SQLiteConnection conn = new SQLiteConnection("Data Source=" + appPath + "\\DataScrape.db; Version=3;New=True;Compress=True;");
        SQLiteCommand sqlCmd = new SQLiteCommand(conn);
        
        // Create DataTables for the WebMethods
        DataTable dtLaptops = new DataTable();
        DataTable dtTablets = new DataTable();
        DataTable dtPhones = new DataTable();
        DataTable dtAllProductsQuery = new DataTable();
        DataTable dtLaptosQuery = new DataTable();
        DataTable dtTabletsQuery = new DataTable();
        DataTable dtPhonesQuery = new DataTable();
        DataTable dtByVendorQuery = new DataTable();



        [WebMethod]
        public string ScrapeLaptops()
        {
            //close connection from previous scrape method
            conn.Close();
            //Scrape first website for LAPTOPS   
            string url = "https://webscraper.io/test-sites/e-commerce/allinone/computers/laptops";
            HtmlWeb web = new HtmlWeb();
            var doc = web.Load(url);

            // gets number of items on page
            int count = doc.DocumentNode.SelectNodes("//div[@class='col-sm-4 col-lg-4 col-md-4']").Count();
            // adjusts count for the 0 start in array
            int correctCount = +count;

            // define array and initiliaze all objects within object array
            Products[] product = new Products[correctCount];
            for (int y = 0; y < correctCount; y++)
            {
                product[y] = new Products();
            }

            // define starting tag
            HtmlNode node = doc.DocumentNode.SelectNodes("//div[@class='col-sm-4 col-lg-4 col-md-4']").First();
            // Scrapes price - uses MTML code from the webpage
            HtmlNode[] pNodes = doc.DocumentNode.SelectNodes(".//h4[@class='pull-right price']").ToArray();
            // Scrapes title/ProductName - uses MTML code from the webpage
            HtmlNode[] bNodes = doc.DocumentNode.SelectNodes(".//a[@class='title']").ToArray();
            // Scrapes description - uses MTML code from the webpage
            HtmlNode[] dNodes = doc.DocumentNode.SelectNodes(".//p[@class='description']").ToArray();

            // stores values in object 
            for (int b = 0; b < correctCount; b++)
            {
                product[b].ProductName = bNodes[b].Attributes["title"].Value;
                product[b].Price = pNodes[b].InnerHtml;
                product[b].Description = dNodes[b].InnerHtml;
            }

            // SQLite Database Queries
            // drop table to prevent duplicate data when testing DROP TABLE Laptops;
            // create table if not exists
            string createTableQuery = @" DROP TABLE Laptops; CREATE TABLE IF NOT EXISTS [Laptops] (
                                        [ProductName] VARCHAR(200) NULL,
                                        [Description] VARCHAR(200) NULL,
                                        [Price] VARCHAR(200) NULL);";

            sqlCmd.CommandText = createTableQuery;
            conn.Open();
            sqlCmd.ExecuteNonQuery();
            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;

            // SQLite query to Insert scraped data into database
            int x = 0;
            while (x < correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = "Select * from Laptops where ProductName='" + product[x].ProductName + "'";
                dataAdapter.Fill(dtLaptops);
                sqlCmd.CommandText = "Insert into Laptops(ProductName, Description, Price) values " +
                        "('" + product[x].ProductName + "','" + product[x].Description + "','" + product[x].Price + "')";
                sqlCmd.ExecuteNonQuery();
                x++;
            }
            string result = JsonConvert.SerializeObject(dtLaptops);
            return result; 
        }

        [WebMethod]
        public string ScrapeTablets()
        {

            //close connection from previous scrape method
            conn.Close();
            //Scrape second website for LAPTOPS 
            string url = "https://webscraper.io/test-sites/e-commerce/allinone/computers/tablets";
            HtmlWeb web = new HtmlWeb();
            var doc = web.Load(url);

            // gets number of items on page
            int count = doc.DocumentNode.SelectNodes("//div[@class='col-sm-4 col-lg-4 col-md-4']").Count();
            // adjusts count for the 0 start in array
            int correctCount = +count;

            // define array and initiliaze all objects within object array
            Products[] product = new Products[correctCount];
            for (int y = 0; y < correctCount; y++)
            {
                product[y] = new Products();
            }
            // define starting tag
            HtmlNode node = doc.DocumentNode.SelectNodes("//div[@class='col-sm-4 col-lg-4 col-md-4']").First();

            // gets price
            HtmlNode[] pNodes = doc.DocumentNode.SelectNodes(".//h4[@class='pull-right price']").ToArray();
            //// gets title
            HtmlNode[] bNodes = doc.DocumentNode.SelectNodes(".//a[@class='title']").ToArray();
            // gets description
            HtmlNode[] dNodes = doc.DocumentNode.SelectNodes(".//p[@class='description']").ToArray();

            // sets values in object 
            for (int b = 0; b < correctCount; b++)
            {
                product[b].ProductName = bNodes[b].Attributes["title"].Value;
                product[b].Price = pNodes[b].InnerHtml;
                product[b].Description = dNodes[b].InnerHtml;
            }
            // SQLite Database Queries
            // drop table to prevent duplicate data when testing DROP TABLE Tablets;
            // create table if not exists
            string createTableQuery = @"DROP TABLE Tablets; CREATE TABLE IF NOT EXISTS [Tablets] (
                                        [ProductName] VARCHAR(200) NULL,
                                        [Description] VARCHAR(200) NULL,
                                        [Price] VARCHAR(200) NULL);";

            sqlCmd.CommandText = createTableQuery;

            conn.Open();
            sqlCmd.ExecuteNonQuery();

            // conn.Close();
            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;
            int x = 0;
            while (x < correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = "Select * from Tablets where ProductName='" + product[x].ProductName + "'";
                dataAdapter.Fill(dtTablets);
                sqlCmd.CommandText = "Insert into Tablets(ProductName, Description, Price) values " +
                        "('" + product[x].ProductName + "','" + product[x].Description + "','" + product[x].Price + "')";
                sqlCmd.ExecuteNonQuery();
                x++;
            }
            string result = JsonConvert.SerializeObject(dtTablets);
            return result;
        }

        [WebMethod]
        public string ScrapePhones()
        {
            //Scrape third website for LAPTOPS 
            //close connection from previous scrape method
            conn.Close();
            //Scrape third website for LAPTOPS
            string url = "https://webscraper.io/test-sites/e-commerce/allinone/phones/touch";
            HtmlWeb web = new HtmlWeb();
            var doc = web.Load(url);

            // gets number of items on page
            int count = doc.DocumentNode.SelectNodes("//div[@class='col-sm-4 col-lg-4 col-md-4']").Count();
            // adjusts count for the 0 start in array
            int correctCount =+ count;

            // define array and initiliaze all objects within object array
            Products[] product = new Products[correctCount];
            for (int y = 0; y < correctCount; y++)
            {
                product[y] = new Products();
            }
            // define starting tag
            HtmlNode node = doc.DocumentNode.SelectNodes("//div[@class='col-sm-4 col-lg-4 col-md-4']").First();

            // gets price
            HtmlNode[] pNodes = doc.DocumentNode.SelectNodes(".//h4[@class='pull-right price']").ToArray();
            //// gets title
            HtmlNode[] bNodes = doc.DocumentNode.SelectNodes(".//a[@class='title']").ToArray();
            // gets description
            HtmlNode[] dNodes = doc.DocumentNode.SelectNodes(".//p[@class='description']").ToArray();

            // sets values in object 
            for (int b = 0; b < correctCount; b++)
            {
                product[b].ProductName = bNodes[b].Attributes["title"].Value;
                product[b].Price = pNodes[b].InnerHtml;
                product[b].Description = dNodes[b].InnerHtml;
            }

            // SQLite Database Queries
            // drop table to prevent duplicate data when testing DROP TABLE Phones;
            // create table if not exists 

            string createTableQuery = @"DROP TABLE Phones;CREATE TABLE IF NOT EXISTS [Phones] (
                                        [ProductName] VARCHAR(200) NULL,
                                        [Description] VARCHAR(200) NULL,
                                        [Price] VARCHAR(200) NULL);";

            sqlCmd.CommandText = createTableQuery;
            conn.Open();
            sqlCmd.ExecuteNonQuery();
            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;

            int x = 0;
            while (x < correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = "Select * from Phones where ProductName='" + product[x].ProductName + "'";
                dataAdapter.Fill(dtPhones);
                sqlCmd.CommandText = "Insert into Phones(ProductName, Description, Price) values " +
                        "('" + product[x].ProductName + "','" + product[x].Description + "','" + product[x].Price + "')";
                sqlCmd.ExecuteNonQuery();
                x++;
            }
            string result = JsonConvert.SerializeObject(dtPhones);
            return result;
        }

        [WebMethod]
        public string AllProductsQuery()
        {
            conn.Close();
            string allProductsQuery = @"select t.*
                                        from Laptops t
                                        union all
                                        select t.*
                                        from Phones t
                                        union all
                                        select t.*
                                        from Tablets t
                                        order by price;";

            sqlCmd.CommandText = allProductsQuery;
            conn.Open();
            //sqlCmd.ExecuteNonQuery();
            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;
            dataAdapter = new SQLiteDataAdapter(sqlCmd);
            int correctCount = 1;
            int x = 0;
            while (x < correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = allProductsQuery;
                dataAdapter.Fill(dtAllProductsQuery);
                sqlCmd.ExecuteNonQuery();
                x++;
            }

            string result = JsonConvert.SerializeObject(dtAllProductsQuery);
            return result;
        }

        [WebMethod]
        public string LaptopsQuery()
        {
            conn.Close();
            string laptopsQuery = @"select * from Laptops order by price;";

            sqlCmd.CommandText = laptopsQuery;
            conn.Open();
            //sqlCmd.ExecuteNonQuery();
            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;
            dataAdapter = new SQLiteDataAdapter(sqlCmd);
            int correctCount = 1;
            int x = 0;
            while (x < correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = laptopsQuery;
                dataAdapter.Fill(dtLaptosQuery);
                sqlCmd.ExecuteNonQuery();
                x++;
            }
            string result = JsonConvert.SerializeObject(dtLaptosQuery);
            return result;
        }

        [WebMethod]
        public string PhonesQuery()
        {
            conn.Close();
            string phonesQuery = @"select * from Phones order by price;";

            sqlCmd.CommandText = phonesQuery;
            conn.Open();
            //sqlCmd.ExecuteNonQuery();
            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;
            dataAdapter = new SQLiteDataAdapter(sqlCmd);
            int correctCount = 1;
            int x = 0;
            while (x < correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = phonesQuery;
                dataAdapter.Fill(dtPhonesQuery);
                sqlCmd.ExecuteNonQuery();
                x++;
            }
            string result = JsonConvert.SerializeObject(dtPhonesQuery);
            return result;
        }

        [WebMethod]
        public string TabletsQuery()
        {
            conn.Close();
            string tabletsQuery = @"select * from Tablets order by price;";

            sqlCmd.CommandText = tabletsQuery;
            conn.Open();
            //sqlCmd.ExecuteNonQuery();
            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;
            dataAdapter = new SQLiteDataAdapter(sqlCmd);
            int correctCount = 1;
            int x = 0;
            while (x < correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = tabletsQuery;
                dataAdapter.Fill(dtTabletsQuery);
                sqlCmd.ExecuteNonQuery();
                x++;
            }
            string result = JsonConvert.SerializeObject(dtTabletsQuery);
            return result;
        }

        [WebMethod]
        public string ByVendorQuery()
        {
            conn.Close();
            string byVendorsQuery = @"select t.Description, t.Price
                                            from Laptops t
                                            union all
                                            select t.Description, t.Price
                                            from Phones t
                                            union all
                                            select t.Description ,t.Price
                                            from Tablets t
                                            order by Price;";

            sqlCmd.CommandText = byVendorsQuery;
            conn.Open();
            //sqlCmd.ExecuteNonQuery();
            sqlCmd = new SQLiteCommand(conn);
            SQLiteDataAdapter dataAdapter;
            dataAdapter = new SQLiteDataAdapter(sqlCmd);
            int correctCount = 1;
            int x = 0;
            while (x < correctCount)
            {
                dataAdapter = new SQLiteDataAdapter(sqlCmd);
                sqlCmd.CommandText = byVendorsQuery;
                dataAdapter.Fill(dtByVendorQuery);
                sqlCmd.ExecuteNonQuery();
                x++;
            }
            string result = JsonConvert.SerializeObject(dtByVendorQuery);
            return result;
        }
    }
}
