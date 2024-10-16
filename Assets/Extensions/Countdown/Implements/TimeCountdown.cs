using System;
using UnityEngine;

namespace ChuuniExtension.CountdownTools
{
    [Serializable]
    public class TimeCountdown : ICountdown
    {
        private float timer;
        private float threshold;
        private float fps = 0;

        public bool TimesUp => timer <= 0;
        public float DeltaTime {
            get{
                if(fps == 0){
                    return Time.deltaTime;
                }
                return 1 / fps;
            }
        }

        public TimeCountdown(float threshold, float fps = 0){
            this.threshold = threshold;
            this.fps = fps;
            timer = threshold;
        }

        public void Update() => timer -= DeltaTime;
        public void Reset() => timer = threshold;
    }
}
