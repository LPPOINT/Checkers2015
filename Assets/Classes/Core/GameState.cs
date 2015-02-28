namespace Assets.Classes.Core
{
    public class GameState<T> : GameStateBase where T : GameStateBase
    {
        private static T instance;

        public static T Instance
        {
            get { return instance ?? (instance = GameStates.Instance.GetState<T>()); }
        }
    }
}
