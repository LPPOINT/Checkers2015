using Assets.Classes.Core;

namespace Assets.Classes.Implementation
{
    public class PlayerBehaviour : RoseEntity
    {

        public GameColor Color;

        public bool IsCurrentPlayer
        {
            get { return Round.Instance.CurrentPlayer == this; }
        }

        public virtual void OnInputReceived()
        {
        }

        public virtual void OnInputWithheld()
        {
            
        }

        public virtual void OnInputLost()
        {
            
        }

        protected void Move(CheckerMove move)
        {
            Round.Instance.RegisterCurrentPlayerMove(move);
        }
    }
}
