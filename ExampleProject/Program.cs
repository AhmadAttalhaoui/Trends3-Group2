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
           
            var xml_path = "C:\\Users\\user\\source\\repos\\Trends3-Group2\\startcode\\Trends3Interface";
            var xsd_path = new Uri(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).LocalPath;

            //string[] errorList = new string[];

            XmlSchemaSet schema = new XmlSchemaSet();
            schema.Add("", xsd_path + "\\GenerationRequest.xsd");
            XDocument doc = XDocument.Load(xml_path + "\\GenerationRequest.xml");

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
            }

            Queue<IXmlLineInfo> tickets = new Queue<IXmlLineInfo>();

            tickets = await TicketsAsync(2);

            for (int i = 0; i < tickets.Count; i++)
            {
                doc.Element("Ticket");
                tickets.Enqueue(doc);
            }

        }

        private static Task<Queue<IXmlLineInfo>> TicketsAsync(int v)
        {
            throw new NotImplementedException();
        }
    }
}