#
#
#    Name: EDIDemo1.py
#  Author: Neal Walters
#    Date: 6/28/2020
# Description: Create an EDI 850 from an object in memory
#
#

import sys
#from xml_marshaller import xml_marshaller
from Classes import PurchaseOrder850
from Classes import PurchaseOrder850LineItem
from dicttoxml import dicttoxml
import enhancedminidom
from xml.dom.minidom import parseString
from lxml import etree

def stripSpecialCharacters(text, elementSeparator, segmentSeparator):
    outText = text.replace(elementSeparator, "")
    outText = outText.replace(segmentSeparator, "")
    return outText

def getXpathValue(doc, xpath):
    xpathresult = doc.xpath(xpath + "/text()", ysmart_strings=False)
    if len(xpathresult) == 0:
        result = ""
    else:
        #print(xpathResult)  # just to show we are getting back array/list
        result  = xpathresult[0]
    return result

#
# Create an object from our two classes
#
print("---Building po850 object------")
po850 = PurchaseOrder850()
po850.PONum = "NW3001"
po850.POType = "NE"
po850.PODate = "20200623"
po850.RefDemo = "75222"
po850.VendorRefNum = "88444"
po850.ShipToCode = "SPID"
po850.ShipToName = "MegaCorp Main Warehouse *** C/O Fred Jones"
po850.BuyerName = "Jane Doe"
po850.BuyerPhone = "972-555-1212"
po850.BuyerEmail = "JaneDoe@gmail.com"

# add two line items to the PO
po850Line1 = PurchaseOrder850LineItem();
po850Line1.LineNum = "01"
po850Line1.Qty = 5
po850Line1.UOM = "EA"
po850Line1.Price = 99.97
po850Line1.Basis = "PE"
po850Line1.PartNum = "AB01"
po850Line1.Descr = "Digital *@* Widgets"
po850Line1.DateRequested = "20200705"
po850.add_lineitem(po850Line1)

po850Line2 = PurchaseOrder850LineItem();
po850Line2.LineNum = "02"
po850Line2.Qty = 8
po850Line2.UOM = "IN"
po850Line2.Price = 45.99
po850Line2.Basis = "PE"
po850Line2.PartNum = "GC02"
po850Line2.Descr = "14K Gold Chain - Weight=~.08Cm"
po850Line2.DateRequested = "20200709"
po850.add_lineitem(po850Line2)

po850.print()

# Show how to build BEG Segment

#begSegment = "BEG*00*" + po850.POType + "*" + po850.PONum + "**" +  po850.PODate + "~"
#print ("Sample BEG segment")
#print(begSegment)

po850XML = ""
segmentCounter = 0

##### ST Segment #####
stSegmentXML = "<ST><ST00>ST</ST00><ST01>850</ST01><ST02>0001</ST02></ST>"
po850XML += stSegmentXML
segmentCounter += 1

##### BEG Segment #####
begSegmentXML = """
<BEG>
   <BEG00>BEG</BEG00>
   <BEG01>00</BEG01>
   <BEG02>&POType</BEG02>
   <BEG03>&PONumber</BEG03>
   <BEG04/>
   <BEG05>&PODate</BEG05>
</BEG>"""
begSegmentXML = begSegmentXML.replace("&POType", po850.POType)
begSegmentXML = begSegmentXML.replace("&PONumber", po850.PONum)
begSegmentXML = begSegmentXML.replace("&PODate", po850.PODate)
segmentCounter += 1

print(begSegmentXML)
po850XML += begSegmentXML


##### REF SEgments #####

refSegmentsXML = """
<REF>
   <REF00>REF</REF00>
   <REF01>AB</REF01>
   <REF02>&ABDemo</REF02>
</REF>
<REF>
   <REF00>REF</REF00>
   <REF01>VR</REF01>
   <REF02>&VendorRefNum</REF02>
</REF>"""
segmentCounter = segmentCounter + 2
refSegmentsXML = refSegmentsXML.replace("&ABDemo", po850.RefDemo)
refSegmentsXML = refSegmentsXML.replace("&VendorRefNum", po850.VendorRefNum)
po850XML += refSegmentsXML

##### PER Segment #####
perSegmentXML = """
<PER>
   <PER00>PER</PER00>
   <PER01>BD</PER01>
   <PER02>&BuyerName</PER02>
   <PER03>TE</PER03>
   <PER04>&BuyerPhone</PER04>
   <PER05>EM</PER05>
   <PER06>&BuyerEmail</PER06>
</PER>"""
perSegmentXML = perSegmentXML.replace("&BuyerName", po850.BuyerName);
perSegmentXML = perSegmentXML.replace("&BuyerPhone", po850.BuyerPhone);
perSegmentXML = perSegmentXML.replace("&BuyerEmail", po850.BuyerEmail);
po850XML += perSegmentXML
segmentCounter += 1

##### PER Segment #####

