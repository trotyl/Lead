using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChatClassPlatform.Message
{
    public abstract class AbstractMessage
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public int CreateTime { get; set; }
        public string MsgType { get; set; }
        public int MsgId { get; set; }

        public abstract string ToXml();
    }
}