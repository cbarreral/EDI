using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Xml; 
using System.Xml.Serialization;
using System.Configuration; 

namespace EDIX12Parser
{
    public class PurchaseOrder850
    {
        public string PONum;
        public string PODateText;
        public DateTime PODate; 
        public string POType;
        public string VendorNumber;
        public string BuyerName;
        public string BuyerTelephone; 
        public List<PurchaseOrder850LineItem> LineItems; 
    }
    public class PurchaseOrder850LineItem
    {
        public string lineItem;
        public int quantity;
        public string uom;
        public decimal price;
        public string basisOfUnitPrice;
        public string catalogNumber;
        public string description;
        public string dateRequiredTxt;
        public DateTime dateRequired; 
    }
    class Program
    {
        static void Main(string[] args)
        {
            //parseEDI850ToObject(); 
            //parseEDI850ToEDIXML(); 

            createEDI850XMLFromClass(); // both steps 
            //createEDIFromEDIXML(null);  // second step of converting EDIXML to EDI 
            Console.WriteLine("\n\n Press enter to end:");
            Console.ReadLine();
        }
        /// <summary>
        /// This function is not limited to 850. 
        /// It should take any file in the following format and convert to EDI. 
        /// The root name doesn't matter.  The children of the root should 
        /// be the segment name.  For the children of each segment, 
        /// it just looks at the values (not names).  So every element 
        /// must be accounted for, but may be blank. 
          ///<EDI850>
          ///<ST>
          ///  <ST01>850</ST01>
          ///  <ST02>0001</ST02>
          ///</ST>
          ///<BEG>
          ///  <BEG01>00</BEG01>
          ///  <BEG02>NE</BEG02>
          ///  <BEG03>1234567890</BEG03>
          ///  <BEG04>
          ///  </BEG04>
          ///  <BEG05>20080827</BEG05>
          ///</BEG>
          /// etc...
          ///</EDI850>
        /// </summary>
        public static void createEDIFromEDIXML(string inputXmlFilename)
        {

            if (String.IsNullOrEmpty(inputXmlFilename)) 
            {
                // Allow the other function to call use with a file name, 
                // or allow us to call this routine for testing without
                // a filename, and use this one for testing. 
                inputXmlFilename = @"c:\EDIClass\EDIToXML\Sample_Create_850_Out_1234567890.xml";
            }
            string outputEdiFilename = @"c:\EDIClass\XMLToEDI\Sample_Create_850_Out_1234567890.edi";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(inputXmlFilename);

            //TODO - get these from config file 
            string elementSeparator = "*";
            string segmentSeparator = "~"; 


            string xpath1 = "/*/*";
            XmlNodeList nodes = xmlDoc.SelectNodes(xpath1);
            StringBuilder sbEDI = new StringBuilder();

            //TODO - come up with a better way to handle ISA and GS headers. 
            //TODO - put todays date/time correctly in ISA/GS 
            //TODO - get control numbers from config file or some database 
            //       or maybe generate random numbers when testing??? 
            string isaHeader = "ISA*00*          *00*          *01*0011223456     *01*999999999      *950120*0147*U*00401*000000005*0*P" + elementSeparator + ">" + segmentSeparator;
            string gsHeader = "GS*PO***20200622*0900*001*X*004010" + segmentSeparator;
            isaHeader = isaHeader.Replace("*", elementSeparator).Replace("~", segmentSeparator);
            gsHeader = gsHeader.Replace("*", elementSeparator).Replace("~", segmentSeparator);

            sbEDI.Append(isaHeader);
            sbEDI.Append(gsHeader); 


            foreach (XmlNode node in nodes)
            {
                string segName = node.Name; 
                Console.WriteLine("Segment:" + segName);

                string xpath2 = "*";
                XmlNodeList elNodes = node.SelectNodes(xpath2);

                StringBuilder sbSegment = new StringBuilder();
                sbSegment.Append(segName);
                sbSegment.Append(elementSeparator);

                int loopCounter = 0; 
                foreach (XmlNode elNode in elNodes)
                {
                    loopCounter++; 
                    string elValue = getSafeElementValue(elNode);
                    elValue = stripEDISeparators(elValue, elementSeparator, segmentSeparator); 
                    sbSegment.Append(elValue);
                    
                    if (loopCounter != elNodes.Count)
                    { 
                        sbSegment.Append(elementSeparator);
                    }
                    

                    Console.WriteLine("   " + loopCounter +
                        " elValue:" + elValue + 
                        " Segment=" + sbSegment);
                    //Console.WriteLine(sbSegment); 
                }
                sbSegment.Append(segmentSeparator);
                Console.WriteLine(sbSegment);
                sbEDI.Append(sbSegment);
                //Console.ReadLine();   // todo remove 


            }  //end of segment loop 

            // Handle the two closing envelope segments 
            string geSegment = "GE*1*001~";
            string ieaSegment = "IEA*1*000000005~";

            geSegment = geSegment.Replace("*", elementSeparator).Replace("~", segmentSeparator);
            ieaSegment = ieaSegment.Replace("*", elementSeparator).Replace("~", segmentSeparator);

            sbEDI.Append(geSegment);
            sbEDI.Append(ieaSegment); 

            File.WriteAllText(outputEdiFilename, sbEDI.ToString()); 

        }

