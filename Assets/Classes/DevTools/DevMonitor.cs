using System;
using System.Text;
using Assets.Classes.Core;
using Assets.Classes.Foundation.Classes;
using UnityEngine.UI;

namespace Assets.Classes.DevTools
{
    public class DevMonitor : RoseEntity
    {

        public Text UIText;

        public bool IsActive { get; private set; }

        public void SwitchActiveStatus()
        {
            if (IsActive)
            {
                IsActive = false;
                UIText.enabled = false;
            }
            else if (!IsActive)
            {
                IsActive = true;
                UIText.enabled = true;
            }
        }

        private void Update()
        {
            if(!IsActive) return;

            var b = new StringBuilder();
            var fps = FPSMonitoring.Instance.CurrentFPS;

            if (fps < 57)
            {
                b.AppendLine("FPS: " + (int) fps);
            }
            else
            {
                b.AppendLine("FPS: max");
            }

            b.AppendLine("Version: " + GameCore.Instance.GameVersion);

            var text = b.ToString();

            if (UIText.text != text)
            {
                UIText.text = text;
            }

        }
        protected override void Awake()
        {
            IsActive = true;
        }
    }
}
