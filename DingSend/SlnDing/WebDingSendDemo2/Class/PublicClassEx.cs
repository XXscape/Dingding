using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace DingDingInterface
{
    /// <summary>
    /// 访问票据
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// 票据的值
        /// </summary>
        public String Value { get; set; }

        /// <summary>
        /// 票据的开始时间
        /// </summary>
        public DateTime Begin { get; set; } = DateTime.Parse("1970-01-01");
    }

    public enum ErrCodeEnum
    {
        OK = 0,

        VoildAccessToken = 40014,

        /// <summary>
        /// 未知
        /// </summary>
        Unknown = int.MaxValue
    }
    public class TokenResult : ResultPackage
    {
        public string Access_token { get; set; }
    }
    public class DepartResultSet : ResultPackage
    {
        public List<Depart> department { get; set; }
    }
    public class ConstVars
    {
        /// <summary>
        /// 缓存的JS票据的KEY
        /// </summary>
        public const string CACHE_JS_TICKET_KEY = "CACHE_JS_TICKET_KEY";

        /// <summary>
        /// 缓存时间
        /// </summary>
        public const int CACHE_TIME = 5000;
    }

    /// <summary>
    /// Url的Key
    /// </summary>
    public sealed class DDKeys
    {
        public const string corpid = "corpid";

        public const string corpsecret = "corpsecret";

        public const string department_id = "department_id";

        public const string userid = "userid";

        public const string chatid = "chatid";

        public const string access_token = "access_token";

        public const string jsapi_ticket = "jsapi_ticket";

        public const string noncestr = "noncestr";

        public const string timestamp = "timestamp";

        public const string url = "url";
    }
    /// <summary>
    /// SDK使用的URL
    /// </summary>
    public sealed class DDUrls
    {
        /// <summary>
        /// 创建会话
        /// </summary>
        public const string chat_create = "https://oapi.dingtalk.com/chat/create";
        /// <summary>
        /// 获取会话信息
        /// </summary>
        public const string chat_get = "https://oapi.dingtalk.com/chat/get";
        /// <summary>
        /// 发送会话消息
        /// </summary>
        public const string chat_send = "https://oapi.dingtalk.com/chat/send";
        /// <summary>
        /// 更新会话信息
        /// </summary>
        public const string chat_update = "https://oapi.dingtalk.com/chat/update";

        /// <summary>
        /// 获取部门列表
        /// </summary>
        public const string department_list = "https://oapi.dingtalk.com/department/list";

        /// <summary>
        /// 获取访问票记
        /// </summary>
        public const string gettoken = "https://oapi.dingtalk.com/gettoken";

        /// <summary>
        /// 发送消息
        /// </summary>
        public const string message_send = "https://oapi.dingtalk.com/message/send";

        /// <summary>
        /// 用户列表
        /// </summary>
        public const string user_list = "https://oapi.dingtalk.com/user/list";
        /// <summary>
        /// 用户详情
        /// </summary>
        public const string user_get = "https://oapi.dingtalk.com/user/get";

        /// <summary>
        /// 获取JSAPI的票据
        /// </summary>
        public const string get_jsapi_ticket = "https://oapi.dingtalk.com/get_jsapi_ticket";
    }

    public class ResultPackage
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public ErrCodeEnum ErrCode { get; set; } = ErrCodeEnum.Unknown;

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrMsg { get; set; }

        /// <summary>
        /// 结果的json形式
        /// </summary>
        public String Json { get; set; }


        #region IsOK Function              
        public bool IsOK()
        {
            return ErrCode == ErrCodeEnum.OK;
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            String info = $"{nameof(ErrCode)}:{ErrCode},{nameof(ErrMsg)}:{ErrMsg}";

            return info;
        }
        #endregion
    }


    /// <summary>
    /// 请求协助类
    /// </summary>
    public class RequestHelper
    {
        #region Get
        /// <summary>
        /// 执行基本的命令方法,以Get方式
        /// </summary>
        /// <param name="apiurl"></param>
        /// <returns></returns>
        public static String Get(string apiurl)
        {
            WebRequest request = WebRequest.Create(@apiurl);
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            Encoding encode = Encoding.UTF8;
            StreamReader reader = new StreamReader(stream, encode);
            string resultJson = reader.ReadToEnd();
            return resultJson;
        }
        #endregion

        #region Post
        /// <summary>
        /// 以Post方式提交命令
        /// </summary>
        public static String Post(string apiurl, string jsonString)
        {
            WebRequest request = WebRequest.Create(@apiurl);
            request.Method = "POST";
            request.ContentType = "application/json";

            byte[] bs = Encoding.UTF8.GetBytes(jsonString);
            request.ContentLength = bs.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(bs, 0, bs.Length);
            newStream.Close();

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            Encoding encode = Encoding.UTF8;
            StreamReader reader = new StreamReader(stream, encode);
            string resultJson = reader.ReadToEnd();
            return resultJson;
        }
        #endregion
    }
    /// <summary>
    /// 分析器
    /// </summary>
    public class Analyze
    {
        #region Get Function  
        /// <summary>
        /// 发起GET请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        public static T Get<T>(String requestUrl) where T : ResultPackage, new()
        {
            String resultJson = RequestHelper.Get(requestUrl);
            return AnalyzeResult<T>(resultJson);
        }
        #endregion

        #region Post Function
        /// <summary>
        /// 发起POST请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUrl"></param>
        /// <param name="requestParamOfJsonStr"></param>
        /// <returns></returns>
        public static T Post<T>(String requestUrl, String requestParamOfJsonStr) where T : ResultPackage, new()
        {
            String resultJson = RequestHelper.Post(requestUrl, requestParamOfJsonStr);
            return AnalyzeResult<T>(resultJson);
        }
        #endregion

        #region AnalyzeResult
        /// <summary>
        /// 分析结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resultJson"></param>
        /// <returns></returns>
        private static T AnalyzeResult<T>(string resultJson) where T : ResultPackage, new()
        {
            ResultPackage tempResult = null;
            if (!String.IsNullOrEmpty(resultJson))
            {
                tempResult = JsonConvert.DeserializeObject<ResultPackage>(resultJson);
            }
            T result = null;
            if (tempResult != null && tempResult.IsOK())
            {
                result = JsonConvert.DeserializeObject<T>(resultJson);
            }
            else if (tempResult != null)
            {
                result = tempResult as T;
            }
            else if (tempResult == null)
            {
                result = new T();
            }
            result.Json = resultJson;
            return result;
        }
        #endregion      

        public class SendMessageResult : ResultPackage
        {
            public string receiver { get; set; }
        }
    }

    /****************************************************/


    /// <summary>
    /// 缓存接口
    /// </summary>
    interface ICacheProvider
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns>缓存对象或null,不存在或者过期返回null</returns>
        object GetCache(string key);

        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值</param>
        /// <param name="expire">缓存有效期，单位为秒，默认300</param>
        void SetCache(string key, object value, int expire = 300);
    }


    /// <summary>
    /// 时间戳
    /// </summary>
    public class TimeStamp
    {
        public static long Now()
        {
            return (long)((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
        }
        public static DateTime ToDateTime(long timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp);
        }
        public static string ToDateTimeString(long timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp).ToString();
        }
        public static string ToDateTimeString(long timestamp, string format)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp).ToString(format);
        }
    }

    /// <summary>
    /// 缓存项
    /// </summary>
    public class CacheItem
    {

        #region 属性
        private object _value;
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private string _key;
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }
        #endregion

        #region 内部变量
        /// <summary>
        /// 插入时间
        /// </summary>
        private long _insertTime;
        /// <summary>
        /// 过期时间
        /// </summary>
        private int _expire;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">缓存的KEY</param>
        /// <param name="value">缓存的VALUE</param>
        /// <param name="expire">缓存的过期时间</param>
        public CacheItem(string key, object value, int expire)
        {
            this._key = key;
            this._value = value;
            this._expire = expire;
            this._insertTime = TimeStamp.Now();
        }
        #endregion

        #region Expired
        /// <summary>
        /// 是否过期
        /// </summary>
        /// <returns></returns>
        public bool Expired()
        {
            return TimeStamp.Now() > this._insertTime + _expire;
        }
        #endregion
    }
    /// <summary>
    /// 简易缓存
    /// </summary>
    public class SimpleCacheProvider : ICacheProvider
    {
        private static SimpleCacheProvider _instance = null;

        #region GetInstance
        /// <summary>
        /// 获取缓存实例
        /// </summary>
        /// <returns></returns>
        public static SimpleCacheProvider GetInstance()
        {
            if (_instance == null) _instance = new SimpleCacheProvider();
            return _instance;
        }
        #endregion

        private Dictionary<string, CacheItem> _caches;

        private SimpleCacheProvider()
        {
            this._caches = new Dictionary<string, CacheItem>();
        }

        #region GetCache
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetCache(string key)
        {
            object obj = this._caches.ContainsKey(key) ? this._caches[key].Expired() ? null : this._caches[key].Value : null;
            return obj;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetCache<T>(String key)
        {
            object obj = GetCache(key);
            if (obj == null)
            {
                return default(T);
            }
            T result = (T)obj;
            return result;
        }
        #endregion

        #region SetCache
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expire"></param>
        public void SetCache(string key, object value, int expire = 300)
        {
            this._caches[key] = new CacheItem(key, value, expire);
        }
        #endregion



    }


    /// <summary>
    /// JSAPI时用的票据
    /// </summary>
    public class JSTicket : ResultPackage
    {
        public string ticket { get; set; }
        public int expires_in { get; set; }
    }

    /// <summary>
    /// 部门
    /// </summary>
    public class Depart
    {
        /// <summary>
        /// 部门id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 父部门id，根部门为1
        /// </summary>
        public string parentid { get; set; }

        /// <summary>
        /// 是否同步创建一个关联此部门的企业群, true表示是, false表示不是
        /// </summary>
        public bool createDeptGroup { get; set; }

        /// <summary>
        /// 当群已经创建后，是否有新人加入部门会自动加入该群, true表示是, false表示不是
        /// </summary>
        public bool autoAddUser { get; set; }


    }

    public class SendMessageResult : ResultPackage
    {
        public string receiver { get; set; }
    }

    

    public class SignPackage
    {
        public string agentId { get; set; }
        public string corpId { get; set; }
        public string timeStamp { get; set; }
        public string nonceStr { get; set; }
        public string signature { get; set; }
        public string url { get; set; }
        public string rawstring { get; set; }
        public string jsticket { get; set; }        
    }
}






