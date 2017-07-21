using holaMundoOpenCV.Class;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace vertecDmgDtctr.Clases
{
    class restServices
    {

        public bool Validate(string email, string password)
        {
            string url = "http://vertecmx.com/API/users";
            string apiKey = "qhXkNkwupto4e291116";
            bool status = false;
            try
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(url + "?correo=" + email + "&password=" + password + "&VERTEC-KEY=" + apiKey);
                StreamReader reader = new StreamReader(stream);
                Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(reader.ReadLine());
                status = ((bool)jObject["logged"]);
                string lavariable = reader.ReadLine();
                Console.WriteLine("lavariable: "+lavariable);
                Console.WriteLine("id:"+jObject["id"]);
                
                if (status)
                {
                    Console.WriteLine("Logged: " + status);
                    User.user_id = (int)jObject["id"];
                    return status;
                }
                return status;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
                return status;
            }
        }
        public void Send(int id, string type, string description, string timeIn, string timeOut, int damage, float meanprocessed, float meanfilled, float entropyprocessed, float entropyfilled, string imei, string manufacturer, string model, string version, string serialNumber)
        {

            Console.WriteLine("Post Method");
            Console.WriteLine("id: "+id);
            Console.WriteLine("type: " + type);
            Console.WriteLine("desc: " + description);
            Console.WriteLine("ti: " + timeIn);
            Console.WriteLine("to: " + timeOut);
            Console.WriteLine("da: " + damage);
            Console.WriteLine("mf: " + meanfilled);
            Console.WriteLine("mp: " + meanprocessed);
            Console.WriteLine("ep: " + entropyprocessed);
            Console.WriteLine("ef: " + entropyfilled);
            Console.WriteLine("imei: " + imei);
            Console.WriteLine("manuf: " + manufacturer);
            Console.WriteLine("model: " + model);
            Console.WriteLine("ver_: " + version);
            Console.WriteLine("sm: " + serialNumber);

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("user_id", id.ToString());
            data.Add("timeIn", timeIn);
            data.Add("timeOut", timeOut);
            data.Add("danio", damage.ToString());
            data.Add("meanprocessed", meanprocessed.ToString());
            data.Add("meanfilled", meanfilled.ToString());
            data.Add("entropyprocessed", entropyprocessed.ToString());
            data.Add("entropyfilled", entropyfilled.ToString());
            data.Add("imei", imei);
            data.Add("fabricante", manufacturer);
            data.Add("modelo", model);
            data.Add("version", version);
            data.Add("serial", serialNumber);
            data.Add("type", type);
            data.Add("description", description);
            HttpPostRequest("http://www.vertecmx.com/API/user_device", data);
        }
        public void HttpPostRequest(string url, Dictionary<string, string> postParameters)
        {
            string postData = "";
            foreach (string key in postParameters.Keys)
            { postData += HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(postParameters[key]) + "&"; }
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            byte[] data = Encoding.ASCII.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.Headers.Add("VERTEC-KEY", "qhXkNkwupto4e291116");
            Stream responseStream = request.GetRequestStream();
            responseStream.Write(data, 0, data.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            JObject jObject = JObject.Parse(reader.ReadLine());
            string responsejson = ((string)jObject["response"]);
            Console.WriteLine("Response from server: "    + responsejson);
            //Console.WriteLine(reader.ReadToEnd());
            reader.Close();
            stream.Close();
            response.Close();
            responseStream.Close();
        }
    }
}
