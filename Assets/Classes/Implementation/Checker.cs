using System;
using Assets.Classes.Core;
using DG.Tweening;
using UnityEngine;

namespace Assets.Classes.Implementation
{
    public class Checker : BoardEntity
    {

        public enum State
        {
            Showing,
            Moving,
            Idle,
            OutOfGame
        }

        public GameColor Color;
        public State CurrentState { get; set; }


        public override Vector3 Offset
        {
            get { return  new Vector3(0.04f, 0.031f, 1);}
        }

        public const string CheckerMoveComplateEventName = "CheckerMoveComplete";


        private Tweener TweenMoveTo(Vector3 pos, float speed, Ease ease, Action onComplete)
        {
            return transform.DOMove(pos, speed)
                .SetSpeedBased()
                .SetEase(ease)
                .OnComplete(() =>
                            {
                                if (onComplete != null) onComplete();
                            });
        }

        public void Move(int newX, int newY)
        {
            var oldX = X;
            var oldY = Y;
            X = newX;
            Y = newY;
            var pos = CalculateWorldPositionAt(X, Y);
            TweenMoveTo(pos, 3, Ease.Linear, () => GameMessenger.Broadcast(CheckerMoveComplateEventName, this, new CheckerMove(oldX, oldY, newX, newY)));

        }

        public void PositionateAt(int x, int y)
        {
            X = x;
            Y = y;
            PositionateSprite();
        }

    }
}
