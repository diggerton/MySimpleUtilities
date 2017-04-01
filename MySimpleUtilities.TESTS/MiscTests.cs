using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MySimpleUtilities.TESTS
{
    [TestClass]
    public class MiscTests
    {
        [TestMethod]
        public void ComputeMD5Hash()
        {
            var testString = "Hi there!";

            var hashString = Utils.Misc.ComputeMD5Hash(testString);

            Assert.IsNotNull(hashString);
        }
        [TestMethod]
        public void ComputeSHA256Hash()
        {
            var testString = "Hi there!";

            var hashString = Utils.Misc.ComputeSHA256Hash(testString);

            Assert.IsNotNull(hashString);
        }
        [TestMethod]
        public void GetSpecialFolderPath()
        {
            var specialFolder = Environment.SpecialFolder.ApplicationData;

            var specialFolderPath = Environment.GetFolderPath(specialFolder);

            Assert.IsTrue(specialFolderPath.EndsWith("AppData\\Roaming"));
        }
    }
}
