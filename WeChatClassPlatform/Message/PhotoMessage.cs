using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChatClassPlatform.Message
{
    public class PhotoMessage : AbstractMessage
    {
        public string PicUrl { get; set; }
        public string MediaId { get; set; }

        public override string ToXml()
        {
            const string format = @"<xml>
<ToUserName><![CDATA[{0}]]></ToUserName>
<FromUserName><![CDATA[{1}]]></FromUserName> 
<CreateTime>{2}</CreateTime>
<MsgType><![CDATA[{3}]]></MsgType>
<PicUrl><![CDATA[{4}]]></PicUrl>
<MediaId><![CDATA[{5}]]></MediaId>
<MsgId>{6}</MsgId>
</xml>";
            return string.Format(format, ToUserName, FromUserName, CreateTime, MsgType, PicUrl, MediaId, MsgId);
        }

    }
}