using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Xml;
using System.Text.Json;
using Newtonsoft.Json;

namespace EDIX12Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            string edi = "990";
            string ediFilename = @"c:\Users\Carlos\Documents\GitHub\EDI\Archivos\EDIClass_2021_02_17\Samples\990_Ejemplo.edi";
           // string ediFilename = @"c:\Users\Carlos\Documents\GitHub\EDI\Archivos\EDIClass_2021_02_17\Samples\204_Ejemplo.edi";
            //string ediFilename = @"c:\Users\Carlos\Documents\GitHub\EDI\Archivos\EDIClass_2021_02_17\Samples\Sample_850_01_Orig.edi";
            string ediFileContents = File.ReadAllText(ediFilename);

            //temporary variables for parsing 
            string poNumber = "";

            Console.WriteLine(ediFileContents);

            /* string elementSeparator = ediFileContents.Substring(103, 1);
             string lineSeparator = ediFileContents.Substring(105, 1);*/
            string elementSeparator = "*";
            string lineSeparator = "~";

            ediFileContents = ediFileContents.Replace("\r", "").Replace("\n", "");

            Console.WriteLine("elementSeparator=" + elementSeparator);
            Console.WriteLine("lineSeparator=" + lineSeparator);
            Console.Read();
            StringBuilder sbXML = new StringBuilder();
            sbXML.Append("<EDI"+edi+">");

            string[] lines = ediFileContents.Split(char.Parse(lineSeparator));
            Console.WriteLine("Number of lines=" + lines.Length);

            bool segmentNeedsClose = false;
            string segment = "";

            foreach (string line in lines)
            {
                Console.WriteLine(line);
                string[] elements = line.Split(char.Parse(elementSeparator));
                int loopCounter = 0;
                string elNum = "";
                string elName = "";
                foreach (string el in elements)
                {
                    if (loopCounter == 0)
                    {
                        if (segmentNeedsClose)
                        {
                            if (!String.IsNullOrEmpty(segment))
                            {
                                sbXML.Append("</" + segment + ">");
                            }
                        }

                        segment = el;
                        if (!String.IsNullOrEmpty(segment))
                        {
                            sbXML.Append("<" + segment + ">");
                        }
                        segmentNeedsClose = true;

                    }
                    else
                    {
                        elNum = loopCounter.ToString("D2");
                        elName = segment + elNum;
                        Console.WriteLine(elName + "=" + el);
                        sbXML.Append("<" + elName + ">" + el + "</" + elName + ">");

                        if (elName == "BEG03")
                        {
                            poNumber = el;
                        }
                    }
                    loopCounter++;
                }
                
                 
                /*
                Console.WriteLine("*** PONum=" + po850.PONum + " PODate=" + po850.PODate + " POType=" + po850.POType);
                Console.WriteLine("*** Vendor=" + po850.VendorNumber + " BuyerName=" + po850.BuyerName + " Telephone=" + po850.BuyerTelephone);
                foreach (PurchaseOrder850LineItem item in po850.LineItems)
                {
                    Console.WriteLine("***** " +
                                      item.lineItem + " " +
                                      " qty=" + item.quantity + " " +
                                      " uom=" + item.uom + " " +
                                      " price=" + item.price + " " +
                                      " basis=" + item.basisOfUnitPrice + " " +
                                      " desc=" + item.description +
                                      " reqDate=" + item.dateRequired
                                      );
                }
                Console.ReadLine(); 
                */
            }

            sbXML.Append("</EDI" + edi + ">");
            // TODO Write XML to File 
            XmlDocument xmlDoc = new XmlDocument();
            string strXML = sbXML.ToString();
            xmlDoc.LoadXml(strXML);
            var json = JsonConvert.SerializeXmlNode(xmlDoc);
            string outputFilenameXML = @"c:\EDIClass\EDIToXML\" + Path.GetFileName(ediFilename);
            string outputFilenameJson = @"c:\EDIClass\EDIToXML\" + Path.GetFileName(ediFilename);

            // add ponumber from BEG03 to filename 
            outputFilenameXML = outputFilenameXML.Replace(".edi",
                "_" + poNumber + ".xml");

            outputFilenameJson = outputFilenameJson.Replace(".edi",
                "_" + poNumber + ".json");
            Console.WriteLine("Se a creado un archivo XLM y un JSON en la ruta: " + outputFilenameXML);
            xmlDoc.Save(outputFilenameXML);
            /* var options = new JsonSerializerOptions { WriteIndented = true };
             string jsonString = Newtonsoft.Json.JsonSerializer.Serialize(json,options);

            */
           
            System.IO.File.WriteAllText(outputFilenameJson, FormatJson(json));
            /*
            **** Enviar Json por una peticion HTTP****
            
            var request = (HttpWebRequest)WebRequest.Create(url); 
            request.Method = "POST"; 
            request.ContentType = "application/xml; charset=utf-8"; 
            request.Timeout = 30000; 
            string json = JsonConvert.SerializeObject(product);
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            request.ContentLength = byteArray.Length;
            var dataStream = new StreamWriter(request.GetRequestStream()); 
            dataStream.Write(byteArray); 
            dataStream.Close();
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
               dataStream = response.GetResponseStream ();     
               using (Stream stream = response.GetResponseStream())
               {
                  using (StreamReader reader = new StreamReader(stream))
                  {
                     string responseFromServer = reader.ReadToEnd ();
                  }
               }
            }
             */
            Console.WriteLine(strXML);
            Console.WriteLine(FormatJson(json));
            Console.ReadLine();
            //json.Save Save(outputFilenameJson);

            Console.WriteLine("\n\n Press enter to end:");
            Console.ReadLine();
        }

        //Dar Serializar json
        private static string FormatJson(string json)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Newtonsoft.Json.Formatting.Indented);
        }
    }
}
