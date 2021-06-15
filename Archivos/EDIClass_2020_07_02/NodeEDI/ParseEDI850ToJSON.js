
module.exports = {
    ParseEDI850ToJSON: ParseEDI850ToJSON 
}

function ParseEDI850ToJSON () {
    /*
        Name: NodeEDI/index.js 
    Author: Neal Walters
        Date: 6/29/2020
    Description: Parse EDI File to object 
                Part of Udemy EDI course


    */ 
    var inEdiFilename = "Samples/Sample_850_01_Orig.edi";
    var outJSONFilename = "Samples/Sample_850_01_Orig.json";
    var outXMLFilename = "Samples/Sample_850_01_Orig.xml";
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

    for (rowNum = 0; rowNum < rows.length; rowNum ++) {
        console.log(rowNum + " " + rows[rowNum]);
        var elements = rows[rowNum].split(elementSeparator);
        for (elNum = 0; elNum < elements.length; elNum ++) {
            if (elNum == 0) {
                segmentName = elements[elNum]; 
            }
            var strElNum = String(elNum).padStart(2,'0');
            var elementName = segmentName + strElNum; 
            var elementValue = elements[elNum]; 
            //console.log("    " + elementName + ": " + elementValue); 

            // Parsing or Mapping 
            if (elementName=="BEG03") {
                PO850.PONumber = elementValue; 
            }
            if (elementName=="BEG05") {
                PO850.PODate = elementValue; 
            }
            if (elementName=="BEG02") {
                PO850.POType = elementValue; 
            }

            if (elementName=="ISA13") {
                PO850.ISAControlNum = elementValue; 
            }

            if (elementName=="GS06") {
                PO850.GSControlNum = elementValue; 
            }

            if (elementName=="REF01" || elementName == "PER01" || elementName == "N101") {
                lastRefCode = elementValue;   // example, lastRefCode="VR" 
            }
            if (lastRefCode == "VR" && elementName=="REF02") {
                PO850.VendorRefNum = elementValue; 
            }
            if (lastRefCode == "AB" && elementName=="REF02") {
                PO850.ABDemo = elementValue; 
            }
            
            if (lastRefCode == "BD" && elementName=="PER02") {
                PO850.BuyerName = elementValue; 
            }

            if (lastRefCode == "BD" && elementName=="PER04") {
                PO850.BuyerTelephone = elementValue; 
            }

            if (lastRefCode == "BD" && elementName=="PER06") {
                PO850.BuyerEmail = elementValue; 
            }

            if (lastRefCode == "ST" && elementName=="N102") {
                PO850.ShipTo = elementValue; 
            }

            if (lastRefCode == "ST" && elementName=="N104") {
                PO850.Warehouse = elementValue; 
            }


            if (elementName=="PO101") {
                PO850LineItem.LineItem = elementValue; 
            }
            if (elementName=="PO102") {
                PO850LineItem.Quantity = elementValue; 
            }
            if (elementName=="PO103") {
                PO850LineItem.UOM = elementValue; 
            }
            if (elementName=="PO104") {
                PO850LineItem.Price = elementValue; 
            }
            if (elementName=="PO105") {
                PO850LineItem.PriceBasis = elementValue; 
            }
            if (elementName=="PO107") {
                PO850LineItem.ProductCode = elementValue; 
            }
            if (elementName=="PID05") {
                PO850LineItem.ProductDescr = elementValue; 
            }
            if (elementName=="DTM02") {
                PO850LineItem.RequiredDate = elementValue; 
            }
            strPO850LineItemObject = JSON.stringify(PO850LineItem); 
            //console.log(strPO850LineItemObject); 

        
            if (elementName=="PO100" || elementName=='CTT00')  {
                countLineItem++; 
                //console.log("***** NEW LINE ITEM ***** Count=" + countLineItem + " ****** "); 
                // don't add the LineItem until after the first row has been built 
                if (countLineItem == 1) {
                    PO850.LineItem = [] 
                }
                if (countLineItem > 1) {
                    //console.log("Push LineItem:" + PO850LineItem.LineItem); 
                    PO850.LineItem.push(PO850LineItem);
                    PO850LineItem = {}; 
                }
            }



        } // element loop 

    }  // row loop 

    // Display our object 
    console.log("--------------Final JSON-------------------")
    strPO850Object = JSON.stringify(PO850, null, 2); 
    console.log(strPO850Object); 

    /* 
    Now that you have object what do do with it? Store it in your ERP (Enterprise Resource Planning) system. 
    1. Store it in your database (build SQL statements or call stored procedure)
    2. Call a Web Service provided by your ERP (Enterprise Resource Planning) system 
    3. Write file to disk in JSON, XML, CSV or some format so that another program can import it and process it. 
    */ 

    fs.writeFileSync(outJSONFilename, strPO850Object); 
    console.log("Wrote JSON to filename=" + outJSONFilename); 

    var js2xmlparser = require("js2xmlparser");
    var strXML = js2xmlparser.parse("PO850", PO850)
    fs.writeFileSync(outXMLFilename, strXML); 
    console.log("Wrote XML to filename=" + outXMLFilename); 

    console.log("\n\n\n\n\n");

}







