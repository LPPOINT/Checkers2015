using System.Collections.Generic;
using System.Linq;
using Assets.Classes.Foundation.Classes;
using Assets.Classes.Foundation.Enums;

namespace Assets.Classes.Implementation
{
    public static class CheckerMoves
    {



        public static IEnumerable<CheckerMove> GetAllowedKillMovesForChecker(Checker checker)
        {
            var offsets = new Point2[4]
                          {
                              new Point2(1, 1),
                              new Point2(1, -1),
                              new Point2(-1, 1),
                              new Point2(-1, -1)
                          };

            foreach (var offset in offsets)
            {
                var posVictim = GetBoardPosition(checker, offset.X, offset.Y);
                var posMoveTo = new Point2(posVictim.X + (1*offset.X), posVictim.Y + (1*offset.Y));

                var vc = Board.Instance.GetCheckerAt(posVictim.X, posVictim.Y);
                var mtc = Board.Instance.GetCheckerAt(posMoveTo.X, posMoveTo.Y);


                if(vc != null && vc.Color != checker.Color && mtc == null && !IsOutOfBoard(posMoveTo.X, posMoveTo.Y))
                    yield return new CheckerMove(checker.X, checker.Y, posMoveTo.X, posMoveTo.Y, posVictim.X, posVictim.Y);

            }

        }
        public static IEnumerable<CheckerMove> GetAllowedDefaultMovesForChecker(Checker checker)
        {
            var side = Board.Instance.GetCheckersSideByColor(checker.Color);

            var offsets = new Point2[2];

            if (side == VerticalDirection.Top)
            {
                offsets[0] = new Point2(1, 1);
                offsets[1] = new Point2(-1, 1);
            }
            else if (side == VerticalDirection.Bottom)
            {
                offsets[0] = new Point2(1, -1);
                offsets[1] = new Point2(-1, -1);
            }

            foreach (var p in offsets)
            {
                var c = GetChecker(checker, p.X, p.Y);
                var pos = GetBoardPosition(checker, p.X, p.Y);
                if (c == null && !IsOutOfBoard(pos.X, pos.Y))
                {
                    yield return new CheckerMove(checker, pos.X, pos.Y);
                }
            }


        }

        public static IEnumerable<CheckerMove> GetAllowedMovesForChecker(Checker checker)
        {
            var km = GetAllowedKillMovesForChecker(checker);

            if (km != null && km.Any()) return km;

            return GetAllowedDefaultMovesForChecker(checker);
        }

        public static IEnumerable<Checker> GetMoveableCheckers(IEnumerable<Checker> checkers)
        {
            var kill = false;
            foreach (var checker in checkers)
            {
                var killMoves = GetAllowedKillMovesForChecker(checker);
                if (killMoves.Any())
                {
                    kill = true;
                    yield return checker;
                }

                if (!kill)
                {
                    var dm = GetAllowedDefaultMovesForChecker(checker);
                    if (dm.Any()) yield return checker;
                }

            }


        }
        public static bool IsMoveableChecker(Checker checker)
        {
            var moves = GetAllowedDefaultMovesForChecker(checker);

            return moves != null && moves.Any();
        }

        private static Checker GetChecker(Checker origin, int xOffset, int yOffset)
        {
            var x = origin.X + xOffset;
            var y = origin.Y + yOffset;
            return Board.Instance.GetCheckerAt(x, y);
        }
        private static bool IsEmptyCell(Checker origin, int xOffset, int yOffset)
        {
            return GetChecker(origin, xOffset, yOffset) == null;
        }

        private static Point2 GetBoardPosition(Checker origin, int xOffset, int yOffset)
        {
            return new Point2(origin.X + xOffset, origin.Y + yOffset);
        }

        private static bool IsOutOfBoard(int x, int y)
        {
            return x < 0 || x >= 8 || y < 0 || y >= 8;
        }

    }
}
