using System;

namespace ChuuniExtension
{
    [Serializable]
    public class Range
    {
        public float Max;
        public float Min;

        public Range(float max, float min){
            if(max < min){
                float temp = max;
                max = min;
                min = temp;
            }
            Max = max;
            Min = min;
        }
    }
}