https://oa.dingtalk.com/index.htm<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebDingJSAPI.aspx.cs" Inherits="WebDingSendDemo.WebDingJSAPI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="scripts/jquery-1.9.1.js"></script>
    <%--<script src="http://g.alicdn.com/dingding/open-develop/0.7.0/dingtalk.js"></script>--%>
    <script src="http://g.alicdn.com/dingding/dingtalk-pc-api/2.5.0/index.js"></script>
    <%--<script src="scripts/dingding.js"></script>--%>
   <script type="text/javascript">


    var _config = {
        appId: "37669503",
        corpId: "<%=_mCorpid%>",
        timeStamp: new Date().getTime() / 1000,
        nonce: "huashuda",
        signature: "<%=signature%>"
    };

       DingTalkPC.config({
           agentId: _config.appId, // 必填，微应用ID
           corpId: _config.corpId,//必填，企业ID
           timeStamp: _config.timeStamp, // 必填，生成签名的时间戳
           nonceStr: _config.nonce, // 必填，生成签名的随机串
           signature: _config.signature, // 必填，签名
           jsApiList: ['device.notification.alert', 'device.notification.confirm'] // 必填，需要使用的jsapi列表
       });

       DingTalkPC.ready(function (res) {
           /*{
               authorizedAPIList: ['device.notification.alert'], //已授权API列表
               unauthorizedAPIList: [''], //未授权API列表
           }*/
           //接口操作应该在ready后才可调用
       });


//    //jsapi的配置。我注销之后代码仍然可正确执行。
//    dd.config({
//        appId: _config.appId,
//        corpId: _config.corpId,
//        timeStamp: _config.timeStamp,
//        nonceStr: _config.nonce,
//        signature: _config.signature,
//        jsApiList: [
//            'device.geolocation.get',
//            'runtime.info',
//            'biz.contact.choose',
//            'device.notification.confirm',
//            'device.notification.alert',
//            'device.notification.prompt',
//            'biz.ding.post',
//            'runtime.permission.requestAuthCode',
//            'biz.ding.post',
//            'biz.contact.choose',
//            'biz.util.uploadImageFromCamera',
//            'biz.contact.complexChoose']
//    });


    
//    dd.ready(function () {        
//        dd.device.geolocation.get({
//            targetAccuracy : '',
//            onSuccess : function (result) {
//                location.href = '@Url.Action("ServerApi","Login")?code=' + result.longitude;               
//            },
//            onFail : function (err) {
//                location.href = '@Url.Action("ServerApi", "Login")?code=' + err.code + ":" + err.errorMessage ;
//            }
//        });



//        // 这里写一个简单的jsapi的弹用，其它api的调用请参照钉钉开发文档-客户端开发文档
//        //dd.device.notification.alert({
//        //    message: "测试弹窗",
//        //    title: "提示",//可传空
//        //    buttonName: "收到",
//        //    onSuccess: function () {
//        //        /*回调*/
//        //    },
//        //    onFail: function (err) { }
//        //});


//        //能成功调用扫一扫
//        dd.biz.util.scan({
//            type: "qrCode",//type为qrCode或者barCode
//            onSuccess: function (data) {
//                location.href = '@Url.Action("ServerApi","Login")?code=' + data.text;
//                //onSuccess将在扫码成功之后回调
//                /* data结构
//                  { 'text': String}
//                */
//            },
//            onFail: function (err) {
//                location.href = '@Url.Action("ServerApi","Login")?code=' + err+"sd";
//            }
//        })


        
       
//    })
</script>
</head>
<body>
    <h1>测试发送Ding</h1>
    <input id="txtMesg" type="text" placeholder="请输入要发送的消息.." />
    <input id="btnSend" type="button" value="Send" />
</body>
</html>
