using System;
using System.Collections.Generic;
using System.Text;

namespace AlgolabAPI
{
    [Serializable]
    public class WebsocketData
    {
        public string Type { get; set; }
        public object Content { get; set; }

    }
    [Serializable]
    public class Depth
    {
        public string Symbol { get; set; }
        public string Market { get; set; }
        public string Direction { get; set; }
        public int Row { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int OrderCount { get; set; }
        public DateTime Date { get; set; }

    }

    [Serializable]
    public class Tick
    {
        public string Symbol { get; set; }
        public string Market { get; set; }
        public double Price { get; set; }
        public double Change { get; set; }
        public double Ask { get; set; }
        public double Bid { get; set; }
        public DateTime Date { get; set; }
        public double ChangePercentage { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double TradeQuantity { get; set; }
        public string Direction { get; set; }
        public double RefPrice { get; set; }
        public double BalancePrice { get; set; }
        public double BalanceAmount { get; set; }
        public string Buying { get; set; }
        public string Selling { get; set; }
    }
}
