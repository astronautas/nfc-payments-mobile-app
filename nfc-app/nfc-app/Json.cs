using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace nfc_app
{
    class Json
    {
        public static string Serialize<T>(T obj)
        {
            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            ser.WriteObject(ms, obj);
            byte[] jsonBytes = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(jsonBytes, 0, jsonBytes.Length);
        }

        public static T Deserialize<T>(string json)
        {
            T obj;
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            obj = (T)(ser.ReadObject(ms));
            ms.Close();
            return obj;
        }
    }
}