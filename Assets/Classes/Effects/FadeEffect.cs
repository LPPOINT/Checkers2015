using System;
using Assets.Classes.Foundation.Classes;
using DG.Tweening;
using UnityEngine;

namespace Assets.Classes.Effects
{
    public class FadeEffect : GameEffect
    {



        [Serializable]
        public class FadeInfo
        {
            public Color FromColor;
            public Color ToColor;

            public float Time;
            public Ease EaseType;

            public FadeInfo()
            {
                
            }

            public FadeInfo(Color fromColor, Color toColor, float time, Ease easeType)
            {
                FromColor = fromColor;
                ToColor = toColor;
                Time = time;
                EaseType = easeType;
            }
        }

        public event EventHandler<GenericEventArgs<FadeInfo>> FadeStarted;

        protected virtual void OnFadeStarted(GenericEventArgs<FadeInfo> e)
        {
            var handler = FadeStarted;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<GenericEventArgs<FadeInfo>> FadeComplete;

        protected virtual void OnFadeComplete(GenericEventArgs<FadeInfo> e)
        {
            var handler = FadeComplete;
            if (handler != null) handler(this, e);
        }


        public FadeOverlay Overlay;

        public bool IsFading { get; private set; }
        public FadeInfo LastFadeInfo { get; private set; }


        private void Awake()
        {
            if (Overlay == null) Overlay = FindObjectOfType<FadeOverlay>();
        }


        private void OnITweenFadeComplete()
        {
            Overlay.DisableOverlay();
            IsFading = false;
            OnFadeComplete(new GenericEventArgs<FadeInfo>(LastFadeInfo));
        }


        public void StartFade(FadeInfo fadeInfo)
        {
            if(IsFading) return;
            LastFadeInfo = fadeInfo;

            Overlay.SetOverlayColor(fadeInfo.FromColor);
            Overlay.EnableOverlay();
            DOTween.To(() => Overlay.renderer.material.color, value => Overlay.renderer.material.color = value,
                fadeInfo.ToColor, fadeInfo.Time)
                .SetEase(fadeInfo.EaseType)
                .OnComplete(OnITweenFadeComplete);

            IsFading = true;
            OnFadeStarted(new GenericEventArgs<FadeInfo>(fadeInfo));
        }
    }
}
