using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDingSendDemo2
{
    /// <summary>  
    /// Url的Key  
    /// </summary>  
    public sealed class Keys
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
    public sealed class Urls
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
}