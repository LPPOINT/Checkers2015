using System;
using System.Collections.Generic;
using Assets.Classes.Cloud;
using Assets.Classes.Core;
using Assets.Classes.Effects;

namespace Assets.Classes.Implementation
{
    public class Checkers : GameCore
    {

        public static Checkers ImplementationInstance
        {
            get { return Instance as Checkers; }
        }

        public override Version GameVersion
        {
            get { return new Version(0, 1); }
        }
        public override string Name
        {
            get { return "Checkers"; }
        }
        public override string AdvertsingProjectId
        {
            get { return ""; }
        }
        public override string UnityAnalyticsProjectId
        {
            get { return ""; }
        }
        public override bool IsPromotionBannerSupported
        {
            get { return false; }
        }


        protected override IGameDatabase InitializeDatabase()
        {
            return new PPGameDatabase();
        }

        protected override IEnumerable<IGameSystem> InitializeCustomGameSystems()
        {
            return new List<IGameSystem>();
        }

    }
}
