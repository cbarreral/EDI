using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Xml.Schema;
using System.IO;
using System.Xml.Serialization;
using System.Text;

namespace EDIX12Parser
{
    static class XMLHelper
    {
        // http://mylifeismymessage.net/c-routine-to-format-pretty-print-xml-for-biztalk/ 
        public static string PrettyPrint(this XmlDocument doc)
        {
            var stringWriter = new StringWriter(new StringBuilder());
            var xmlTextWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented };
            doc.Save(xmlTextWriter);
            return stringWriter.ToString();
        }

        // from http://mylifeismymessage.net/xml-serializerdeserializer/ 
        public static String SerializeObject(Object pObject)
        {

            try
            {

                String XmlizedString = null;
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xs = new XmlSerializer(pObject.GetType());
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                xs.Serialize(xmlTextWriter, pObject);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
                return XmlizedString;

            }

            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return null;
            }

        }
        /// <summary>
        /// Method to reconstruct turn XMLString back into Object
        /// </summary>
        /// 
        /// 

        public static Object DeserializeObject(String pXmlizedString, Type classType)
        {

            XmlSerializer xs = new XmlSerializer(classType);
            MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return xs.Deserialize(memoryStream);

        }

        private static String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        private static Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
    }
}
