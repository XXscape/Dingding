using DingDingInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebDingSendDemo2
{

    public class AuthController : ApiController
    {
        #region GetSignPackage Function                        
        /// <summary>  
        /// 获取签名包  
        /// </summary>  
        public SignPackage GetSignPackage(String url)
        {
            //对于首页的URL，由于可以直接使用域名跳转来进入(比如http://www.king-ecs.com)，所以这时取的URL其实是域名，  
            //而不是当前页的URL(http://www.king-ecs.com/index.html)，这时产生的签名包是针对域名的(即http://www.king-ecs.com)。  
            //这样一来就有会造成服务端计算的签名和钉钉计算出的签名不致，从而导致验证失败。  
            //所以首页URL跳转需要签名名时，需要将URL传入以计算签名包。  
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;  
            var signPackage = SdkTool.FetchSignPackage(url);
            return signPackage;
        }
        #endregion
    }
}