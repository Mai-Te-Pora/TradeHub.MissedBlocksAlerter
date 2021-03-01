using System;
using System.Collections.Generic;
using System.Text;

namespace TradeHub.MissedBlocksAlerter.Models.Tradescan.Responses
{
    public class SigningInfosResponse
    {
        public long Height { get; set; }
        public List<SigningInfosResponseResult> Result { get; set; }
    }

    public class SigningInfosResponseResult
    {
        public SigningInfosResponseResult() { }

        public string Address { get; set; }
        public long MissedBlocksCounter { get; set; }
    }
}
