using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitAuto
{
    public class Offer
    {
        public int index; //순서
        public decimal buying;//매수가
        public decimal selling;//매도가
        public decimal volume;//수량
        public decimal Svolume;//수량
        public int price; //호가금액
        public decimal CancelPrice; //주문 취소


        public string BUUID;
        public string SUUID;


        public bool isbuying;
        public bool isselling;


        public bool isOfferB;
        public bool isOfferS;

        public int tbuy;

        public int predict;
        public int up;
        public int trade;



        public Offer(int indexA, decimal buyingA, decimal sellingA,
            decimal volumeA, string BUUIDA, string SUUIDA, int priceA,
            bool isbuyingA, bool issellingA, bool isOfferBA, bool isOfferSA, int tbuyA, int predictA, int upA, int tradeA, decimal CancelPriceA, decimal SvolumeA)
        {
            index = indexA;
            buying = buyingA;
            selling = sellingA;
            volume = volumeA;

            BUUID = BUUIDA;
            SUUID = SUUIDA;

            price = priceA;

            isbuying = isbuyingA;
            isselling = issellingA;

            isOfferB = isOfferBA;
            isOfferS = isOfferSA;

            tbuy = tbuyA;

            up = upA;
            predict = predictA;
            trade = tradeA;
            CancelPrice = CancelPriceA;
            Svolume = SvolumeA;
        }
    }
}
