using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Classes.Implementation
{
    public class RandomPlayerBehaviour : PlayerBehaviour
    {
        public override void OnInputReceived()
        {
            Invoke("Action", Random.Range(3f, 5f));
            base.OnInputReceived();
        }

        private void Action()
        {
            var l = Board.Instance.Checkers.Where(checker => checker.Color == Color).ToList();
            var c = l[Random.Range(0, l.Count)];

            Vector2? cell = null;
            var loopCheck = 0;
            var loopCheckMax = Math.Pow(64, 2);

            while (cell == null)
            {
                loopCheck++;
                if(loopCheck > loopCheckMax) break;

                var x = Random.Range(0, 8);
                var y = Random.Range(0, 8);

                if(Board.Instance.GetCheckerAt(x, y) == null) cell = new Vector2(x, y);


            }

            if (cell == null)
            {
                ProcessError("Cant find cell!");
                return;
            }

            OnMoveExecuted(new CheckerMove(c.X, c.Y, (int)cell.Value.x, (int)cell.Value.y));

        }

    }
}
