using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitAuto.Mode
{
    class TModeList
    {
        public int index;

        public string buyuuid;
        public string selluuid;

        public bool isSell;

        public decimal buyPrice;
        public decimal sellPrice;

        public decimal pri; //매수 단가



        public TModeList(int indexA, string buyuuidA, string selluuidA, bool isSellA, decimal buyPriceA, decimal sellPriceA, decimal priA)
        {
            index = indexA;

            buyuuid = buyuuidA;
            selluuid = selluuidA;
            
            isSell = isSellA;

            buyPrice = buyPriceA;
            sellPrice = sellPriceA;
            pri = priA;
        }
    }
}