n1SegmentXML = """
<N1>
  <N100>N1</N100>
  <N101>ST</N101>
  <N102>&ShipToName</N102>
  <N103>92</N103>
  <N104>&Warehouse</N104>
</N1>"""
n1SegmentXML = n1SegmentXML.replace("&ShipToName", po850.ShipToName);
n1SegmentXML = n1SegmentXML.replace("&Warehouse", po850.ShipToCode);
po850XML += n1SegmentXML
segmentCounter += 1



##### Repeating Line Items #####

for li in po850.LineItems:
    print("   " + li.LineNum + " Qty=" + str(li.Qty) + " " + li.UOM +
          " Price=" + str(li.Price) + " Basis=" + li.Basis +
          " Date:" + li.DateRequested + " PartNum=" + li.PartNum +
          " Descr:" + li.Descr)

    poloop1SegmentXML = """
    <PO1>
        <PO100>PO1</PO100>
        <PO101>&LineItem</PO101>
        <PO102>&Quantity</PO102>
        <PO103>&UOM</PO103>
        <PO104>&Price</PO104>
        <PO105>&Basis</PO105>
        <PO106>VC</PO106>
        <PO107>&ProductCode</PO107>
      </PO1>
      <PID>
        <PID00>PID</PID00>
        <PID01>F</PID01>
        <PID02/>
        <PID03/>
        <PID04/>
        <PID05>&ProductDescr</PID05>
      </PID>
      <DTM>
        <DTM00>DTM</DTM00>
        <DTM01>106</DTM01>
        <DTM02>&RequiredDate</DTM02>
      </DTM>"""
    poloop1SegmentXML = poloop1SegmentXML.replace("&LineItem", li.LineNum)
    poloop1SegmentXML = poloop1SegmentXML.replace("&Quantity", str(li.Qty))
    poloop1SegmentXML = poloop1SegmentXML.replace("&UOM", li.UOM)
    poloop1SegmentXML = poloop1SegmentXML.replace("&Price", str(li.Price))
    poloop1SegmentXML = poloop1SegmentXML.replace("&Basis", li.Basis)

    poloop1SegmentXML = poloop1SegmentXML.replace("&ProductCode", li.PartNum)
    poloop1SegmentXML = poloop1SegmentXML.replace("&ProductDescr", li.Descr)
    poloop1SegmentXML = poloop1SegmentXML.replace("&RequiredDate", li.DateRequested)

    po850XML += poloop1SegmentXML
    segmentCounter = segmentCounter + 3

##### Final Segments #####
finalSegmentsXML =   """
    <CTT> 
       <CTT00>CTT</CTT00> 
       <CTT01>&CountLineItems</CTT01> 
    </CTT> 
    <SE> 
       <SE00>SE</SE00> 
       <SE01>&segmentCounter</SE01> 
       <SE02>0001</SE02> 
    </SE>"""
segmentCounter = segmentCounter + 2
finalSegmentsXML = finalSegmentsXML.replace("&CountLineItems", str(len(po850.LineItems)));
finalSegmentsXML = finalSegmentsXML.replace("&segmentCounter", str(segmentCounter));

po850XML += finalSegmentsXML

#######################################################
# Print Final XML built
#######################################################

po850XML = "<EDI>" + po850XML + "</EDI>"

print("---- XML Built So Far----")
print(po850XML)

#######################################################
# Convert EDI/XML to EDI
#######################################################

elementSeparator = "|"
rowSeparator = "^"
ediString = ""

xmlRoot = etree.fromstring(po850XML)

#xpath1 = "*"  # /*/*  from document
#nodes = tree.findall(xpath1)
for childSegment in xmlRoot:
    print(childSegment.tag)
    for elIndex, elementSegment in enumerate(childSegment):
        elementSegmentValue = getXpathValue(elementSegment, ".")
        elementSegmentValue = stripSpecialCharacters(elementSegmentValue, elementSeparator, rowSeparator)
        print(elementSegment.tag, elementSegmentValue)
        ediString += elementSegmentValue
        print (elIndex, len(childSegment))
        if (elIndex != len(childSegment) - 1):
            ediString += elementSeparator
    ediString += rowSeparator
    print(ediString)

#print("---- EDI String----")
#ediStringFormatted =ediString.replace("~","~\r\n")
#print(ediString)

outXMLFilename = "Samples/Sample_850_Output_Temp.xml"
fileObj = open(outXMLFilename, "w")
#numCharsWritten = fileObj.write(po850XML.decode("utf-8"))
numCharsWritten = fileObj.write(po850XML)
print("\n ***** NumCharsWritten=" + str(numCharsWritten) + " to file:" + outXMLFilename)
fileObj.close()

outEDIFilename = "Samples/Sample_850_Output_From_Object.edi"
fileObj = open(outEDIFilename, "w")
#numCharsWritten = fileObj.write(po850XML.decode("utf-8"))
numCharsWritten = fileObj.write(ediString)
print("\n ***** NumCharsWritten=" + str(numCharsWritten) + " to file:" + outEDIFilename)
fileObj.close()
