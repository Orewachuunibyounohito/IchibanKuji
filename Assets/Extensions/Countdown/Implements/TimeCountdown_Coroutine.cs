using System;
using System.Collections;
using UnityEngine;

namespace ChuuniExtension.CountdownTools
{
    [Serializable]
    public class TimeCountdown_Coroutine : ICountdown_Coroutine
    {
        private const float FIXED_SECONDS = 0.05f;
        private float duration;
        private float fps = 0;

        public bool TimesUp { get; private set; }
        public float DeltaTime {
            get{
                if(fps == 0){
                    return Time.deltaTime;
                }
                return 1 / fps;
            }
        }

        public TimeCountdown_Coroutine(float duration, float fps = 0){
            this.duration = duration;
            this.fps = fps;
        }

        public void Start() => CountdownTool.CountdownManager.StartCoroutine(CountdownTask());
        public void Reset() => TimesUp = false;

        private IEnumerator CountdownTask(){
            float startTime = Time.time;
            float timer = 0;
            while(timer < duration){
                yield return new WaitForSeconds(FIXED_SECONDS);
                timer = Time.time - startTime;
            }
            TimesUp = true;
        }
    }
}
