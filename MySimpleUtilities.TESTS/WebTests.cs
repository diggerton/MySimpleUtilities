using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using Syroot.Windows.IO;
using System.Net.Http;

namespace MySimpleUtilities.TESTS
{
    [TestClass]
    public class WebTests
    {
        [TestMethod]
        public void FileDownload()
        {
            var url = @"https://iptorrents.com/download.php/2246193/The.Detour.S02E12.720p.HEVC.x265-MeGusta.torrent?torrent_pass=3c6ef58d19a17a3149650d39de35fa5e";
            var web = new Web();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var path = response.SaveFile(@"C:\Temp", false, true);
            }
        }
    }
}
