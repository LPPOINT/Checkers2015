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


        private static float ZMoveOffset = 0.1f;

        public override Vector3 Offset
        {
            get { return  new Vector3(0.0f, 0.0f, 1);}
        }

        public const string CheckerMoveCompleteEventName = "CheckerMoveComplete";
        public const string CheckerMoveStartedEventName = "CheckerMoveStarted";

        private Tweener TweenMoveTo(Vector3 pos, float speed, Ease ease, Action onComplete)
        {
            return transform.DOMove(pos, speed)
                .SetSpeedBased()
                .SetEase(ease)
                .OnStart(() =>
                         {
                             transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - ZMoveOffset);
                         })
                .OnComplete(() =>
                            {
                                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + ZMoveOffset);
                                if (onComplete != null) onComplete();
                            });
        }

        public void Move(CheckerMove move)
        {
            var oldX = X;
            var oldY = Y;
            X = move.NewX;
            Y = move.NewY;
            var pos = CalculateWorldPositionAt(X, Y);
            TweenMoveTo(pos, 3, Ease.Linear, () => GameMessenger.Broadcast(CheckerMoveCompleteEventName, this, move));
            GameMessenger.Broadcast(CheckerMoveStartedEventName, this, move);

        }

        public void PositionateAt(int x, int y)
        {
            X = x;
            Y = y;
            var p = Board.Instance.CalculateCellCenterPosition(X, Y);
            transform.position = new Vector3(p.x, p.y, Board.Instance.transform.position.z - Offset.z);
        }

        protected override void Awake()
        {
            transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0f, 360f));
            base.Awake();
        }

        public void KillSelf()
        {
            Destroy(gameObject);
        }
    }
}
