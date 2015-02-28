using Assets.Classes.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Classes.Implementation
{
    public class UIColorToMove : SingletonEntity<UIColorToMove>
    {
        public string ChangeColorNameAnimationName = "Motion";

        public string WhiteColorName = "White";
        public string BlackColorName = "Black";

        public Text UIText;

        public string GetColorName(GameColor color)
        {
            return color == GameColor.Black ? BlackColorName : WhiteColorName;
        }

        private GameColor lastColor;

        public void ChangeColorName(GameColor newColor)
        {
            lastColor = newColor;
            animator.Play(ChangeColorNameAnimationName);
        }

        public void ChangeColorName()
        {
            ChangeColorName(lastColor == GameColor.Black ? GameColor.White : GameColor.Black);
        }

        public void OnUITextFullyDismissed()
        {
            UIText.text = GetColorName(lastColor);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                ChangeColorName();
            }
        }

    }
}
