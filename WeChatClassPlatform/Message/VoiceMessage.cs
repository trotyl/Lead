using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChatClassPlatform.Message
{
    public class VoiceMessage : AbstractMessage
    {
        public string MediaId { get; set; }
        public string Format { get; set; }

        public override string ToXml()
        {
            const string format = @"<xml>
<ToUserName><![CDATA[{0}]]></ToUserName>
<FromUserName><![CDATA[{1}]]></FromUserName> 
<CreateTime>{2}</CreateTime>
<MsgType><![CDATA[{3}]]></MsgType>
<MediaId><![CDATA[{4}]]></MediaId>
<Format><![CDATA[{5}]]></Format>
<MsgId>{6}</MsgId>
</xml>";
            return string.Format(format, ToUserName, FromUserName, CreateTime, MsgType, MediaId, Format, MsgId);
        }
    }
}