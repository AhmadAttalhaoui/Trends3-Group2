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
using RabbitMQ.Client.Events;

namespace Trends3Interface
{
    class Program
    {
        public static string QueueName = "Queue1";
        static async Task Main(string[] args)
        {


           

            var request_xml_path = @"C:\Users\Gebruiker\Documents\GitHub\Trends3-Group2";




            var xsd_path = new Uri(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).LocalPath;




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
            response.Load(@"C:\Users\Gebruiker\Documents\GitHub\Trends3-Group2\GenerationResponse.xml");

            XmlNode ticket_node = response.SelectSingleNode("GenerationResponse/Ticket");
            XmlNode status_node = response.SelectSingleNode("GenerationResponse/Status");
            XmlNode error_node = response.SelectSingleNode("GenerationResponse/Errors");

            //selectie van de "<Binary> node 
            XmlNode binary_node = response.SelectSingleNode("GenerationResponse/Binary");


            List<string> errorList = new List<string>();
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
                        //response.Save(@"C:\Users\Gebruiker\Documents\GitHub\Trends3-Group2\GenerationResponse.xml");

                        Console.WriteLine("Error: Ticketnummer " + ticket_number + " bevat een fout");
                        
                }
                

            }
            else
            {
                SautinSoft.HtmlToRtf h = new SautinSoft.HtmlToRtf();

                string inputFile = @"C:\Users\Gebruiker\Documents\GitHub\Trends3-Group2\GenerationRequest.xml";
                string outputFile = @"C:\Users\Gebruiker\Documents\GitHub\Trends3-Group2\result.txt";

                if (h.OpenHtml(inputFile))
                {
                    bool ok = h.ToText(outputFile);

                    if (ok)
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(outputFile) { UseShellExecute = true });
                }

                Console.WriteLine("XML naar HTML -> string: ");

                string text = File.ReadAllText(@"C:\Users\Gebruiker\Documents\GitHub\Trends3-Group2\result.txt");
                Console.WriteLine(text);

                string myDataEncoded = EncodeTo64(text);

                Console.WriteLine("HTML naar Base64: " + myDataEncoded + "\r\n");

                string myDataUnencoded = DecodeFrom64(myDataEncoded);

                //Console.WriteLine(myDataUnencoded);

                ticket_node.InnerText = ticket_number;
                status_node.InnerText = "Success";
                binary_node.InnerText = myDataEncoded;
                Console.WriteLine(response.InnerXml);
                Console.WriteLine("Succes: Ticketnummer " + ticket_number + " is correct uitgevoerd" + "\r\n");


                //response.Save(@"C:\Users\Gebruiker\Documents\GitHub\Trends3-Group2\GenerationResponse.xml");

                // IN QUEUE

                string isResent = "Y";
                int numberOfTimes = 3;


                while (string.Equals(isResent, "Y", StringComparison.OrdinalIgnoreCase))
                {

                    for (int i = 0; i < numberOfTimes; i++)
                    {
                        var connectionFactory = new RabbitMQ.Client.ConnectionFactory()

                        {
                            Uri = new Uri("amqp://guest:guest@localhost:5672"),

                        };

                        using (var connection = connectionFactory.CreateConnection())
                        using (var model = connection.CreateModel())
                        {
                            model.QueueDeclare(queue: QueueName,
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

                            string message = text + i;
                            var body = Encoding.UTF8.GetBytes(message);

                            var properties = model.CreateBasicProperties();
                            properties.Persistent = true;
                            model.BasicPublish(exchange: "", routingKey: QueueName, basicProperties: properties, body: body);
                            Console.WriteLine($" {i} keer bericht verzonden");
                          
                        }

                    }


                    Console.WriteLine("Opnieuw queue verzenden? Druk Y om te verzenden anders N");
                    //isResent = Console.ReadLine();
                    if (isResent == Console.ReadLine())
                    {
                        Console.WriteLine("Hoeveel keren wilt u de queue verzenden?");
                        string numberOfTimesStr = Console.ReadLine();

                        Int32.TryParse(numberOfTimesStr, out numberOfTimes);
                    } else
                    {
                        System.Environment.Exit(1);
                        return;
                    }
                    
                    
                }


            }


        }

       
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