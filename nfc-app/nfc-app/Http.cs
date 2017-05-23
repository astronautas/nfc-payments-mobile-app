using System.Net;
using System.Threading.Tasks;
using System.IO;

namespace nfc_app
{
    class Http
    {
        public static async Task<string> Request(string url, string json)
        {
            try
            {
                //var result = new Task<string>();
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(await httpWebRequest.GetRequestStreamAsync()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = await httpWebRequest.GetResponseAsync();
                string result = "";
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                return result;
            }
            catch
            {
                throw;
            }
        }
    }
}