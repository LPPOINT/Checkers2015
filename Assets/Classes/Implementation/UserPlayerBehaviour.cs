using System.Collections.Generic;
using System.Linq;
using Assets.Classes.Effects;
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
        private List<Checker> moveableCheckers;
        private List<CheckerMove> selectedCheckerMoves;

        private void SelectChecker(Checker checker)
        {

            CurrentState = State.WaitForMove;
            selectedChecker = checker;
            selectedCheckerMoves = new List<CheckerMove>(CheckerMoves.GetAllowedMovesForChecker(selectedChecker));
            HighlightSelectedChecker();
        }

        private void OnTap(TapGesture gesture)
        {

            if(!IsCurrentPlayer || Round.Instance.IsGameInputLocked) return;


            var cellPosition = Board.Instance.GetNearestCellPosition(Camera.main.ScreenToWorldPoint(gesture.Position));

            if(cellPosition == null) return;
            var checker = Board.Instance.GetCheckerAt((int)cellPosition.Value.x, (int)cellPosition.Value.y);

            if (CurrentState == State.WaitForSelect)
            {

                if(checker == null || checker.Color != Color) return;
                SelectChecker(checker);

            }
            else if (CurrentState == State.WaitForMove)
            {
                if(checker == selectedChecker) return;

                if (checker != null && checker.Color == Color)
                {
                    SelectChecker(checker);
                    return;
                }

                var move =
                    selectedCheckerMoves.FirstOrDefault(
                        checkerMove =>
                            checkerMove.NewX == (int)cellPosition.Value.x && checkerMove.NewY == (int)cellPosition.Value.y);

                if (move == null)
                {
                    Debug.Log("Move not found");
                }
                else
                {
                    Move(move);
                }
            }

        }

        public override void OnInputReceived()
        {



            moveableCheckers = new List<Checker>(CheckerMoves.GetMoveableCheckers(Board.Instance.Checkers.Where(checker => checker.Color == Color)));
            selectedCheckerMoves = null;
            selectedChecker = null;
            CurrentState = State.WaitForSelect;

            HighlightMoveableCheckers();

            base.OnInputReceived();
        }


        private void HighlightMoveableCheckers()
        {
            Board.Instance.RemoveAllHighlightings();

            foreach (var moveableChecker in moveableCheckers)
            {
                Board.Instance.HighlightCell(moveableChecker.X, moveableChecker.Y, HighlightingPresets.Instance.SelectionColor);
            }

        }

        private void HighlightSelectedChecker()
        {
            HighlightChecker(selectedChecker, selectedCheckerMoves, Board.Instance.Checkers.Where(checker => checker.Color == Color && checker != selectedChecker));
        }

        private void HighlightChecker(Checker c, IEnumerable<CheckerMove> moves, IEnumerable<Checker> darkenHg)
        {
            Board.Instance.RemoveAllHighlightings();
            Board.Instance.HighlightCell(c.X, c.Y, HighlightingPresets.Instance.SelectionColor);

            foreach (var checkerMove in moves)
            {
                Board.Instance.HighlightCell(checkerMove.NewX, checkerMove.NewY, HighlightingPresets.Instance.MoveToColor);

                if (checkerMove.HasVictim)
                {
                    Board.Instance.HighlightCell(checkerMove.VictimX, checkerMove.VictimY, HighlightingPresets.Instance.KillColor);
                }

            }

            foreach (var darkenChecker in darkenHg)
            {
                Board.Instance.HighlightCell(darkenChecker.X, darkenChecker.Y, HighlightingPresets.Instance.DarkenSelectionColor);
            }

        }

    }
}
