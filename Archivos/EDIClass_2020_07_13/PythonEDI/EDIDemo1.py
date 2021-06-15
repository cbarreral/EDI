#
#
#    Name: EDIDemo1.py
#  Author: Neal Walters
#    Date: 6/28/2020
# Description: Parse EDI File To Class/Object Structure (in Memory)
#              Part of Udemy EDI course
#

import sys
import json  # for conversion to dictionary then to XML
# from xml_marshaller import xml_marshaller
from Classes import PurchaseOrder850
from Classes import PurchaseOrder850LineItem
from dicttoxml import dicttoxml
from lxml import etree
#from io import BytesIO
from io import StringIO

#import enhancepipdminidom
#from xml.dom.minidom import parseString  #prettyPrint routine was putting values on separate lins


#
# Start Reading and Parsing the EDI File
#

inEdiFilename = "Samples/Sample_850_01_Orig.edi"
with open(inEdiFilename, 'r') as file:
    fileContents = file.read()

fileContents = fileContents.replace("\n", "").replace("\r", "")
elementSeparator = fileContents[103:104]
rowSeparator = fileContents[105:106]

print("ElementSeparator=" + elementSeparator)
print("    RowSeparator=" + rowSeparator)


#print(fileContents)

rows = fileContents.split(rowSeparator)
#print(rows)

po850 = PurchaseOrder850()

#init temp variables
lastRefCode = ""
firstTime = True
foundLineItem = False

for row in rows:
    print(row)
    elements = row.split(elementSeparator)
    for idx, el in enumerate(elements):
        elementIDNum = str.format("{0:0=2d}", idx)
        if idx == 0:
           segmentName = el
        elementID = segmentName + elementIDNum
        print ("   ", elementID, el)
        if elementID == "BEG02":
            po850.POType = el
        if elementID == "BEG03":
            po850.PONum = el
        if elementID == "BEG05":
            po850.PODate = el

        if elementID  == "REF01":
            lastRefCode = el

        if lastRefCode == "VR" and elementID == "REF02" :
            po850.VendorRefNum = el
        if lastRefCode == "AB" and elementID == "REF02" :
            po850.RefDemo = el

        if elementID == "N101":
            lastRefCode = el
        if lastRefCode == "ST" and elementID == "N102" :
            po850.ShipToName = el
        if lastRefCode == "ST" and elementID == "N103" :
            po850.ShipToCode = el

        if elementID == "PO100":
            if not firstTime:
                po850.add_lineitem(po850Line)
            po850Line = PurchaseOrder850LineItem();
            firstTime = False
            foundLineItem = True

        if elementID == "PO101":
            po850Line.LineNum = el

        if elementID == "PO102":
            po850Line.Qty = el

        if elementID == "PO103":
            po850Line.UOM = el

        if elementID == "PO104":
            po850Line.Price = el

        if elementID == "PO105":
            po850Line.Basis = el

        if elementID == "PO107":
            po850Line.PartNum = el

        if elementID == "PID05":
            po850Line.Descr = el

        if elementID == "DTM02":
            po850Line.DateRequested = el

        if elementID == "CTT01":
            # Hand the last hanging POLine item that has not been written out yet
            if foundLineItem:
                po850.add_lineitem(po850Line)


print("\n================ End of parsing ====================")
po850.print()

#
# Now you a PO Object in memory, what do you do with it?
# Basically, the PO needs to be loaded into your system (ERP or database)
# 1) Serialize it to disk as XML or JSON for some other program to process
# 2) Build SQL statements, or call Stored Procedures to store in a database
# 3) Call an API/Library or a Web Service to add the PO to your system
#

# Demo - Serialize to string and write to disk using Pickle
# pickle.dump goes to file, pickle.dumps goes to a string

#
# So far, I have not found a good serializer that follows the
# exact structure of the Class Structure that we have
#

outXMLFilename = "Samples/Sample_850_01_Orig_Python_B.xml"
fileObj = open(outXMLFilename, "w")
#strXML850 = pickle.dumps(po850);
#strXML850 = xml_marshaller.dumps(po850);
#print(strXML850)

print("----- Serialize Object to Formatted XML String ---")
#po850Dict = vars(po850) # vars is pythonic way of converting to dictionary (but doesn't handle nested classes)
po850JsonObj = json.loads(json.dumps(po850, default=lambda x: x.__dict__))
xmlBytes = dicttoxml(po850JsonObj, attr_type=False, custom_root='PO850') # set root node to Person
print(xmlBytes)  # this is apparently a byte array (not a string)

#dom = parseString(xml)
#xmlFormatted = dom.toprettyxml()   # runs but not recommended, puts too much white space around element values
#print(xmlFormatted)

parser = etree.XMLParser(remove_blank_text=True, encoding='utf-8')
xmlString = xmlBytes.decode("utf-8")     # convert byes to string
xmlString = xmlString.replace('<?xml version="1.0" encoding="UTF-8" ?>', "")
fileObjTemp = StringIO(xmlString)            # create an in memory file object
tree = etree.parse(fileObjTemp, parser)      # input is the file object
xmlFormatted = etree.tostring(tree, pretty_print=True)
print(xmlFormatted.decode("utf-8"))  # if we don't decode here, it doesn't look formatted at all

# xml is bytes and needs to be decoded, and now, so does xmlFormatted
numCharsWritten = fileObj.write(xmlFormatted.decode("utf-8"))
print("\n ***** NumCharsWritten=" + str(numCharsWritten) + " to file:" + outXMLFilename)
fileObj.close()

