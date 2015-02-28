using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Assets.Classes.Core;
using Assets.Classes.Foundation.Extensions;
using Parse;

using UniRx;
using UnityEngine;

namespace Assets.Classes.Cloud
{
    [ParseClassName("PromotionBanner")]
    public class PromotionBanner : ParseObject
    {


        public const string PromotionBannerTextureFailedEvent = "PromotionBannerTextureFailed";
        public const string PromotionBannerTextureSuccessfullEvent = "PromotionBannerTextureSuccessfull";
        public const string PromotionBannerReceiveFailedEvent = "PromotionBannerReceiveFailed";
        public const string PromotionBannerReceiveSuccessfullEvent = "PromotionBannerReceiveSuccessfull";
        public const string PromotionBannerRequestedLanguageNotFoundEvent = "PromotionBannerRequestedLanguageNotFound";

        [ParseFieldName("Link")]
        public string Link
        {
            get { return GetProperty<string>("Link"); }
            set { SetProperty(value, "Link");}
        }

        [ParseFieldName("Image")]
        public ParseFile Image
        {
            get { return GetProperty<ParseFile>("Image"); }
            set { SetProperty(value, "Image"); }
        }

        [ParseFieldName("Language")]
        public string Language
        {
            get { return GetProperty<string>("Language"); }
            set { SetProperty(value, "Language"); }
        }

        [ParseFieldName("GameId")]
        public string GameId
        {
            get { return GetProperty<string>("GameId"); }
            set { SetProperty(value, "GameId"); }
        }


        public Texture2D Texture { get; private set; }

        public bool IsTextureDownloaded
        {
            get { return Texture != null; }
        }


        public void DownloadTexture(Action<Texture2D> handler)
        {
            if (Image == null)
            {
                UnityEngine.Debug.Log("CreateTextureFromImage(): Image == null");
                return;
            }

            ObservableWWW.GetWWW(Image.Url.AbsoluteUri)
                .CatchIgnore(new Action<Exception>(UnityEngine.Debug.LogException))
                .Subscribe(www =>
                                                                          {
                                                                              if (handler != null)
                                                                              {
                                                                                  Texture = www.texture;
                                                                                  handler(Texture);
                                                                              }
                                                                          });

        }
        public void DownloadTexture()
        {
             DownloadTexture(null);
        }


        public override string ToString()
        {
            return string.Format("PromotionBanner[{0}] -> {1}", GameId, Link);
        }


        public static string DefaultLanguageCode = "en-EN";

        public static void ReceiveBannerFromCloudWithCompletionHandler(Action<PromotionBanner> handler,
            string languageCode)
        {
            var q = new ParseQuery<PromotionBanner>()
                .WhereEqualTo("Language", languageCode);


            var searchTask = q.FindAsync();
            searchTask.ContinueWith(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    UnityEngine.Debug.LogError("Banner receiving failed!");
                    GameMessenger.Broadcast(PromotionBannerReceiveFailedEvent);
                    return;
                }

                var results = task.Result;


                if (results == null || !results.Any())
                {


                    if (languageCode == DefaultLanguageCode)
                    {
                        GameMessenger.Broadcast(PromotionBannerRequestedLanguageNotFoundEvent, languageCode);
                        return;
                    }
                    else
                    {
                        ReceiveBannerFromCloudWithCompletionHandler(handler, DefaultLanguageCode);
                        return;
                    }
                }

                var b = results.FirstOrDefault();

                GameMessenger.Broadcast(PromotionBannerReceiveSuccessfullEvent, b);

                if (handler != null)
                    handler(b);

            });
        }
        public static void ReceiveBannerFromCloudWithCompletionHandler(Action<PromotionBanner> handler)
        {
            ReceiveBannerFromCloudWithCompletionHandler(handler, "en-EN");
        }
        public static void ReceiveBannerFromCloud()
        {
            ReceiveBannerFromCloudWithCompletionHandler(null);
        }



    }
}
