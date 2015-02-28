using Assets.Classes.Core;

namespace Assets.Classes.Implementation
{
    public class PlayerBehaviour : RoseEntity
    {

        public GameColor Color;

        public bool IsCurrentPlayer { get; private set; }

        public virtual void OnInputReceived()
        {
            IsCurrentPlayer = true;
        }

        protected void OnMoveExecuted(CheckerMove move)
        {
            IsCurrentPlayer = false;
            Round.Instance.RegisterCurrentPlayerMove(move);
        }
    }
}