        public static void createEDI850XMLFromClass()
        {
            string xmlFilename = @"c:\EDIClass\EDIToXML\Sample_850_01_Orig_Serialized_1234567890_B.xml";

            // Example of how to deserialize back into an object: 
            XmlSerializer xs2 = new XmlSerializer(typeof(PurchaseOrder850));
            StreamReader sr2 = new StreamReader(xmlFilename);
            // deserialize and cast back to proper class
            PurchaseOrder850 po850 = (PurchaseOrder850)xs2.Deserialize(sr2);
            sr2.Close();
            Console.WriteLine("Deserialize completed");


            int intSegmentCounter = 0; 


            StringBuilder sbXML = new StringBuilder();
            sbXML.Append(@"<EDI850>
  <ST>
    <ST01>850</ST01>
    <ST02>0001</ST02>
  </ST>
");
            intSegmentCounter++;

            string xmlBeginSegment = @"
  <BEG>
    <BEG01>00</BEG01>
    <BEG02>&BEG02</BEG02>
    <BEG03>&BEG03</BEG03>
    <BEG04>
    </BEG04>
    <BEG05>&BEG05</BEG05>
  </BEG>
";
            // you could build edi directly, but I think it's less readable. 
            //string ediBeginSegment = @"BEG*00*" + po850.POType +
            //            "*" + po850.PONum + "*" + po850.PODate.ToString("yyyyMMdd"); 

            xmlBeginSegment = xmlBeginSegment.Replace("&BEG02",po850.POType);
            xmlBeginSegment = xmlBeginSegment.Replace("&BEG03", po850.PONum);
            xmlBeginSegment = xmlBeginSegment.Replace("&BEG05", po850.PODate.ToString("yyyyMMdd"));
            intSegmentCounter++;
            sbXML.Append(xmlBeginSegment);

            string xmlRefSegment = @"
  <REF>
    <REF01>VR</REF01>
    <REF02>&REF02</REF02>
  </REF>

";
            xmlRefSegment = xmlRefSegment.Replace("&REF02", po850.VendorNumber);
            intSegmentCounter++;
            sbXML.Append(xmlRefSegment);


            string xmlPerSegment = @"
  <PER>
    <PER01>BD</PER01>
    <PER02>&PER02</PER02>
    <PER03>TE</PER03>
    <PER04>&PER04</PER04>
  </PER>
";
            xmlPerSegment = xmlPerSegment.Replace("&PER02", po850.BuyerName);
            xmlPerSegment = xmlPerSegment.Replace("&PER04", po850.BuyerTelephone);
            intSegmentCounter++;
            sbXML.Append(xmlPerSegment);

            string xmlSegment = @"
  <N1>
    <N101>ST</N101>
    <N102>&N102</N102>
    <N103>&N103</N103>
    <N104>&N104</N104>
  </N1>
";

            xmlSegment = xmlSegment.Replace("&N102", ConfigurationManager.AppSettings.Get("N102"));
            xmlSegment = xmlSegment.Replace("&N103", ConfigurationManager.AppSettings.Get("N103"));
            xmlSegment = xmlSegment.Replace("&N104", ConfigurationManager.AppSettings.Get("N104"));
            intSegmentCounter++;
            sbXML.Append(xmlSegment);

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
                xmlSegment = @"
  <PO1>
    <PO101>&PO101</PO101>
    <PO102>&PO102</PO102>
    <PO103>&PO103</PO103>
    <PO104>&PO104</PO104>
    <PO105>&PO105</PO105>
    <PO106>&PO106</PO106>
    <PO107>&PO107</PO107>
  </PO1>
  <PID>
    <PID01>F</PID01>
    <PID02>
    </PID02>
    <PID03>
    </PID03>
    <PID04>
    </PID04>
    <PID05>&PID05</PID05>
  </PID>
  <DTM>
    <DTM01>106</DTM01>
    <DTM02>&DTM02</DTM02>
  </DTM>

";

                xmlSegment = xmlSegment.Replace("&PO101", item.lineItem);
                xmlSegment = xmlSegment.Replace("&PO102", item.quantity.ToString().Trim());
                xmlSegment = xmlSegment.Replace("&PO103", item.uom);
                xmlSegment = xmlSegment.Replace("&PO104", item.price.ToString().Trim());
                xmlSegment = xmlSegment.Replace("&PO105", item.basisOfUnitPrice);
                xmlSegment = xmlSegment.Replace("&PO106", "VC");
                xmlSegment = xmlSegment.Replace("&PO107", item.catalogNumber);
                //xmlSegment = xmlSegment.Replace("&PO108", item.lineItem);
                //xmlSegment = xmlSegment.Replace("&PO109", item.lineItem);

                
                xmlSegment = xmlSegment.Replace("&PID05", item.description);

                xmlSegment = xmlSegment.Replace("&DTM02", item.dateRequired.ToString("yyyyMMdd"));
                
                intSegmentCounter = intSegmentCounter + 3; 
                sbXML.Append(xmlSegment);

            }

            xmlSegment = @"
  <CTT>
    <CTT01>&CTT01</CTT01>
  </CTT>
  <SE>
    <SE01>&SE01</SE01>
    <SE02>0001</SE02>
  </SE>";
            intSegmentCounter = intSegmentCounter + 2; 
            xmlSegment = xmlSegment.Replace("&CTT01", po850.LineItems.Count.ToString());
            xmlSegment = xmlSegment.Replace("&SE01", intSegmentCounter.ToString().Trim());
            sbXML.Append(xmlSegment);



            // end logic 
            sbXML.Append(@"</EDI850>");

            Console.WriteLine("--- Interim EDI XML ---");
            Console.WriteLine(sbXML); 


            string strXML = sbXML.ToString();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strXML);

