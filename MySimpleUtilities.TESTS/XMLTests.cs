using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;

namespace MySimpleUtilities.TESTS
{
    [TestClass]
    public class XMLTests
    {
        [TestMethod]
        public void ParseXMLToObject()
        {
            var obj = new TestObject();
            var xmlText =   "<item>"
                +               "<title>title element text</title>"
                +               "<link>https://www.google.com</link>"
                +               "<pubDate>Sun, 12 Feb 2017 09:10:57 +0000</pubDate>"
                +               "<description>description element text</description>"
                +           "</item>";

            obj = Utils.XML.ParseToObject<TestObject>(xmlText);

            Assert.AreEqual("title element text", obj.Title);
            Assert.AreEqual("https://www.google.com", obj.Link);
            Assert.AreEqual("Sun, 12 Feb 2017 09:10:57 +0000", obj.PubDate);
            Assert.AreEqual("description element text", obj.Description);
        }

        [Serializable, XmlRoot(ElementName = "item")]
        public class TestObject
        {
            [XmlElement(ElementName = "title")]
            public string Title { get; set; }

            [XmlElement(ElementName = "link")]
            public string Link { get; set; }

            [XmlElement(ElementName = "pubDate")]
            public string PubDate { get; set; }

            [XmlElement(ElementName = "description")]
            public string Description { get; set; }
        }
    }
}
