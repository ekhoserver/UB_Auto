using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitAuto
{
    class CancelResponse
    {
        public string uuid { get; set; }
        public string side { get; set; }
        public string state { get; set; }
        public string price { get; set; }
        public string market { get; set; }
        public string volume { get; set; } //주문 수량
        public string remaining_volume { get; set; } //남은 수량
        public string executed_volume { get; set; } //체결 수량







    }
}
