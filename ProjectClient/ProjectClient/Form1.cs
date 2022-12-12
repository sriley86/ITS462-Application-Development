using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//added using statements
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;

namespace ProjectClient
{
    public partial class Form1 : Form
    {
        //Declare  an application path.
        static string appPath = @"C:\ITS462\ProjectClient\ProjectClient";

        // sets the localhost webservice
        localhost.WebService1 proxy = new localhost.WebService1();

        // create object for webservicesettings method
        HttpClient client = new HttpClient();

        public Form1()
        {
            InitializeComponent();
        }
        private void WebServicesSettings()
        {
            //service is load through the Uri 
            client.BaseAddress = new Uri("http://localhost:32920/WebService1.asmx/");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //Upon loading the form the WebService is loaded
            WebServicesSettings();
        }
        private DataTable stringSplitAllProducts(string allProductsJson)
        {
            //remove tags from the DOM - 3 tags are removed
            string[] json = allProductsJson.Split('>'); //split and store in json
            string[] finalJson = json[2].Split('<');
            DataTable dtAllProductsQuery = JsonConvert.DeserializeObject<DataTable>(finalJson[0]);
            return dtAllProductsQuery;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if else if to select and display the service database query
            if (comboBox1.Text == "All Products")
            {
                HttpResponseMessage message = client.GetAsync("AllProductsQuery?").Result;
                string allProductsJson = message.Content.ReadAsStringAsync().Result;

                dataGridView1.DataSource = stringSplitAllProducts(allProductsJson);
                
            }
            else if (comboBox1.Text == "Laptops")
            {
                HttpResponseMessage message = client.GetAsync("LaptopsQuery?").Result;
                string laptopsJson = message.Content.ReadAsStringAsync().Result;

                dataGridView1.DataSource = stringSplitAllProducts(laptopsJson);

            }
            else if (comboBox1.Text == "Tablets")
            {
                HttpResponseMessage message = client.GetAsync("TabletsQuery?").Result;
                string tabletsJson = message.Content.ReadAsStringAsync().Result;

                dataGridView1.DataSource = stringSplitAllProducts(tabletsJson);

            }
            else if (comboBox1.Text == "Phones")
            {
                HttpResponseMessage message = client.GetAsync("PhonesQuery?").Result;
                string phonesJson = message.Content.ReadAsStringAsync().Result;

                dataGridView1.DataSource = stringSplitAllProducts(phonesJson);

            }
            else if (comboBox1.Text == "By Vendor")
            {
                HttpResponseMessage message = client.GetAsync("ByVendorQuery?").Result;
                string byVendorJson = message.Content.ReadAsStringAsync().Result;

                dataGridView1.DataSource = stringSplitAllProducts(byVendorJson);

            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            // display the retrieved data in a report format that can be printed.
            TextWriter writer = new StreamWriter(appPath + "\\Report.txt");
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++) //rows
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++) //columns
                {
                    writer.Write("\t" + dataGridView1.Rows[i].Cells[j].Value.ToString());
                }
                writer.WriteLine("");


            }
            writer.Close();

            MessageBox.Show("Report.txt has been Created.");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //Exit the application
            Close();
        }


    }
}
