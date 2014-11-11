using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Entidades
{
    public class Helper
    {

        public static string ToCapitalize(string inputString)
        {

            System.Globalization.CultureInfo cultureInfo =

            System.Threading.Thread.CurrentThread.CurrentCulture;

            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;

            if (inputString != null)
            {
                return textInfo.ToTitleCase(inputString.ToLower());
            }
            else
                return inputString;

        }

        public static string SerializeObject(object myObject)
        {
            var stream = new MemoryStream();
            var xmldoc = new XmlDocument();
            var serializer = new XmlSerializer(myObject.GetType(), new Type[] { typeof(Entidades.ContratoEmpresas), typeof(Entidades.Contrato) });
            
            using (stream)
            {
                serializer.Serialize(stream, myObject);
                stream.Seek(0, SeekOrigin.Begin);
                xmldoc.Load(stream);
            }

            return xmldoc.InnerXml;
        }

        public static object DeSerializeObject(object myObject, Type objectType)
        {
            var xmlSerial = new XmlSerializer(objectType, new Type[] { typeof(Entidades.ContratoEmpresas), typeof(Entidades.Contrato) });
            var xmlStream = new StringReader(myObject.ToString());
            return xmlSerial.Deserialize(xmlStream);
        }

    }
}
