using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WeChatClassPlatform.Message
{
    public class TextMessage
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public int CreateTime { get; set; }
        public string MsgType { get; set; }
        public string Content { get; set; }
        public int MsgId { get; set; }


        public TextMessage() { }

        public TextMessage(string xml)
        {
            const string format = @"<xml>
<ToUserName><?<ToUserName>.*?></ToUserName>
<FromUserName><?<FromUserName>.*?></FromUserName> 
<CreateTime>?<CreateTime>\d+?</CreateTime>
<MsgType><?<MsgType>.*?></MsgType>
<Content><?<Content>.*?></Content>
<MsgId>?<MsgId>\d+?</MsgId>
</xml>";
            Regex re = new Regex(format, RegexOptions.Compiled);
            Match match = re.Match(xml);
            if (match.Success)
            {
                GroupCollection groups = match.Groups;
                ToUserName = groups["ToUserName"].Value;
                FromUserName = groups["FromUserName"].Value;
                CreateTime = int.Parse(groups["CreateTime"].Value);
                MsgType = groups["MsgType"].Value;
                Content = groups["Content"].Value;
                MsgId = int.Parse(groups["MsgId"].Value);
            }
        }

        public string ToXml()
        {
            const string format = @"<xml>
<ToUserName><![CDATA[{0}]]></ToUserName>
<FromUserName><![CDATA[{1}]]></FromUserName> 
<CreateTime>{2}</CreateTime>
<MsgType><![CDATA[{3}]]></MsgType>
<Content><![CDATA[{4}]]></Content>
<MsgId>{5}</MsgId>
</xml>";
            return string.Format(format, ToUserName, FromUserName, CreateTime, MsgType, Content, MsgId);
        }
    }
}