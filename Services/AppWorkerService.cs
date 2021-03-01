using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradeHub.MissedBlocksAlerter.Services;
using TradeHub.MissedBlocksAlerter.Settings;

namespace TradeHub.MissedBlocksAlerter.Services
{
    public class AppWorkerService : BackgroundService, IDisposable
    {
        private readonly ILogger<AppWorkerService> _logger;
        private readonly IOptionsSnapshot<AppSettings> _appSettings;

        private readonly PushbulletService _pushbulletService;
        private readonly TradescanService _tradescanService;

        private Timer _timer;

        private DateTime? _lastError = null;
        private long _missedBlocks = 0;

        public AppWorkerService(ILogger<AppWorkerService> logger, IOptionsSnapshot<AppSettings> appSettings, PushbulletService pushbulletService, TradescanService tradescanService)
        {
            _logger = logger;
            _appSettings = appSettings;
            _pushbulletService = pushbulletService;
            _tradescanService = tradescanService;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var deviceName = _appSettings.Value.Pushbullet.Target;
            var deviceIden = await _pushbulletService.GetDeviceIdenByDeviceName(deviceName);
            if (!string.IsNullOrEmpty(deviceIden))
            {
                _logger.LogInformation($"Target: {deviceName}, Identifier: {deviceIden}");

                await _pushbulletService.CreatePush(deviceIden, "Started successfully");

                _timer = new Timer((state) => DoWorkAsync(deviceIden), null, TimeSpan.Zero, TimeSpan.FromSeconds(_appSettings.Value.Supervision.SecondsBetween));
                while (!cancellationToken.IsCancellationRequested) { }

                await _pushbulletService.CreatePush(deviceIden, "Stopping");
            }
            else
            {
                _logger.LogError("An error occured while retrieving your target device identifier from Pushbullet API");
            }

            await Task.CompletedTask;
        }

        public async void DoWorkAsync(string deviceIden)
        {
            var blockHeightAndMissedBlocks = await _tradescanService.GetBlockHeightAndMissedBlocksByConsensusAddr(_appSettings.Value.Validator.ConsensusAddr);

            if (!blockHeightAndMissedBlocks.Key.HasValue || !blockHeightAndMissedBlocks.Value.HasValue)
            {
                if (_lastError.HasValue && (DateTime.Now - _lastError.Value).TotalMinutes < _appSettings.Value.ErrorReports.MinutesBetween)
                    return;

                if (_appSettings.Value.ErrorReports.Enabled)
                {
                    var errorMessage = "An error occurred while retrieving signing informations from Tradescan API";
                    _logger.LogError(errorMessage);
                    await _pushbulletService.CreatePush(deviceIden, errorMessage);

                    _lastError = DateTime.Now;
                }
            }
            else
            {
                var blockHeight = blockHeightAndMissedBlocks.Key.Value;
                var missedBlocks = blockHeightAndMissedBlocks.Value.Value;

                _logger.LogDebug($"Height: {blockHeight}, MissedBlocks: {missedBlocks}");

                if (missedBlocks >= _appSettings.Value.Thresholds.MaxBlocksMissed && _missedBlocks != missedBlocks)
                {
                    var warningMessage = $"Your validator missed {missedBlocks} block(s)";
                    _logger.LogWarning(warningMessage);
                    await _pushbulletService.CreatePush(deviceIden, warningMessage);

                    _missedBlocks = missedBlocks;
                }
            }
        }

        public new void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
