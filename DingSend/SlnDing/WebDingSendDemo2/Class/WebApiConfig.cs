using DingDingInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebDingSendDemo2
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //注册路由映射  
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",//action表示按方法路由  
                defaults: new { id = RouteParameter.Optional }
            );
        }

        #region Register Function      
        ///// <summary>  
        ///// 登记群  
        ///// </summary>  
        ///// <param name="chatId"></param>  
        //[HttpGet]//方法名以Get开头则不需要加HttpGet的标识，否则无法以Get方式发起请求  
        //public RequestResult Register(String chatId)
        //{
        //    //记录到数据库  
        //    return null;
        //}
        #endregion
    }


}