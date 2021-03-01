using System;
using System.Collections.Generic;
using System.Text;

namespace TradeHub.MissedBlocksAlerter.Settings
{
    public class AppSettings
    {
        public ValidatorSettings Validator { get; set; }
        public PushbulletSettings Pushbullet { get; set; }

        public SupervisionSettings Supervision { get; set; }
        public ThresholdsSettings Thresholds { get; set; }
        public ReportsSettings ErrorReports { get; set; }
        public ReportsSettings StatusReports { get; set; }
    }

    public class ValidatorSettings
    {
        public string ConsensusAddr { get; set; }
    }

    public class PushbulletSettings
    {
        public string AccessToken { get; set; }
        public string Target { get; set; }
    }

    public class SupervisionSettings
    {
        public string SigningInfosEndpoint { get; set; }
        public int SecondsBetween { get; set; }
    }

    public class ThresholdsSettings
    {
        public long MaxBlocksMissed { get; set; }
    }

    public class ReportsSettings
    {
        public bool Enabled { get; set; }
        public int MinutesBetween { get; set; }
    }
}
