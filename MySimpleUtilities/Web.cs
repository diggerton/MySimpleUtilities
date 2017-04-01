using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MySimpleUtilities
{
    public class Web
    {
        public async Task<string> GetDocumentFromWebAsync(string _url, HttpClient _client = null)
        {
            HttpClient client = null;
            if (_client == null)
                client = new HttpClient();
            else
                client = _client;

            string data;
            using (client)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _url);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                data = await response.Content.ReadAsStringAsync();
            }
            return data;
        }


    }

    public static class WebExtensions
    {
        /// <summary>
        /// Gets file name from HttpWebResponse
        /// </summary>
        /// <param name="_response"></param>
        /// <returns>File name.  Returns empty string if no information.</returns>
        public static string GetFileName(this HttpWebResponse _response)
        {
            var fileName = _response.GetResponseHeader("content-disposition")
                        ?.Split(';')
                        ?.FirstOrDefault(x => x.Trim().StartsWith("filename"))
                        ?.Split('=')[1]
                        ?.Replace("\"", "");

            return fileName ?? string.Empty;
        }

        /// <summary>
        /// Saves file from HttpWebResponse and returns path to file.
        /// </summary>
        /// <param name="_response"></param>
        /// <param name="_directory"></param>
        /// <param name="_useFileNameAsSubdirectory"></param>
        /// <param name="_replaceSpaceChars"></param>
        /// <param name="_replacementChar"></param>
        /// <returns>Full path to file saved.</returns>
        public static string SaveFile(
            this HttpWebResponse _response, string _directory, 
            bool _useFileNameAsSubdirectory = false, 
            bool _replaceSpaceChars = false, 
            char _replacementChar = '_')
        {
            var fileName = _replaceSpaceChars ? _response.GetFileName().Replace(' ', _replacementChar) : _response.GetFileName();
            string directory = _directory;
            string fullSavePath = _directory;

            if (_useFileNameAsSubdirectory)
            {
                var fileNameWithoutExtension = string.Empty;
                if(fileName.IndexOf('.') > -1)
                {
                    fileNameWithoutExtension = fileName.Substring(0, fileName.LastIndexOf('.'));
                }
                directory = Path.Combine(directory, fileNameWithoutExtension);
                fullSavePath = Path.Combine(directory, fileName);
            }

            fullSavePath = Path.Combine(directory, fileName);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (var fileStream = File.Create(fullSavePath))
            {
                _response.GetResponseStream().CopyTo(fileStream);
            }

            return fullSavePath;
        }
    }
}
