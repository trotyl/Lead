using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WeChatClassPlatform.Message
{
    public static class Message
    {
        public static AbstractMessage Parse(string xml)
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
            AbstractMessage msg;
            switch (msgType)
            {
                case "text":
                    msg = new TextMessage(xml);
                    break;
                case "image":
                    msg = new ImageMessage();
                    break;
                default:
                    msg = null;
                    break;
            }
            return msg;
        }

    }
}