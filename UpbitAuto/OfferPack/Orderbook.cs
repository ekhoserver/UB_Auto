using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitAuto
{
    class Orderbook
    {
        public string market { get; set; }
        public string timestamp { get; set; }
        public string total_ask_size { get; set; }
        public string total_bid_size { get; set; }

        public List<Orderbook_Unit> orderbook_units { get; set; }
    }
}
