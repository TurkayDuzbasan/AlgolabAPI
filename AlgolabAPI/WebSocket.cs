using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlgolabAPI
{
    public static class WebSocket
    {
        public static string checker = Program.ComputeSha256Hash(Program.APIKEY + Program.hostname+"/ws");
        public static ClientWebSocket webSocket = new ClientWebSocket();
        public static DateTime senddate = DateTime.Now;
        public static async Task ConnectToWebsocket()
        {           
            try
            {
                webSocket.Options.SetRequestHeader("APIKEY", Program.APIKEY);
                webSocket.Options.SetRequestHeader("Authorization", Program.HASH);
                webSocket.Options.SetRequestHeader("Checker", checker);
                await webSocket.ConnectAsync(new Uri(Program.websocketurl), CancellationToken.None);

                
                await Task.WhenAll(Receive(webSocket), Send(webSocket));
            }
            catch (Exception ex)
            {                
            }
        }
        private static async Task Receive(ClientWebSocket webSocket)
        {
            byte[] buffer = new byte[1024];
            while (webSocket.State == System.Net.WebSockets.WebSocketState.Open)
            {
                try
                {
                    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    string a = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                    WebsocketData model = new WebsocketData();
                    model = JsonConvert.DeserializeObject<WebsocketData>(a);
                    if (model != null && model.Type == "D")
                    {
                        Depth depthmodel = JsonConvert.DeserializeObject<Depth>(JsonConvert.SerializeObject(model.Content));

                        if (depthmodel.Symbol == "GARAN")
                        {

                            Console.WriteLine(JsonConvert.SerializeObject(depthmodel));
                        }
                    }
                    //Console.WriteLine(a);
                }
                catch (Exception ex)
                {                    
                }
            }
            if (webSocket.State != System.Net.WebSockets.WebSocketState.Open)
            {
                ConnectToWebsocket();
            }
        }

        private static async Task Send(ClientWebSocket webSocket)
        {
            
            while (webSocket.State == System.Net.WebSockets.WebSocketState.Open)
            {

                if (senddate < DateTime.Now)
                {
                    senddate = DateTime.Now.AddMinutes(15);
                    string message = "{\"Type\":\"H\",\"Token\":\"" + Program.HASH + "\"}";

                    var encoded = Encoding.UTF8.GetBytes(message);
                    var buffer = new ArraySegment<Byte>(encoded, 0, encoded.Length);
                    await webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

                }
            }
        }
    }
}
