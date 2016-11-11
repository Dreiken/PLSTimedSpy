using Rocket.API;

namespace PLSTimedSpy
{
    public class PLSTimedSpyConfiguration : IRocketPluginConfiguration
    {
        public ushort TimeSpyFrequency, KillSpyScreens, TimeSpyScreens, PingVerificationTimeSeconds;
        public bool BackupSpy, TimeSpy, KillSpy, OnlySteamId;

        public PLSTimedSpyConfiguration()
        {

        }

        public void LoadDefaults()
        {
            TimeSpyFrequency = 300;
            TimeSpy = true;
            KillSpy = true;
            BackupSpy = true;
            OnlySteamId = false;
            KillSpyScreens = 4;
            TimeSpyScreens = 4;
        }
    }
}
