
module.exports = {
    ParseJSONToEDI850: ParseJSONToEDI850 
}

function ParseJSONToEDI850 () {
    /*
        Name: NodeEDI/index.js 
    Author: Neal Walters
        Date: 6/29/2020
    Description: Parse JSON to EDI 850. 
                 Part of Udemy EDI course


    */ 
    var inJSONilename = "Samples/Sample_850_01_Orig.json";
    var outEDIFilename = "Samples/Sample_850_02_Generated_EDI.edi";
    var fs = require('fs');

    var fileContents = fs.readFileSync(inJSONilename, 'utf8');
    console.log("FileContents="); 
    console.log(fileContents); 
    var PO850 = JSON.parse(fileContents); 

    console.log("Confirm JSON.Parse working:"); 
    console.log("   PONum=" + PO850.PONumber); 
    console.log("  PODate=" + PO850.PODate); 
    console.log("   Buyer=" + PO850.BuyerName); 

    // Display Loop 
    for (var i=0; i<PO850.LineItem.length; i++){
        var li = PO850.LineItem[i];
        console.log( "LineItem=" + li.LineItem + " Qty=" + li.Quantity + 
                       " Descr" + li.ProductDescr);

    }

    

    var elementSeparator= "`"; 
    var rowSeparator = "~"; 
    var segmentCounter = 0; 

    // TODO - build ISA/GSA Header - need a database or file to keep 
    // track of last control number - add 1 to it each time. 
    // We won't do this part. 


    // BEG Segment    : BEG*00*NE*1234567890**20080827~ 
    var begSegment = "BEG*00*" + PO850.POType + "*" + PO850.PONumber + "**" + PO850.PODate 
    console.log("BEG Segment:" + begSegment);

    var stSegmentXML = "<ST><ST00>ST</ST00><ST01>850</ST01><ST02>0001</ST02></ST>";

    var begSegmentXML = "<BEG>" + 
    "   <BEG00>BEG</BEG00>" + 
    "   <BEG01>00</BEG01>" + 
    "   <BEG02>&POType</BEG02>" + 
    "   <BEG03>&PONumber</BEG03>" + 
    "   <BEG04/>" + 
    "   <BEG05>&PODate</BEG05>" + 
    "</BEG>";
    begSegmentXML = begSegmentXML.replace("&POType",PO850.POType); 
    begSegmentXML = begSegmentXML.replace("&PONumber",PO850.PONumber); 
    begSegmentXML = begSegmentXML.replace("&PODate",PO850.PODate); 

    console.log(begSegmentXML); 

    //Two REF (Reference Segments), AB=ABDemo and VR=VendorReferenceNumber 
    //REF*AB*123456~
    //REF*VR*2046159~

    
    var refSegmentsXML =  "<REF>" + 
    "   <REF00>REF</REF00>" + 
    "   <REF01>AB</REF01>" + 
    "   <REF02>&ABDemo</REF02>" + 
    "</REF>" + 
    "<REF>" + 
    "   <REF00>REF</REF00>" + 
    "   <REF01>VR</REF01>" + 
    "   <REF02>&VendorRefNum</REF02>" + 
    "</REF>"; 
    refSegmentsXML = refSegmentsXML.replace("&VendorRefNum",PO850.VendorRefNum); 
    refSegmentsXML = refSegmentsXML.replace("&ABDemo",PO850.ABDemo); 
    console.log(refSegmentsXML); 


    var perSegmentXML = "<PER>" + 
    "<PER00>PER</PER00>" + 
    "   <PER01>BD</PER01>" + 
    "   <PER02>&BuyerName</PER02>" + 
    "   <PER03>TE</PER03>" + 
    "   <PER04>&BuyerPhone</PER04>" + 
    "   <PER05>EM</PER05>" + 
    "   <PER06>&BuyerEmail</PER06>" + 
    "</PER>"; 
    perSegmentXML = perSegmentXML.replace("&BuyerName",PO850.BuyerName); 
    perSegmentXML = perSegmentXML.replace("&BuyerPhone",PO850.BuyerTelephone); 
    perSegmentXML = perSegmentXML.replace("&BuyerEmail",PO850.BuyerEmail); 
    console.log(perSegmentXML); 

   var n1SegmentXML = "<N1>" + 
   "  <N100>N1</N100>" + 
   "  <N101>ST</N101>" + 
   "  <N102>&ShipTo</N102>" + 
   "  <N103>92</N103>" + 
   "  <N104>&Warehouse</N104>" + 
   "</N1>"; 
   n1SegmentXML = n1SegmentXML.replace("&ShipTo",PO850.ShipTo); 
   n1SegmentXML = n1SegmentXML.replace("&Warehouse",PO850.Warehouse); 
   console.log(n1SegmentXML); 

   po850XML = "<PO850> " + stSegmentXML + begSegmentXML + refSegmentsXML + perSegmentXML + n1SegmentXML; 
   segmentCounter = 6; 



   //------------Handle line items array 

   for (var i=0; i<PO850.LineItem.length; i++) {
        segmentCounter = segmentCounter + 3; 
        var li = PO850.LineItem[i];
        var poloop1SegmentXML = "  <PO1>" +
                   "    <PO100>PO1</PO100>" +
                   "    <PO101>&LineItem</PO101>" +
                   "    <PO102>&Quantity</PO102>" +
                   "    <PO103>&UOM</PO103>" +
                   "    <PO104>&Price</PO104>" +
                   "    <PO105>&PriceBasis</PO105>" +
                   "    <PO106>VC</PO106>" +
                   "    <PO107>&ProductCode</PO107>" +
                   "  </PO1>" +
                   "  <PID>" +
                   "    <PID00>PID</PID00>" +
                   "    <PID01>F</PID01>" +
                   "    <PID02/>" +
                   "    <PID03/>" +
                   "    <PID04/>" +
                   "    <PID05>&ProductDescr</PID05>" +
                   "  </PID>" +
                   "  <DTM>" +
                   "    <DTM00>DTM</DTM00>" +
                   "    <DTM01>106</DTM01>" +
                   "    <DTM02>&RequiredDate</DTM02>" +
                   "  </DTM>";
                   poloop1SegmentXML = poloop1SegmentXML.replace("&LineItem",li.LineItem); 
                   poloop1SegmentXML = poloop1SegmentXML.replace("&Quantity",li.Quantity); 
                   poloop1SegmentXML = poloop1SegmentXML.replace("&UOM",li.UOM); 
                   poloop1SegmentXML = poloop1SegmentXML.replace("&Price",li.Price); 
                   poloop1SegmentXML = poloop1SegmentXML.replace("&PriceBasis",li.PriceBasis); 


                   
                   poloop1SegmentXML = poloop1SegmentXML.replace("&ProductCode",cleanEdiText(li.ProductCode, elementSeparator, rowSeparator)); 
                   poloop1SegmentXML = poloop1SegmentXML.replace("&ProductDescr",cleanEdiText(li.ProductDescr, elementSeparator, rowSeparator)); 
                   poloop1SegmentXML = poloop1SegmentXML.replace("&RequiredDate",li.RequiredDate); 
                   po850XML += poloop1SegmentXML 

      
    }

    //--- any final closing elements with counts 
    segmentCounter = segmentCounter + 2; 
    var finalSegmentsXML =   "<CTT>" + 
    "   <CTT00>CTT</CTT00>" + 
    "   <CTT01>&CountLineItems</CTT01>" + 
    "</CTT>" + 
    "<SE>" + 
    "   <SE00>SE</SE00>" + 
    "   <SE01>&segmentCounter</SE01>" + 
    "   <SE02>0001</SE02>" + 
    "</SE>"; 


    finalSegmentsXML = finalSegmentsXML.replace("&CountLineItems", PO850.LineItem.length); 
    finalSegmentsXML = finalSegmentsXML.replace("&segmentCounter", segmentCounter); 
    //n1SegmentXML = n1SegmentXML.replace("&Warehouse",PO850.Warehouse); 
    console.log(finalSegmentsXML); 
    po850XML += finalSegmentsXML; 

    //--- close root element 
    
    po850XML += "</PO850>"; 
    //---- Show Pretty-Printed XML in Debug Window 
    var format = require('xml-formatter');
    var po850XMLFmt = format(po850XML,  {collapseContent: true}); 
    console.log("---------------Formatted temp XML for 850 ---------------"); 
    console.log (po850XMLFmt); 
 

   //-------------------------------------------- Start converting XML to EDI 
   cumEDIString = ""; 

   var xpathObj = require('xpath'), dom = require('xmldom').DOMParser
   var xpath = '/PO850/*';
   var xmlDoc = new dom().parseFromString(po850XML);
   var nodes = xpathObj.select(xpath,xmlDoc);
   for (var i=0; i < nodes.length; i++) {
       var node = nodes[i]; 
       console.log("i=" + i + " " + node.localName + " : " + node.toString()); 

       var xmlDoc2 = new dom().parseFromString(node.toString());
       var xpath2 = '/*/*';
       var ediEleNodes = xpathObj.select(xpath2,xmlDoc2);
       for (var j=0; j < ediEleNodes.length; j++) {
          var ediEleNode = ediEleNodes[j];
          var ediEleNodeValue = ""; 
          if (ediEleNode.firstChild != null) {
              ediEleNodeValue = ediEleNode.firstChild.toString(); 
          }
          console.log("    j=" + j + " " + ediEleNode.localName + " : " + ediEleNodeValue); 
          cumEDIString += ediEleNodeValue; 
          if (j < ediEleNodes.length - 1) {
            cumEDIString +=  elementSeparator; 
          }
          console.log("EDI=" + cumEDIString); 

       }
       cumEDIString += rowSeparator;   // replace last element separator with row separator 
       //console.log("EDI=" + cumEDIString); 
    }
                         







    // Display our XML string 
    console.log("--------------Final EDI -------------------"); 
    console.log(cumEDIString); 

    /* 
    Now that you have object what do do with it? Store it in your ERP (Enterprise Resource Planning) system. 
    1. Store it in your database (build SQL statements or call stored procedure)
    2. Call a Web Service provided by your ERP (Enterprise Resource Planning) system 
    3. Write file to disk in JSON, XML, CSV or some format so that another program can import it and process it. 
    */ 

    fs.writeFileSync(outEDIFilename, cumEDIString); 
    console.log("Wrote JSON to filename=" + outEDIFilename); 

    console.log("\n\n\n\n\n");

}


function cleanEdiText (text, elementSeparator, rowSeparator) {
    resultText = text.replaceAll(elementSeparator,''); 
    resultText = resultText.replaceAll(rowSeparator,''); 
    return resultText; 
}

String.prototype.replaceAll = function(search, replace){
    return this.replace(new RegExp(search, 'g'), replace)
 }





