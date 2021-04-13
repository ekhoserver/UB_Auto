using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitAuto
{
    class UserInfo
    {
        public string currency { get; set; }
        public string balance { get; set; }
        public string locked { get; set; }
        public string avg_buy_price { get; set; }
        public string avg_buy_price_modified { get; set; }
        public string unit_currency { get; set; }
    }
}
