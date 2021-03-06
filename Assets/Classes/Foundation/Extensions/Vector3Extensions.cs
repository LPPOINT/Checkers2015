﻿using UnityEngine;

namespace Assets.Classes.Foundation.Extensions
{
    public static class Vector3Extensions
    {
        // NOTE: No operators-as-extension methods! 
        // http://stackoverflow.com/questions/172658/operator-overloading-with-c-extension-methods
/*        public static Vector3 operator *(this Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }*/

        // Converts a Vector 3 to Vector 2
        public static Vector2 ToVector2(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }
    }
}