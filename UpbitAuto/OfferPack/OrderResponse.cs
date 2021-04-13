using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitAuto
{
    class OrdersResponse
    {
        public string market { get; set; }
        public string uuid { get; set; }
        public string paid_fee { get; set; }
        public string reserved_fee { get; set; }
        public string volume { get; set; }
        public string executed_volume { get; set; }
        public string remaining_volume { get; set; }
        public string state { get; set; }

        public string price { get; set; }
        public string side { get; set; }
        public string trades_count { get; set; }

        public List<Trade> trades { get; set; }
    }
}
