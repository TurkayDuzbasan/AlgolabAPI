using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace AlgolabAPI
{
    public class Program
    {
        public static string hostname = "https://www.algolab.com.tr";
        public static string apiurl = "https://www.algolab.com.tr/api";
        public static string websocketurl = "wss://www.algolab.com.tr/api/ws";
        public static string APIKEY = "API-";

        public static string URL_LOGIN_USER = "/api/LoginUser";
        public static string URL_LOGIN_CONTROL = "/api/LoginUserControl";
        public static string URL_GETEQUITYINFO = "/api/GetEquityInfo";
        public static string URL_GETSUBACCOUNTS = "/api/GetSubAccounts";
        public static string URL_INSTANTPOSITION = "/api/InstantPosition";
        public static string URL_TODAYTRANSACTION = "/api/TodaysTransaction";
        public static string URL_VIOPCUSTOMEROVERALL = "/api/ViopCustomerOverall";
        public static string URL_VIOPCUSTOMERTRANSACTIONS = "/api/ViopCustomerTransactions";
        public static string URL_SENDORDER = "/api/SendOrder";
        public static string URL_MODIFYORDER = "/api/ModifyOrder";
        public static string URL_DELETEORDER = "/api/DeleteOrder";
        public static string URL_DELETEORDERVIOP = "/api/DeleteOrderViop";
        public static string URL_SESSIONREFRESH = "/api/SessionRefresh";


        public static string HASH = "";

        public static string USERNAME = "";
        public static string PASSWORD = "";
        public static string SMSCODE = "";

        public static void Main(string[] args)
        {

            var login = LoginUser(USERNAME, PASSWORD);

            string token = login.Content.token;
            SMSCODE = "";
            var control = LoginControl(token, SMSCODE);
            HASH = control.Content.Hash;

           
            WebSocket.ConnectToWebsocket();

            string symbol = "TSKB";
            var Equity = GetEquityInfo(symbol);

            var SubAccounts = GetSubAccounts();

            var instantPosition = InstantPosition("");

            var todaysTransaction = TodaysTransaction("");

            var viopCustomerOverall = ViopCustomerOverall("");
            var viopCustomerTransactions = ViopCustomerTransactions("");

            var sendOrder = SendOrder("EKGYO", "BUY", "limit", "3.90", "1", true, true, "");
            string orderid = sendOrder.Content.ToString().Split(';')[0].Split(":")[1].Trim();

            var modifyOrder = ModifyOrder(orderid, "3.91", "1", false, "");

            var deleteOrder = DeleteOrder(orderid, "");

            var sessionRefresh = SessionRefresh();

            Console.ReadLine();
        }

        public  static Response LoginUser(string username,string password)
        {
            try
            {
                string eUsername = OpenSSLEncryptApi(username, APIKEY.Split('-')[1]);
                string ePassword = OpenSSLEncryptApi(password, APIKEY.Split('-')[1]);

                string postData = "{\"Username\":\"" + eUsername + "\",\"Password\":\"" + ePassword + "\"}";


                string result = string.Empty;

                var request = (HttpWebRequest)WebRequest.Create(apiurl+ URL_LOGIN_USER);

                var data = Encoding.UTF8.GetBytes(postData);

                request.Headers.Add("APIKEY", APIKEY);

                request.ContentType = "application/json; charset=utf-8";
                request.Method = "POST";
                request.Accept = "application/json; charset=utf-8";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return DeserializeJson<Response>(result);
            }
            catch (Exception ex)
            {
                return new Response() { Success = false, Message = ex.Message ,Content=ex};
            }
        }

        public static Response LoginControl(string token, string smscode)
        {
            try
            {
                string eToken = OpenSSLEncryptApi(token, APIKEY.Split('-')[1]);
                string eSmscode = OpenSSLEncryptApi(smscode, APIKEY.Split('-')[1]);

                string postData = "{\"token\":\"" + eToken + "\",\"Password\":\"" + eSmscode + "\"}";


                string result = string.Empty;

                var request = (HttpWebRequest)WebRequest.Create(apiurl + URL_LOGIN_CONTROL);

                var data = Encoding.UTF8.GetBytes(postData);

                request.Headers.Add("APIKEY", APIKEY);

                request.ContentType = "application/json; charset=utf-8";
                request.Method = "POST";
                request.Accept = "application/json; charset=utf-8";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return DeserializeJson<Response>(result);
            }
            catch (Exception ex)
            {
                return new Response() { Success = false, Message = ex.Message, Content = ex };
            }
        }

        public static Response GetEquityInfo(string symbol)
        {
            try
            {                

                string postData = "{\"symbol\":\"" + symbol + "\"}";


                string result = string.Empty;

                var request = (HttpWebRequest)WebRequest.Create(apiurl + URL_GETEQUITYINFO);

                var data = Encoding.UTF8.GetBytes(postData);

                request.Headers.Add("APIKEY", APIKEY);
                request.Headers.Add("Authorization", HASH);
                request.Headers.Add("Checker", ComputeSha256Hash(APIKEY + hostname + URL_GETEQUITYINFO+postData));
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "POST";
                request.Accept = "application/json; charset=utf-8";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return DeserializeJson<Response>(result);
            }
            catch (Exception ex)
            {
                return new Response() { Success = false, Message = ex.Message, Content = ex };
            }
        }

        public static Response GetSubAccounts()
        {
            try
            {

                string postData = "";


                string result = string.Empty;

                var request = (HttpWebRequest)WebRequest.Create(apiurl + URL_GETSUBACCOUNTS);

                var data = Encoding.UTF8.GetBytes(postData);

                request.Headers.Add("APIKEY", APIKEY);
                request.Headers.Add("Authorization", HASH);
                request.Headers.Add("Checker", ComputeSha256Hash(APIKEY + hostname + URL_GETSUBACCOUNTS + postData));
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "POST";
                request.Accept = "application/json; charset=utf-8";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return DeserializeJson<Response>(result);
            }
            catch (Exception ex)
            {
                return new Response() { Success = false, Message = ex.Message, Content = ex };
            }
        }

        public static Response InstantPosition(string Subaccount)
        {
            try
            {

                string postData = "{\"Subaccount\":\""+Subaccount+"\"}";


                string result = string.Empty;

                var request = (HttpWebRequest)WebRequest.Create(apiurl + URL_INSTANTPOSITION);

                var data = Encoding.UTF8.GetBytes(postData);

                request.Headers.Add("APIKEY", APIKEY);
                request.Headers.Add("Authorization", HASH);
                request.Headers.Add("Checker", ComputeSha256Hash(APIKEY + hostname + URL_INSTANTPOSITION + postData));
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "POST";
                request.Accept = "application/json; charset=utf-8";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return DeserializeJson<Response>(result);
            }
            catch (Exception ex)
            {
                return new Response() { Success = false, Message = ex.Message, Content = ex };
            }
        }

        public static Response TodaysTransaction(string Subaccount)
        {
            try
            {

                string postData = "{\"Subaccount\":\"" + Subaccount + "\"}";


                string result = string.Empty;

                var request = (HttpWebRequest)WebRequest.Create(apiurl + URL_TODAYTRANSACTION);

                var data = Encoding.UTF8.GetBytes(postData);

                request.Headers.Add("APIKEY", APIKEY);
                request.Headers.Add("Authorization", HASH);
                request.Headers.Add("Checker", ComputeSha256Hash(APIKEY + hostname + URL_TODAYTRANSACTION + postData));
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "POST";
                request.Accept = "application/json; charset=utf-8";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return DeserializeJson<Response>(result);
            }
            catch (Exception ex)
            {
                return new Response() { Success = false, Message = ex.Message, Content = ex };
            }
        }

        public static Response ViopCustomerOverall(string Subaccount)
        {
            try
            {

                string postData = "{\"Subaccount\":\"" + Subaccount + "\"}";


                string result = string.Empty;

                var request = (HttpWebRequest)WebRequest.Create(apiurl + URL_VIOPCUSTOMEROVERALL);

                var data = Encoding.UTF8.GetBytes(postData);

                request.Headers.Add("APIKEY", APIKEY);
                request.Headers.Add("Authorization", HASH);
                request.Headers.Add("Checker", ComputeSha256Hash(APIKEY + hostname + URL_VIOPCUSTOMEROVERALL + postData));
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "POST";
                request.Accept = "application/json; charset=utf-8";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return DeserializeJson<Response>(result);
            }
            catch (Exception ex)
            {
                return new Response() { Success = false, Message = ex.Message, Content = ex };
            }
        }

        public static Response ViopCustomerTransactions(string Subaccount)
        {
            try
            {

                string postData = "{\"Subaccount\":\"" + Subaccount + "\"}";


                string result = string.Empty;

                var request = (HttpWebRequest)WebRequest.Create(apiurl + URL_VIOPCUSTOMERTRANSACTIONS);

                var data = Encoding.UTF8.GetBytes(postData);

                request.Headers.Add("APIKEY", APIKEY);
                request.Headers.Add("Authorization", HASH);
                request.Headers.Add("Checker", ComputeSha256Hash(APIKEY + hostname + URL_VIOPCUSTOMERTRANSACTIONS + postData));
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "POST";
                request.Accept = "application/json; charset=utf-8";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return DeserializeJson<Response>(result);
            }
            catch (Exception ex)
            {
                return new Response() { Success = false, Message = ex.Message, Content = ex };
            }
        }

        public static Response SendOrder(string symbol,string direction,string pricetype,string price,string lot,bool sms,bool email, string Subaccount)
        {
            try
            {

                string postData = "{\"symbol\":\""+symbol+"\",\"direction\":\""+ direction+"\",\"pricetype\":\""+ pricetype + "\",\"price\":\""+ price + "\",\"lot\":\""+ lot + "\",\"sms\":"+ sms.ToString().ToLower() + ",\"email\":"+ email.ToString().ToLower() + ",\"subAccount\":\""+ Subaccount + "\"}";


                string result = string.Empty;

                var request = (HttpWebRequest)WebRequest.Create(apiurl + URL_SENDORDER);

                var data = Encoding.UTF8.GetBytes(postData);

                request.Headers.Add("APIKEY", APIKEY);
                request.Headers.Add("Authorization", HASH);
                request.Headers.Add("Checker", ComputeSha256Hash(APIKEY + hostname + URL_SENDORDER + postData));
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "POST";
                request.Accept = "application/json; charset=utf-8";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return DeserializeJson<Response>(result);
            }
            catch (Exception ex)
            {
                return new Response() { Success = false, Message = ex.Message, Content = ex };
            }
        }

        public static Response ModifyOrder(string id, string price, string lot, bool viop, string Subaccount)
        {
            try
            {

                string postData = "{\"id\":\"" + id + "\",\"price\":\"" + price + "\",\"lot\":\"" + lot + "\",\"viop\":" + viop.ToString().ToLower() + ",\"subAccount\":\"" + Subaccount + "\"}";


                string result = string.Empty;

                var request = (HttpWebRequest)WebRequest.Create(apiurl + URL_MODIFYORDER);

                var data = Encoding.UTF8.GetBytes(postData);

                request.Headers.Add("APIKEY", APIKEY);
                request.Headers.Add("Authorization", HASH);
                request.Headers.Add("Checker", ComputeSha256Hash(APIKEY + hostname + URL_MODIFYORDER + postData));
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "POST";
                request.Accept = "application/json; charset=utf-8";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return DeserializeJson<Response>(result);
            }
            catch (Exception ex)
            {
                return new Response() { Success = false, Message = ex.Message, Content = ex };
            }
        }

        public static Response DeleteOrder(string id, string Subaccount)
        {
            try
            {

                string postData = "{\"id\":\"" + id + "\",\"subAccount\":\"" + Subaccount + "\"}";


                string result = string.Empty;

                var request = (HttpWebRequest)WebRequest.Create(apiurl + URL_DELETEORDER);

                var data = Encoding.UTF8.GetBytes(postData);

                request.Headers.Add("APIKEY", APIKEY);
                request.Headers.Add("Authorization", HASH);
                request.Headers.Add("Checker", ComputeSha256Hash(APIKEY + hostname + URL_DELETEORDER + postData));
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "POST";
                request.Accept = "application/json; charset=utf-8";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return DeserializeJson<Response>(result);
            }
            catch (Exception ex)
            {
                return new Response() { Success = false, Message = ex.Message, Content = ex };
            }
        }

        public static Response DeleteOrderViop(string id,string adet, string Subaccount)
        {
            try
            {

                string postData = "{\"id\":\"" + id + "\",\"adet\":\"" + adet + "\",\"Subaccount\":\"" + Subaccount + "\"}";


                string result = string.Empty;

                var request = (HttpWebRequest)WebRequest.Create(apiurl + URL_DELETEORDERVIOP);

                var data = Encoding.UTF8.GetBytes(postData);

                request.Headers.Add("APIKEY", APIKEY);
                request.Headers.Add("Authorization", HASH);
                request.Headers.Add("Checker", ComputeSha256Hash(APIKEY + hostname + URL_DELETEORDERVIOP + postData));
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "POST";
                request.Accept = "application/json; charset=utf-8";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return DeserializeJson<Response>(result);
            }
            catch (Exception ex)
            {
                return new Response() { Success = false, Message = ex.Message, Content = ex };
            }
        }

        public static bool SessionRefresh()
        {
            try
            {

                string postData = "";


                string result = string.Empty;

                var request = (HttpWebRequest)WebRequest.Create(apiurl + URL_SESSIONREFRESH);

                var data = Encoding.UTF8.GetBytes(postData);

                request.Headers.Add("APIKEY", APIKEY);
                request.Headers.Add("Authorization", HASH);
                request.Headers.Add("Checker", ComputeSha256Hash(APIKEY + hostname + URL_SESSIONREFRESH + postData));
                request.ContentType = "application/json; charset=utf-8";
                request.Method = "POST";
                request.Accept = "application/json; charset=utf-8";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                return DeserializeJson<bool>(result);
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string OpenSSLEncryptApi(string plainText, string apikey)
        {
            TripleDESCryptoServiceProvider keys = new TripleDESCryptoServiceProvider();
            keys.GenerateIV();
            keys.GenerateKey();
            string key = apikey;
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static T DeserializeJson<T>(string obj)
        {
            try
            {
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

                return JsonConvert.DeserializeObject<T>(obj, jsonSerializerSettings);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
