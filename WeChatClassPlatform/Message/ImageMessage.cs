using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace WeChatClassPlatform.Message
{
    public class ImageMessage : AbstractMessage
    {
        public string PicUrl { get; set; }
        public string MediaId { get; set; }

        public ImageMessage() { }

        public ImageMessage(string xml)
        {
            const string format = @"<xml>
<ToUserName><?<ToUserName>.*?></ToUserName>
<FromUserName><?<FromUserName>.*?></FromUserName> 
<CreateTime>?<CreateTime>\d+?</CreateTime>
<MsgType><?<MsgType>.*?></MsgType>
<PicUrl><?<PicUrl>.*?></PicUrl>
<MediaId><?<MediaId>.*?></MediaId>
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
                PicUrl = groups["PicUrl"].Value;
                MediaId = groups["MediaId"].Value;
                MsgId = int.Parse(groups["MsgId"].Value);
            }
        }


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