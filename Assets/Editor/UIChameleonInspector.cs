using Assets.Classes.Core;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomEditor(typeof(UIChameleon))]
    public class UIChameleonInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var t = (UIChameleon) target;

            if (GUILayout.Button("Update all chameleons"))
            {
                foreach ( var c in FindObjectsOfType<UIChameleon>())
                {
                    c.UpdateColor();
                }
            }
            
        }
    }
}
