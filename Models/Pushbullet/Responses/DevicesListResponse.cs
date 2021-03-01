using System;
using System.Collections.Generic;
using System.Text;

namespace TradeHub.MissedBlocksAlerter.Models.Pushbullet.Responses
{
    public class DevicesListResponse
    {
        public List<DevicesListResponseResult> Devices { get; set; }
    }

    public class DevicesListResponseResult
    {
        public string Iden { get; set; }
        public string Nickname { get; set; }
    }
}
