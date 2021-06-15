#
#
#    Name: EDIDemo1.py
#  Author: Neal Walters
#    Date: 6/28/2020
# Description: Parse EDI File To Class/Object Structure (in Memory)
#              Part of Udemy EDI course
#

import sys
from xml_marshaller import xml_marshaller


#
# Define Classes
#

class Person:
    def __init__(self, firstname, lastname):
        self.firstName = firstname
        self.lastName = lastname


#person1 = Person("John", "Doe")
##strXmlPerson = xml_marshaller.dumps(person1);
#person = objectify.Element("Person")
#strXmlPerson = lxml.etree.tostring(person1, pretty_print=True)
#print(strXmlPerson)
#sys.exit()


class PurchaseOrder850:

    def __init__(self):
        self.PONum = ""
        self.PODate = ""
        self.POType = ""
        self.VendorRefNum = "" #from REF*VR
        self.RefDemo = "" #from REF*AB
        self.ShipToName = "" #from N102
        self.ShipToCode = "" #from N103
        self.LineItems = []

    def add_lineitem(self, lineItem):
        self.LineItems.append(lineItem);

    def print(self):
        print("PONum=" + self.PONum +
              " PODate:" + self.PODate +
              " Type: " + self.POType +
              " VendorRefNum: " + self.VendorRefNum +
              " RefDemo: " + self.RefDemo +
              " ShipToCode: " + self.ShipToCode +
              " ShipToName: " + self.ShipToName);
        for li in self.LineItems:
            print ("   " + li.LineNum + " Qty=" + str(li.Qty) + " " + li.UOM +
                   " Price=" + str(li.Price) + " Basis=" + li.Basis +
                   " Date:" + li.DateRequested + " PartNum=" + li.PartNum +
                   " Descr:" + li.Descr)

class PurchaseOrder850LineItem:
    # handle the PO1Loop - which consists of POLoop1, PID, & DTM
    def __init__(self):
        self.LineNum = ""
        self.Qty = ""
        self.UOM = ""
        self.Price = ""
        self.Basis = ""
        self.PartNum = ""
        self.Descr = ""
        self.DateRequested = ""



# demo how to use object/class variable
# po850 = PurchaseOrder850()
# po850.PONum = "Test"
# po850.POType = "NE"
# po850.PODate = "20200623"
#
# po850Line1 = PurchaseOrder850LineItem();
# po850Line1.LineNum = "01"
# po850Line1.Qty = 5
# po850Line1.UOM = "EA"
# po850Line1.Price = 99.97
# po850Line1.Basis = "PE"
# po850Line1.Descr = "Digital Widgets"
# po850Line1.DateRequested = "20200705"
# po850.add_lineitem(po850Line1)
#
# po850Line2 = PurchaseOrder850LineItem();
# po850Line2.LineNum = "02"
# po850Line2.Qty = 8
# po850Line2.UOM = "IN"
# po850Line2.Price = 45.99
# po850Line2.Basis = "PE"
# po850Line2.Descr = "14K Gold Chain"
# po850Line2.DateRequested = "20200709"
# po850.add_lineitem(po850Line2)
#
# po850.print()
#
# sys.exit()



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

outXMLFilename = "Samples/Sample_850_01_Orig_Python.xml"
fileObj = open(outXMLFilename, "w")
#strXML850 = pickle.dumps(po850);
strXML850 = xml_marshaller.dumps(po850);
print(strXML850)
numCharsWritten = fileObj.write(strXML850.decode("utf-8"))
print("\n ***** NumCharsWritten=" + str(numCharsWritten) + " to file:" + outXMLFilename)
fileObj.close()

