using System;
using Assets.Classes.Foundation.Classes;
using UnityEngine;

namespace Assets.Classes.Effects
{
    public class ShakeEffect : GameEffect
    {
        public class ShakeInfo
        {

            public static readonly ShakeInfo Default = new ShakeInfo(new Vector3(0.5f, 0.5f, 0), 0.3f);

            public ShakeInfo(Vector3 amount, float time)
            {
                Amount = amount;
                Time = time;
            }

            public Vector3 Amount { get; private set; }
            public float Time { get; private set; }
        }


        public event EventHandler<GenericEventArgs<ShakeInfo>> ShakeStarted;
        public event EventHandler<GenericEventArgs<ShakeInfo>> ShakeCompleted;

        public bool IsShaking { get; private set; }
        public ShakeInfo ActualShakeInfo { get; private set; }

        private void OnITweenShakingStart()
        {
            IsShaking = true;
            var h = ShakeStarted;
            if(h != null) h(this, new GenericEventArgs<ShakeInfo>(ActualShakeInfo));
        }

        private void OnITweenShakingComplete()
        {
            IsShaking = false;
            var h = ShakeCompleted;
            if (h != null) h(this, new GenericEventArgs<ShakeInfo>(ActualShakeInfo));
        }


        public void Shake(ShakeInfo info)
        {
            iTween.ShakePosition(Camera.main.gameObject, iTween.Hash(
                    "time", info.Time,
                    "amount", info.Amount,

                    "onstart", "OnITweenShakingStart",
                    "oncomplete", "OnITweenShakingComplete",
                    "onstarttarget", gameObject,
                    "oncompletetarget", gameObject
                ));

            ActualShakeInfo = info;

        }

    }
}
