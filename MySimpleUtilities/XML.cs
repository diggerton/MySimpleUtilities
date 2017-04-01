using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MySimpleUtilities
{
    public class XML
    {
        public T ParseToObject<T>(string _document)
        {
            var serializer = new XmlSerializer(typeof(T));
            var deserializedObject = (T)serializer.Deserialize(new StringReader(_document));
            return deserializedObject;
        }
    }
}
