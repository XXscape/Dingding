/// <reference path="zepto.min.js" />  
var jsApiList = ['biz.chat.chooseConversationByCorpId', 'biz.chat.toConversation'];
var global = {
    corpId: '',
    configJsSdk: function (jsApiList, fnReady, fnError) {
        /// <summary>配置微信的JSSDK</summary>  
        /// <param name="jsApiList" type="function">要请求的api列表,以['biz.chat.chooseConversationByCorpId','biz.chat.toConversation']的形式</param>  
        /// <param name="fnReady" type="function">dd.ready调用的函数</param>  
        /// <param name="fnError" type="function">dd.error调用的函数</param>    
        var url = window.location.href;
        $.getJSON("/api/Auth/GetSignPackage", "url=" + window.location.href, function (response, textStatus, jqXHR) {
            try {
                var signPackage = response;
                global.corpId = signPackage.corpId;
                dd.config(
                       {
                           agentId: signPackage.agentId,
                           corpId: signPackage.corpId,
                           timeStamp: signPackage.timeStamp,
                           nonceStr: signPackage.nonceStr,
                           signature: signPackage.signature,
                           jsApiList: jsApiList
                       });
                dd.ready(function () {
                    if (fnReady != null) {
                        fnReady();
                    }
                });

                dd.error(function (err) {
                    if (err == null) {
                        alert('dd error: ' + JSON.stringify(err));
                    }
                    else {
                        fnError();
                    }
                });
            }
            catch (e) {

            }
        });
    }
}