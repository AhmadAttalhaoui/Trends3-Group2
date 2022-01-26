//using System.Data.SqlClient;

//using var connection = new SqlConnection("Data Source=database; User ID=SA;Password=MyVerySecurePassword$123");
//connection.Open();

using System;

using System.Collections.Generic;
using SautinSoft;
using System.Collections;
using RabbitMQ.Client;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Text;
using Newtonsoft.Json;

namespace Trends3Interface
{
    class Program
    {
        static async Task Main(string[] args)
        {

            /*string UserName = "guest";

            string Password = "guest";

            string HostName = "localhost";*/



            //Main entry point to the RabbitMQ .NET AMQP client

            var connectionFactory = new RabbitMQ.Client.ConnectionFactory()

            {
                Uri = new Uri("amqp://guest:guest@localhost:5672"),
                /*UserName = UserName,

                Password = Password,

                HostName = HostName*/

            };
            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();
            model.QueueDeclare("demo-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null) ;

            // invert van html to string word document.

            SautinSoft.HtmlToRtf h = new SautinSoft.HtmlToRtf();

            /*string inputFile = @"C:\Users\Gebruiker\Documents\GitHub\Trends3-Group2\GenerationRequest.xml";*/
            string inputFile = @"C:\Users\user\source\repos\Trends3-Group2\GenerationRequest.xml";
        /* string outputFile = @"C:\Users\Gebruiker\Documents\GitHub\Trends3-Group2\result.txt";*/
        
                string outputFile = @"C:\Users\user\source\repos\Trends3-Group2\result.txt";
            if (h.OpenHtml(inputFile))
            {
                bool ok = h.ToText(outputFile);

                if (ok)
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(outputFile){ UseShellExecute = true });
            }

            /*string text = File.ReadAllText(@"C:\Users\Gebruiker\Documents\GitHub\Trends3-Group2\result.txt");*/
            string text = File.ReadAllText(@"C:\Users\user\source\repos\Trends3-Group2\result.txt");
            Console.WriteLine(text);

            string myDataEncoded = EncodeTo64(text);

            Console.WriteLine(myDataEncoded);

            string myDataUnencoded = DecodeFrom64(myDataEncoded);

            Console.WriteLine(myDataUnencoded);

            EncodeTo64(text);
            var message = new { text };
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            model.BasicPublish("xml", "queus",null, body);








            //var xml_path = "C:\\Trends3\\startcode\\Trends3Interface";
            //
            //var request_xml_path = "C:\\Users\\Rogier\\source\\repos\\Trends3_Group2";
            var request_xml_path = @"C:\Users\Gebruiker\Documents\GitHub\Trends3-Group2";




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
            //response.Load(@"C:\\Users\\Rogier\\source\\repos\\Trends3_Group2\\GenerationResponse.xml");
            response.Load(@"C:\Users\Gebruiker\Documents\GitHub\Trends3-Group2\GenerationResponse.xml");

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
                    //response.Save(@"C:\\Users\\Rogier\\source\\repos\\Trends3_Group2\\GenerationResponse.xml");
                    response.Save(@"C:\Users\Gebruiker\Documents\GitHub\Trends3-Group2\GenerationResponse.xml");


                }

            }
            else
            {

                //hier moet GenerationRequest.xml omgezet worden in html + naar string + base 64 encoded string
                //
                ticket_node.InnerText = ticket_number;
                status_node.InnerText = "Success";
                binary_node.InnerText = myDataEncoded;
                Console.WriteLine(response.InnerXml);

                //response.Save(@"C:\\Users\\Rogier\\source\\repos\\Trends3_Group2\\GenerationResponse.xml");
                response.Save(@"C:\Users\Gebruiker\Documents\GitHub\Trends3-Group2\GenerationResponse.xml");


            }

            //zet generation response op out queue


            //    Console.WriteLine("Validation succeeded");
            //    tickets.Enqueue(doc);
            //}

            //Console.WriteLine(tickets.Count);

            /*tickets = await TicketsAsync(2);*/

            /*for (int i = 0; i < tickets.Count; i++)
            {
                
                tickets.Enqueue(doc);
                
            }
            Console.WriteLine(tickets);*/
            /*byte[] messagebuffer = Encoding.Default.GetBytes("response");
            var properties = model.CreateBasicProperties();

            properties.Persistent = false;
            model.BasicPublish("demoExchange", "ticket_number",  request, messagebuffer);*/

            /*Console.WriteLine("Message Sent");*/
        }

        /*  private static Task<Queue<IXmlLineInfo>> TicketsAsync(int v)
          {
              throw new NotImplementedException();
          }*/
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        static public string EncodeTo64(string toEncode)

        {

            byte[] toEncodeAsBytes

                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);

            string returnValue

                  = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;

        }

        static public string DecodeFrom64(string encodedData)

        {

            byte[] encodedDataAsBytes

                = System.Convert.FromBase64String(encodedData);

            string returnValue =

               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

            return returnValue;

        }

    }
}