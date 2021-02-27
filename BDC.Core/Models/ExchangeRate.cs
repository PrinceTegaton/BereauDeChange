using System;

namespace BDC.Core.Models
{
    public class ExchangeRate : BaseModel
    {
        public string SourceCurrency { get; set; }
        public string TargetCurrency { get; set; }
        public decimal Rate { get; set; }
        public DateTime TradingStart { get; set; }
        public DateTime TradingEnd { get; set; }
    }
}
