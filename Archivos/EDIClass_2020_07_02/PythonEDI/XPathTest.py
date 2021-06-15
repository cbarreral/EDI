from lxml import etree

root = etree.XML("<person><firstname>John</firstname><lastname>Doe</lastname></person>")
find_text = etree.XPath("/person/firstname/text()", smart_strings=False)
firstName = find_text(root)[0]
print("Firstname=" + firstName)