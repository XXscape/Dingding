using DingDingInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebDingSendDemo
{
    public partial class WebDingJSAPI : System.Web.UI.Page
    {
        public string _mCorpid = "dinge714a8d5baa070ee";
        public string _mCorpsecret = "UiVUD1oQjc9NBUUScpFvRUpt73nZrkvq10DjcbNkcD9OAYKNmZS1S1OmzHoBLX7u";
        public string _mAgentId = "37513447";//
        public string signature = string.Empty;


        DingInterface _mding = new DingInterface();
        public string _mstr { get; set; }
        GetAccessTokenRet _mRet { get; set; }
        OperateJson opt = new OperateJson();
        protected void Page_Load(object sender, EventArgs e)
        {

            string str = "https://oapi.dingtalk.com/gettoken";
            string strPostData = "corpid=" + _mCorpid + "&corpsecret=" + _mCorpsecret;
            //https://oapi.dingtalk.com/gettoken?corpid=ding76413ee54b2f3446&corpsecret=vvOWfQ2avv6Hfev8ZqB1lyasHV-gxKQWgADST01u3W85XXq4Oh2toollCbslZtGy
            string jsonText = _mding.HttpGet(str, strPostData);
            if (!string.IsNullOrEmpty(jsonText))
            {
                _mRet = opt.OptJson<GetAccessTokenRet>(jsonText);
                if (_mRet.errcode == 0)
                {
                }
            }
            str = "https://oapi.dingtalk.com/get_jsapi_ticket";
            strPostData = "access_token=" + _mRet.access_token;
            jsonText = _mding.HttpGet(str, strPostData);
            GetAccessTicketRet ret=new GetAccessTicketRet();
            if (!string.IsNullOrEmpty(jsonText))
            {
                ret = opt.OptJson<GetAccessTicketRet>(jsonText);
                if (_mRet.errcode == 0)
                {
                }
            }
            TimeSpan timespan = DateTime.Now - DateTime.Parse("1970-1-1");
            

            Dictionary<string, object> keyArray = new Dictionary<string, object>();
            keyArray.Add("noncestr", "huashuda");
            keyArray.Add("timestamp", timespan.TotalSeconds);
            keyArray.Add("jsapi_ticket", ret.ticket);
            keyArray.Add("url", System.Web.HttpUtility.UrlDecode(HttpContext.Current.Request.Url.ToString()));
            //List keyArray = sort("huashuda", timespan.TotalSeconds, ret.ticket, System.Web.HttpUtility.UrlDecode(HttpContext.Current.Request.Url.ToString()));
            keyArray = SortArrayByAscii(keyArray);

            String str1 = ChangeListToString(keyArray);

            signature = SHA1(str1);
        }

        public Dictionary<string, object> SortArrayByAscii(Dictionary<string, object> keyArray)
        {
            keyArray = keyArray.OrderBy(o => o.Key).ToDictionary(o=>o.Key,v=>v.Value);
            return keyArray;
        }

        public string ChangeListToString(Dictionary<string, object> keyArray)
        {
            var str = string.Empty;
            foreach (var item in keyArray)
            {
                str += item.Key + "=" + item.Value + "&";
            }
            str.Substring(0, str.Length - 1);
            return str;
        }

        private static string SHA1(string text)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(text);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }
    }
}