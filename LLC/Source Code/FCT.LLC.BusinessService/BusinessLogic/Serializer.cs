using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class Serializer
    {
        //method courtesy of:-http://stackoverflow.com/questions/4123590/serialize-an-object-to-xml
        public static string XMLSerialize<T>(T value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            try
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                var stringWriter = new StringWriter();
                using (var writer = XmlWriter.Create(stringWriter))
                {
                    xmlserializer.Serialize(writer, value);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured during XML serialization", ex);
            }
        }
    }
}
