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
            get { return "RoseCheckers"; }
        }
        public override string AdvertsingProjectId
        {
            get { return "131626185"; }
        }
        public override string UnityAnalyticsProjectId
        {
            get { return "2ac294b3-3a67-49dc-9658-ade08db13324"; }
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
            yield return new Store();
        }

    }
}
