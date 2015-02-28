using System.Linq;
using UnityEngine;

namespace Assets.Classes.Implementation
{
    public class UserPlayerBehaviour : PlayerBehaviour
    {

        public enum State
        {
            WaitForSelect,
            WaitForMove
        }


        public State CurrentState { get; private set; }


        private Checker selectedChecker;

        private void OnTap(TapGesture gesture)
        {

            if(!IsCurrentPlayer) return;

            Log("Tap");

            var cellPosition = Board.Instance.GetNearestCellPosition(gesture.Position);
            if(cellPosition == null) return;
            var checker = Board.Instance.GetCheckerAt((int)cellPosition.Value.x, (int)cellPosition.Value.y);

            if (CurrentState == State.WaitForSelect)
            {

                if(checker == null || checker.Color != Color) return;

                Log("Checker selected");

                CurrentState = State.WaitForMove;
                selectedChecker = checker;

            }
            else if (CurrentState == State.WaitForMove)
            {
                if(checker != null) return;

                Log("Move");

                OnMoveExecuted(new CheckerMove(selectedChecker.X, selectedChecker.Y, (int)cellPosition.Value.x, (int)cellPosition.Value.y));
            }

        }

        public override void OnInputReceived()
        {

            Board.Instance.RemoveAllHighlightings();

            //foreach (var c in Board.Instance.Checkers.Where(checker => checker.Color == Color))
            //{
            //    Board.Instance.HighlightCell(c.X, c.Y, UnityEngine.Color.green);
            //}

            base.OnInputReceived();
        }
    }
}
