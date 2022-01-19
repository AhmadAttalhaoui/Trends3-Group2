//using System.Data.SqlClient;

//using var connection = new SqlConnection("Data Source=database; User ID=SA;Password=MyVerySecurePassword$123");
//connection.Open();

using System;
using System.Collections;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;



namespace Trends3Interface
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var xml_path = "C:\\Users\\ahmad\\source\\repos\\Trends3-Group2";
            var xsd_path = new Uri(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).LocalPath;

            //string[] errorList = new string[];

            XmlSchemaSet schema = new XmlSchemaSet();
            schema.Add("", xsd_path + "\\GenerationRequest.xsd");
            XDocument doc = XDocument.Load(xml_path + "\\GenerationRequest.xml");
            Queue<XDocument> tickets = new Queue<XDocument>();

            bool validationErrors = false;

            doc.Validate(schema, (s, e) =>
            {
                Console.WriteLine(e.Message);
                validationErrors = true;
            });

            if (validationErrors)
            {
                Console.WriteLine("Validation Failed");

            }
            else
            {
                Console.WriteLine("Validation succeeded");
                tickets.Enqueue(doc);
            }
            Console.WriteLine(tickets.Count);
             
            /*tickets = await TicketsAsync(2);*/

            /*for (int i = 0; i < tickets.Count; i++)
            {
                
                tickets.Enqueue(doc);
                
            }

            Console.WriteLine(tickets);*/
        }

      /*  private static Task<Queue<IXmlLineInfo>> TicketsAsync(int v)
        {
            throw new NotImplementedException();
        }*/
    }
}