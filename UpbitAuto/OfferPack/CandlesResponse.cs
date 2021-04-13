using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitAuto.OfferPack
{
    class CandlesResponse
    {
        public string market { get; set; }

        public string candle_date_time_kst { get; set; }

        public string opening_price { get; set; }
        public string high_price { get; set; }
        public string low_price { get; set; }
        public string trade_price { get; set; }
        public string timestamp { get; set; }
        public string candle_acc_trade_price { get; set; }
        public string candle_acc_trade_volume { get; set; }
    }
}
