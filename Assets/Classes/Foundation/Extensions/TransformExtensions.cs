using UnityEngine;

namespace Assets.Classes.Foundation.Extensions
{
    public static class TransformExtensions
    {
        public static Vector3 WorldToLocal(this Transform t, Vector3 position)
        {
            return t.transform.position - position;
        }

        public static Vector3 SetScale(this Transform t, float scale)
        {
            t.localScale = new Vector3(scale, scale, t.localScale.z);
            return t.lossyScale;
        }

    }
}