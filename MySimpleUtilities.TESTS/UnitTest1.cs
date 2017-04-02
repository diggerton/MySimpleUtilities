using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace MySimpleUtilities.TESTS
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var str = " --path=E:\\Media\\TV E:\\Media\\TorrentFiles\\The.Expanse.S02E10.720p.HDTV.x264-SVA_[IPT].torrent";

            var s = Regex.Unescape(str);


        }
    }
}
