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


        public void ChangeMoveStatus(GameColor newPlayerColor)
        {
            
        }

        private void RegisterCheckerMoveComplete(Checker checker, CheckerMove move)
        {
            ActivateOtherPlayer();
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

        protected override void Awake()
        {
            GameMessenger.AddListener<Checker, CheckerMove>(Checker.CheckerMoveComplateEventName, RegisterCheckerMoveComplete);
            ActivatePlayer(GameColor.White);
            base.Awake();
        }
    }
}
