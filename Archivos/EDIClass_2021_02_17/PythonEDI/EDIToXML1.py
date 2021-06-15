#
#
#    Name: EDIToXML1.py
#  Author: Neal Walters
#    Date: 6/28/2020
# Description: Parse EDI File Directly to XML
#              Part of Udemy EDI course
#

import sys
from lxml import etree


def getXpathValue(doc, xpath):
    xpathresult = doc.xpath(xpath + "/text()", ysmart_strings=False)
    if len(xpathresult) == 0:
        result = ""
    else:
        #print(xpathResult)  # just to show we are getting back array/list
        result  = xpathresult[0]
    return result

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

xml = etree.Element("PO850")
doc = etree.ElementTree(xml)

#init temp variables
lastRefCode = ""
firstTime = True
foundLineItem = False
segment = None
PO1Loop = None

for row in rows:
    print("row=" + row + " length=" + str(len(row)))
    if len(row) > 0:
        elements = row.split(elementSeparator)
        for idx, el in enumerate(elements):

            if idx == 0:
                segmentName = el
                print("New segment: " + segmentName)
                if segmentName == "CTT":
                    foundLineItem = False
                if segmentName == "PO1":
                    foundLineItem = True
                    PO1Loop = etree.SubElement(xml, "PO1Loop")
                if foundLineItem:
                    segment = etree.SubElement(PO1Loop, segmentName)
                else:
                    segment = etree.SubElement(xml, segmentName)

            elementIDNum = str.format("{0:0=2d}", idx)
            elementID = segmentName + elementIDNum
            print("   ", elementID, el)

            if idx != 0:
                ediField = etree.SubElement(segment, elementID)
                ediField.text = el




print("\n================ End of parsing ====================")


outXMLFilename = "Samples/Sample_850_01_Direct_XML.xml"


strXML850 = etree.tostring(doc)
print(strXML850)

etree2 = etree.ElementTree(xml)
etree2.write(outXMLFilename, pretty_print=True)

#
# demo of how to XPath to Segments
#



# We know PO Number is BEG03
PONum = getXpathValue(xml, "/PO850/BEG/BEG03[1]")
print("PONum=" + PONum)

PODate = getXpathValue(xml, "/PO850/BEG/BEG05[1]")
print("PODate=" + PODate)
