//using System.Data.SqlClient;

//using var connection = new SqlConnection("Data Source=database; User ID=SA;Password=MyVerySecurePassword$123");
//connection.Open();

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Trends3Interface
{
    class Program
    {
        static void Main(string[] args)
        {

            //var xml_path = "C:\\Trends3\\startcode\\Trends3Interface";
            //
            var request_xml_path = "C:\\Users\\Rogier\\source\\repos\\Trends3_Group2";
            var xsd_path = new Uri(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).LocalPath;



            List<string> errorList = new List<string>();

            XmlSchemaSet schema = new XmlSchemaSet();
            schema.Add("", xsd_path + "\\GenerationRequest.xsd");
            XDocument req_doc = XDocument.Load(request_xml_path + "\\GenerationRequest.xml");

            XmlDocument request = new XmlDocument();
            request.Load(request_xml_path + "\\GenerationRequest.xml");
            XmlNode ticket_node_req = request.SelectSingleNode("GenerationRequest/Ticket");
            string ticket_number = ticket_node_req.InnerText;

            bool validationErrors = false;

            //Code om GenerationResponse.xml aan te passen. 
            XmlDocument response = new XmlDocument();
            response.Load(@"C:\\Users\\Rogier\\source\\repos\\Trends3_Group2\\GenerationResponse.xml");
            XmlNode ticket_node = response.SelectSingleNode("GenerationResponse/Ticket");
            XmlNode status_node = response.SelectSingleNode("GenerationResponse/Status");
            XmlNode error_node = response.SelectSingleNode("GenerationResponse/Errors");

            //selectie van de "<Binary> node 
            XmlNode binary_node = response.SelectSingleNode("GenerationResponse/Binary");


           req_doc.Validate(schema, (s, e) =>
            {
                errorList.Add(e.Message);
                validationErrors = true;
                });

            if (validationErrors)
            {
                //Console.WriteLine("Validation Failed");
                status_node.InnerText = "Failure";
                foreach (string item in errorList)
                {
                    
                    ticket_node.InnerText = ticket_number;
                    
                    XmlElement elem = response.CreateElement("Error");
                    elem.InnerText = item;
                    error_node.AppendChild(elem);
                    Console.WriteLine(response.InnerXml);
                    response.Save(@"C:\\Users\\Rogier\\source\\repos\\Trends3_Group2\\GenerationResponse.xml");

                }

            }
            else
            {
                //hier moet GenerationRequest.xml omgezet worden in html + naar string + base 64 encoded string
                //
                ticket_node.InnerText = ticket_number;
                status_node.InnerText = "Success";
                binary_node.InnerText = "base-64 code";
                Console.WriteLine(response.InnerXml);

                response.Save(@"C:\\Users\\Rogier\\source\\repos\\Trends3_Group2\\GenerationResponse.xml");


            }

            //zet generation response op out queue

        }

/*laat ons testen*/

       
    }
}