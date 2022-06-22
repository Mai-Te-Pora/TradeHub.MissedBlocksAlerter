using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TradeHub.MissedBlocksAlerter.Models.Pushbullet.Responses;
using TradeHub.MissedBlocksAlerter.Settings;

namespace TradeHub.MissedBlocksAlerter.Services
{
    public class PushbulletService
    {
        private readonly ILogger<PushbulletService> _logger;
        private readonly AppSettings _appSettings;

        private RestClient _restClient;
        private JsonSerializerSettings _jsonSerializerSettings;

        public PushbulletService(ILogger<PushbulletService> logger, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _appSettings = appSettings?.Value;

            _restClient = new RestClient(Constants.PUSHBULLET_BASE_URL);
            _restClient.AddDefaultHeader("Access-Token", _appSettings.Pushbullet.AccessToken);
            _restClient.UseNewtonsoftJson(new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver() { NamingStrategy = new SnakeCaseNamingStrategy() }
            });
        }

        public async Task<string> GetDeviceIdenByDeviceName(string deviceName)
        {
            var devicesListReq = new RestRequest(Constants.PUSHBULLET_ENDPOINT_DEVICES)
            {
                RequestFormat = DataFormat.Json
            };
            var devicesList = await _restClient.ExecuteAsync<DevicesListResponse>(devicesListReq);

            return devicesList.Data?.Devices?.FirstOrDefault(x => x.Nickname == deviceName)?.Iden; ;
        }

        public async Task<bool> CreatePush(string deviceIden, string message)
        {
            var createPushReq = new RestRequest(Constants.PUSHBULLET_ENDPOINT_PUSH, Method.Post)
            {
                RequestFormat = DataFormat.Json
            };
            createPushReq.AddJsonBody(new
            {
                DeviceIden = deviceIden,
                Type = "note",
                Title = "MissedBlocksAlerter",
                Body = message
            });

            var createPush = await _restClient.ExecuteAsync(createPushReq);

            return createPush.StatusCode == HttpStatusCode.OK;
        }
    }
}
