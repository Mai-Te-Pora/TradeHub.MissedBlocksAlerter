# TradeHub.MissedBlocksAlerter

TradeHub.MissedBlocksAlerter is a missed blocks alerter for Switcheo TradeHub.
It notify you on any device (phone, tablet or desktop) through Pushbullet if your configured thresholds are reached.

## Requirements
- Your validator consensus address *(swthvalcons[...])*.
- A [Pushbullet account](https://www.pushbullet.com/) with an access token.
- A [Pushbullet client](https://www.pushbullet.com/apps) installed on your preferred device.

## Installation
### Linux
```bash
curl -L https://github.com/Mai-Te-Pora/TradeHub.MissedBlocksAlerter/releases/download/v1.0.0/install-linux-x64.tar.gz | tar -xz
```

### Windows
Download and extract the latest release corresponding to your operating system (x64) or (x86).

[Latest releases](https://github.com/Mai-Te-Pora/TradeHub.MissedBlocksAlerter/releases)

## Configuration
Update the `appsettings.json` file with your information.

```json
{
  "Validator": {
    "ConsensusAddr": "<YOUR_VALIDATOR_CONSENSUS_ADDRESS>"
  },
  "Pushbullet": {
    "AccessToken": "<YOUR_PUSHBULLET_ACCESS_TOKEN>",
    "Target": "<YOUR_TARGET_DEVICE_NAME>"
  },
  "Supervision": {
    "SigningInfosEndpoint": "https://tradescan.switcheo.org/slashing/signing_infos?limit=100",
    "SecondsBetween": 120
  },
  "Thresholds": {
    "MaxBlocksMissed": 20
  },
  "ErrorReports": {
    "Enabled": true,
    "MinutesBetween": 2
  }
}
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[MIT](https://choosealicense.com/licenses/mit/)
