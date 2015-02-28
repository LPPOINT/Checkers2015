namespace Assets.Classes.Core
{
    public interface IGameSystem
    {
        void PreLoad();
        void Load();
        void PostLoad();

        void PreStartup();
        void Startup();
        void PostStarup();

        void PreShutdown();
        void Shutdown();
        void PostShutdown();

        void PostUpdate();
        void Update();
        void PreUpdate();
        void FixedUpdate();

    }

    public class GameSystemBase : IGameSystem
    {
        public virtual void PreLoad()
        {

        }

        public virtual void Load()
        {

        }

        public virtual void PostLoad()
        {

        }

        public virtual void PreStartup()
        {

        }

        public virtual void Startup()
        {

        }

        public virtual void PostStarup()
        {

        }

        public virtual void PreShutdown()
        {

        }

        public virtual void Shutdown()
        {

        }

        public virtual void PostShutdown()
        {

        }

        public virtual void PostUpdate()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void PreUpdate()
        {
            
        }

        public virtual void FixedUpdate()
        {

        }
    }

    public class UniqueGameSystem<T> : GameSystemBase where T : class, IGameSystem
    {

        private static T instance;
        public static T Instance
        {
            get { return instance ?? (instance = GameCore.Instance.GetGameSystem<T>()); }
        }

        public static bool IsAvailable
        {
            get { return Instance != null; }
        }

      
    }

}
