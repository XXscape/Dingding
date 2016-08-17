using DingDingInterface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebDingSendDemo2
{
    public partial class WebDingTest : System.Web.UI.Page
    {
        /// <summary>
        /// 创建静态字段，保证全局一致
        /// </summary>
        public static AccessToken AccessToken = new AccessToken();//static
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public static string FormatApiUrlWithToken(string strurl)
        {
            return strurl + "?access_token=" + AccessToken.Value;
        }

        #region UpdateAccessToken  
        /// <summary>  
        ///更新票据  
        /// </summary>  
        /// <param name="forced">true:强制更新.false:按缓存是否到期来更新</param>  
        public static void UpdateAccessToken(bool forced = false)
        {
            if (!forced && AccessToken.Begin.AddSeconds(ConstVars.CACHE_TIME) >= DateTime.Now)
            {//没有强制更新，并且没有超过缓存时间  
                return;
            }
            string CorpID = ConfigHelper.FetchCorpID();
            string CorpSecret = ConfigHelper.FetchCorpSecret();
            string TokenUrl = Urls.gettoken;
            string apiurl = $"{TokenUrl}?{Keys.corpid}={CorpID}&{Keys.corpsecret}={CorpSecret}";
            TokenResult tokenResult = Analyze.Get<TokenResult>(apiurl);
            if (tokenResult.ErrCode == ErrCodeEnum.OK)
            {
                AccessToken.Value = tokenResult.Access_token;
                AccessToken.Begin = DateTime.Now;
            }
        }
        #endregion

        #region FetchDepartList  
        public static DepartResultSet FetchDepartList()
        {
            string apiurl = FormatApiUrlWithToken(Urls.department_list);
            var result = Analyze.Get<DepartResultSet>(apiurl);
            return result;
        }
        #endregion

        /// <summary>  
        /// 发送消息  
        /// </summary>  
        /// <param name="toUser">目标用户</param>  
        /// <param name="toParty">目标部门.当toParty和toUser同时指定时，以toParty来发送。</param>  
        /// <param name="content">消息文本</param>  
        /// <returns></returns>  
        private static SendMessageResult SendTextMsg(string toUser, string toParty, string content)
        {
            var txtmsg = new
            {
                touser = toUser,
                toparty = toParty,
                msgtype = "text",//MsgType.text.ToString(),
                agentid = ConfigHelper.FetchAgentID(),
                text = new
                {
                    content = content
                }
            };
            string apiurl = FormatApiUrlWithToken(Urls.message_send);
            string json = JsonConvert.SerializeObject(txtmsg);
            var result = Analyze.Post<SendMessageResult>(apiurl, json);
            return result;
        }

        #region FetchJSTicket Function    
        /// <summary>  
        /// 获取JS票据  
        /// </summary>  
        /// <param name="url"></param>  
        /// <returns></returns>  
        public static JSTicket FetchJSTicket()
        {
            var cache = SimpleCacheProvider.GetInstance();
            var jsTicket = cache.GetCache<JSTicket>(ConstVars.CACHE_JS_TICKET_KEY);
            if (jsTicket == null)
            {
                String apiurl = FormatApiUrlWithToken(Urls.get_jsapi_ticket);
                jsTicket = Analyze.Get<JSTicket>(apiurl);
                cache.SetCache(ConstVars.CACHE_JS_TICKET_KEY, jsTicket, ConstVars.CACHE_TIME);
            }
            return jsTicket;
        }
        #endregion

        #region FetchSignPackage Function   
        /// <summary>  
        /// 获取签名包  
        /// </summary>  
        /// <param name="url"></param>  
        /// <returns></returns>  
        public static SignPackage FetchSignPackage(String url, JSTicket jsticket)
        {
            int unixTimestamp = SignPackageHelper.ConvertToUnixTimeStamp(DateTime.Now);
            string timestamp = Convert.ToString(unixTimestamp);
            string nonceStr = SignPackageHelper.CreateNonceStr();
            if (jsticket == null)
            {
                return null;
            }

            // 这里参数的顺序要按照 key 值 ASCII 码升序排序   
            string rawstring = $"{Keys.jsapi_ticket}=" + jsticket.ticket
                             + $"&{Keys.noncestr}=" + nonceStr
                             + $"&{Keys.timestamp}=" + timestamp
                             + $"&{Keys.url}=" + url;
            string signature = SignPackageHelper.Sha1Hex(rawstring).ToLower();

            var signPackage = new SignPackage()
            {
                agentId = ConfigHelper.FetchAgentID(),
                corpId = ConfigHelper.FetchCorpID(),
                timeStamp = timestamp,
                nonceStr = nonceStr,
                signature = signature,
                url = url,
                rawstring = rawstring,
                jsticket = jsticket.ticket
            };
            return signPackage;
        }

        /// <summary>  
        /// 获取签名包  
        /// </summary>  
        /// <param name="url"></param>  
        /// <returns></returns>  
        public static SignPackage FetchSignPackage(String url)
        {
            int unixTimestamp = SignPackageHelper.ConvertToUnixTimeStamp(DateTime.Now);
            string timestamp = Convert.ToString(unixTimestamp);
            string nonceStr = SignPackageHelper.CreateNonceStr();
            var jsticket = FetchJSTicket();
            var signPackage = FetchSignPackage(url, jsticket);
            return signPackage;
        }
        #endregion
    }
}