using System;

namespace Assets.Classes.Core
{
    /// <summary>
    /// NOT TESTED!
    /// </summary>
    public class Sessions : UniqueGameSystem<Sessions>
    {

        public static class Keys
        {
            public static string LastSessionStartDate = "LastSessionStartDate";
            public static string LastSessionEndDate = "LastSessionEndDate";
            public static string LastSessionVersion = "LastSessionVersion";
            public static string CurrentSessionDate = "CurrentSessionDate";
            public static string CurrentSessionVersion = "CurrentSessionVersion";
        }

        public const string SessionsVersionsChangedEvent = "SessionsVersionsChanged";

        public DateTime? LastSessionStartDate { get; private set; }
        public DateTime? LastSessionEndDate { get; private set; }
        public TimeSpan? LastSessionDuration { get; private set; }
        public Version LastSessionVersion { get; private set; }

        public bool IsFirstSession
        {
            get { return LastSessionStartDate == null; }
        }
        public bool IsLastSessionWasUnexpectedShutdowned
        {
            get { return LastSessionStartDate != null && LastSessionEndDate == null; }
        }

        public override void Load()
        {
            LastSessionStartDate = GameCore.Instance.Database.GetDate(Keys.LastSessionStartDate);
            LastSessionEndDate = GameCore.Instance.Database.GetDate(Keys.LastSessionEndDate);

            if (LastSessionStartDate != null && LastSessionEndDate != null)
            {
                LastSessionDuration = LastSessionEndDate.Value - LastSessionStartDate.Value;
            }

           LastSessionVersion = GameCore.Instance.Database.GetVersion(Keys.LastSessionVersion);

           GameCore.Instance.Database.SetDate(Keys.CurrentSessionDate, DateTime.Now);
           GameCore.Instance.Database.SetVersion(Keys.CurrentSessionVersion, GameCore.Instance.GameVersion);

           GameCore.Instance.Database.SetDate(Keys.LastSessionStartDate, DateTime.Now);
           GameCore.Instance.Database.SetVersion(Keys.LastSessionVersion, GameCore.Instance.GameVersion);

            if (LastSessionVersion != GameCore.Instance.GameVersion)
            {
                GameMessenger.Broadcast(SessionsVersionsChangedEvent, LastSessionVersion, GameCore.Instance.GameVersion);
            }

        }

        public override void Shutdown()
        {
            GameCore.Instance.Database.SetDate(Keys.LastSessionEndDate, DateTime.Now);
        }


    }
}
