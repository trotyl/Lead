using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WeChatClassPlatform.Message
{
    public class Message
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public int CreateTime { get; set; }
        public string MsgType { get; set; }
        public int MsgId { get; set; }

        public Message Parse(string xml)
        {
            const string pattern = @"<MsgType><?<MsgType>.*?></MsgType>";
            Regex re = new Regex(pattern, RegexOptions.Compiled);
            Match match = re.Match(pattern);
            string msgType = "";
            if (match.Success)
            {
                GroupCollection groups = match.Groups;
                msgType = groups["MsgType"].Value;
            }
            Message msg;
            switch (msgType)
            {
                case "text":
                    msg = new TextMessage(xml);
                    break;
                default:
                    msg = null;
                    break;
            }
            return msg;
        }
    }
}