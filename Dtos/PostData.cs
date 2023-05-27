namespace API.Dtos
{
    public class PostData
    {
        public string? MerchantTradeNo { get; set; } // 特店交易編號 orderNo
        public Int32 TotalAmount { get; set; } = 0; // 交易金額
        public string? TradeDesc { get; set; } // 交易備註
        public string? ItemName { get; set; } // 商品名稱
        public string? ReturnURL { get; set; } // 付款完成通知網址
        public string? ClientBackURL { get; set; } // 付款結果網址
        public string MerchantID { get; set; } = "3002599"; // 特店編號 MerchantID
        public string? MerchantTradeDate { get; set; } //
        public string PaymentType { get; set; } = "aio";
        public string ChoosePayment { get; set; } = "Credit"; // 預設付款方式 信用卡
        public Int32 EncryptType { get; set; } = 1;
        public string? CheckMacValue { get; set; }
    }
}