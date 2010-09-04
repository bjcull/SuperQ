using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace SuperQ.Helpers
{
    public static class Serialization
    {
        public static string ToXml(object obj)
        {
            XmlSerializer s = new XmlSerializer(obj.GetType());
            using (StringWriter writer = new StringWriter())
            {
                s.Serialize(writer, obj);
                return writer.ToString();
            }
        }

        public static T FromXml<T>(string data)
        {
           XmlSerializer s = new XmlSerializer(typeof(T));
           using (StringReader reader = new StringReader(data))
           {
               object obj = s.Deserialize(reader);
               return (T)obj;
           }
        }
    }
}
