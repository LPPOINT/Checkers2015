using Assets.Classes.Core;
using CUDLR;
using Parse;
using UniRx;
using UnityEngine;

namespace Assets.Classes.Cloud
{
    public class PromotionBanners : GameSystemBase
    {

        public override void Load()
        {
            ParseObject.RegisterSubclass<PromotionBanner>();
            ReceiveBannerAndShowIfNeeded();
        }

        public  void WaitForReadyAndShowBanner(PromotionBanner banner)
        {
            if (banner.IsTextureDownloaded) ShowBanner(banner);
            else banner.DownloadTexture(d => ShowBanner(banner));
        }

        public  void ShowBanner(PromotionBanner banner)
        {
            if (!banner.IsTextureDownloaded)
            {
                WaitForReadyAndShowBanner(banner);
                return;
            }
            UIPopups.Instance.ShowPromotionBanner(banner);
        }

        [CUDLRCommand("promotion_show")]
        public  void ReceiveBannerAndShowIfNeeded()
        {
            if(!GameCore.Instance.IsPromotionBannerSupported)
                return;
            PromotionBanner.ReceiveBannerFromCloudWithCompletionHandler(banner => Scheduler.MainThread.Schedule(() => ShowBanner(banner)), "en-EN");
        }
    }
}
