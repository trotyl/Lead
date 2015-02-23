using System;
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
                case "<![CDATA[event]]>":
                    return DealWithEvent(dict);
                case "<![CDATA[text]]>":
                //return DealWithText(dict);
                case "<![CDATA[image]]>":
                //return DealWithImage(dict);
                case "<![CDATA[voice]]>":
                //return DealWithVoice(dict);
                case "<![CDATA[video]]>":
                //return DealWithVideo(dict);
                case "<![CDATA[location]]>":
                    //return DealWithLocation(dict);
                    return DealWithText(dict);
                default:
                    return null;
            }
        }

        private ActionResult DealWithText(Dictionary<string, string> dict)
        {
            return Redirect("http://dev.skjqr.com/api/u/yzj1995@vip.qq.com/index.php");
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
                case "<![CDATA[subscribe]]>":
                    result = "感谢订阅！么么哒！";
                    break;
                case "<![CDATA[CLICK]]>":
                    result = DealWithClick(requestDictionary);
                    break;
                case "<![CDATA[LOCATION]]>":
                case "<![CDATA[VIEW]]>":
                default:
                    return null;
            }
            respnseDictionary["Content"] = string.Format("<![CDATA[{0}]]>", result);
            XElement el = new XElement("root", requestDictionary.Select(kv => new XElement(kv.Key, kv.Value)));
            return Content(el.ToString());
        }

        private string DealWithClick(Dictionary<string, string> requestDictionary)
        {
            switch (requestDictionary["Event"])
            {
                case "<![CDATA[subscribe]]>":
                    result = "感谢订阅！么么哒！";
                    break;
                case "<![CDATA[CLICK]]>":
                    result = DealWithClick(requestDictionary);
                    break;
                case "<![CDATA[LOCATION]]>":
                case "<![CDATA[VIEW]]>":
                default:
                    return null;
            }
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
