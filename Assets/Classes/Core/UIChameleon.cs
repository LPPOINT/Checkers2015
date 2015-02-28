using UnityEngine;
using UnityEngine.UI;

namespace Assets.Classes.Core
{
    public class UIChameleon : RoseEntity, CameraSupervisor.ICameraColorObserver
    {

        public Graphic Target;


        public void OnCameraBackgroundColorChanged(Color oldColor, Color newColor)
        {
            
        }

        protected override void Awake()
        {
            if (Target == null) Target = GetComponent<Graphic>();
            CameraSupervisor.Main.AddObserver(this);
            UpdateColor();

            base.Awake();
        }

        private void Start()
        {
            UpdateColor();
        }

        public void OnCameraPropertyChanged(Color oldValue, Color newValue)
        {
            Target.color = newValue;
        }

        public void UpdateColor()
        {
            Target.color = Camera.main.backgroundColor;
        }
    }
}
