
module.exports = {
    ParseEDI850ToEDIXML: ParseEDI850ToEDIXML 
}

function ParseEDI850ToEDIXML () {
    /*
        Name: NodeEDI/index.js 
    Author: Neal Walters
        Date: 6/29/2020
    Description: Parse EDI File to object 
                Part of Udemy EDI course


    */ 
    var inEdiFilename = "Samples/Sample_850_01_Orig.edi";
    var outXMLFilename = "Samples/Sample_850_01_Orig_EDIXML.xml";
    var fs = require('fs');

    var fileContents = fs.readFileSync(inEdiFilename, 'utf8');
    console.log(fileContents); 
    console.log("--------------Replace--------------------------------------")
    fileContents = fileContents.replace(/[\r\n]/g, '')
    console.log(fileContents);

    var elementSeparator = fileContents.substring(103,104)
    var rowSeparator = fileContents.substring(105,106)

    console.log("--------------Separators--------------------------------------")
    console.log("elementSeparator=" + elementSeparator); 
    console.log("rowSeparator=" + rowSeparator); 

    var rows = fileContents.split(rowSeparator);
    var segmentName = ""; 

    var PO850LineItem = {}; 
    //var PO850 = {"LineItem": []}; 
    var PO850 = {}; 

    var lastRefCode = "";
    var firstTime = true; 
    var countLineItem = 0; 
    var currentGroup = ""; 

    var builder = require('xmlbuilder');
    var xmlRoot = builder.create('PO850');

    for (rowNum = 0; rowNum < rows.length - 1; rowNum ++) {
        console.log(rowNum + " " + rows[rowNum]);
        var elements = rows[rowNum].split(elementSeparator);
        for (elNum = 0; elNum < elements.length; elNum ++) {
            if (elNum == 0) {
                segmentName = elements[elNum]; 
    
                if (segmentName == "CTT") {
                    currentGroup = ""; 
                }
    
                if (segmentName == "PO1")
                {
                    currentGroup = "POLoop1"; 
                    var xmlPOLoop1 = xmlRoot.ele(currentGroup);
                }

                // Put segment name under either the root or the POLoop, depending on where we are. 
                if (currentGroup == "POLoop1") {
                    var xmlSegment = xmlPOLoop1.ele(segmentName);
                }    
                else 
                {
                    var xmlSegment = xmlRoot.ele(segmentName);
                }
            }

            var strElNum = String(elNum).padStart(2,'0');
            var elementName = segmentName + strElNum; 
            var elementValue = elements[elNum]; 


            console.log("    " + elementName + ": " + elementValue); 
            var xmlElement = xmlSegment.ele(elementName,null,elementValue);
 


        } // element loop 

    }  // row loop 

    // Display our object 
    console.log("--------------Final XML-------------------")
    strPO850Object = JSON.stringify(PO850, null, 2); 
    console.log(strPO850Object); 

    /* 
    Now that you have object what do do with it? Store it in your ERP (Enterprise Resource Planning) system. 
    1. Store it in your database (build SQL statements or call stored procedure)
    2. Call a Web Service provided by your ERP (Enterprise Resource Planning) system 
    3. Write file to disk in JSON, XML, CSV or some format so that another program can import it and process it. 
    */ 

    var strXml = xmlRoot.end({ pretty: true});
    console.log(strXml);
    fs.writeFileSync(outXMLFilename, strXml); 
    console.log("Wrote XML to filename=" + outXMLFilename); 

    console.log("\n\n\n\n\n");

}







