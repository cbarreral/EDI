using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; 
using indice.Edi.Tests.Models;
using indice.Edi; 

namespace EDIDevDemo
{

    class Program
    {
        static void Main(string[] args)
        {
            //var inputEDIFilename = @"c:\EDIClass\Samples\Sample_850_01_Orig.edi";
            var inputEDIFilename = @"c:\Users\Carlos\Documents\GitHub\EDI\Archivos\EDIClass_2021_02_17\Samples\Sample_850_01_Orig.edi";
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
                // you can use subscripts, 
                // otherwise you will need loops here. 
                Console.WriteLine("PO Number:" + po850.Groups[0].Orders[0].PurchaseOrderNumber);
                Console.WriteLine("PO Date:" + po850.Groups[0].Orders[0].PurchaseOrderDate);

                foreach (var lineitem in po850.Groups[0].Orders[0].Items)
                {
                    Console.WriteLine(" LineItem:");
                    Console.WriteLine("  ItemNum=" + lineitem.OrderLineNumber);
                    Console.WriteLine("  Qty=" + lineitem.QuantityOrdered);
                    Console.WriteLine("  Price=" + lineitem.UnitPrice);
                    Console.WriteLine("  PartNo=" + lineitem.BuyersPartno); 
                    Console.WriteLine("  Descr=" + lineitem.ProductDescription);
                }
                // store PO into Database
                //    (create SQL statements or call Stored Proc) 
                // right to XML for ERP system 
                // call some web service 

            }
            Console.WriteLine("\n\n Press enter to end:");
            Console.ReadLine(); 
        }
    }
}
