﻿namespace Rabun.Oanda.Rest.Models
{
    public class TransactionTakeProfitField : TransactionSimple
    {
        public long TradeId { get; set; }
        public float Pl { get; set; }
        public int Interest { get; set; }
        public float AccountBalance { get; set; }
    }
}