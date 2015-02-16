using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeChatClassPlatform.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                string echoStr = Request.QueryString["echoStr"];
                if (String.IsNullOrEmpty(echoStr))
                {
                    return View();
                }
                if (CheckSignature())
                {
                    return Content(echoStr);
                }
            }

            else if (Request.HttpMethod.ToUpper() == "POST")
            {
                StreamReader stream = new StreamReader(Request.InputStream);
                string xml = stream.ReadToEnd();
                return null;
                //processRequest(xml);  
            }

            return null;
        }

        private bool CheckSignature()
        {
            string token = "Trotyl";
            string signature = Request.QueryString["signature"];
            string timestamp = Request.QueryString["timestamp"];
            string nonce = Request.QueryString["nonce"];
            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            return tmpStr.ToLower() == signature.ToLower();
        }

        public void processRequest(String xml)
        {
            //try
            //{

            //    // xml请求解析    
            //    Hashtable requestHT = WeixinServer.ParseXml(xml);

            //    // 发送方帐号（open_id）    
            //    string fromUserName = (string)requestHT["FromUserName"];
            //    // 公众帐号    
            //    string toUserName = (string)requestHT["ToUserName"];
            //    // 消息类型    
            //    string msgType = (string)requestHT["MsgType"];

            //    //文字消息  
            //    if (msgType == ReqMsgType.Text)
            //    {
            //        // Response.Write(str);  

            //        string content = (string)requestHT["Content"];
            //        if (content == "1")
            //        {
            //            // Response.Write(str);  
            //            Response.Write(GetNewsMessage(toUserName, fromUserName));
            //            return;
            //        }
            //        if (content == "2")
            //        {
            //            Response.Write(GetUserBlogMessage(toUserName, fromUserName));
            //            return;
            //        }
            //        if (content == "3")
            //        {
            //            Response.Write(GetGroupMessage(toUserName, fromUserName));
            //            return;
            //        }
            //        if (content == "4")
            //        {
            //            Response.Write(GetWinePartyMessage(toUserName, fromUserName));
            //            return;
            //        }
            //        Response.Write(GetMainMenuMessage(toUserName, fromUserName, "你好，我是vinehoo,"));

            //    }
            //    else if (msgType == ReqMsgType.Event)
            //    {
            //        // 事件类型    
            //        String eventType = (string)requestHT["Event"];
            //        // 订阅    
            //        if (eventType == ReqEventType.Subscribe)
            //        {

            //            Response.Write(GetMainMenuMessage(toUserName, fromUserName, "谢谢您的关注！,"));

            //        }
            //        // 取消订阅    
            //        else if (eventType == ReqEventType.Unsubscribe)
            //        {
            //            // TODO 取消订阅后用户再收不到公众号发送的消息，因此不需要回复消息    
            //        }
            //        // 自定义菜单点击事件    
            //        else if (eventType == ReqEventType.CLICK)
            //        {
            //            // TODO 自定义菜单权没有开放，暂不处理该类消息    
            //        }
            //    }
            //    else if (msgType == ReqMsgType.Location)
            //    {
            //    }


            //}
            //catch (Exception e)
            //{

            //}
        }
    }
}
