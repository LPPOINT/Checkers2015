using DG.Tweening;

namespace Assets.Classes.Core
{
    public class Tweenings : GameSystemBase
    {
        public override void Load()
        {
            DOTween.Init();
        }
    }
}
