using System.Collections.Generic;
using Assets.Classes.Core;
using UnityEngine;
using UnityEngine.Cloud.Analytics;
using UnityEngine.UI;

namespace Assets.Classes.Cloud
{
    public class UIPromotionBanner : UIPopup
    {

        public PromotionBanner Banner;


        public Image UIBannerImage;

        private void SendPromotionResponse(string status)
        {
            Analytics.Instance.SendEvent("PromotionBannerResponse", new Dictionary<string, object>()
                                                                  {
                                                                      {"status", status},
                                                                      {"banner-link", Banner.Link},
                                                                      {"banner-image", Banner.Image.Url.AbsoluteUri},
                                                                      {"banner-language", Banner.Language}
                                                                  });
        }

        private void SendAgreeToAnalytics()
        {
            SendPromotionResponse("agree");
        }

        private void SendDisagreeToAnalytics()
        {
            SendPromotionResponse("disagree");
        }

        public void OnAgreeClick()
        {
            SendAgreeToAnalytics();
            Application.OpenURL(Banner.Link);
        }

        public void OnDisagreeClick()
        {
            SendDisagreeToAnalytics();
            Hide();
        }

        public override void Show()
        {
            OnShowed();
            animator.Play("Show");
            UIBannerImage.sprite = Sprite.Create(Banner.Texture,
                new Rect(0, 0, Banner.Texture.width, Banner.Texture.height),
                new Vector2(Banner.Texture.width/2, Banner.Texture.height/2));
            base.Show();
        }

        public override void Hide()
        {
            animator.Play("Hide");
            OnHided();
            base.Hide();
        }
    }
}
