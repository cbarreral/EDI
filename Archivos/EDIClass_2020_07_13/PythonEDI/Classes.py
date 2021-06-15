#
# Define Classes
#
#from xml_marshaller import xml_marshaller
import sys
from dicttoxml import dicttoxml
import enhancedminidom
from xml.dom.minidom import parseString

class Person:
    def __init__(self, firstname, lastname):
        self.firstName = firstname
        self.lastName = lastname


# person1 = Person("John", "Doe")
# personDict = vars(person1) # vars is pythonic way of converting to dictionary
# xml = dicttoxml(personDict, attr_type=False, custom_root='Person') # set root node to Person
# print(xml)
# dom = parseString(xml)
# xmlFormatted = dom.toprettyxml()
# print(xmlFormatted)
# print ("---- sys.exit() from Classes:Person----")
# sys.exit()


class PurchaseOrder850:

    def __init__(self):
        self.PONum = ""
        self.PODate = ""
        self.POType = ""
        self.VendorRefNum = "" #from REF*VR
        self.RefDemo = "" #from REF*AB
        self.ShipToName = "" #from N102
        self.ShipToCode = "" #from N103
        self.BuyerName = "" #from PER02
        self.BuyerPhone = "" #from PER04
        self.BuyerEmail = "" #from PER06
        self.LineItems = []

    def add_lineitem(self, lineItem):
        self.LineItems.append(lineItem);

    def print(self):
        print("PONum:" + self.PONum +
              " PODate:" + self.PODate +
              " Type:" + self.POType +
              " VendorRefNum:" + self.VendorRefNum +
              " RefDemo:" + self.RefDemo +
              " ShipToCode:" + self.ShipToCode +
              " ShipToName:" + self.ShipToName +
              " BuyerName:" + self.BuyerName +
              " Phone:" + self.BuyerPhone +
              " Email:" + self.BuyerEmail);
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



