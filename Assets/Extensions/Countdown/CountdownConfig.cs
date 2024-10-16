using System;

namespace ChuuniExtension.CountdownTools
{
    [Serializable]
    public class CountdownConfig
    {
        public float Speed;
        public float Distance;
        public float Interval;

        public CountdownConfig(float speed, float distance, float interval){
            Speed = speed;
            Distance = distance;
            Interval = interval;
        }

        public static CountdownConfig DistanceConfig(float speed, float distance){
            return new CountdownConfig(speed, distance, default);
        }
        public static CountdownConfig TimeConfig(float interval){
            return new CountdownConfig(default, default, interval);
        }
    }
}
