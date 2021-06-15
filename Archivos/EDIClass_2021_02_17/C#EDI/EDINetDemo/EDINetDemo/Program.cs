using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using indice.Edi;
using indice.Edi.Tests.Models;
using System.IO; 

namespace EDINetDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var po850 = buildPO850(); 
            serializePO850(po850);
            //deserializePO850(); 

            Console.WriteLine("\n\n Press enter to end:");
            Console.ReadLine();


        }

        public static PurchaseOrder_850 buildPO850()
        {
            //real world - you are get your data from 
            // 1) database (SQL) 
            // 2) call web service 
            // 3) Other file on disk that you have to convert. 

            var po850 = new PurchaseOrder_850();

            var order = new PurchaseOrder_850.Order();
            order.PurchaseOrderNumber = "NW001";
            order.PurchaseOrderDate = "20200706";
            order.PurchaseOrderTypeCode = "NE";

            order.ReferenceIdentificationQualifier = "WO";
            order.ReferenceIdentification = "WO388271";
            order.CurrencyCode = "USD";
            order.TransactionSetCode = "850";
            order.TransactionSetControlNumber = "0001";
            order.SegmentsCount = 54;
            order.TrailerTransactionSetControlNumber = order.TransactionSetControlNumber;
            order.TransportationTermscode = null; 
            order.LocationQualifier = null; 


            var lineitem = new PurchaseOrder_850.OrderDetail();
            lineitem.OrderLineNumber = "010";
            lineitem.QuantityOrdered = 40;
            lineitem.UnitOfMeasurement = "EA";
            lineitem.UnitPrice = 19.99M;
            lineitem.BuyersPartno = "ABC123";
            lineitem.BuyersPartnoQualifier = "VC"; 
            lineitem.ProductDescription = "Digital Widget";
            lineitem.MSG = null;

            var dateRequested = new PurchaseOrder_850.DTM();
            //dateRequested.DateTimeQualifier = "126";
            dateRequested.Date = DateTime.Parse("2020/07/13");

            var shipNoLaterDate = new PurchaseOrder_850.DTM();
            //dateRequested.DateTimeQualifier = "131";
            dateRequested.Date = DateTime.Parse("2020/07/20");

            lineitem.DeliveryRequestedDate = dateRequested;
            lineitem.ShipNoLaterDate = shipNoLaterDate;

            order.Items = new List<PurchaseOrder_850.OrderDetail>();
            order.Items.Add(lineitem);

            var gsGroup = new PurchaseOrder_850.FunctionalGroup();
            gsGroup.Date = DateTime.Now;
            gsGroup.ApplicationSenderCode = "Sender";
            gsGroup.ApplicationReceiverCode = "Receiver";
            gsGroup.FunctionalIdentifierCode = "PO";
            gsGroup.GroupControlNumber = 9001;
            gsGroup.GroupTrailerControlNumber = gsGroup.GroupControlNumber; 
            gsGroup.TransactionsCount = 1;
            gsGroup.Version = "004010";
            gsGroup.AgencyCode = "X";
            gsGroup.Orders = new List<PurchaseOrder_850.Order>();
            gsGroup.Orders.Add(order);


            po850.AcknowledgementRequested = false;
            po850.Component_Element_Separator = '*'; 
            po850.ControlNumber = 20001;
            po850.TrailerControlNumber = po850.ControlNumber; 
            po850.ControlVersion = 401; 
            po850.Control_Standards_ID = "U";
            po850.ID_Qualifier = "ZZ";
            po850.ID_Qualifier2 = "ZZ";
            po850.Sender_ID = "MEGACORP";
            po850.Receiver_ID = "WALMART";
            po850.Usage_Indicator = "P";
            po850.Groups = new List<PurchaseOrder_850.FunctionalGroup>();
            po850.Groups.Add(gsGroup);
            

            return po850; 


        }

        public static void serializePO850(PurchaseOrder_850 argPO850)
        {
            string outputEDIFilename = @"c:\Users\Carlos\Documents\GitHub\EDI\Archivos\EDIClass_2021_02_17\Samples\Sample_850_01_Orig.edi ";

            var grammar = EdiGrammar.NewX12();
            grammar.SetAdvice(
                segmentNameDelimiter: '*',
                dataElementSeparator: '*',
                componentDataElementSeparator: '>',
                segmentTerminator: '~',
                releaseCharacter: null,
                reserved: null,
                decimalMark: '.');

            // serialize to file.
            using (var textWriter = new StreamWriter(File.Open(outputEDIFilename, FileMode.Create)))
            {
                using (var ediWriter = new EdiTextWriter(textWriter, grammar))
                {
                    new EdiSerializer().Serialize(ediWriter, argPO850);
                }

            }
        }

        public static void deserializePO850()
        { 
            //string inputEDIFilename = @"c:\EDIClass\Downloads\EDI.Net-master\test\indice.Edi.Tests\Samples\x12.850.edi";
            string inputEDIFilename = @"c:\EDIClass\Samples\Sample_850_01_Orig.edi";

            var grammar = EdiGrammar.NewX12();
            grammar.SetAdvice(
                segmentNameDelimiter: '*',
                dataElementSeparator: '*',
                componentDataElementSeparator: '>',
                segmentTerminator: '~',
                releaseCharacter: null,
                reserved: null,
                decimalMark: '.');


           var po850 = default(PurchaseOrder_850);
            using (var stream = new StreamReader(inputEDIFilename))
            {
                po850 = new EdiSerializer().Deserialize<PurchaseOrder_850>(stream, grammar);

                // If you have only one ST and one PO/850 per file, 
                // you can use subscript 0, 
                // otherwise you will need loops here. 

                Console.WriteLine("PO Number:" +
                  po850.Groups[0].Orders[0].PurchaseOrderNumber);
                Console.WriteLine("PO Date:" +
                  po850.Groups[0].Orders[0].PurchaseOrderDate);

                foreach (var lineitem in po850.Groups[0].Orders[0].Items)
                {
                    Console.WriteLine(" LineItem:");
                    Console.WriteLine("  ItemNum=" + lineitem.OrderLineNumber);
                    Console.WriteLine("  Qty=" + lineitem.QuantityOrdered);
                    Console.WriteLine("  Price=" + lineitem.UnitPrice);
                    Console.WriteLine("  PartNo=" + lineitem.BuyersPartno);
                    Console.WriteLine("  Descr=" + lineitem.ProductDescription);
                }

                // 1) store PO into Database
                //    (create SQL statements or call Stored Proc) 
                // 2) write to XML for ERP system 
                // 3) call some web service 

            } // end of using





        }


    }
}