            string outputFilename = @"c:\EDIClass\EDIToXML\Sample_Create_850_Out_1234567890.xml";
            xmlDoc.Save(outputFilename);

            // the output of this function can be input to the next function 
            createEDIFromEDIXML(outputFilename); 

        }



        public static void parseEDI850ToObject()
        {
            string ediFilename = @"c:\EDIClass\Samples\Sample_850_01_Orig.edi";
            string ediFileContents = File.ReadAllText(ediFilename);
            bool verboseDebug = false;  // show extra debug when this is true 
                                        // and might have to press enter from console
                                        // after each segment. 

            //temporary variables for parsing 
            string currentRef01 = "";
            string currentPer01 = "";

            Console.WriteLine(ediFileContents);

            string elementSeparator = ediFileContents.Substring(103, 1);
            string segmentSeparator = ediFileContents.Substring(105, 1);

            ediFileContents = ediFileContents.Replace("\r", "").Replace("\n", "");

            Console.WriteLine("elementSeparator=" + elementSeparator);
            Console.WriteLine("segmentSeparator=" + segmentSeparator);

            PurchaseOrder850 po850 = new PurchaseOrder850();
            PurchaseOrder850LineItem lineitem = new PurchaseOrder850LineItem();
            po850.LineItems = new List<PurchaseOrder850LineItem>();

            string[] lines = ediFileContents.Split(char.Parse(segmentSeparator));
            Console.WriteLine("Number of lines=" + lines.Length);

            foreach (string line in lines)
            {
                Console.WriteLine("Line:" + line);
                string[] elements = line.Split(char.Parse(elementSeparator));
                int loopCounter = 0;
                string segment = "";
                string elNum = "";
                string elName = "";
                foreach (string el in elements)
                {
                    if (loopCounter == 0)
                    {
                        segment = el;
                        if (verboseDebug)
                        {
                            Console.WriteLine("Segment=" + segment); 
                        }

                    }
                    else
                    {
                        elNum = loopCounter.ToString("D2");
                        elName = segment + elNum;
                        if (verboseDebug)
                        {
                            Console.WriteLine(elName + "=" + el);
                        }

                        switch (elName)
                        {
                            case "BEG03":
                                po850.PONum = el;
                                break;
                            case "BEG05":
                                po850.PODateText = el;
                                //TODO Handle DateTime and validation Issues 
                                po850.PODate = DateTime.ParseExact(
                                     el, "yyyyMMdd", CultureInfo.InvariantCulture);
                                break;
                            case "BEG02":  // <-- Changed to BEG02 from BEG01 
                                po850.POType = el;
                                break;
                            case "REF01":
                                currentRef01 = el;
                                break;
                            case "REF02":
                                if (currentRef01 == "VR")
                                {
                                    po850.VendorNumber = el;
                                }
                                break;
                            case "PER01":
                                currentPer01 = el;
                                break;
                            case "PER02":
                                if (currentPer01 == "BD")
                                {
                                    po850.BuyerName = el;
                                }
                                break;
                            case "PER04":
                                if (currentPer01 == "BD")
                                {
                                    po850.BuyerTelephone = el;
                                }
                                break;
                            case "PO101":
                                lineitem.lineItem = el;
                                break;
                            case "PO102":
                                lineitem.quantity = Int32.Parse(el);
                                break;
                            case "PO103":
                                lineitem.uom = el;
                                break;
                            case "PO104":
                                lineitem.price = Decimal.Parse(el);
                                break;
                            case "PO105":
                                lineitem.basisOfUnitPrice = el;
                                break;
                            case "PO107":
                                lineitem.catalogNumber = el;
                                break;
                            case "PID05":
                                lineitem.description = el;
                                break;
                            case "DTM02":
                                lineitem.dateRequiredTxt = el;
                                lineitem.dateRequired = DateTime.ParseExact(
                                                        el, "yyyyMMdd",
                                                        CultureInfo.InvariantCulture);
                                po850.LineItems.Add(lineitem);
                                lineitem = new PurchaseOrder850LineItem();
                                break;

                        } // end Switch statement 
                    } //end if loopCounter = 0 
                    loopCounter++;
                } // end loop through elements
                if (verboseDebug)
                  {
                    showPO(po850); 
                    Console.ReadLine();
                  }
            } // end loop through rows  

            Console.WriteLine("\n\n---- EDI File Parsed, PO850 object built----"); 
            showPO(po850);

            string outputFilename = @"c:\EDIClass\EDIToXML\" +
                    Path.GetFileName(ediFilename);
            // add ponumber from BEG03 to filename 
            outputFilename = outputFilename.Replace(".edi",
                "_Serialized_" + po850.PONum + ".xml");


            XmlSerializer xs1 = new XmlSerializer(typeof(PurchaseOrder850));
            StreamWriter sw1 = new StreamWriter(outputFilename);
            xs1.Serialize(sw1, po850);
            sw1.Close();
            Console.WriteLine("Serialize completed");


            // Example of how to deserialize back into an object: 
            XmlSerializer xs2 = new XmlSerializer(typeof(PurchaseOrder850));
            StreamReader sr2 = new StreamReader(outputFilename);
            // deserialize and cast back to proper class
            PurchaseOrder850 po850Deserialized = (PurchaseOrder850)xs2.Deserialize(sr2);
            sr2.Close();
            Console.WriteLine("Deserialize completed");
            showPO(po850Deserialized); 

        }
        public static void parseEDI850ToEDIXML()
        {
            string ediFilename = @"c:\EDIClass\Samples\Sample_850_01_Orig.edi";
            string ediFileContents = File.ReadAllText(ediFilename);
            bool verboseDebug = false;  // show extra debug when this is true 
                                        // and might have to press enter from console
                                        // after each segment. 

            Console.WriteLine(ediFileContents);

            string elementSeparator = ediFileContents.Substring(103, 1);
            string segmentSeparator = ediFileContents.Substring(105, 1);

            ediFileContents = ediFileContents.Replace("\r", "").Replace("\n", "");

            Console.WriteLine("elementSeparator=" + elementSeparator);
            Console.WriteLine("segmentSeparator=" + segmentSeparator);

            PurchaseOrder850 po850 = new PurchaseOrder850();
            PurchaseOrder850LineItem lineitem = new PurchaseOrder850LineItem();
            po850.LineItems = new List<PurchaseOrder850LineItem>();

            string[] lines = ediFileContents.Split(char.Parse(segmentSeparator));
            Console.WriteLine("Number of lines=" + lines.Length);

            StringBuilder sbXML = new StringBuilder();
            sbXML.Append("<EDI>");
            bool boolSegmentOpen = false;
            bool boolPO1LoopOpen = false;
            int POLoopCounter = 0;
            string filenamePONumber = "";
            string segment = "";

            foreach (string line in lines)
            {
                Console.WriteLine("Line:" + line);
                string[] elements = line.Split(char.Parse(elementSeparator));
                int loopCounter = 0;
                string elNum = "";
                string elName = "";
                foreach (string el in elements)
                {
                    if (loopCounter == 0)
                    {
                        if (boolSegmentOpen)
                        {
                            sbXML.Append("</" + segment + ">");
                            boolSegmentOpen = false;

                        }
                        segment = el;
                        if (verboseDebug)
                        {
                            Console.WriteLine("Segment=" + segment);
                        }
                        if (! string.IsNullOrEmpty(segment))
                        { 
                            // Create a wrapper group called <PO1Loop> 
                            // around the three lines (PO1/PID/DTM) 
                            if (segment == "PO1" || segment == "CTT")
                            {
                                if (boolPO1LoopOpen)
                                {
                                    sbXML.Append("</PO1Loop>");
                                    boolPO1LoopOpen = false; 
                                }
                                if (segment != "CTT")
                                {
                                    POLoopCounter++; 
                                    sbXML.Append("<PO1Loop lineNumber='" +
                                        POLoopCounter + 
                                        "'>");
                                    boolPO1LoopOpen = true;
                                }
                            }
                            sbXML.Append("<" + segment + ">");
                        }
                        boolSegmentOpen = true; 

                    }
                    else
                    {
                        elNum = loopCounter.ToString("D2");
                        elName = segment + elNum;
                        sbXML.Append("<" + elName + ">" + 
                            el + 
                            "</" + elName + ">");
                        if (verboseDebug)
                        {
                            Console.WriteLine(elName + "=" + el);
                        }
                        if (elName == "BEG03")
                        {
                            filenamePONumber = el; 
                        }

                    } //end if loopCounter = 0 
                    loopCounter++;
                } // end loop through elements
                if (verboseDebug)
                {
                    showPO(po850);
                    Console.ReadLine();
                }
            } // end loop through rows  

            sbXML.Append("</EDI>");

            XmlDocument xmlDoc = new XmlDocument();
            string strXML = sbXML.ToString(); 
            xmlDoc.LoadXml(strXML);
            string xmlFormatted = XMLHelper.PrettyPrint(xmlDoc); 


            Console.WriteLine("\n\n---- EDI File Parsed, XML built----");
            Console.WriteLine(xmlFormatted); 

            string outputFilename = @"c:\EDIClass\EDIToXML\" +
                    Path.GetFileName(ediFilename);
            // add ponumber from BEG03 to filename 
            outputFilename = outputFilename.Replace(".edi",
                "_EDIXML_" + filenamePONumber + ".xml");

            File.WriteAllText(outputFilename, xmlFormatted);

        }



        public static string stripEDISeparators(string text, string elementSeparator, string segmentSeparator)
        {
            string newText = text.Replace(elementSeparator, "").Replace(segmentSeparator, "");
            if (newText != text)
            {
                Console.WriteLine("EDI Scrubbed value from:" + text);
                Console.WriteLine("EDI Scrubbed value to:  " + newText);
            }
            return newText; 
        }

        public static string getSafeElementValue(XmlNode xmlNode)
        {
            XmlNode firstChild = xmlNode.FirstChild;
            if (firstChild == null)
            {
                return ""; 
            }
            else
            {
                return firstChild.Value; 
            }
        }


        public static void showPO(PurchaseOrder850 argPo850)
        {
            Console.WriteLine("*** PONum=" + argPo850.PONum + " PODate=" + argPo850.PODate + " POType=" + argPo850.POType);
            Console.WriteLine("*** Vendor=" + argPo850.VendorNumber + " BuyerName=" + argPo850.BuyerName + " Telephone=" + argPo850.BuyerTelephone);
            foreach (PurchaseOrder850LineItem item in argPo850.LineItems)
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

        }
    }
}
