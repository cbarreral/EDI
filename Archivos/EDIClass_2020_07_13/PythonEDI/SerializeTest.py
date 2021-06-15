import sys
from dicttoxml import dicttoxml
import enhancedminidom
from xml.dom.minidom import parseString

class Person:
    def __init__(self, _firstname, _lastname, _address):
        self.firstName = _firstname
        self.lastName = _lastname
        self.homeAddress = _address
class Address:
    def __init__(self, _city, _state):
        self.city = _city
        self.state = _state

address1 = Address("Dallas", "TX")
person1 = Person("John", "Doe", address1)
personDict = vars(person1) # vars is pythonic way of converting to dictionary
xml = dicttoxml(personDict, attr_type=False, custom_root='Person') # set root node to Person
print(xml)
dom = parseString(xml)
xmlFormatted = dom.toprettyxml()
print(xmlFormatted)
