/*
    Name: NodeEDI/index.js 
  Author: Neal Walters
    Date: 6/29/2020
 Description: Parse EDI File to object 
              Part of Udemy EDI course


*/ 
console.log("Start of index.js"); 
moduleParseEDI850ToJSON = require("./ParseEDI850ToJSON"); 
moduleParseEDI850ToEDIXML = require("./ParseEDI850ToEDIXML"); 
moduleParseJSONToEDI850 = require("./ParseJSONToEDI850");  //TODO 

//moduleParseEDI850ToJSON.ParseEDI850ToJSON(); 
moduleParseJSONToEDI850.ParseJSONToEDI850(); 
//moduleParseEDI850ToEDIXML.ParseEDI850ToEDIXML(); 

console.log("End of index.js"); 