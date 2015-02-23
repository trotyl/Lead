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
using WeChatClassPlatform.Models;

namespace WeChatClassPlatform.Controllers
{
    public class HomeController : Controller
    {
        public static readonly Dictionary<string, User> Users = new Dictionary<string, User>();

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

            UserCheck(dict);

            var raw = @"<xml>
<ToUserName><![CDATA[{0}]]></ToUserName>
<FromUserName><![CDATA[{1}]]></FromUserName>
<CreateTime>{2}</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<Content><![CDATA[{3}]]></Content>
</xml>";

            string result;
            switch (dict["MsgType"])
            {
                case "event":
                    result = DealWithEvent(dict);
                    break;
                case "text":
                    result = DealWithText(dict);
                    break;
                case "image":
                case "voice":
                case "video":
                case "location":
                default:
                    return null;
            }

            var response = String.Format(raw,
                dict["FromUserName"],
                dict["ToUserName"],
                dict["CreateTime"],
                result);

            return Content(response);
        }

        private void UserCheck(Dictionary<string, string> dict)
        {
            var name = dict["FromUserName"];
            if (!Users.ContainsKey(name))
            {
                var user = new User { Id = name, IsChatState = false, };
                Users[name] = user;
            }
        }

        private string DealWithText(Dictionary<string, string> req)
        {
            var user = req["FromUserName"];
            if (!Users[user].IsChatState)
            {
                return DealWithCommand(req);
            }

            var gbk = HttpUtility.UrlEncode(req["Content"], Encoding.GetEncoding("GBK"));
            var client = new HttpClient();
            var result = client.GetStringAsync(
               string.Format("http://dev.skjqr.com/api/weixin.php?email=yzj1995@vip.qq.com&appkey=9d6d258d0e8a3645b740615d0d007af0&msg={0}", gbk)).Result;
            return result.Replace("[msg]", "").Replace("[/msg]", "").Trim();
        }

        private string DealWithCommand(Dictionary<string, string> req)
        {
            var commands = req["Content"].Split(' ');
            var name = req["FromUserName"];
            var user = Users[name];
            string result = null;
            if (commands[0] == "set")
            {
                switch (commands[1])
                {
                    case "name":
                    case "username":
                    case "nickname":
                        user.Name = commands[2];
                        Users[name] = user;
                        result = "成功更改用户昵称~";
                        break;
                    case "number":
                    case "student":
                    case "id":
                    case "no":
                        user.Number = commands[2];
                        Users[name] = user;
                        result = "成功更改用户学号~";
                        break;
                    case "password":
                    case "pass":
                    case "pwd":
                        user.Password = commands[2];
                        Users[name] = user;
                        result = "成功更改用户密码~";
                        break;
                }
            }

            if (commands[0] == "get")
            {
                switch (commands[1])
                {
                    case "name":
                    case "username":
                    case "nickname":
                        result = "当前用户昵称为：" + user.Name;
                        break;
                    case "number":
                    case "student":
                    case "id":
                    case "no":
                        result = "当前用户学号为：" + user.Number;
                        break;
                    case "password":
                    case "pass":
                    case "pwd":
                        result = "无法查看明文密码，如密码有误请重新设置！";
                        break;
                }
            }

            return result ?? "指令过于高深...无法理解...";
        }

        private string DealWithEvent(Dictionary<string, string> req)
        {
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
            return result;
        }

        private string DealWithClick(Dictionary<string, string> requestDictionary)
        {
            string result;
            switch (requestDictionary["EventKey"])
            {
                case "simsimi":
                    var userName = requestDictionary["FromUserName"];
                    if (Users.ContainsKey(userName) && Users[userName].IsChatState)
                    {
                        Users[userName].IsChatState = false;
                        result = "聊天机器人已关闭~要是对我有什么不满可以现在就说哦~";
                    }
                    else
                    {
                        Users[userName].IsChatState = true;
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

        private string DealWithLocation(Dictionary<string, string> dict)
        {
            throw new NotImplementedException();
        }

        private string DealWithVideo(Dictionary<string, string> dict)
        {
            throw new NotImplementedException();
        }

        private string DealWithVoice(Dictionary<string, string> dict)
        {
            throw new NotImplementedException();
        }

        private string DealWithImage(Dictionary<string, string> dict)
        {
            throw new NotImplementedException();
        }
    }
}
