using System.Linq;
using Assets.Classes.Core;
using UnityEngine;

namespace Assets.Classes.Implementation
{
    [EntryState]
    public class Round : GameState<Round>
    {


        public PlayerBehaviour WhitePlayer;
        public PlayerBehaviour BlackPlayer;

        public PlayerBehaviour CurrentPlayer { get; private set; }

        public bool IsGameInputLocked { get; private set; }

        public void LockInput()
        {
            IsGameInputLocked = true;
        }

        public void UnlockInput()
        {
            IsGameInputLocked = false;
        }

        public void ChangeMoveStatus(GameColor newPlayerColor)
        {
            
        }

        private void ShowWinnerPopup(GameColor winerColor)
        {
            UIPopups.Instance.ShowToast(winerColor + " Wins!", 3);
        }

        private void RegisterCheckerMoveComplete(Checker checker, CheckerMove move)
        {
            UnlockInput();
            if (move.HasVictim)
            {
                Board.Instance.KillChecker(Board.Instance.GetCheckerAt(move.VictimX, move.VictimY));


                var probe = Board.Instance.Checkers.FirstOrDefault().Color;
                if (Board.Instance.Checkers.All(checker1 => checker1.Color == probe))
                {
                    Debug.Log("GameOver");
                    
                    ShowWinnerPopup(probe);

                    NewGame();

                    return;
                }

                var nextKillMoves = CheckerMoves.GetAllowedKillMovesForChecker(checker);
                if (nextKillMoves.Any())
                {
                    ActivateOtherPlayer();
                    //CurrentPlayer.OnInputWithheld();
                }
                else
                {
                    ActivateOtherPlayer();
                }
            }
            else
            {
                ActivateOtherPlayer();
            }
        }

        private void RegisterCheckerMoveStarted(Checker checker, CheckerMove move)
        {
            LockInput();
        }
        public void RegisterCurrentPlayerMove(CheckerMove move)
        {
            Board.Instance.Move(move);
        }

        public PlayerBehaviour GetPlayerByColor(GameColor color)
        {
            return color == GameColor.Black ? BlackPlayer : WhitePlayer;
        }

        private void ActivatePlayer(GameColor color)
        {
            ActivatePlayer(GetPlayerByColor(color));
        }
        private void ActivatePlayer(PlayerBehaviour player)
        {
            CurrentPlayer = player;
            CurrentPlayer.OnInputReceived();
            UIColorToMove.Instance.ChangeColorName(CurrentPlayer.Color);
        }
        private void ActivateOtherPlayer()
        {
            ActivatePlayer(CurrentPlayer.Color == GameColor.Black ? GameColor.White : GameColor.Black);
        }


        public void NewGame()
        {
            ActivatePlayer(GameColor.White);
            Board.Instance.PositionateCheckersByDefault();
        }

        protected override void Awake()
        {
            GameMessenger.AddListener<Checker, CheckerMove>(Checker.CheckerMoveCompleteEventName, RegisterCheckerMoveComplete);
            GameMessenger.AddListener<Checker, CheckerMove>(Checker.CheckerMoveStartedEventName, RegisterCheckerMoveStarted);
            base.Awake();
        }

        private void Start()
        {
            NewGame();
        }

    }
}
