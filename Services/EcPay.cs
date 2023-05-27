using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using API.Dtos;

namespace API.Services.EcPay
{
    public class EcPay
    {
        public EcPay()
        {
            ECPayHashKey = "pwFHCqoQZGmho4w6";
            ECPayHashIV = "EkRm7iFT261dpevs";
            ECPayUrl = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5";
            MerchantID = "3002607";
        }
        public string ECPayHashKey { get; set; }
        public string ECPayHashIV { get; set; }
        public string ECPayUrl { get; set; }
        public string MerchantID { get; set; }
        public string gen_chk_mac_value(PostData param) {
            String keyAndyValue = "";
            PostData data = new PostData();
            data = param;
            List<string> paramters = new List<string>();
            FieldInfo[] fields = data.GetType().GetFields(BindingFlags.Public 
                                             | BindingFlags.Instance 
                                             | BindingFlags.NonPublic 
                                             | BindingFlags.Static);
            foreach(FieldInfo f in fields){
                if (f.GetValue(data) != null)
                {
                    var name = f.Name.Replace(">k__BackingField", "");
                    name = name.Replace("<", "");
                    paramters.Add(name + "=" + f.GetValue(data) + "&");
                }
            }
            paramters = paramters.OrderBy(p => p).ToList();
            foreach (var item in paramters)
            {
                keyAndyValue += item;
            }
            keyAndyValue = keyAndyValue.TrimEnd('&');
            keyAndyValue = $@"HashKey={ECPayHashKey}&{keyAndyValue}&HashIV={ECPayHashIV}";
            Console.WriteLine("檢核碼計算順序: {0}", keyAndyValue);
            keyAndyValue = HttpUtility.UrlEncode(keyAndyValue).ToLower();
            Console.WriteLine("UrlEncode Lower:{0}", keyAndyValue);
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(keyAndyValue);
            var hash = sha256.ComputeHash(bytes);
            var hex = BitConverter.ToString(hash).Replace("-", "").ToUpper();
            return hex;
        }
        public string gen_html_post_form(PostData postData, string act,string id = "ecpayform", string inputType = "hidden", Boolean submit = false) {
            var html = $@"<form id=""{id}"" action=""{act}"" method=""post"">";
            FieldInfo[] fields = postData.GetType().GetFields(BindingFlags.Public 
                                             | BindingFlags.Instance 
                                             | BindingFlags.NonPublic 
                                             | BindingFlags.Static);
            foreach(FieldInfo f in fields){
                var name = f.Name.Replace(">k__BackingField", "");
                name = name.Replace("<", "");
                html += $@"<input type=""{inputType}"" name=""{name}"" id=""{name}"" value=""{f.GetValue(postData)}"" />";
            }
            if (submit == true) {
                html += $@"<script type=""text/javascript"">document.getElementById(""{id}"").submit();</script>";
            }
            html += "</form>";
            Console.Write("html: " + html);
            return html;
        }

        public string aio_check_out_credit_onetime(PostData postData)
        {
            PostData data = new PostData();
            data = postData;
            data.MerchantID = this.MerchantID;
            // data.MerchantTradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            data.MerchantTradeDate = "2023/05/12 12:00:00";
            data.PaymentType = "aio";
            data.ChoosePayment = "Credit";
            data.EncryptType = 1;
            data.CheckMacValue = this.gen_chk_mac_value(data);
            Console.WriteLine("data.CheckMacValue: {0}", data.CheckMacValue);
            string html = this.gen_html_post_form(data, this.ECPayUrl, "ecpayform", "hidden", true);
            return html;
        }
    }
}
