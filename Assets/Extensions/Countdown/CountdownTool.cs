using UnityEngine;

namespace ChuuniExtension.CountdownTools
{
    public class CountdownTool : MonoBehaviour
    {
        private static MonoBehaviour _countdownManager;

        public static MonoBehaviour CountdownManager{
            get{
                if(_countdownManager == null){
                    _countdownManager = new GameObject("CountdownManager").AddComponent<CountdownTool>();
                    DontDestroyOnLoad(_countdownManager);
                }
                return _countdownManager;
            }
            private set{
                _countdownManager = value;
            }
        }
    }
}