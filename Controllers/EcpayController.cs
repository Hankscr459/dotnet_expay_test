using API.Dtos;
using API.Services.EcPay;
using Microsoft.AspNetCore.Mvc;

namespace ecpay.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EcpayController : ControllerBase
{
    [HttpGet]
    public ContentResult Ecpay()
    {
        var ecPay = new EcPay();
        PostData postData = new PostData();
        var random = new Random();
        var randomVlaue = random.Next(0, 99);
        postData.MerchantTradeNo = $"D132281069{randomVlaue}";
        postData.TotalAmount = 1000;
        postData.TradeDesc = "測試機";
        postData.ItemName = "商品一批";
        postData.ReturnURL = "http://localhost:5000/api/ecpay/ecpay_callback";
        postData.ClientBackURL = "http://localhost:5000/api/ecpay/ecpay_callback";
        var ecHtml = ecPay.aio_check_out_credit_onetime(postData);
        // Console.WriteLine("postData.MerchantID: {0}", postData.MerchantID);
        var html = $@"
            var ecpay_div = document.createElement(""div"");
            ecpay_div.innerHTML = {ecHtml}
            document.body.appendChild(ecpay_div);
        ";
        return new ContentResult
        {
            Content = html,
            ContentType = "text/html"
        };
    }

    [HttpGet("ecpay_callback")]
    public object CallBack()
    {
        return new { success = true };
    }
}

// html form
// <form id="ecpayform" action="https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5" method="post">
//  <input type="hidden" name="MerchantTradeNo" id="MerchantTradeNo" value="D13228106959" />
//  <input type="hidden" name="TotalAmount" id="TotalAmount" value="1000" />
//  <input type="hidden" name="TradeDesc" id="TradeDesc" value="測試機" />
//  <input type="hidden" name="ItemName" id="ItemName" value="商品一批" />
//  <input type="hidden" name="ReturnURL" id="ReturnURL" value="http://localhost:5000/api/ecpay/ecpay_callback" />
//  <input type="hidden" name="ClientBackURL" id="ClientBackURL" value="http://localhost:5000/api/ecpay/ecpay_callback" />
//  <input type="hidden" name="MerchantID" id="MerchantID" value="3002607" />
//  <input type="hidden" name="MerchantTradeDate" id="MerchantTradeDate" value="2023/05/12 12:00:00" />
//  <input type="hidden" name="PaymentType" id="PaymentType" value="aio" />
//  <input type="hidden" name="ChoosePayment" id="ChoosePayment" value="Credit" />
//  <input type="hidden" name="EncryptType" id="EncryptType" value="1" />
//  <input type="hidden" name="CheckMacValue" id="CheckMacValue" value="03186AB090571F5535BC0D34FA85FFD206C0E2D6F0A795FFA40EA3944127C05C" />
//  <script type="text/javascript">document.getElementById("ecpayform").submit();</script>
// </form>

// 檢核碼計算順序: HashKey=pwFHCqoQZGmho4w6&ChoosePayment=Credit&ClientBackURL=http://localhost:5000/api/ecpay/ecpay_callback&EncryptType=1&ItemName=商品一批&MerchantID=3002607&MerchantTradeDate=2023/05/12 12:00:00&MerchantTradeNo=D13228106959&PaymentType=aio&ReturnURL=http://localhost:5000/api/ecpay/ecpay_callback&TotalAmount=1000&TradeDesc=測試機&HashIV=EkRm7iFT261dpevs

// UrlEncode Lower:hashkey%3dpwfhcqoqzgmho4w6%26choosepayment%3dcredit%26clientbackurl%3dhttp%3a%2f%2flocalhost%3a5000%2fapi%2fecpay%2fecpay_callback%26encrypttype%3d1%26itemname%3d%e5%95%86%e5%93%81%e4%b8%80%e6%89%b9%26merchantid%3d3002607%26merchanttradedate%3d2023%2f05%2f12+12%3a00%3a00%26merchanttradeno%3dd13228106959%26paymenttype%3daio%26returnurl%3dhttp%3a%2f%2flocalhost%3a5000%2fapi%2fecpay%2fecpay_callback%26totalamount%3d1000%26tradedesc%3d%e6%b8%ac%e8%a9%a6%e6%a9%9f%26hashiv%3dekrm7ift261dpevs

// CheckMacValue: 03186AB090571F5535BC0D34FA85FFD206C0E2D6F0A795FFA40EA3944127C05C