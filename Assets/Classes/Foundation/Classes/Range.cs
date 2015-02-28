using System;
using Random = UnityEngine.Random;

namespace Assets.Classes.Foundation.Classes
{
    [Serializable]
    public class Range
    {

        public Range()
        {
            
        }

        public Range(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Min;
        public float Max;

        public float GenerateRandom()
        {
            return Random.Range(Min, Max);
        }



    }

     [Serializable]
    public class NumericRange
    {
        public NumericRange()
        {
            
        }

        public NumericRange(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public int Min;
        public int Max;

        public int GenerateRandom()
        {
            return Random.Range(Min, Max);
        }

    }

}
