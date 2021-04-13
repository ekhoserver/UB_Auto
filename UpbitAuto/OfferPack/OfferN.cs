using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitAuto.OfferPack
{
    class OfferN
    {
        public int index;
        public string market;
        public decimal per;
        public int Tprice;
        public bool isResponse;
        public string buyUUID;
        public bool isbuy;
        public decimal volume;

        public decimal Sprice;


        public OfferN(int indexA,  string marketA, decimal perA, int TpriceA, bool isResponseA, string buyUUIDA, bool isbuyA, decimal volumeA, decimal SpriceA)
        {
            index = indexA;
            market = marketA;
            per = perA;
            Tprice = TpriceA;
            isResponse = isResponseA;
            buyUUID = buyUUIDA;
            isbuy = isbuyA;
            volume = volumeA;
            Sprice = SpriceA;
        }
    }
}
