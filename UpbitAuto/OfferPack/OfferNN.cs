using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitAuto.OfferPack
{
    class OfferNN
    {
        public int index;
        public string market;

        public decimal BuyPrice;
        public decimal SellPrice;

        public int Tprice;
        public bool isResponse;
        public string buyUUID;
        public bool isbuy;
        public decimal volume;

        public decimal Sprice;
        public bool isOffer;


        public OfferNN(int indexA, string marketA, decimal BuyPriceA, decimal SellPriceA, int TpriceA, bool isResponseA, string buyUUIDA, bool isbuyA, decimal volumeA, decimal SpriceA,bool isOfferA)
        {
            index = indexA;
            market = marketA;

            Tprice = TpriceA;
            isResponse = isResponseA;
            buyUUID = buyUUIDA;
            isbuy = isbuyA;
            volume = volumeA;
            Sprice = SpriceA;

            BuyPrice = BuyPriceA;
            SellPrice = SellPriceA;
            isOffer = isOfferA;
        }
    }
}
