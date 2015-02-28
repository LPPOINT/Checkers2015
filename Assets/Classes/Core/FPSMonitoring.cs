using UnityEngine;

namespace Assets.Classes.Core
{
    public class FPSMonitoring : UniqueGameSystem<FPSMonitoring>
    {
        public enum TargetFrameRate
        {
            FrameRate60 = 60,
            FrameRate30 = 30
        }

        public TargetFrameRate FrameRate;

        public override void Load()
        {
            Application.targetFrameRate = (int)FrameRate;
        }
        public override void Update()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            var fps = 1.0f / deltaTime;
            CurrentFPS = fps;
        }


        private float deltaTime;

        public float CurrentFPS { get; private set; }
    }
}
