using Soomla.Store;

namespace Assets.Classes.Core
{
    public class Store : UniqueGameSystem<Store>
    {


        public Store()
        {
            Assets = null;
        }

        public Store(IStoreAssets assets)
        {
            Assets = assets;
        }

        public IStoreAssets Assets { get; private set; }

        public override void Load()
        {
            if (Assets == null)
            {
                Logs.Instance.ProcessError("Trying to initialize store without store assets");
                return;
            }

            SoomlaStore.Initialize(Assets);

        }
    }
}
