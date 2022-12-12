# ITS462-Application-Integration

Statement of Work:

• The application for this project provides a tool for a user to compare price information of various 
  computer products from various websites. The application will be designed as a client application that 
  uses services hosted on a server that get data from a database and provide it to the client application.
• The server-side of the application will scrape and retrieve information from at least two websites that 
  have computer products (desktop computers, laptops, tablets, phones, etc.). The information that must 
  be scraped includes: 1) the site offering the products for sale, 2) the product vendor name, 3) product 
  model, product description, product price, and optionally any product specs if available. 
      o A website that includes links to an example scraper test site with computer product information
        is https://webscraper.io/test-sites. You can find another site for the second source of 
        computer product sales.
      o The application will use a third-party tool to scrape the website to extract information. You can 
        pick a tool of your choosing. An example of such a tool is “HTML Agility Pack”. It is 
        available as a NuGet package and it scrapes the webpage HTML and builds a DOM with the 
        scraped data (XPATH and XSLT is supported).
▪ Html Agility Pack website: (http://htmlagilitypack.codeplex.com/) 
• The server-side of the application will store the scraped data in a database.
      o You can pick any database system to store the data.
• The server-side of the application will implement either WCF or ASMX services that can be called by 
  the client application to retrieve the specific scraped data from the database.
• A client will be implemented as a Windows or Web form application with the following requirements:
      o A list or drop-down control that allows the user to select the type of data on which a price 
        comparison is desired (this is a querying capability).
▪ The query list must include at least: 1) computer type (desktop, laptop, tablet, phone), 
    2) computer vendor, 3) model, and 4) price.
      o The client application must have a control to display all the data retrieved from the database by 
       the user selection. The database query must return and display all the information in the 
       database about the selected product.
      o The results will be shown in a price ascending order.
• The client application will have a feature to display the retrieved data in a report format that can be 
  printed. 
• A Software Requirements Specification and software design document will be compiled for the 
  application
  
  Deliverables:
1. A working, high-quality application.
2. An electronic copy of your project report which includes the requirements, software design, user manual, 
  source code, database file, and screen shots. Please zip everything and submit to Brightspace. Each team 
  member MUST individually submit the zip file to Brightspace (same file for both team members).
