using Assets.Classes.Core;
using Assets.Classes.Foundation.Classes;
using DG.Tweening;
using UnityEngine;

namespace Assets.Classes.Effects
{
    /// <summary>
    /// TODO: complete this shit
    /// </summary>
    public class Blink : GameEffect
    {
        private FadeEffect.FadeInfo In = new FadeEffect.FadeInfo(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), 0.125f, Ease.Linear );
        private FadeEffect.FadeInfo Out = new FadeEffect.FadeInfo(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), 0.3f, Ease.Linear);


        public const string BlinkCompleteEventName = "BlinkComplete";


        private void OnInComplete(object sender, GenericEventArgs<FadeEffect.FadeInfo> fi)
        {
            var fadeEffect = Effects.GameEffects.GetEffectInstanceInInstance<FadeEffect>();
            fadeEffect.StartFade(Out);
            fadeEffect.FadeComplete -= OnInComplete;
            fadeEffect.FadeComplete += OnOutComplete;
        }

        private void OnOutComplete(object sender, GenericEventArgs<FadeEffect.FadeInfo> fi)
        {
            var fadeEffect = Effects.GameEffects.GetEffectInstanceInInstance<FadeEffect>();
            fadeEffect.FadeComplete -= OnOutComplete;
            GameMessenger.Broadcast(BlinkCompleteEventName);
        }

        public void Play()
        {
            var fadeEffect = Effects.GameEffects.GetEffectInstanceInInstance<FadeEffect>();
            fadeEffect.StartFade(In);
            fadeEffect.FadeComplete += OnInComplete;
        }

        public static void PlayInInstance()
        {
            Effects.GameEffects.GetEffectInstanceInInstance<Blink>().Play();
        }

    }
}
