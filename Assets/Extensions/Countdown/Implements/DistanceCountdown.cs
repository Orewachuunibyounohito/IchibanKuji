using System;
using UnityEngine;

namespace ChuuniExtension.CountdownTools
{
    [Serializable]
    public class DistanceCountdown : ICountdown{
        private float speed;
        private float distance;
        private float threshold;
        private float fps = 0;

        public bool TimesUp => distance <= 0;

        public float DeltaTime {
            get{
                if(fps == 0){
                    return Time.deltaTime;
                }
                return 1 / fps;
            }
        }

        public DistanceCountdown(float speed, float threshold, float fps = 0){
            this.speed = speed;
            this.threshold = threshold;
            this.fps = fps;
            distance = threshold;
        }

        public void Update(){
            float step = speed * DeltaTime;
            distance -= step;
        }

        public void Reset() => distance = threshold;
    }
}
