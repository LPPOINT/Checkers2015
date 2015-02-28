using System;
using CUDLR;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Advertisements.Optional;
using UnityEngine.UI;

namespace Assets.Classes.Core
{
    public  class Advertisements : GameSystemBase
    {

        public  readonly TimeSpan MinIntervalBetweenAdsAfterRun = new TimeSpan(0, 0, 1, 0);
        public  readonly string IsAdvertisementsEnabledPlayerPrefsKey = "IsAdvertisementsEnabled";

        public  bool IsAdvertisementsEnabled { get; private set; }

        public const string VideoAdsZone = "defaultVideoAndPictureZone";
        public const string RewardAdsZone = "rewardedVideoZone";
        public const string PictureAdsZone = "pictureAds";

        public override void Load()
        {
            Advertisement.Initialize(GameCore.Instance.AdvertsingProjectId);
            IsAdvertisementsEnabled = PlayerPrefs.GetInt(IsAdvertisementsEnabledPlayerPrefsKey, 1) == 1;
        }

        [CUDLRCommand("ads_enable")]
        public  void EnableAds()
        {
            IsAdvertisementsEnabled = true;
            PlayerPrefs.SetInt(IsAdvertisementsEnabledPlayerPrefsKey, 1);
        }

        [CUDLRCommand("ads_disable")]
        public  void DisableAds()
        {
            IsAdvertisementsEnabled = false;
            PlayerPrefs.SetInt(IsAdvertisementsEnabledPlayerPrefsKey, 0);
        }


        [CUDLRCommand("ads_show")]
        public  void ShowAdvertisement(string zoneId)
        {
            if (string.IsNullOrEmpty(zoneId)) 
                zoneId = VideoAdsZone;
            ShowAdvertisement(zoneId, null);
        }
        public  void ShowAdvertisement(string zoneId, Action<ShowResult> onComplete, ref DateTime? showTime)
        {
            if (Advertisement.isReady())
            {
                Advertisement.Show(zoneId, new ShowOptions()
                {
                    resultCallback = onComplete,
                    pause = true
                });

                if(showTime != null)
                    showTime = DateTime.Now;
            }
            else
            {
                Debug.LogWarning("ShowAdvertisement(): Advertisement.isReady() == false");
            }
        }
        public  void ShowAdvertisement(string zoneId, Action<ShowResult> onComplete)
        {
            DateTime? d = null; //TODO: fix this shit
            ShowAdvertisement(zoneId, null, ref d);
        }


        public  void ShowVideo(Action<ShowResult> onComplete)
        {
            ShowAdvertisement(VideoAdsZone, onComplete, ref lastVideoShowTime);
        }
        public  void ShowRewardVideo(Action<ShowResult> onComplete)
        {
            ShowAdvertisement(RewardAdsZone, onComplete, ref lastVideoShowTime);
        }
        public  void ShowPicture(Action<ShowResult> onComplete)
        {
            ShowAdvertisement(PictureAdsZone, onComplete, ref lastVideoShowTime);
        }


        private  DateTime? lastVideoShowTime;
        public  bool ShouldShowVideoAfterRun
        {
            get { return IsAdvertisementsEnabled && (lastVideoShowTime == null || (DateTime.Now - lastVideoShowTime) > MinIntervalBetweenAdsAfterRun); }
        }

        public  bool ProcessRunComplete()
        {
            var r = ShouldShowVideoAfterRun;
            if(r)
                ShowVideo(null);
            return r;
        }


    }
}
