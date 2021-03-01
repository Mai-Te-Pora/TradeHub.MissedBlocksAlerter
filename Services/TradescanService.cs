using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.MissedBlocksAlerter.Models.Tradescan.Responses;
using TradeHub.MissedBlocksAlerter.Settings;

namespace TradeHub.MissedBlocksAlerter.Services
{
    public class TradescanService
    {
        private readonly ILogger<TradescanService> _logger;
        private readonly IOptionsSnapshot<AppSettings> _appSettings;

        private RestClient _restClient;

        public TradescanService(ILogger<TradescanService> logger, IOptionsSnapshot<AppSettings> appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
            _restClient = new RestClient();
        }

        public async Task<KeyValuePair<long?, long?>> GetBlockHeightAndMissedBlocksByConsensusAddr(string consensusAddr)
        {
            var signingInfosReq = new RestRequest(_appSettings.Value.Supervision.SigningInfosEndpoint, Method.GET, DataFormat.Json);
            var signingInfos = await _restClient.ExecuteAsync<SigningInfosResponse>(signingInfosReq);

            var blockHeight = signingInfos.Data?.Height;
            var missedBlocks = signingInfos.Data?.Result?.FirstOrDefault(x => x.Address == consensusAddr)?.MissedBlocksCounter;

            return new KeyValuePair<long?, long?>(blockHeight, missedBlocks);
        }
    }
}
