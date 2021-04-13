using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitAuto
{
    class CoinInfo
    {
        public string market { get; set; }

        public string trade_date { get; set; }

        public string trade_time { get; set; }

        public string trade_date_kst { get; set; }

        public string trade_time_kst { get; set; }

        public string trade_timestamp { get; set; }

        public string opening_price { get; set; }

        public string high_price { get; set; }

        public string low_price { get; set; }

        public string trade_price { get; set; }

        public string prev_closing_price { get; set; }

        public string change { get; set; }

        public string change_price { get; set; }
        public string acc_trade_price { get; set; }
        public string acc_trade_price_24h { get; set; }
    }
}
