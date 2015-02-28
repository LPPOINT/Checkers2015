using System;
using Assets.Classes.Core;
using DG.Tweening;
using UnityEngine;

namespace Assets.Classes.Implementation
{
    public class Highlighting : BoardEntity
    {


        public Color HighlightingColor;


        public override Vector3 Offset
        {
            get { return new Vector3(0, 0, 0.5f); }
        }

        public const string HighlightingDisposedEventName = "HighlightingDisposed";

        public void SetDirty()
        {
            spriteRenderer.color = HighlightingColor;
        }

        private void PlayColorMotion(Color from, Color to, float time, Ease ease, Action onComplete)
        {
            DOTween.To(() => spriteRenderer.color, value => spriteRenderer.color = value, to, time)
                .SetEase(ease)
                .OnComplete(() =>
                            {
                                if (onComplete != null) onComplete();
                            }).Play();
        }

        public void Dispose()
        {
            PlayColorMotion(spriteRenderer.color, new Color(0, 0, 0, 0), 0.2f, Ease.Linear, () =>
                                                                                            {
                                                                                                GameMessenger.Broadcast(HighlightingDisposedEventName, this);
                                                                                                Destroy(gameObject);
                                                                                            });
        }

        public void DisposeImmediate()
        {
            GameMessenger.Broadcast(HighlightingDisposedEventName, this);
            Destroy(gameObject);
        }

        public void DisposeImmediateWithoutEvent()
        {
            Destroy(gameObject);
        }


        private void Start()
        {
            PlayColorMotion(new Color(0, 0, 0, 0), HighlightingColor, 0.3f, Ease.Linear, null);
            PositionateSprite();
            base.Awake();
        }
    }
}
