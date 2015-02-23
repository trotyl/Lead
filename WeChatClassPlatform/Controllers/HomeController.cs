using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace WeChatClassPlatform.Controllers
{
    public class HomeController : Controller
    {
        private static Dictionary<string, bool> _chatSwitch = new Dictionary<string, bool>();

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
                    return DealWithText(dict["Content"]);
                default:
                    return null;
            }
        }

        private ActionResult DealWithText(string text)
        {
            var gbk = HttpUtility.UrlEncode(text, Encoding.GetEncoding("GBK"));
            var client = new HttpClient();
            var result = client.GetStringAsync(
               string.Format("http://dev.skjqr.com/api/weixin.php?email=yzj1995@vip.qq.com&appkey=9d6d258d0e8a3645b740615d0d007af0&msg={0}", gbk)).Result;
            return Content(result.Replace("[msg]", "").Replace("[/msg]", ""));
        }

        private ActionResult DealWithEvent(Dictionary<string, string> req)
        {
            var raw = @"<xml>
<ToUserName><![CDATA[{0}]]></ToUserName>
<FromUserName><![CDATA[{1}]]></FromUserName>
<CreateTime>{2}</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<Content><![CDATA[{3}]]></Content>
</xml>";

            string result;
            switch (req["Event"])
            {
                case "subscribe":
                    result = "感谢订阅！么么哒！";
                    break;
                case "CLICK":
                    result = DealWithClick(req);
                    break;
                case "LOCATION":
                case "VIEW":
                default:
                    return null;
            }
            var response = String.Format(raw,
                req["FromUserName"],
                req["ToUserName"],
                req["CreateTime"],
                result);
            return Content(response);
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
