using Assets.Classes.Core;
using UnityEngine;

namespace Assets.Classes.Implementation
{
    public class HighlightingPresets : SingletonEntity<HighlightingPresets>
    {

        public Color SelectionColor = Color.green;
        public Color MoveToColor = Color.blue;
        public Color KillColor = Color.red;
    }
}
