﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace WeChatClassPlatform.Controllers
{
    public class HomeController : Controller
    {
        private static Dictionary<string, bool> _chatSwitch; 

        public ActionResult Index()
        {
            switch (Request.HttpMethod.ToUpper())
            {
                case "GET":
                    return DealWithHttpGet();
                case "POST":
                    return DealWithHttpPost();
                default:
                    return null;
            }
        }

        private ActionResult DealWithHttpGet()
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
            return null;
        }

        private ActionResult DealWithHttpPost()
        {
            StreamReader stream = new StreamReader(Request.InputStream);
            string xml = stream.ReadToEnd();
            XElement rootElement = XElement.Parse(xml);
            Dictionary<string, string> dict = rootElement.Elements().ToDictionary(el => el.Name.LocalName, el => el.Value);

            switch (dict["MsgType"])
            {
                case "event":
                    return DealWithEvent(dict);
                case "text":
                //return DealWithText(dict);
                case "image":
                //return DealWithImage(dict);
                case "voice":
                //return DealWithVoice(dict);
                case "video":
                //return DealWithVideo(dict);
                case "location":
                    //return DealWithLocation(dict);
                    return DealWithText(xml);
                default:
                    return null;
            }
        }

        private ActionResult DealWithText(string xml)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://dev.skjqr.com");
                var content = new FormUrlEncodedContent(new[] 
                {
                    new KeyValuePair<string, string>("", xml)
                });
                var result = client.PostAsync("/api/u/yzj1995@vip.qq.com/index.php", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                return Content(resultContent);
            }
        }

        private ActionResult DealWithEvent(Dictionary<string, string> requestDictionary)
        {
            Dictionary<string, string> respnseDictionary = new Dictionary<string,string>();
            respnseDictionary["ToUserName"] = requestDictionary["FromUserName"];
            respnseDictionary["FromUserName"] = requestDictionary["ToUserName"];
            respnseDictionary["CreateTime"] = requestDictionary["CreateTime"];
            respnseDictionary["MsgType"] = "<![CDATA[text]]>";

            string result;
            switch (requestDictionary["Event"])
            {
                case "subscribe":
                    result = "感谢订阅！么么哒！";
                    break;
                case "CLICK":
                    result = DealWithClick(requestDictionary);
                    break;
                case "LOCATION":
                case "VIEW":
                default:
                    return null;
            }
            respnseDictionary["Content"] = string.Format("<![CDATA[{0}]]>", result);
            XElement el = new XElement("root", requestDictionary.Select(kv => new XElement(kv.Key, kv.Value)));
            return Content(el.ToString());
        }

        private string DealWithClick(Dictionary<string, string> requestDictionary)
        {
            string result;
            switch (requestDictionary["EventKey"])
            {
                case "simsimi":
                    var userName = requestDictionary["FromUserName"];
                    if (_chatSwitch.ContainsKey(userName) && _chatSwitch[userName])
                    {
                        _chatSwitch[userName] = false;
                        result = "聊天机器人已关闭~要是对我有什么不满可以现在就说哦~";
                    }
                    else
                    {
                        _chatSwitch[userName] = true;
                        result = "聊天机器人已开启~可以尽情聊天啦~";
                    }
                    break;
                case "CLICK":
                    result = DealWithClick(requestDictionary);
                    break;
                case "food":
                case "reading":
                case "study":
                case "activity":
                case "schedule":
                case "exam":
                case "score":
                default:
                    result = "没太懂你在做什么啦~抱歉咯~";
                    break;
            }
            return result;
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

        private ActionResult DealWithLocation(Dictionary<string, string> dict)
        {
            throw new NotImplementedException();
        }

        private ActionResult DealWithVideo(Dictionary<string, string> dict)
        {
            throw new NotImplementedException();
        }

        private ActionResult DealWithVoice(Dictionary<string, string> dict)
        {
            throw new NotImplementedException();
        }

        private ActionResult DealWithImage(Dictionary<string, string> dict)
        {
            throw new NotImplementedException();
        }
    }
}
